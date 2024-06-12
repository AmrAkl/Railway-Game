using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    internal class Background
    {
        public int x, y;
        public Bitmap img;
        public Rectangle src, dst;
        public Background()
        {
            img = new Bitmap("background.PNG");
            src.X = 0; src.Y = 0;
            src.Width = img.Width;
            src.Height = img.Height;
        }
        public void Draw(Graphics g, int xdraw)
        {
            
                dst.Y = y;
                dst.X = x - xdraw;
                g.DrawImage(img, dst, src, GraphicsUnit.Pixel);
            
        }
    }
}
