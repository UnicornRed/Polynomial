using System;
using System.Drawing;

namespace WinFormsUI
{
    public class ColorsAndSize
    {
        #region Переменные

        public static string filesWay = @"../../../Resources";

        public enum Style { Default, Neon, Cat };

        public Style StyleNow;
        public Color Header, Line, HeaderText, LineButton, Button, ActivePage;
        public String helpText;

        public readonly StringFormat SF = new StringFormat();
        public readonly StringFormat SFH = new StringFormat();
        public readonly Font font = new Font("Arial", 8.75F, FontStyle.Regular);
        public readonly Font fontHelp = new Font("Arial", 8.75F, FontStyle.Regular);

        #endregion

        #region Конструктор и Методы

        public ColorsAndSize()
        {
            SF.Alignment = StringAlignment.Near;
            SF.LineAlignment = StringAlignment.Center;
            font = new Font("Arial", 8.75F, FontStyle.Regular);

            SFH.Alignment = StringAlignment.Center;
            SFH.LineAlignment = StringAlignment.Center;
            fontHelp = new Font("Arial", 20F, FontStyle.Regular);

            ChangeColors(Style.Default);
        }

        public void ChangeColors(Style style)
        {
            StyleNow = style;

            switch (style)
            {
                case (Style.Default):
                    Header = Color.White;
                    Line = Color.Black;
                    HeaderText = Color.Black;
                    Button = Color.Gray;
                    LineButton = Color.Black;
                    ActivePage = Color.PapayaWhip;
                    break;
                case (Style.Neon):
                    Header = Color.Black;
                    Line = Color.MidnightBlue;
                    HeaderText = Color.MediumOrchid;
                    Button = Color.DarkSlateBlue;
                    LineButton = Color.MediumOrchid;
                    ActivePage = Color.MidnightBlue;
                    break;
                case (Style.Cat):
                    Header = Color.LavenderBlush;
                    Line = Color.DeepPink;
                    HeaderText = Color.Indigo;
                    Button = Color.Pink;
                    LineButton = Color.DeepPink;
                    ActivePage = Color.SkyBlue;
                    break;
            }
        }

        #endregion  
    }
}
