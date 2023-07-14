using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormsUI
{
    partial class Menu
    {
        private bool isScrolling = false;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));

            this.menuStripForButtons = new MenuStrip();
            this.toolStripStyles = new ToolStripMenuItem();
            this.toolStripNeonStyle = new ToolStripMenuItem();
            this.toolStripDefaultStyle = new ToolStripMenuItem();
            this.toolStripCatStyle = new ToolStripMenuItem();
            this.toolStripComp = new ToolStripMenuItem();
            this.toolStripTxt = new ToolStripMenuItem();
            this.toolStripTxtRead = new ToolStripMenuItem();
            this.toolStripTxtWrite = new ToolStripMenuItem();
            this.toolStripHelp = new ToolStripMenuItem();
            this.toolStripSound = new ToolStripMenuItem();
            this.compiler = new ImprovedRichTextBox();
            this.numerator = new ImprovedRichTextBox();
            this.outputBox = new RichTextBox();
            this.timeManager = new RichTextBox();
            this.image = new PictureBox();
            this.openFile = new OpenFileDialog();
            this.saveFile = new SaveFileDialog();
            // 
            // menuStripForButtons
            //
            this.menuStripForButtons.BackColor = SystemColors.MenuBar;
            this.menuStripForButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStyles,
            this.toolStripComp,
            this.toolStripTxt,
            this.toolStripHelp,
            this.toolStripSound});
            this.menuStripForButtons.Location = new Point(16, 8);
            this.menuStripForButtons.Size = new Size(400, 30);
            this.menuStripForButtons.TabIndex = 2;
            this.menuStripForButtons.Dock = DockStyle.None;
            this.menuStripForButtons.Renderer = new ToolStripProfessionalRenderer(new TestColorTable(this));
            // 
            // toolStripStyles
            // 
            this.toolStripStyles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDefaultStyle,
            this.toolStripNeonStyle,
            this.toolStripCatStyle});
            this.toolStripStyles.Text = "Style";
            // 
            // toolStripDefaultStyle
            // 
            this.toolStripDefaultStyle.Text = "Default";
            this.toolStripDefaultStyle.Click += new System.EventHandler(this.ToolStripDefaultStyle_Click);
            // 
            // toolStripNeonStyle
            // 
            this.toolStripNeonStyle.Text = "Dark/Neon";
            this.toolStripNeonStyle.Click += new System.EventHandler(this.ToolStripNeonStyle_Click);
            //
            // toolStripCatStyle
            // 
            this.toolStripCatStyle.Text = "Child/Cat";
            this.toolStripCatStyle.Click += new System.EventHandler(this.ToolStripCatStyle_Click);
            // 
            // toolStripComp
            // 
            this.toolStripComp.Text = "Comp";
            this.toolStripComp.Click += new System.EventHandler(this.ToolStripComp_Click);
            // 
            // toolStripTxt
            // 
            this.toolStripTxt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTxtRead,
            this.toolStripTxtWrite});
            this.toolStripTxt.Text = "File";
            // 
            // toolStripTxtRead
            // 
            this.toolStripTxtRead.Text = "Read File";
            this.toolStripTxtRead.Click += new System.EventHandler(this.ToolStripTxtRead_Click);
            // 
            // toolStripComp
            // 
            this.toolStripTxtWrite.Text = "Write to File";
            this.toolStripTxtWrite.Click += new System.EventHandler(this.ToolStripTxtWrite_Click);
            // 
            // toolStripHelp
            // 
            this.toolStripHelp.Text = "Help";
            this.toolStripHelp.Click += new System.EventHandler(this.ToolStripHelp_Click);
            // 
            // toolStripSound
            // 
            this.toolStripSound.AutoSize = false;
            this.toolStripSound.Size = new Size(80, 24);
            this.toolStripSound.Text = "Sound: on";
            this.toolStripSound.Click += new System.EventHandler(this.ToolStripSound_Click);
            // 
            // compiler
            // 
            this.compiler.Size = new Size(470, 580);
            this.compiler.Location = new Point(55, 90);
            this.compiler.ReadOnly = false;
            this.compiler.BorderStyle = BorderStyle.None;
            this.compiler.WordWrap = false;
            this.compiler.ScrollBars = RichTextBoxScrollBars.Both;
            this.compiler.Font = new Font("Consolas", 10F, FontStyle.Regular);
            this.compiler.Visible = false;
            this.compiler.Click += new System.EventHandler(this.Compiler_Click);
            this.compiler.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Compiler_KeyDownAndUp);
            this.compiler.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Compiler_KeyUp);
            this.compiler.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Compiler_KeyDownAndUp);
            this.compiler.Scroll += RTF_Scroll;
            //
            //numerator
            //
            this.numerator.Size = new Size(40, 580);
            this.numerator.Location = new Point(15, 90);
            this.numerator.ReadOnly = true;
            this.numerator.BorderStyle = BorderStyle.None;
            this.numerator.ScrollBars = RichTextBoxScrollBars.None;
            this.numerator.Font = new Font("Consolas", 10F, FontStyle.Regular);
            this.numerator.Visible = false;
            this.numerator.Scroll += RTF_Scroll;
            // 
            // outputBox
            // 
            this.outputBox.Size = new Size(530, 180);
            this.outputBox.Location = new Point(535, 90);
            this.outputBox.ReadOnly = false;
            this.outputBox.WordWrap = false;
            this.outputBox.ScrollBars = RichTextBoxScrollBars.Both;
            this.outputBox.BorderStyle = BorderStyle.None;
            this.outputBox.Font = new Font("Consolas", 12F, FontStyle.Regular);
            this.outputBox.Visible = false;
            // 
            // timeManager
            // 
            this.timeManager.Size = new Size(530, 200);
            this.timeManager.Location = new Point(535, 290);
            this.timeManager.ReadOnly = true;
            this.timeManager.BorderStyle = BorderStyle.None;
            this.timeManager.WordWrap = false;
            this.timeManager.ScrollBars = RichTextBoxScrollBars.Both;
            this.timeManager.Font = new Font("Consolas", 12F, FontStyle.Regular);
            this.timeManager.Visible = false;
            //
            // image
            //
            this.image.Size = new Size(150, 150);
            this.image.SizeMode = PictureBoxSizeMode.Zoom;
            this.image.Location = new Point(725, 510);
            this.image.Image = null;
            //
            // openFile
            //
            this.openFile.DefaultExt = "txt";
            string filePath = Directory.GetCurrentDirectory();
            this.openFile.InitialDirectory = filePath.Substring(0, filePath.Length - 35);
            // 
            // FormBasicAlg
            // 
            this.Controls.Add(this.menuStripForButtons);
            this.Controls.Add(this.compiler);
            this.Controls.Add(this.numerator);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.timeManager);
            this.Controls.Add(this.image);
        }

        #region Переменные

        public MenuStrip menuStripForButtons;
        public ToolStripMenuItem toolStripStyles;
        public ToolStripMenuItem toolStripDefaultStyle;
        public ToolStripMenuItem toolStripNeonStyle;
        public ToolStripMenuItem toolStripCatStyle;
        public ToolStripMenuItem toolStripComp;
        public ToolStripMenuItem toolStripTxt;
        public ToolStripMenuItem toolStripTxtRead;
        public ToolStripMenuItem toolStripTxtWrite;
        public ToolStripMenuItem toolStripHelp;
        public ToolStripMenuItem toolStripSound;
        public OpenFileDialog openFile;
        public SaveFileDialog saveFile;

        public ImprovedRichTextBox compiler;
        public ImprovedRichTextBox numerator;
        public RichTextBox outputBox;
        public RichTextBox timeManager;
        public PictureBox image;

        #endregion
    }
}

