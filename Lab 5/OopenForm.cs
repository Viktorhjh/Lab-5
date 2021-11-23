using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_5
{
    public partial class OopenForm : Form
    {
        public OopenForm()
        {
            InitializeComponent();
            textBox1.Text = @"D:\";
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        public string getString()
        {
            return textBox1.Text;
        }
    }
}
