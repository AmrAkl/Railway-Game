using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    internal class DDA
    {
        public float X, Y;
        float dy, dx, m;
        public float cx, cy;
        int speed = 10;
        public void calc(DDA End)
        {
            dy = End.Y - Y;
            dx = End.X - X;
            m = dy / dx;
            cx = X;
            cy = Y;
        }
        public bool CalcNextPoint(DDA End)
        {
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (X < End.X)
                {
                    cx += speed;
                    cy += m * speed;
                    if (cx >= End.X)
                    {
                        return false;
                    }

                }
                else
                {
                    cx -= speed;
                    cy -= m * speed;
                    if (cx <= End.X)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (Y < End.Y)
                {
                    cy += speed;
                    cx += 1 / m * speed;
                    if (cy >= End.Y)
                    {
                        return false;
                    }
                }
                else
                {
                    cy -= speed;
                    cx -= 1 / m * speed;
                    if (cy <= End.Y)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

    }
}
