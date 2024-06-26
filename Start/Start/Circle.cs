﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    internal class Circle : Rail
    {
        public int Rad;
        public int XC;
        public int YC;
        public float thRadian;
        public Circle()
        {
            speed = 0.1f;
        }
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
        public override PointF calcNextPoint()
        {
            PointF p = new PointF();

            thRadian = (float)(theta * Math.PI / 180);

            p.X = (float)(Rad * Math.Cos(thRadian)) + XC;
            p.Y = (float)(Rad * Math.Sin(thRadian)) + YC;
            theta -= (100 * speed );
            return p;
        }
        public override void Draw(Graphics g, Color c, int xs)
        {
            for (float i = 0; i <= (2 * Math.PI); i += (float)((2 * Math.PI) / 720))
            {
                //(float)(i * Math.PI / 180);

                float x = (float)(Rad * Math.Cos(i));
                float y = (float)(Rad * Math.Sin(i));

                x += XC;
                y += YC;

                g.FillEllipse(new SolidBrush(c), x - xs, y, 5, 5);
            }
        }
        public override void Relocate(PointF e)
        {
            float deltX = e.X - srt.X;
            float deltY = e.Y - srt.Y;
            XC += (int)deltX;
            YC += (int)deltY;
            srt.X += deltX;
            srt.Y += deltY;
            end.X += deltX;
            end.Y += deltY;
        }
        public override void Resize(int sign)
        {
            Rad += 5 * sign;
            YC -= 5 * sign;
        }
    }
}
