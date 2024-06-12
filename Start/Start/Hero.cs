using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start
{
    internal class Hero
    {
        public float x, y;
        public float dt;
        public float theta;
        public Hero()
        {
            x = y = dt = theta = 0;
        }
        public void reset(float xx,float yy)
        {
            dt = theta = 0;
            x = xx;
            y = yy;
        }
    }
}
