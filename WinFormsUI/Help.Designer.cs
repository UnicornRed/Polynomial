using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsUI
{
    partial class Help
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.info = new RichTextBox();
            this.imageInfo = new PictureBox();

            //
            //info
            //
            this.info.Size = new Size(450, 590);
            this.info.Location = new Point(15, 10);
            this.info.ReadOnly = true;
            this.info.BorderStyle = BorderStyle.None;
            this.info.Font = new Font("Arial", 10F, FontStyle.Regular);

            using (StreamReader sr = new StreamReader(@"../../../Resources/Help.txt"))
                info.Text = sr.ReadToEnd();
            //
            //imageInfo
            //
            this.imageInfo.Size = new Size(60, 60);
            this.imageInfo.SizeMode = PictureBoxSizeMode.Zoom;
            this.imageInfo.Location = new Point(210, 610);
            this.imageInfo.Image = null;

            Controls.Add(info);
            Controls.Add(imageInfo);
        }

        private RichTextBox info;
        private PictureBox imageInfo;
    }
}