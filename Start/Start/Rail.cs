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
        public abstract void Draw(Graphics g);
        public abstract void Relocate(Point end);
    }
}
