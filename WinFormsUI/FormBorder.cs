using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsUI
{
    public partial class FormBorder : Form
    {
        #region Переменные

        readonly private string filesWay = @"../../../Resources";

        bool MousePressed = false;
        bool IsFull;
        readonly bool IsMain;

        public String text;
        public Image icon;

        Point ClickPosition, MouseStartPosition;
        Rectangle rectExit, rectWindow, rectHide;

        bool ExitFocus, WindowFocus, HideFocus;

        public delegate void ChangeScreen();
        public event ChangeScreen EventScreen;

        #endregion

        #region Конструкторы и События изменения стиля и размера

        public FormBorder()
        {
            IsMain = true;
            IsFull = false;

            paint = new ColorsAndSize();

            InitializeComponentMain();

            BorderSettings(this);

            EventScreen += ChangeBorderSize;
            EventScreen += menu.ChangeMenuSize;
            menu.EventStyle += paint.ChangeColors;
            menu.EventStyle += ChangeBorderStyle;

            ChangeBorderStyle(paint.StyleNow);
        }

        public FormBorder (ColorsAndSize paint)
        {
            Name = "Help";
            IsMain = false;

            this.paint = paint;

            InitializeComponentHelp();

            BorderSettings(this);

            ChangeBorderStyle(paint.StyleNow);
        }

        private void BorderSettings(FormBorder Border)
        {
            DoubleBuffered = true;

            Border.FormBorderStyle = FormBorderStyle.None;

            Border.MouseDown += Form_BorderMouseDown;
            Border.MouseUp += Form_BorderMouseUp;
            Border.MouseMove += Form_BorderMouseMove;
            Border.Paint += Form_BorderPaint;
        }

        public void ChangeBorderStyle(ColorsAndSize.Style style)
        {
            switch (style)
            {
                case ColorsAndSize.Style.Default:
                    if (IsMain)
                        text = "Arithmetic";
                    else
                        text = "Help";
                    icon = Image.FromFile(filesWay + "/Normal.ico");

                    Icon = new Icon(filesWay + "/Normal.ico");
                    break;
                case ColorsAndSize.Style.Neon:
                    if (IsMain)
                        text = "The coolest Arithmetic";
                    else
                        text = "The coolest Help";
                    icon = Image.FromFile(filesWay + "/Neon.ico");

                    Icon = new Icon(filesWay + "/Neon.ico");
                    break;
                case ColorsAndSize.Style.Cat:
                    if (IsMain)
                        text = "CatArithmetic....Murrr";
                    else
                        text = "CatHelp....Meow";
                    icon = Image.FromFile(filesWay + "/Cat.ico");

                    Icon = new Icon(filesWay + "/Cat.ico");
                    break;
            }

            Text = text;
        }

        private void ChangeBorderSize()
        {
            if (IsFull)
            {
                WindowState = FormWindowState.Normal;
                IsFull = false;

                Width = 1080;
                Height = 720;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                IsFull = true;

                Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                Height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            }

            Invalidate();
        }

        #endregion

        #region События Рисования графики, Нажатия, Наведения и Перемещения

        private void Form_BorderPaint(object sender, PaintEventArgs e)
        {
            DrawStyle(e.Graphics);
        }

        protected void Form_BorderMouseDown(object sender, MouseEventArgs e)
        {
            MouseStartPosition = Location;
            ClickPosition = Cursor.Position;

            MousePressed = true;
        }

        private void Form_BorderMouseUp(object sender, MouseEventArgs e)
        {
            MousePressed = false;

            if (e.Button == MouseButtons.Left)
            {
                if (rectExit.Contains(e.Location))
                {
                    Close();
                }
                else if (rectHide.Contains(e.Location))
                {
                    HideFocus = false;

                    WindowState = FormWindowState.Minimized;
                }
                else if (IsMain && rectWindow.Contains(e.Location))
                {
                    WindowFocus = false;

                    EventScreen();
                }
            }
        }

        private void Form_BorderMouseMove(object sender, MouseEventArgs e)
        {
            if (MousePressed)
            {
                Size LocationChange = new Size(Point.Subtract(Cursor.Position, new Size(ClickPosition)));

                Location = Point.Add(MouseStartPosition, LocationChange);
            }
            else
            {
                if (rectExit.Contains(e.Location))
                {
                    if (!ExitFocus)
                    {
                        ExitFocus = true;
                        Invalidate();
                    }
                }
                else
                {
                    if (ExitFocus)
                    {
                        ExitFocus = false;
                        Invalidate();
                    }
                }

                if (IsMain && rectWindow.Contains(e.Location))
                {
                    if (!WindowFocus)
                    {
                        WindowFocus = true;
                        Invalidate();
                    }
                }
                else
                {
                    if (WindowFocus)
                    {
                        WindowFocus = false;
                        Invalidate();
                    }
                }

                if (rectHide.Contains(e.Location))
                {
                    if (!HideFocus)
                    {
                        HideFocus = true;
                        Invalidate();
                    }
                }
                else
                {
                    if (HideFocus)
                    {
                        HideFocus = false;
                        Invalidate();
                    }
                }
            }
        }

        #endregion

        #region Рисование графики

        private void DrawStyle(Graphics graph)
        {
            graph.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle rectHeader = new Rectangle(0, 0, Width - 1, 29);
            Rectangle rectLine = new Rectangle(0, 29, Width - 1, Height - 30);
            Rectangle rectText = new Rectangle(30, 0, Width - 50, 29);
            Rectangle rectIcon = new Rectangle(3, 2, 23, 22);
            rectExit = new Rectangle(rectHeader.Width - rectHeader.Height, rectHeader.Y, rectHeader.Height - 1, rectHeader.Height - 1);
            Rectangle rectExitPict = new Rectangle(rectExit.X + rectExit.Width / 2 - 5, rectExit.Height / 2 - 5, 10, 10);

            graph.DrawRectangle(new Pen(paint.Line), rectHeader);
            graph.FillRectangle(new SolidBrush(paint.Header), rectHeader);
            graph.DrawRectangle(new Pen(paint.Line), rectLine);
            graph.DrawImage(icon, rectIcon);
            graph.DrawString(text, paint.font, new SolidBrush(paint.HeaderText), rectText, paint.SF);

            graph.DrawRectangle(new Pen(paint.Line), rectExit);
            graph.FillRectangle(new SolidBrush(ExitFocus ? paint.Button : paint.Header), rectExit);
            DrawCross(graph, rectExitPict, new Pen(paint.LineButton) { Width = 1.85F });

            if (IsMain)
            {
                rectWindow = new Rectangle(rectHeader.Width - rectHeader.Height - 30, rectHeader.Y, rectHeader.Height - 1, rectHeader.Height - 1);
                Rectangle rectWindowPict = new Rectangle(rectHeader.Width - rectHeader.Height - 21, rectHeader.Y + 8, rectHeader.Height - 18, rectHeader.Height - 18);
                rectHide = new Rectangle(rectHeader.Width - rectHeader.Height - 60, rectHeader.Y, rectHeader.Height - 1, rectHeader.Height - 1);

                graph.DrawRectangle(new Pen(paint.Line), rectWindow);
                graph.FillRectangle(new SolidBrush(WindowFocus ? paint.Button : paint.Header), rectWindow);

                if (IsFull)
                    DrawDoubleWindow(graph, rectWindowPict, new Pen(paint.LineButton) { Width = 1.55F });
                else
                    DrawWindow(graph, rectWindowPict, new Pen(paint.LineButton) { Width = 1.55F });
            }
            else
            {
                rectHide = new Rectangle(rectHeader.Width - rectHeader.Height - 30, rectHeader.Y, rectHeader.Height - 1, rectHeader.Height - 1);
            }

            graph.DrawRectangle(new Pen(paint.Line), rectHide);
            graph.FillRectangle(new SolidBrush(HideFocus ? paint.Button : paint.Header), rectHide);
            DrawHide(graph, rectHide, new Pen(paint.LineButton) { Width = 1.55F });
        }

        private void DrawCross(Graphics graph, Rectangle rect, Pen pen)
        {
            graph.DrawLine(pen, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            graph.DrawLine(pen, rect.X + rect.Width, rect.Y, rect.X, rect.Y + rect.Height);
        }

        private void DrawWindow(Graphics graph, Rectangle rect, Pen pen)
        {
            graph.DrawRectangle(pen, rect);
        }

        private void DrawDoubleWindow(Graphics graph, Rectangle rect, Pen pen)
        {
            Rectangle rectHelper = rect;

            rectHelper.X += 2;
            rectHelper.Y -= 2;
            rect.X -= 1;
            rect.Y += 1;

            graph.DrawRectangle(pen, rect);

            graph.DrawRectangle(pen, rectHelper);
        }

        private void DrawHide(Graphics graph, Rectangle rect, Pen pen)
        {
            graph.DrawLine(pen, rect.X + 5, rect.Y + rect.Height - 10, rect.X + rect.Width - 5, rect.Y + rect.Height - 10);
        }

        #endregion
    }
}
