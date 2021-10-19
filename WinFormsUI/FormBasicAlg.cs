using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasicAlg;

namespace WinFormsUI
{
    public partial class FormBasicAlg : Form
    {
        CalculatingExpressions calculatingExpressions = new CalculatingExpressions();

        public FormBasicAlg()
        {
            InitializeComponent();
        }

        private void FormBasicAlg_Load(object sender, EventArgs e)
        {
            
        }

        private void ReloadVar()
        {
            richTextBoxVariable.Clear();

            foreach (var i in calculatingExpressions)
                richTextBoxVariable.Text += i + " = " + calculatingExpressions.Vars[i].ToString() + "\n";
        }

        private void richTextBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                try
                {
                    richTextBoxOutput.Text = calculatingExpressions.CalculatePoly((richTextBoxInput.Text).Replace("\n", "")).ToString();
                }
                catch (Exception er)
                {
                    richTextBoxOutput.Text = er.Message;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                calculatingExpressions.NewVar(richTextBoxInput.Text);

                ReloadVar();
                richTextBoxInput.Clear();
            }
            catch (Exception er)
            {
                richTextBoxOutput.Text = er.Message;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                calculatingExpressions.DeleteVar(richTextBoxInput.Text);

                richTextBoxInput.Clear();

                ReloadVar();
            }
            catch (Exception er)
            {
                richTextBoxOutput.Text = er.Message;
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                string filename = openFileDialog1.FileName;
                string variable;

                using (StreamReader sr = new StreamReader(filename))
                {
                    while ((variable = sr.ReadLine()) != null)
                        calculatingExpressions.NewVar(variable);
                }

                ReloadVar();
            }
            catch (Exception er)
            {
                richTextBoxOutput.Text = er.Message;
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                
                string filename = saveFileDialog1.FileName;

                System.IO.File.WriteAllText(filename, "");

                foreach (var i in calculatingExpressions)
                    System.IO.File.AppendAllText(filename, i + " = " + calculatingExpressions.Vars[i].ToString() + "\n");

                richTextBoxOutput.Text = "Variables have been successfully written to the file: " + filename;
            }
            catch (Exception er)
            {
                richTextBoxOutput.Text = er.Message;
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
