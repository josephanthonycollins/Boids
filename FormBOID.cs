using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

//Name:Joseph Collins
//ID: 98718584
//Module: AM6008
//Assignment 2

namespace BOID2014
{
    public partial class FormBOID : Form
    {

        //coordinates of the container
        int minx = 0, maxx = 0, miny = 0, maxy = 0;

        bool swarmonly = true;

        //image objects
        Image preyicon;
        Image predatoricon;

        //variable to keep track of the number of iterations
        public int iteration = 0;

        //timer object, used to run the simulations
        Timer timer = new Timer();

        //model object, contains the BOIDs (i.e. Prey) and the Predator
        model m = new model(30);

        public FormBOID()
        {
            InitializeComponent();
            //what is the min and max coordinates of the client rectangle
            calcParameters();
            //creating the symbols for the Prey and the Predator
            preyicon = CreateIcon(Brushes.Blue);
            predatoricon = CreateIcon(Brushes.Red);
            timer.Tick += timer_Tick;
            timer.Interval = 50;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iteration == 0)
            {
                m.initialstate();
            }
            timer.Interval = 50;
            timer.Start();
            iteration += 1;

        }

        private void playSlowlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iteration == 0)
            {
                m.initialstate();
            }
            timer.Interval = 500;
            timer.Start();
            iteration += 1;
        }


        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FormBOID_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //draw the bounding rectangle
            g.DrawRectangle(Pens.CornflowerBlue, minx, miny, maxx - minx, maxy - miny);
            //draw the boids
            foreach (BOID b in m.Flock)
            {
                float angle;
                if (b.velocity.Xvalue == 0) angle = 90;
                else angle = (float)(Math.Atan(b.velocity.Yvalue / b.velocity.Xvalue) * 57.3);
                if (b.velocity.Xvalue < 0) angle += 180;
                PointF p = new Point((int)b.position.Xvalue, (int)b.position.Yvalue);
                Matrix matrix = new Matrix();
                matrix.RotateAt(angle, p);
                e.Graphics.Transform = matrix;
                e.Graphics.DrawImage(preyicon, p);
            }
            if (swarmonly == false)
            {
                float theta;
                if (m.Predator.velocity.Xvalue == 0) theta = 90;
                else theta = (float)(Math.Atan(m.Predator.velocity.Yvalue / m.Predator.velocity.Xvalue) * 57.3);
                if (m.Predator.velocity.Xvalue < 0) theta += 180;
                PointF pred = new Point((int)m.Predator.position.Xvalue, (int)m.Predator.position.Yvalue);
                Matrix mat = new Matrix();
                mat.RotateAt(theta, pred);
                e.Graphics.Transform = mat;
                e.Graphics.DrawImage(predatoricon, pred);
            }
        }


        void timer_Tick(object sender, EventArgs e)
        {
            //updating the BOIDs and Predator
            if (swarmonly == false)
            {
                m.calcv1();
                m.calcv2();
                m.calcv3();
                m.calcv5();
                m.predatorhunt();
                m.updateFlockPositions();
                m.updatePredator();
                this.Refresh();
            }
            //updating the BOIDs, no longer any need for vector v5
            if (swarmonly == true)
            {
                m.calcv1();
                m.calcv2();
                m.calcv3();
                foreach (BOID b in m.Flock)
                {
                    b.v5.Xvalue = 0;
                    b.v5.Yvalue = 0;
                }
                m.updateFlockPositions();
                this.Refresh();
            }

        
        }


        private void FormBOID_Resize(object sender, EventArgs e)
        {
            this.calcParameters();
            this.Invalidate(true);
            this.Update();
            //this.Refresh();
        }

        private void calcParameters()
        {
            minx = 20;
            maxx = minx + this.ClientRectangle.Width - 40;
            miny = MainMenuStrip.Height + 20;
            maxy = miny + this.ClientRectangle.Height - 40 - MainMenuStrip.Height;
            //passing the parameters to the model object
            m.Maxx = maxx;
            m.Minx = minx;
            m.Maxy = maxy;
            m.Miny = miny;
        }

        //code to create the icon
        private static Image CreateIcon(Brush brush)
        {
            Bitmap icon = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(icon);
            Point p1 = new Point(0, 16);
            Point p2 = new Point(16, 8);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(5, 8);
            Point[] points = { p1, p2, p3, p4 };
            g.FillPolygon(brush, points);
            return icon;
        }

        private void flockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swarmonly = true;
        }

        private void flockAndPredatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swarmonly = false;
        }

    }


}

