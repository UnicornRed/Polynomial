using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using PSharpCompiler;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WinFormsUI
{
    public partial class Menu : Control
    {
        #region Переменные

        FormBorder Helper;

        int PageNow = 0, PageCount = 0;
        const int PageMax = 5;

        Rectangle rectPlus;
        readonly Rectangle[] rectPages = new Rectangle[PageMax];
        readonly Rectangle[] rectClose = new Rectangle[PageMax];

        bool PlusFocus;
        readonly bool[] PageFocus = new bool[PageMax];
        readonly bool[] CloseFocus = new bool[PageMax];

        bool IsSoundOn;
        bool IsFull;

        readonly double widthRatio, heightRatio;

        readonly string[] compilerStr = new string[PageMax];
        readonly string[] outputBoxStr = new string[PageMax];
        readonly string[] timeManagerStr = new string[PageMax];
        readonly int[] imageNow = new int[PageMax];
        readonly string[] numeratorStr = new string[PageMax];
        readonly int[] numNow = new int[PageMax];

        readonly int[] countN = new int[PageMax];

        public string filesWay = @"../../../Resources";
        Font fontMenu;
        SoundPlayer Sound;

        public ColorsAndSize paint;

        public delegate void ChangeStyle(ColorsAndSize.Style style);
        public event ChangeStyle EventStyle;

        string[] ListOfCommands1 = { "if", "while", "goto", "write", "pol", "matrix", "to", "coef", "Rand" };
        string[] ListOfCommands2 = { "EvalP", "Diff", "Rand", "det", "ChPol", "EvalM" };
        string[] CheckList = { @"\s$|\($|\)$|{$|}$|\n$| $|;$|,$|^$", @"^\s|^\(|^\)|^{|^}|^\n|^ |^,|^;|^$" };

        Color Commands1, Commands2;

        #endregion

        #region Конструктор и События изменения стиля и размера

        public Menu(ColorsAndSize paint)
        {
            InitializeComponent();

            IsFull = false;
            IsSoundOn = true;

            DoubleBuffered = true;

            EventStyle += ChangeStyleMenu;

            this.paint = paint;       

            for (int i = 0; i < PageMax; i++)
            {
                PageFocus[i] = false;
                CloseFocus[i] = false;
                imageNow[i] = 1;
                numNow[i] = 0;
            }

            Paint += Form_MenuPaint;
            MouseUp += Form_MenuMouseUp;
            MouseMove += Form_MenuMouseMove;

            widthRatio = (double)System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width / 1080;
            heightRatio = (double)System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height / 720;

            ChangeStyleMenu(paint.StyleNow);

            SoundPlay(1);
        }

        private void ChangeStyleMenu(ColorsAndSize.Style style)
        {
            switch (style)
            {
                case (ColorsAndSize.Style.Default):
                    BackColor = Color.LightGray;

                    menuStripForButtons.BackColor = Color.White;
                    menuStripForButtons.ForeColor = Color.Black;

                    toolStripStyles.BackColor = Color.White;

                    toolStripDefaultStyle.BackColor = Color.PapayaWhip;
                    toolStripDefaultStyle.ForeColor = Color.Black;

                    toolStripNeonStyle.BackColor = Color.White;
                    toolStripNeonStyle.ForeColor = Color.Black;

                    toolStripCatStyle.BackColor = Color.White;
                    toolStripCatStyle.ForeColor = Color.Black;

                    toolStripComp.BackColor = Color.White;

                    toolStripTxt.BackColor = Color.White;

                    toolStripTxtRead.BackColor = Color.White;
                    toolStripTxtRead.ForeColor = Color.Black;

                    toolStripTxtWrite.BackColor = Color.White;
                    toolStripTxtWrite.ForeColor = Color.Black;

                    toolStripHelp.BackColor = Color.White;
                    toolStripSound.BackColor = Color.White;

                    compiler.BackColor = Color.White;
                    compiler.ForeColor = Color.Black;
                    numerator.BackColor = Color.White;
                    numerator.ForeColor = Color.Black;
                    outputBox.BackColor = Color.White;
                    outputBox.ForeColor = Color.Black;
                    timeManager.BackColor = Color.White;
                    timeManager.ForeColor = Color.Black;

                    Commands1 = Color.Blue;
                    Commands2 = Color.Lime;
                    break;
                case (ColorsAndSize.Style.Neon):
                    BackColor = Color.Black;

                    menuStripForButtons.BackColor = Color.Black;
                    menuStripForButtons.ForeColor = Color.MediumOrchid;

                    toolStripStyles.BackColor = Color.Black;

                    toolStripDefaultStyle.BackColor = Color.Black;
                    toolStripDefaultStyle.ForeColor = Color.MediumOrchid;

                    toolStripNeonStyle.BackColor = Color.Black;
                    toolStripNeonStyle.ForeColor = Color.Red;

                    toolStripCatStyle.BackColor = Color.Black;
                    toolStripCatStyle.ForeColor = Color.MediumOrchid;

                    toolStripComp.BackColor = Color.Black;

                    toolStripTxt.BackColor = Color.Black;

                    toolStripTxtRead.BackColor = Color.Black;
                    toolStripTxtRead.ForeColor = Color.MediumOrchid;

                    toolStripTxtWrite.BackColor = Color.Black;
                    toolStripTxtWrite.ForeColor = Color.MediumOrchid;

                    toolStripHelp.BackColor = Color.Black;
                    toolStripSound.BackColor = Color.Black;

                    compiler.BackColor = Color.Black;
                    compiler.ForeColor = Color.MediumOrchid;
                    numerator.BackColor = Color.Black;
                    numerator.ForeColor = Color.MediumOrchid;
                    outputBox.BackColor = Color.Black;
                    outputBox.ForeColor = Color.MediumOrchid;
                    timeManager.BackColor = Color.Black;
                    timeManager.ForeColor = Color.MediumOrchid;

                    Commands1 = Color.Red;
                    Commands2 = Color.Cyan;
                    break;
                    case(ColorsAndSize.Style.Cat):
                    BackColor = Color.LightSkyBlue;

                    menuStripForButtons.BackColor = Color.LavenderBlush;
                    menuStripForButtons.ForeColor = Color.Indigo;

                    toolStripStyles.BackColor = Color.LavenderBlush;

                    toolStripDefaultStyle.BackColor = Color.Azure;
                    toolStripDefaultStyle.ForeColor = Color.Indigo;

                    toolStripNeonStyle.BackColor = Color.Azure;
                    toolStripNeonStyle.ForeColor = Color.Indigo;

                    toolStripCatStyle.BackColor = Color.SkyBlue;
                    toolStripCatStyle.ForeColor = Color.Indigo;

                    toolStripComp.BackColor = Color.LavenderBlush;

                    toolStripTxt.BackColor = Color.LavenderBlush;

                    toolStripTxtRead.BackColor = Color.Azure;
                    toolStripTxtRead.ForeColor = Color.Indigo;

                    toolStripTxtWrite.BackColor = Color.Azure;
                    toolStripTxtWrite.ForeColor = Color.Indigo;

                    toolStripHelp.BackColor = Color.LavenderBlush;
                    toolStripSound.BackColor = Color.LavenderBlush;

                    compiler.BackColor = Color.MistyRose;
                    compiler.ForeColor = Color.Indigo;
                    numerator.BackColor = Color.MistyRose;
                    numerator.ForeColor = Color.Indigo;
                    outputBox.BackColor = Color.MistyRose;
                    outputBox.ForeColor = Color.Indigo;
                    timeManager.BackColor = Color.MistyRose;
                    timeManager.ForeColor = Color.Indigo;

                    Commands1 = Color.Crimson;
                    Commands2 = Color.RoyalBlue;
                    break;
            }
        }

        public void ChangeMenuSize()
        {
            double w, h;

            if (IsFull)
            {
                w = 1 / widthRatio;
                h = 1 / heightRatio;

                IsFull = false;
            }
            else
            {
                w = widthRatio;
                h = heightRatio;

                IsFull = true;
            }

            Size = new Size(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height - 30);

            compiler.Size = new Size((int)(compiler.Width * w), (int)(compiler.Height * h));
            if (IsFull)
                numerator.Size = new Size((int)(numerator.Width * w), (int)(numerator.Height * h));
            else
            {
                this.numerator.Size = new Size(40, 580);
            }
            outputBox.Size = new Size((int)(outputBox.Width * w), (int)(outputBox.Height * h));
            timeManager.Size = new Size((int)(timeManager.Width * w), (int)(timeManager.Height * h));
            image.Size = new Size((int)(image.Width * w), (int)(image.Height * h));

            compiler.Location = new Point((int)(compiler.Location.X * w), (int)(compiler.Location.Y * h));
            numerator.Location = new Point(compiler.Location.X - numerator.Width, (int)(numerator.Location.Y * h));
            outputBox.Location = new Point((int)(outputBox.Location.X * w), (int)(outputBox.Location.Y * h));
            timeManager.Location = new Point((int)(timeManager.Location.X * w), (int)(timeManager.Location.Y * h));
            image.Location = new Point((int)(image.Location.X * w), (int)(image.Location.Y * h));

            Invalidate();
        }

        #endregion

        #region События Наведения, Нажатия, Рисования графики и Синхронизации ползунков

        private void Form_MenuMouseMove(object sender, MouseEventArgs e)
        {
            if (rectPlus.Contains(e.Location))
            {
                if (!PlusFocus)
                {
                    PlusFocus = true;
                    Invalidate();
                }
            }
            else
            {
                if (PlusFocus)
                {
                    PlusFocus = false;
                    Invalidate();
                }
            }

            for (int i = 0; i < PageCount; i++)
            {
                if (rectPages[i].Contains(e.Location) && !rectClose[i].Contains(e.Location))
                {
                    if (!PageFocus[i])
                    {
                        PageFocus[i] = true;
                        Invalidate();
                    }
                }
                else
                {
                    if (PageFocus[i])
                    {
                        PageFocus[i] = false;
                        Invalidate();
                    }
                }

                if (rectClose[i].Contains(e.Location))
                {
                    if (!CloseFocus[i])
                    {
                        CloseFocus[i] = true;
                        Invalidate();
                    }
                }
                else
                {
                    if (CloseFocus[i])
                    {
                        CloseFocus[i] = false;
                        Invalidate();
                    }
                }
            }
        }

        private void Form_MenuMouseUp(object sender, MouseEventArgs e)
        {
            if (rectPlus.Contains(e.Location))
            {
                if (PageCount + 1 <= PageMax)
                {
                    if (PageNow != 0)
                        HidePage(PageNow - 1);

                    PageCount++;
                    PageNow = PageCount;

                    OpenPage(PageNow - 1);

                    Invalidate();
                }
            }
            else
            {
                for (int i = 0; i < PageCount; i++)
                {
                    if (rectClose[i].Contains(e.Location))
                    {
                        if (i == PageNow - 1)
                        {
                            PageNow = 0;
                            HidePage(i);
                        }
                        else if (PageNow - 1 > i)
                            PageNow--;

                        PageCount--;
                        ClosePage(i);

                        Invalidate();
                    }
                    else if (rectPages[i].Contains(e.Location))
                    {
                        if (PageNow - 1 == i)
                        {
                            PageNow = 0;

                            HidePage(i);
                        }
                        else
                        {
                            if (PageNow != 0)
                                HidePage(PageNow - 1);

                            PageNow = i + 1;
                            OpenPage(i);
                        }

                        Invalidate();
                    }
                }
            }
        }

        private void Form_MenuPaint(object sender, PaintEventArgs e)
        {
            DrawPage(e.Graphics);

            DrawStripMenuBorder(e.Graphics);

            if (PageNow != 0)
                DrawRichTextBoxBorder(e.Graphics);
        }

        private void RTF_Scroll(object sender, MessageEventArgs e)
        {
            if (!isScrolling)
            {
                isScrolling = true;

                ImprovedRichTextBox senderRtf = sender as ImprovedRichTextBox;
                ImprovedRichTextBox rtf = senderRtf == compiler ? numerator : compiler;

                Message m = e.Message;
                m.HWnd = rtf.Handle;
                rtf.SendScrollMessage(m);

                isScrolling = false;
            }
        }

        #endregion

        #region Рисование графики

        private void DrawPage(Graphics graph)
        {
            double w, h, z;

            if (IsFull)
            {
                w = widthRatio;
                h = heightRatio;
                z = 80;
                fontMenu = new Font("Arial", (float)(w * 7.75F), FontStyle.Regular);
            }
            else
            {
                w = 1;
                h = 1;
                z = 95;
                fontMenu = new Font("Arial", 8.75F, FontStyle.Regular);
            }

            if (PageCount != PageMax)
            {
                rectPlus = new Rectangle((int)((15 + z * PageCount) * w), (int)(45 * h), (int)(30 * h), (int)(30 * h));
                graph.FillRectangle(new SolidBrush(PlusFocus ? paint.Button : paint.Header), rectPlus);
                graph.DrawRectangle(new Pen(paint.Line), rectPlus);
                DrawPlus(graph, rectPlus, new Pen(paint.LineButton) { Width = (float)(w * 2.0F) });
            }
            else
            {
                rectPlus = new Rectangle(0, 0, 0, 0);
            }

            if (PageCount > 0)
            {
                for(int i = 0; i < PageCount; i++)
                {
                    rectPages[i] = new Rectangle((int)((15 + z * i) * w), (int)(45 * h), (int)(90 * h), (int)(30 * h));
                    if (i == PageNow - 1)
                        graph.FillRectangle(new SolidBrush(paint.ActivePage), rectPages[i]);
                    else
                        graph.FillRectangle(new SolidBrush(PageFocus[i] ? paint.Button : paint.Header), rectPages[i]);
                    graph.DrawString(" Page " + (i + 1), fontMenu, new SolidBrush(paint.HeaderText), rectPages[i], paint.SF);
                    graph.DrawRectangle(new Pen(paint.Line), rectPages[i]);

                    rectClose[i] = new Rectangle(rectPages[i].X + rectPages[i].Width - (int)(22 * w), rectPages[i].Y + rectPages[i].Height - (int)(22 * h), (int)(14 * h), (int)(14 * h));
                    graph.FillRectangle(new SolidBrush(CloseFocus[i] ? paint.Button : paint.Header), rectClose[i]);
                    graph.DrawRectangle(new Pen(paint.Line), rectClose[i]);
                    Rectangle rectClosePict = new Rectangle(rectClose[i].X + (int)(4 * h), rectClose[i].Y + (int)(4 * h), (int)(6 * h), (int)(6 * h));
                    DrawCross(graph, rectClosePict, new Pen(paint.LineButton) { Width = (float)(w * 1.55F) });
                }
            }
        }

        private void DrawPlus(Graphics graph, Rectangle rect, Pen pen)
        {
            graph.DrawLine(pen, rect.X + 7, rect.Y + rect.Height / 2, rect.X + rect.Width - 7, rect.Y + rect.Height / 2);

            graph.DrawLine(pen, rect.X + rect.Width / 2, rect.Y + 7, rect.X + rect.Width / 2, rect.Y + rect.Height - 7);
        }

        private void DrawCross(Graphics graph, Rectangle rect, Pen pen)
        {
            graph.DrawLine(pen, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            graph.DrawLine(pen, rect.X + rect.Width, rect.Y, rect.X, rect.Y + rect.Height);
        }

        private void DrawStripMenuBorder(Graphics graph)
        {
            Rectangle rectStripMenu = new Rectangle(menuStripForButtons.Location.X - 1, menuStripForButtons.Location.Y - 1, menuStripForButtons.Width + 5, menuStripForButtons.Height + 1);

            graph.FillRectangle(new SolidBrush(paint.Header), rectStripMenu);
            graph.DrawRectangle(new Pen(paint.Line), rectStripMenu);
        }

        private void DrawRichTextBoxBorder(Graphics graph)
        {
            graph.DrawRectangle(new Pen(paint.Line), numerator.Location.X - 1, numerator.Location.Y - 1, numerator.Width + compiler.Width + 1, numerator.Height + 1);
            graph.DrawRectangle(new Pen(paint.Line), outputBox.Location.X - 1, outputBox.Location.Y - 1, outputBox.Width + 1, outputBox.Height + 1);
            graph.DrawRectangle(new Pen(paint.Line), timeManager.Location.X - 1, timeManager.Location.Y - 1, timeManager.Width + 1, timeManager.Height + 1);
        }

        #endregion

        #region Обработчики Кликов и Нажатий

        private void ToolStripDefaultStyle_Click(object sender, EventArgs e)
        {
            if (paint.StyleNow != ColorsAndSize.Style.Default)
            {
                EventStyle(ColorsAndSize.Style.Default);
                HighlightCommands(compiler.SelectionStart);

                if (PageNow != 0)
                    ChangePict();

                if (IsSoundOn)
                    SoundPlay(1);
            }
        }

        private void ToolStripNeonStyle_Click(object sender, EventArgs e)
        {
            if (paint.StyleNow != ColorsAndSize.Style.Neon)
            {
                EventStyle(ColorsAndSize.Style.Neon);
                HighlightCommands(compiler.SelectionStart);

                if (PageNow != 0)
                    ChangePict();

                if (IsSoundOn)
                    SoundPlay(1);
            }
        }

        private void ToolStripCatStyle_Click(object sender, EventArgs e)
        {
            if (paint.StyleNow != ColorsAndSize.Style.Cat)
            {
                EventStyle(ColorsAndSize.Style.Cat);
                HighlightCommands(compiler.SelectionStart);

                if (PageNow != 0)
                    ChangePict();

                if (IsSoundOn)
                    SoundPlay(1);
            }
        }

        public void StartStyle(ColorsAndSize.Style style) => EventStyle(style);

        private void ToolStripComp_Click(object sender, EventArgs e)
        {
            if (PageNow != 0)
            {
                Compiler comp = new Compiler(compiler.Text);
                comp.CompileCode();

                if (comp.OutputError == "")
                {
                    outputBox.Text = comp.OutputStr;
                    timeManager.Text = "";

                    imageNow[PageNow - 1] = 0;

                    if (IsSoundOn)
                        SoundPlay(0);
                }
                else
                {
                    outputBox.Text = "";
                    timeManager.Text = comp.OutputError;

                    imageNow[PageNow - 1] = -1;

                    if (IsSoundOn)
                        SoundPlay(-1);
                }

                ChangePict();
            }
            else if (IsSoundOn)
                SoundPlay(-1);
        }

        private void ToolStripTxtRead_Click(object sender, EventArgs e)
        {
            if (PageNow != 0)
            {
                if (openFile.ShowDialog() == DialogResult.Cancel)
                    return;

                string filename = openFile.FileName;

                using (StreamReader sr = new StreamReader(filename))
                    compiler.Text = sr.ReadToEnd();

                if (outputBox.Text != "")
                    outputBox.Text += "\n";

                Numeration();
                HighlightCommands(compiler.Text.Length);

                outputBox.Text += "File was read correctly!";
            }
            else if (IsSoundOn)
                SoundPlay(-1);
        }

        private void ToolStripTxtWrite_Click(object sender, EventArgs e)
        {
            if (PageNow != 0)
            {
                if (saveFile.ShowDialog() == DialogResult.Cancel)
                    return;

                string filename = saveFile.FileName;

                System.IO.File.WriteAllText(filename, compiler.Text);

                if (outputBox.Text != "")
                    outputBox.Text += "\n";

                outputBox.Text += "Text was read correctly!";
            }
            else if (IsSoundOn)
                SoundPlay(-1);
        }

        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 1)
            {
                Helper = new FormBorder(paint)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };

                EventStyle += Helper.ChangeBorderStyle;
                EventStyle += Helper.help.ChangeStyleHelp;

                Helper.Show();
            }
            else if (Helper.WindowState == FormWindowState.Minimized)
            {
                Helper.WindowState = FormWindowState.Normal;
            }
        }

        private void ToolStripSound_Click(object sender, EventArgs e)
        {
            if (IsSoundOn)
            {
                toolStripSound.Text = "Sound: off";
                IsSoundOn = false;
            }
            else
            {
                toolStripSound.Text = "Sound: on";
                IsSoundOn = true;
            }
        }

        private void Compiler_Click(object sender, EventArgs e)
        {
            imageNow[PageNow - 1] = 1;

            ChangePict();

            if (compiler.Text == "")
            {
                numNow[PageNow - 1] = 1;
                numerator.Text = "  1|\n";
            }
        }

        private void Compiler_KeyDownAndUp(object obj, KeyEventArgs e)
        {
            Numeration();
        }

        private void Compiler_KeyUp(object obj, KeyEventArgs e)
        {
            HighlightCommands(compiler.SelectionStart);
        }

        private void Numeration()
        {
            int n = NCounter();

            if (n > countN[PageNow - 1])
            {
                while (numNow[PageNow - 1] < n + 1)
                {
                    numNow[PageNow - 1]++;

                    if (numNow[PageNow - 1] >= 1000)
                    {
                        string str = numNow[PageNow - 1].ToString();
                        int l = str.Length - 3;

                        numerator.Text += str[l] + str[l + 1] + str[l + 2] + "|" + "\n";
                    }
                    else
                    {
                        string numStrStr = "   " + numNow[PageNow - 1] + "|";
                        int numStrHelp = numNow[PageNow - 1];

                        while (numStrHelp != 0)
                        {
                            numStrStr = numStrStr[1..];
                            numStrHelp /= 10;
                        }
                        numerator.Text += numStrStr + "\n";
                    }

                }

                countN[PageNow - 1] = n;
            }
            else if (n < countN[PageNow - 1])
            {
                while (numNow[PageNow - 1] > n + 1)
                {
                    numNow[PageNow - 1]--;

                    numerator.Text = numerator.Text.Remove(numerator.Text.Length - 1);

                    while (numerator.Text[^1] != '\n')
                        numerator.Text = numerator.Text.Remove(numerator.Text.Length - 1);
                }

                countN[PageNow - 1] = n;
            }
        }

        private int NCounter()
        {
            return compiler.Text.Split('\n').Length - 1;
        }

        private void HighlightCommands(int k)
        {
            int counter;

            compiler.Select(0, compiler.Text.Length);
            compiler.SelectionColor = compiler.ForeColor;

            for (int i = 0; i < ListOfCommands1.Length; i++)
            {
                string[] strs = compiler.Text.Split(ListOfCommands1[i]);

                if (strs.Length != 1)
                {
                    counter = 0;

                    for (int j = 0; j + 1 < strs.Length; j++)
                    {
                        counter += strs[j].Length;

                        if ((Regex.IsMatch(strs[j], CheckList[0]) || strs[j] == "") && (Regex.IsMatch(strs[j + 1], CheckList[1]) || strs[j + 1] == ""))
                        {
                            compiler.Select(counter, ListOfCommands1[i].Length);
                            compiler.SelectionColor = Commands1;
                        }

                        counter += ListOfCommands1[i].Length;
                    }
                }
            }

            for (int i = 0; i < ListOfCommands2.Length; i++)
            {
                string[] strs = compiler.Text.Split(ListOfCommands2[i]);

                if (strs.Length != 1)
                {
                    counter = 0;

                    for (int j = 0; j + 1 < strs.Length; j++)
                    {
                        counter += strs[j].Length;

                        if ((Regex.IsMatch(strs[j], CheckList[0])) && (Regex.IsMatch(strs[j+1], CheckList[1])))
                        {
                            compiler.Select(counter, ListOfCommands2[i].Length);
                            compiler.SelectionColor = Commands2;
                        }

                        counter += ListOfCommands2[i].Length;
                    }
                }
            }

            if (compiler.Text.Length != 0)
                compiler.Select(k, 0);

            compiler.SelectionColor = compiler.ForeColor;
        }

        #endregion

        #region Звуки и Картинки

        private void SoundPlay(int typeOfSound)
        {
            string way = filesWay + "/Sounds";

            switch (typeOfSound)
            {
                case (-1):
                    way += "/Error";
                    break;
                case (0):
                    way += "/Write";
                    break;
                case (1):
                    way += "/Enter";
                break;
            }

            switch (paint.StyleNow)
            {
                case (ColorsAndSize.Style.Default):
                    way += ".wav";
                    break;
                case (ColorsAndSize.Style.Neon):
                    way += "Neon.wav";
                    break;
                case (ColorsAndSize.Style.Cat):
                    way += "Cat.wav";
                    break;
            }

            Sound = new SoundPlayer(way);
            Sound.Play();
        }

        private void ChangePict()
        {
            string way = filesWay + "/Images";

            switch (imageNow[PageNow - 1])
            {
                case (-1):
                    way += "/Error";
                    break;
                case (0):
                    way += "/Ok";
                    break;
                case (1):
                    way += "/Waiting";
                    break;
            }

            switch (paint.StyleNow)
            {
                case (ColorsAndSize.Style.Default):
                    way += ".png";
                    break;
                case (ColorsAndSize.Style.Neon):
                    way += "Neon.png";
                    break;
                case (ColorsAndSize.Style.Cat):
                    way += "Cat.png";
                    break;
            }

            image.Image = Image.FromFile(way);
        }

        #endregion

        #region Работа со страницами

        public void OpenPage(int n)
        {
            compiler.Text = compilerStr[n];
            outputBox.Text = outputBoxStr[n];
            timeManager.Text = timeManagerStr[n];
            numerator.Text = numeratorStr[n];

            outputBox.Visible = true;
            timeManager.Visible = true;
            compiler.Visible = true;
            numerator.Visible = true;

            HighlightCommands(compiler.Text.Length);
            ChangePict();
        }

        public void HidePage(int n)
        {
            compilerStr[n] = compiler.Text;
            outputBoxStr[n] = outputBox.Text;
            timeManagerStr[n] = timeManager.Text;
            numeratorStr[n] = numerator.Text;

            outputBox.Visible = false;
            timeManager.Visible = false;
            compiler.Visible = false;
            numerator.Visible = false;

            image.Image = null;
        }

        public void ClosePage(int n)
        {
            for (int i = n; i <= PageMax - 1; i++)
            {
                if (i != PageMax - 1)
                {
                    compilerStr[i] = compilerStr[i + 1];
                    outputBoxStr[i] = outputBoxStr[i + 1];
                    timeManagerStr[i] = timeManagerStr[i + 1];
                    numeratorStr[i] = numeratorStr[i + 1];
                    imageNow[i] = imageNow[i + 1];
                }
                else
                {
                    imageNow[i] = 1;
                }
            }

        }
    }

    #endregion

    #region Улучшения графической состовляющей MenuStrip и синхронизация ползунков

    public class TestColorTable : ProfessionalColorTable
    {
        readonly Menu form;

        public TestColorTable(Menu Form)
        {
            this.form = Form;
        }

        public override Color MenuBorder  //рамочка
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Black;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Violet;
                else
                    return Color.Pink;
            }
        }

        public override Color MenuItemBorder //обводка при наведении
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Black;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Violet;
                else
                    return Color.Pink;
            }
        }

        public override Color ButtonPressedBorder //обводка при нажатии
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Black;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Violet;
                else
                    return Color.Pink;
            }
        }

        public override Color MenuItemSelectedGradientBegin //цвет при наведении 1ый
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.White;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.MidnightBlue;
                else
                    return Color.MintCream;
            }
        }

        public override Color MenuItemSelectedGradientEnd // цвет при наведении 2ой
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Gray;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Black;
                else
                    return Color.Pink;
            }
        }

        public override Color MenuItemPressedGradientBegin // цвет после нажатия 1ый
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.White;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.MidnightBlue;
                else
                    return Color.MintCream;
            }
        }

        public override Color MenuItemPressedGradientEnd // цвет после нажатия 2ой
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Gray;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Black;
                else
                    return Color.Pink;
            }
        }

        public override Color ToolStripDropDownBackground //цвет рамочки внутри (3/4)
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Gray;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Black;
                else
                    return Color.Plum;
            }
        }

        public override Color ImageMarginGradientBegin //цвет рамочки внутри (1/4)
        {
            get
            {
                if (form.paint.StyleNow == ColorsAndSize.Style.Default)
                    return Color.Gray;
                if (form.paint.StyleNow == ColorsAndSize.Style.Neon)
                    return Color.Black;
                else
                    return Color.Plum;
            }
        }
    }

    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// сообщение
        /// </summary>
        public Message Message { get; private set; }

        /// <summary>
        /// конструктор
        /// </summary>
        public MessageEventArgs()
        {
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="msg"> сообщение </param>
        public MessageEventArgs(Message msg)
        {
            this.Message = msg;
        }
    }

    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public class ImprovedRichTextBox : RichTextBox
    {
        private const int WM_HSCROLL = 276;
        private const int WM_VSCROLL = 277;
        const int WM_MOUSEWHEEL = 0x020A;

        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;

        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        /// <summary>
        /// конструктор
        /// </summary>
        public ImprovedRichTextBox()
        {
        }

        public event MessageEventHandler Scroll;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
            {
                OnScroll(m);
            }

            if (m.Msg != WM_MOUSEWHEEL)
                base.WndProc(ref m);
        }

        /// <summary>
        /// вызов события 'Scroll'
        /// </summary>
        /// <param name="m"></param>
        protected virtual void OnScroll(Message m)
        {
            Scroll?.Invoke(this, new MessageEventArgs(m));
        }

        /// <summary>
        /// послать событие прокрутки
        /// </summary>
        /// <param name="m"></param>
        public void SendScrollMessage(Message m)
        {
            base.WndProc(ref m);

            // прокрутка
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    SetScrollPos(Handle, SB_HORZ, m.WParam.ToInt32() >> 16, true);
                    break;
                case WM_VSCROLL:
                    SetScrollPos(Handle, SB_VERT, m.WParam.ToInt32() >> 16, true);
                    break;
            }
        }
    }

    #endregion
}
