using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AIO.Modules.Keno
{
    public class stratGrid : Control
    {
        private Rectangle perimeterRect;

        private Rectangle[] squareRects;
        public int[] squareData { get; set; }

        private Brush idleColor = Brushes.LightGray;
        private Brush SetecledColor = Brushes.MediumPurple;
        private Brush WinColor = Brushes.LimeGreen;
        private Brush UnhitColor = Brushes.MistyRose;
        private int _squareSpacing;

        public bool selectAllowed = true;
        public int SquareSpacing
        {
            get { return _squareSpacing; }
            set
            {
                _squareSpacing = Math.Abs(value);
                Invalidate();
            }
        }

        public stratGrid()
        {

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            Size = new Size(100, 100);
            SquareSpacing = 6;

            Reset();

        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            e.Graphics.Clear(Parent.BackColor);

            int index = 0;

            int squareWidth = (212 - (SquareSpacing * 6)) / 5;
            int start = (212 - (squareWidth * 5) - (SquareSpacing * 4)) / 2;

            for (int col = 0; col < 5; col++)
            {
                int y = start + (squareWidth + SquareSpacing) * col;
                for (int row = 0; row < 8; row++)
                {

                    int x = start + (squareWidth + SquareSpacing) * row;
                    squareRects[index] = new Rectangle(x, y, squareWidth, squareWidth);

                    if (squareData[index] == 1)
                    {
                        e.Graphics.FillRectangle(SetecledColor, squareRects[index]);
                        e.Graphics.DrawString(" " + (index + 1).ToString(), new Font("Segoe UI", 11), Brushes.Black, squareRects[index]);
                    }
                    else if (squareData[index] == 0)
                    {
                        e.Graphics.FillRectangle(idleColor, squareRects[index]);
                        e.Graphics.DrawString(" " + (index + 1).ToString(), new Font("Segoe UI", 11), Brushes.Black, squareRects[index]);
                    }
                    else if (squareData[index] == 2)
                    {
                        e.Graphics.FillRectangle(UnhitColor, squareRects[index]);
                        e.Graphics.DrawString(" " + (index + 1).ToString(), new Font("Segoe UI", 11), Brushes.Red, squareRects[index]);
                    }
                    else if (squareData[index] == 3)
                    {
                        e.Graphics.FillRectangle(WinColor, squareRects[index]);
                        e.Graphics.DrawString(" " + (index + 1).ToString(), new Font("Segoe UI", 11), Brushes.Black, squareRects[index]);
                    }

                    index++;

                }
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {

            base.OnSizeChanged(e);

            if (Width > Height)
            {
                Height = Width;
            }
            else if (Height > Width)
            {
                Width = Height;
            }

            perimeterRect = new Rectangle(0, 0, Width - 1, Height - 1);

        }

        public void Reset()
        {

            squareRects = new Rectangle[40];
            squareData = new int[40];

            for (int i = 0; i < squareData.Length; i++) squareData[i] = 0;

            Invalidate();

        }

        public void Clear(List<int> StratergyArray)
        {

            squareRects = new Rectangle[40];
            squareData = new int[40];

            for (int i = 0; i < squareData.Length; i++)
            {
                squareData[i] = 0;
            }

            foreach (var s in StratergyArray)
            {
                squareData[s] = 1;
            }

            Invalidate();

        }

        public void SetValue(int index, int c)
        {
            if (c == 1)
            {
                if (squareData.Count(x => x == 1) < 10)
                {
                    squareData[index] = c;
                    Invalidate();
                }
            }
            else
            {
                squareData[index] = c;
                Invalidate();
            }


        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            for (int i = 0; i < squareRects.Length; i++)
            {
                if (squareRects[i].Contains(e.Location))
                {
                    if (selectAllowed == true)
                    {
                        if (squareData[i] == 0)
                        {
                            SetValue(i, 1);
                            KenoUI.initForm.button1.Enabled = true;
                            KenoUI.initForm.button2.Enabled = true;
                        }
                        else
                        {
                            SetValue(i, 0);
                        }
                        KenoUI.initForm.StratergyArray.Clear();
                        KenoUI.initForm.AppendMultipliers(squareData);
                    }
                }
            }
        }
    }


}
