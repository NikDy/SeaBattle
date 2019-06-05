using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private Form1 parentForm = null;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            parentForm = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = parentForm.networker.StartToBeHost();
            label2.Refresh();
            Clipboard.SetText(label2.Text);
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            if (parentForm.networker.WaitForConnect() == 1)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") parentForm.networker.Connect(textBox1.Text);
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Enabled = true;
        }
    }
}
