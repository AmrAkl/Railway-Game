using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start
{
    public partial class Form1 : Form
    {
        Bitmap off;
        Timer T = new Timer();
        //push test
        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
            T.Start();
            T.Tick += new EventHandler(T_Tick);
            this.WindowState = FormWindowState.Maximized;
        }

        void T_Tick(object sender, EventArgs e)
        {
            DrawDubb(this.CreateGraphics());
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}

