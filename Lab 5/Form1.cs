using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab_5
{
    public partial class Form1 : Form
    {
        Default df = new Default();
        Compress compress = new Compress();
        Encrypting encrypting = new Encrypting();

        string path, text;
        bool compression = false, encryption = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Open
        private void button1_Click(object sender, EventArgs e)
        {
            OopenForm OopenForm = new OopenForm();
            OopenForm.ShowDialog();
            path = OopenForm.getString() + ".txt";
            df.setPath(path);
            compress.setPath(path);
            encrypting.setPath(path);
            textBox1.Text = path;
            textBox1.Visible = true;            
        }

        //End
        private void endToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }      

        //Read
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (encryption && compression)
            {
                text = encrypting.Decrypt();
                text = compress.Unzip();                
                richTextBox1.Text = text;
            }

            if (encryption)
            {
                richTextBox1.Text = encrypting.Decrypt();
            }
            else
            {
                if (compression)
                {
                    richTextBox1.Text = compress.Unzip();
                }
                else
                {
                    richTextBox1.Text = df.readFile();
                }
            }

        }

        //Compress
        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compression = !compression;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();            
        }

        //Encrypt
        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            encryption = !encryption;
        }

        //Write
        private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(encryption && compression)
            {
                text = richTextBox1.Text;
                text = encrypting.Encrypt(text);
                compress.Zip(text);
            }

            if (encryption)
            {
                encrypting.Encrypt(richTextBox1.Text);                
            }
            else
            {
                if (compression)
                {
                    compress.Zip(richTextBox1.Text);
                }
                else
                {
                    df.writeFile(richTextBox1.Text);
                }
            }

        }
    }
}