﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    public class Circle
    {
        public int Rad;
        public int XC;
        public int YC;
        public float thRadian;
        public void Drawcircle(Graphics g)
        {
            for (float i = 0; i <= (2 * Math.PI); i += (float)((2 * Math.PI) / 360))
            {
                //(float)(i * Math.PI / 180);

                float x = (float)(Rad * Math.Cos(i));
                float y = (float)(Rad * Math.Sin(i));

                x += XC;
                y += YC;

                g.FillEllipse(Brushes.Black, x, y, 5, 5);
            }
        }
        public PointF Getnextpoint(int theta)
        {

            PointF p = new PointF();

            thRadian = (float)(theta * Math.PI / 180);

            p.X = (float)(Rad * Math.Cos(thRadian)) + XC;
            p.Y = (float)(Rad * Math.Sin(thRadian)) + YC;
            return p;
        }
    }
}