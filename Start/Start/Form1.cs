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
        enum Modes { Edit, Move };
        Modes curr = Modes.Edit;
        Rail currrail = null;
        List<Background> bgs = new List<Background>();
        int xdraw = 0;


        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
            this.KeyDown += Form1_KeyDown;
            T.Interval = 100;
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
                case Keys.D4:
                    
                    break;
                case Keys.D5:
                    
                    break;
                case Keys.Right:
                    {
                        int index = cars.IndexOf(currrail);
                        if (index + 1 < cars.Count)
                            index++;
                        currrail = cars[index];
                    }
                    break;
                case Keys.Left:
                    {
                        int index = cars.IndexOf(currrail);
                        if (index  > 0)
                            index--;
                        currrail = cars[index];
                    }
                    break;
                case Keys.Up:
                    currrail.Resize(1);
                    break;
                case Keys.Down:
                    currrail.Resize(-1);
                    break;
                case Keys.D:
                    MoveRight();
                    break;
                case Keys.A:
                    MoveLeft();
                    break;
                case Keys.U:
                    curveDirection = -1;
                    break;
                case Keys.I:
                    curveDirection = -1;
                    break;
                case Keys.Space:
                    {
                        lastShapeX = (int)currrail.end.X;
                        int index = cars.IndexOf(currrail);
                        if (selectedShape == 1)
                            index++;
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
                        if (index + 1 < cars.Count)
                            index++;
                        currrail = cars[index];
                        if (selectedShape != 1)
                            currrail = cars[cars.Count - 1];
                        else
                            currrail = cars[cars.Count - 2];
                    }
                    break; 
                    
            }
            for(int i = 1;i<cars.Count;i++)
            {
                cars[i].Relocate(cars[i-1].end);
            }
            //if (e.KeyCode == Keys.Space)
            //{
            //    switch (selectedShape)
            //    {
            //        case 0:
            //            AddLine();
            //            break;
            //        case 1:
            //            AddCircle();
            //            break;
            //        case 2:
            //            AddCurve();
            //            break;
            //    }
            //}
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
            AddLine();
            Circle circle = new Circle();
            circle.Rad = 80;
            circle.XC = (int)lastShapeX + circle.Rad - 60;
            circle.YC = (int)actor.coor.Y - circle.Rad / 2 - 40;
            lastShapeX += circle.Rad - 40;
            circle.srt = circle.end = new PointF(lastShapeX,actor.coor.Y);
            cars.Add(circle);
            AddLine();
        }

        void AddCurve()
        {
            BezierCurve curve = new BezierCurve();
            Point p = new Point(lastShapeX, (int)actor.coor.Y + actor.height +1);
            curve.srt = new Point(lastShapeX, (int)actor.coor.Y + actor.height); ;
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 150, (int)actor.coor.Y + actor.height );
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 200, (int)actor.coor.Y - (300 * curveDirection));
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 250, (int)actor.coor.Y - (300 * curveDirection));
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 300, (int)actor.coor.Y + actor.height );
            curve.ControlPoints.Add(p);
            p = new Point(lastShapeX + 450, (int)actor.coor.Y + actor.height +1);
            curve.end = new Point(lastShapeX + 450, (int)actor.coor.Y + actor.height);
            curve.ControlPoints.Add(p);
            lastShapeX += 120;
            cars.Add(curve);
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
            Point p = curve.ControlPoints[2];
            if (p.Y < actor.coor.Y) //Handle up curve
            {
                p.Y -= 5 * sign;
            }
            else
            {
                p.Y -= 5 * sign;
            }
            curve.ControlPoints[2] = p;
            p = curve.ControlPoints[3];
            if (p.Y < actor.coor.Y) //Handle up curve
            {
                p.Y -= 5 * sign;
            }
            else
            {
                p.Y -= 5 * sign;
            }
            curve.ControlPoints[3] = p;
            cars[lastIndex] = curve;
        }

        void T_Tick(object sender, EventArgs e)
        {
            switch (curr)
            {
                case Modes.Edit:
                    if(xdraw + 500 < currrail.srt.X)
                    {
                        //MoveBackground(5);
                        xdraw += 10;
                        if (xdraw + 200 > currrail.srt.X)
                            xdraw = (int)currrail.srt.X - 500;
                    }
                    if(xdraw > currrail.srt.X - 500)
                    {
                        //MoveBackground(-5);
                        xdraw -= 10;
                        if (xdraw < currrail.srt.X - 500)
                            xdraw = (int)currrail.srt.X - 500;
                    }
                    break;
                case Modes.Move:
                    break;
            }
            DrawDoubleBuffer(this.CreateGraphics());
        }

        void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            actor = new Actor();
            actor.coor.X = 500;
            actor.coor.Y = this.ClientSize.Height / 2;
            actor.width = 20;
            actor.height = 20;
            lastShapeX = 500;
            AddLine();
            currrail = cars[0];
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDoubleBuffer(e.Graphics);
            CreateBackgrounds();
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            Drawbackground(g);
            g.DrawRectangle(Pens.Black, new Rectangle((int)actor.coor.X - xdraw, (int)actor.coor.Y, actor.width, actor.height));
            foreach (var car in cars)
            {

                car.Draw(g, Color.Black,xdraw);
            }
            currrail.Draw(g, Color.Blue, xdraw);
        }

        void CreateBackgrounds()
        {
            int x = 0;
            for (int i = 0; i < 10; i++)
            {
                Background pnn = new Background();
                pnn.x = x;
                pnn.y = 0;
                pnn.dst.Width = Width;
                pnn.dst.Height = Height;
                bgs.Add(pnn);
                x += Width - 4;
            }
        }
        void MoveBackground(int x)
        {
            for (int i = 0; i < bgs.Count; i++)
            {
                bgs[i].x += x;
            }
        }
        void Drawbackground(Graphics g)
        {
            for (int i = 0; i < bgs.Count; i++)
            {
                bgs[i].Draw(g, xdraw);
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