using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    internal class DDA : Rail
    {
        float dy, dx, m;
        public float cx, cy;
        int speed = 10;
        public void calc()
        {
            dy = end.Y - srt.Y;
            dx = end.X - srt.X;
            m = dy / dx;
            cx = srt.X;
            cy = srt.Y;
        }
        
        public override PointF calcNextPoint()
        {
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (srt.X < end.X)
                {
                    cx += speed;
                    cy += m * speed;
                    if (cx >= end.X)
                    {
                        return new Point((int)end.X + 1, (int)end.Y-20);
                    }

                }
                else
                {
                    cx -= speed;
                    cy -= m * speed;
                    if (cx <= end.X)
                    {
                        return new Point((int)end.X + 1, (int)end.Y-20);
                    }
                }
            }
            else
            {
                if (srt.Y < end.Y)
                {
                    cy += speed;
                    cx += 1 / m * speed;
                    if (cy >= end.Y)
                    {
                        return new Point((int)end.X + 1, (int)end.Y - 20);
                    }
                }
                else
                {
                    cy -= speed;
                    cx -= 1 / m * speed;
                    if (cy <= end.Y)
                    {
                        return new Point( (int)end.X + 1, (int)end.Y-20);
                    }
                }

            }
            return new PointF(cx,cy-20);
        }
        public override void Draw(Graphics g, Color c, int x)
        {
            Pen pen = new Pen(new SolidBrush(c));
            pen.Width = 5;
            g.DrawLine(pen, srt.X - x,srt.Y, end.X - x,end.Y);
        }
        public override void Relocate(PointF e)
        {
            float deltx = e.X - srt.X;
            float deltY = e.Y - srt.Y;
            srt.X += (int)deltx;
            srt.Y += (int)deltY;
            end.X += (int)deltx; 
            end.Y += (int)deltY;
            calc();
        }
        public override void Resize(int sign)
        {
            end.X += 5 * sign;
        }
    }
}
