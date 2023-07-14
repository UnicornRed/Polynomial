using System.Drawing;
using System.Windows.Forms;

namespace WinFormsUI
{
    public partial class Help : Control
    {
        readonly ColorsAndSize paint;

        public string filesWay = @"../../../Resources/Images/Help";

        public Help(ColorsAndSize paint)
        {
            InitializeComponent();

            Paint += Form_HelpPaint;

            this.paint = paint;

            ChangeStyleHelp(paint.StyleNow);
        }

        public void ChangeStyleHelp(ColorsAndSize.Style style)
        {
            switch (style)
            {
                case ColorsAndSize.Style.Default:
                    BackColor = Color.LightGray;
                    imageInfo.Image = Image.FromFile(filesWay + ".png");
                    info.BackColor = Color.White;
                    info.ForeColor = Color.Black;
                    break;
                case ColorsAndSize.Style.Neon:
                    BackColor = Color.Black;
                    imageInfo.Image = Image.FromFile(filesWay + "Neon.png");
                    info.BackColor = Color.Black;
                    info.ForeColor = Color.MediumOrchid;
                    break;
                case ColorsAndSize.Style.Cat:
                    BackColor = Color.LightSkyBlue;
                    imageInfo.Image = Image.FromFile(filesWay + "Cat.png");
                    info.BackColor = Color.MistyRose;
                    info.ForeColor = Color.Indigo;
                    break;
            }
        }

        private void Form_HelpPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(paint.Line), info.Location.X - 1, info.Location.Y - 1, info.Width + 1, info.Height + 1);
        }
    }
}
