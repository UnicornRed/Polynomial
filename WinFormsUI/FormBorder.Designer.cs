using System.Drawing;

namespace WinFormsUI
{
    partial class FormBorder
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponentMain()
        {
            Height = 720;
            Width = 1080;

            menu = new Menu(paint);
            menu.Size = new Size(Width - 2, Height - 31);
            menu.Location = new Point(1, 30);

            Controls.Add(menu);
        }

        private void InitializeComponentHelp()
        {
            Height = 720;
            Width = 480;

            help = new Help(paint);
            help.Size = new Size(Width - 2, Height - 31);
            help.Location = new Point(1, 30);

            Controls.Add(help);
        }

        public ColorsAndSize paint;
        private Menu menu;
        public Help help;
    }
}
