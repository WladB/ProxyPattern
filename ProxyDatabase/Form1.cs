using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fields = new List<string>();
        }
        
      // List<string> fields = new List<string>();
        List<string> valuefields = new List<string>();
 
        List<string> fields;
        IDbManager mn = new ProxyDbManager(new RealDbManager());
        private void button2_Click(object sender, EventArgs e)
        {
            fields.Add(textBox2.Text + " " + textBox3.Text);
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (string s in fields)
            {
                if (s.Contains("not null primary key"))
                    check = true;
                break;

            };
            if (!check)
            {
                fields[0] += " not null primary key";
            }
            mn.CreateTable(textBox1.Text, fields.ToArray());
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            fields.Clear();
        }

        private void AddingDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
