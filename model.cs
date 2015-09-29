using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Name:Joseph Collins
//ID: 98718584
//Module: AM6008
//Assignment 2

namespace BOID2014
{
    class model
    {
        //creating a list of BOID objects, this will represent the flock
        private List<BOID> flock = new List<BOID>();

        internal List<BOID> Flock
        {
            get { return flock; }
            set { flock = value; }
        }

        //
        public model()
        {

        }

        //creating the blank BOID objects
        public model(int n)
        {
            int count = n;
            for (int i = 0; i < n; i++)
                Flock.Add(new BOID());
        }

        //Predator object
        private BOID predator = new BOID();

        internal BOID Predator
        {
            get { return predator; }
            set { predator = value; }
        }

        //property to limit the velocity of the boids
        private double vlim = 5;

        public double Vlim
        {
            get { return vlim; }
            set { vlim = value; }
        }

        //property to limit the velocity of the Predator
        private double predlim = 5;

        public double Predlim
        {
            get { return predlim; }
            set { predlim = value; }
        }


        //will need a method to initialise the Flock and Predator
        public void initialstate()
        {
            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {
                System.Threading.Thread.Sleep(50);
                int seed = (int)DateTime.Now.Ticks;
                Random r = new Random(seed);
                Flock[i].position.Xvalue = r.Next(Minx, Maxx);
                Flock[i].position.Yvalue = r.Next(Miny, Maxy);
            }
            Random rand = new Random();
            Predator.position.Xvalue = rand.Next(Minx, Maxx);
            Predator.position.Yvalue = rand.Next(Miny, Maxy);
        }

        //passing in the maximum and minimum values for the container form which the predator/prey reside in
        private int minx = 0;

        public int Minx
        {
            get { return minx; }
            set { minx = value; }
        }

        private int miny = 0;

        public int Miny
        {
            get { return miny; }
            set { miny = value; }
        }

        private int maxx = 0;

        public int Maxx
        {
            get { return maxx; }
            set { maxx = value; }
        }

        private int maxy = 0;

        public int Maxy
        {
            get { return maxy; }
            set { maxy = value; }
        }


        //method to implement rule 1 - BOIDs try to fly towards the centre of mass of the neighbouring BOIDs
        public void calcv1()
        {

            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {
                Vector sum = new Vector(0);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    sum = sum + Flock[j].position;
                }
                if (count == 1)
                {
                    sum.Xvalue = sum.Xvalue / (2 - 1);
                    sum.Yvalue = sum.Yvalue / (2 - 1);
                }
                else
                {
                    sum.Xvalue = sum.Xvalue / (count - 1);
                    sum.Yvalue = sum.Yvalue / (count - 1);
                }
                Flock[i].v1 = sum - Flock[i].position;
                Flock[i].v1.Xvalue = Flock[i].v1.Xvalue / 100;
                Flock[i].v1.Yvalue = Flock[i].v1.Yvalue / 100;
            }
        }

        //method to implement rule 2 - BOIDs try to keep a small distance away from other BOIDs
        public void calcv2()
        {

            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {

                Vector c = new Vector(0);
                for (int j = 0; j < count; j++)
                {
                    double distance = 0;
                    if (i == j)
                        continue;
                    distance = Math.Sqrt((Flock[j].position.Xvalue - Flock[i].position.Xvalue) * (Flock[j].position.Xvalue - Flock[i].position.Xvalue) + (Flock[j].position.Yvalue - Flock[i].position.Yvalue) * (Flock[j].position.Yvalue - Flock[i].position.Yvalue));
                    if (distance < 25)
                    {
                        c = c - (Flock[j].position - Flock[i].position);
                    }
                }
                Flock[i].v2.Xvalue = c.Xvalue;
                Flock[i].v2.Yvalue = c.Yvalue;
            }
        }


        //method to implement rule 3 - BOIDs try to match velocity with near BOIDs
        public void calcv3()
        {
            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {
                Vector sum = new Vector(0);
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                        continue;
                    sum = sum + Flock[j].velocity;
                }
                if (count == 1)
                {
                    sum.Xvalue = sum.Xvalue / (2 - 1);
                    sum.Yvalue = sum.Yvalue / (2 - 1);
                }
                else
                {
                    sum.Xvalue = sum.Xvalue / (count - 1);
                    sum.Yvalue = sum.Yvalue / (count - 1);
                }
                Flock[i].v3 = sum - Flock[i].velocity;
                Flock[i].v3.Xvalue = Flock[i].v3.Xvalue / 8;
                Flock[i].v3.Yvalue = Flock[i].v3.Yvalue / 8;
            }
        }

        //rule 4 - no longer used
        
        //rule 5 - moving away from the Predator
        public void calcv5()
        {
            int count = Flock.Count;
            double m = 10;
            Vector preylocation = Predator.position;
            for (int i = 0; i < count; i++)
            {
                Flock[i].v5 = preylocation - Flock[i].position;
                Flock[i].v5.Xvalue = -1 * Flock[i].v5.Xvalue / 100;
                Flock[i].v5.Yvalue = -1 * Flock[i].v5.Yvalue / 100;
            }
        }

        //bounding the position
        public void boundprey()
        {
            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {
                if (Flock[i].position.Xvalue < Minx)
                    Flock[i].position.Xvalue = Minx + 10;
                if (Flock[i].position.Xvalue > Maxx)
                    Flock[i].position.Xvalue = Maxx - 10;
                if (Flock[i].position.Yvalue < Miny)
                    Flock[i].position.Yvalue = Miny + 10;
                if (Flock[i].position.Yvalue > Maxy)
                    Flock[i].position.Yvalue = Maxy - 10;
            }
        }

        //method to limit the velocity so that the BOIDs don't move away from each other too quickly
        public Vector limitvelocity(Vector v,double lim)
        {
            double m = 0;
            Vector w = new Vector();
            m = v.magnitude(v);
            if (m > lim)
            {
                w.Xvalue = lim * (v.Xvalue/m);
                w.Yvalue = lim * (v.Yvalue/m);
            }
            else
            {
                w.Xvalue = 1 * v.Xvalue;
                w.Yvalue = 1 * v.Yvalue;
            }
            return w;
        }


        //move the Flock
        public void updateFlockPositions()
        {
            int count = Flock.Count;
            for (int i = 0; i < count; i++)
            {
                Flock[i].velocity = Flock[i].velocity + Flock[i].v1 + Flock[i].v2 + Flock[i].v3 + Flock[i].v5;
                Flock[i].velocity = limitvelocity(Flock[i].velocity,Vlim);
                Flock[i].position = Flock[i].position + Flock[i].velocity;
            }
            this.boundprey();
        }


        //method to find which BOID is close to the Predator, the Predator then moves in that direction
        public void predatorhunt()
        {
            int count = Flock.Count;
            double[] d = new double[count];
            for (int i = 0; i < count; i++)
            {
                d[i] = Math.Sqrt((Predator.position.Xvalue - Flock[i].position.Xvalue) * (Predator.position.Xvalue - Flock[i].position.Xvalue) + (Predator.position.Yvalue - Flock[i].position.Yvalue) * (Predator.position.Yvalue - Flock[i].position.Yvalue));
            }
            //Now we determine which BOID is closest to the Predator
            double min = 1000000;
            int counter = 0;
            for (int i = 0; i < count; i++)
            {
                if (d[i] < min)
                {
                    counter = i;
                    min = d[i];
                }
            }
            //we now know which BOID in the Flock is closest, vector v1 of the Predator will contain the move towards it
            Predator.v1.Xvalue = (Flock[counter].position.Xvalue - Predator.position.Xvalue)/20;
            Predator.v1.Yvalue = (Flock[counter].position.Yvalue - Predator.position.Yvalue)/20;
        }

        //trying to bound the predator
        public void predatorbound()
        {
            if (Predator.position.Xvalue < Minx)
                Predator.position.Xvalue = Minx + 10;
            if (Predator.position.Xvalue > Maxx)
                Predator.position.Xvalue = Maxx - 10;
            if (Predator.position.Yvalue < Miny)
                Predator.position.Yvalue = Miny + 10;
            if (Predator.position.Yvalue > Maxy)
                Predator.position.Yvalue = Maxy - 10;
        }

        //algorithm for finding the next position and velocity of the Predator
        public void updatePredator()
        {
            Predator.velocity = Predator.velocity + Predator.v1;
            Predator.velocity = limitvelocity(Predator.velocity,Predlim);
            Predator.position = Predator.position + Predator.velocity;
            this.predatorbound();
        }


    }


}
