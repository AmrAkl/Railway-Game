using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Start
{
    public class Actor
    {
        public PointF coor = new PointF();
        public int width, height;
    }
    public partial class Form1 : Form
    {
        Bitmap off;
        Timer T = new Timer();
        List<Rail> cars = new List<Rail>();
        Actor actor;
        int selectedShape = 0, lastShapeX, curveDirection = 1;

        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
            this.KeyDown += Form1_KeyDown;
            T.Start();
            T.Tick += new EventHandler(T_Tick);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    selectedShape = 0;
                    break;
                case Keys.D2:
                    selectedShape = 1;
                    break;
                case Keys.D3:
                    selectedShape = 2;
                    break;
                case Keys.Right:
                    MoveRight();
                    break;
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Up:
                    ChangeLength(1);
                    break;
                case Keys.Down:
                    ChangeLength(-1);
                    break;
                case Keys.U:
                    curveDirection = -1;
                    break;
                case Keys.D:
                    curveDirection = -1;
                    break;
            }

            if (e.KeyCode == Keys.Space)
            {
                switch (selectedShape)
                {
                    case 0:
                        AddLine();
                        break;
                    case 1:
                        AddCircle();
                        break;
                    case 2:
                        AddCurve();
                        break;
                }
            }
            //DrawDoubleBuffer(this.CreateGraphics());
        }

        void ChangeLength(int sign)
        {
            switch (selectedShape)
            {
                case 0:
                    ChangeLineLength(sign);
                    break;
                case 1:
                    ChangeCircleRadius(sign);
                    break;
                case 2:
                    ChangeCurveHeight(sign);
                    break;
            }
            //DrawDoubleBuffer(this.CreateGraphics());
        }

        void MoveRight()
        {
            PointF p = actor.coor;
            p.X += 10;
            actor.coor = p;
        }

        void MoveLeft()
        {
            PointF p = actor.coor;
            p.X -= 10;
            actor.coor = p;
        }

        void AddLine()
        {
            DDA line = new DDA();
            PointF start = new PointF();
            start.X = lastShapeX;
            start.Y = actor.coor.Y + actor.height;
            line.srt = start;
            PointF end = new PointF();
            end.X = lastShapeX + actor.width + 100;
            end.Y = actor.coor.Y + actor.height;
            lastShapeX += 100 + actor.width;
            line.end = end;
            line.calc();
            cars.Add(line);
        }

        void AddCircle()
        {
            Circle circle = new Circle();
            circle.Rad = 80;
            circle.XC = (int)lastShapeX + circle.Rad - 60;
            circle.YC = (int)actor.coor.Y - circle.Rad / 2 - 20;
            lastShapeX += circle.Rad - 40;
            cars.Add(circle);
        }

        void AddCurve()
        {
            BezierCurve curve = new BezierCurve();
            Point p = new Point(lastShapeX, (int)actor.coor.Y + actor.height);
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 50, (int)actor.coor.Y - (300 * curveDirection));
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 120, (int)actor.coor.Y + actor.height);
            curve.ControlPoints.Add(p);
            lastShapeX += 120;
            cars.Add(curve);
        }

        void ChangeLineLength(int sign)
        {
            DDA line = cars.OfType<DDA>().LastOrDefault();
            int lastIndex = cars.LastIndexOf(line);
            PointF p = line.end;
            p.X += 5 * sign;
            line.end = p;
            cars[lastIndex] = line;
        }

        void ChangeCircleRadius(int sign)
        {
            Circle circle = cars.OfType<Circle>().LastOrDefault();
            int lastIndex = cars.LastIndexOf(circle);
            circle.Rad += 5 * sign;
            cars[lastIndex] = circle;
        }

        void ChangeCurveHeight(int sign)
        {
            BezierCurve curve = cars.OfType<BezierCurve>().LastOrDefault();
            int lastIndex = cars.LastIndexOf(curve);
            Point p = curve.ControlPoints[1];
            if (p.Y < actor.coor.Y) //Handle up curve
            {
                p.Y -= 5 * sign;
            }
            else
            {
                p.Y -= 5 * sign;
            }
            curve.ControlPoints[1] = p;
            cars[lastIndex] = curve;
        }

        void T_Tick(object sender, EventArgs e)
        {
            DrawDoubleBuffer(this.CreateGraphics());
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            actor = new Actor();
            actor.coor.X = 20;
            actor.coor.Y = this.ClientSize.Height / 2;
            actor.width = 20;
            actor.height = 20;
            lastShapeX = actor.width + 5;
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDoubleBuffer(e.Graphics);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            g.DrawRectangle(Pens.Black, new Rectangle((int)actor.coor.X, (int)actor.coor.Y, actor.width, actor.height));
            foreach (var car in cars)
            {
                car.Draw(g);
            }
        }

        void DrawDoubleBuffer(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}