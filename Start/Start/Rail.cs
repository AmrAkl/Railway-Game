using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    public abstract  class Rail
    {
        public PointF srt;
        public PointF end;
        public int flag = 0;
        public float theta;
        public float speed;
        public Rail() {
            srt = new Point();
            end = new Point();
        }
        public Rail(Point s,Point e)
        {
            srt = s;
            end = e;
        }
        public void SetPoints(Point s,Point e)
        {
            srt = s;
            end = e;
        }
        public abstract PointF calcNextPoint();
        public abstract void Draw(Graphics g, Color clr, int x);
        public abstract void Relocate(PointF end);
        public abstract void Resize(int sign);
    }
}
