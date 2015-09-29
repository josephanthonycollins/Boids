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
    
    class Vector
    {
        //x cordinate
        private double xvalue=0;

        public double Xvalue
        {
            get { return xvalue; }
            set { xvalue = value; }
        }

        //y cordinate
        private double yvalue = 0;

        public double Yvalue
        {
            get { return yvalue; }
            set { yvalue = value; }
        }

        //default constructor
        public Vector()
        {
            this.Xvalue = 0;
            this.Yvalue = 0;
        }

        //constructor taking a single value
        public Vector(double val)
        {
            this.Xvalue = val;
            this.Yvalue=val;
        }

        //constructor taking an x and y coordinate
        public Vector(double xval,double yval)
        {
            this.Xvalue = xval;
            this.Yvalue = yval;
        }

        //overloading + operator
        public static Vector operator +(Vector a, Vector b)
        {
             Vector tmp = new Vector();
             tmp.Xvalue = a.Xvalue + b.Xvalue;
             tmp.Yvalue = a.Yvalue + b.Yvalue;
             return tmp;
        }

        //overloading - operator
        public static Vector operator -(Vector a, Vector b)
        {
            Vector tmp = new Vector();
            tmp.Xvalue = a.Xvalue - b.Xvalue;
            tmp.Yvalue = a.Yvalue - b.Yvalue;
            return tmp;
        }

        //method to calculate the "Distance" between two vector objects
        public double Distance(Vector a, Vector b)
        {
            double d = 0;
            d = (a.Xvalue - b.Xvalue) * (a.Xvalue - b.Xvalue) + (a.Yvalue - b.Yvalue) * (a.Yvalue - b.Yvalue);
            d = Math.Sqrt(d);
            return d;
        }

        //method to calculate the magnitude of a vector object
        public double magnitude(Vector a)
        {
            double d = 0;
            d = a.Xvalue  * a.Xvalue + a.Yvalue * a.Yvalue;
            d = Math.Sqrt(d);
            return d;
        }

    }
}
