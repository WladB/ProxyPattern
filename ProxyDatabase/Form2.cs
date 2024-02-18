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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        IDbManager mn = new ProxyDbManager(new RealDbManager());

        int top = 0;
        List<string> fields = new List<string>();
        List<string> valuefields = new List<string>();
        string primarykey = "";
        string ColumnName = "";
        private void form2_Load(object sender, EventArgs e)
        {
           
            mn.AllTables(listBox1);
           
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            mn.TableRecords(listBox1.SelectedItem.ToString(), dataGridView1);
            dataGridView1.Visible = true;
            mn.ViewTable(fields, listBox1.SelectedItem.ToString());
            top = 0;
            panel1.Controls.Clear();
            foreach (string str in fields)
            {

                Label label = new Label();
                label.Text = str;
                label.AutoSize = true;
                label.Top = top;
                label.Left = 50;

                panel1.Controls.Add(label);
                TextBox textBox = new TextBox();
                textBox.Size = new Size(463, 22);
                textBox.Location = new Point(1, 25);
                textBox.Name = str;
                textBox.Top = top + 20;
                textBox.Left = 50;
                panel1.Controls.Add(textBox);
                top += 45;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            valuefields.Clear();
            foreach (string str in fields)
            {
                valuefields.Add((panel1.Controls[str] as TextBox).Text);
            }

             mn.AddRecord(listBox1.SelectedItem.ToString(), valuefields);
            
            mn.TableRecords(listBox1.SelectedItem.ToString(), dataGridView1);
        }
        int i = 1;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                primarykey = dataGridView1.Rows[e.RowIndex].Cells[(dataGridView1.Columns[mn.ViewPK(listBox1.SelectedItem.ToString())]).Index].Value.ToString();
                ColumnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                label1.Text = ColumnName;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
              
                if (radioButton2.Checked)
                {
                    i = 1;
                    mn.DeleteRecord(listBox1.SelectedItem.ToString(), primarykey);
                   
                    foreach (DataGridViewCell s in dataGridView1.Rows[e.RowIndex].Cells)
                    {
                        panel1.Controls[i].Text = "";
                        i += 2;
                    }
                    mn.TableRecords(listBox1.SelectedItem.ToString(), dataGridView1);
                }
                else
                {
                    i = 1;
                    foreach (DataGridViewCell s in dataGridView1.Rows[e.RowIndex].Cells)
                    {
                        panel1.Controls[i].Text = s.Value.ToString();
                        i += 2;
                    }
                }
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                valuefields.Clear();
                foreach (string str in fields)
                {
                    valuefields.Add((panel1.Controls[str] as TextBox).Text);
                }
                mn.UpdateRecord(listBox1.SelectedItem.ToString(), ColumnName, textBox1.Text, primarykey);
               
                mn.TableRecords(listBox1.SelectedItem.ToString(), dataGridView1);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
            panel1.Visible = true;
            button1.Visible = true;
            button2.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           
            panel1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
           
            panel1.Visible = false;
            button1.Visible = false;
            button2.Visible = true;
            label1.Visible = true;
            textBox1.Visible = true;

        }
    }
}
