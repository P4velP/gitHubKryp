using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playfair
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool key = false;


        private string spacingWord(string word, int indexSpace)
        {
            string fixWord = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (i != 0 && i % indexSpace == 0)
                {
                    fixWord += ' ';
                }
                fixWord += word[i];
            }
            return fixWord;
        }

private void button1_Click(object sender, EventArgs e)
        { 
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                Matrix2DArray matrix = new Matrix2DArray(textBox1.Text,textBox2.Text);
                string array = matrix.RepairKeyWord(textBox1.Text);
                if(array.Length >= 5)
                { 
                    key = true;
                    char[,] matrixArray = matrix.KeyWordFilling(array);
                    matrix.CreateMatrix(matrixArray, dataGridView1);

                }
                else
                {
                    MessageBox.Show("Zadejte delší klíčkové slovo","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve zadejte klíč","Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (key)
            {
                if (!String.IsNullOrEmpty(textBox2.Text))
                {
                    Matrix2DArray matrix = new Matrix2DArray(textBox1.Text, textBox3.Text);

                    if (textBox2.Text.Length >= 2)
                    {
                        string correctword = matrix.RepairWord(textBox2.Text);

                        textBox3.Text = spacingWord(correctword, 2);

                        string array = matrix.RepairKeyWord(textBox1.Text);
                        char[,] matrixArray = matrix.KeyWordFilling(array);
                        string word = matrix.Matrix2DEncoder(matrixArray, correctword);

                        textBox4.Text = spacingWord(word, 5);
                    }
                    else
                    {
                        MessageBox.Show("Vstupní řetězec musí být větší jak 2 znaky", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Zadejte vstupní řetěžec", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve zadejte klíč", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (key)
            {
                if (!String.IsNullOrEmpty(textBox6.Text))
                {
                    Matrix2DArray matrix = new Matrix2DArray(textBox1.Text, textBox6.Text);

                    if (textBox6.Text.Length >= 2)
                    {
                        string unspacingword = textBox6.Text.Replace(" ","");
                        string correctword = matrix.RepairWord(unspacingword);
                        textBox5.Text = spacingWord(correctword,2);
                        string array = matrix.RepairKeyWord(textBox1.Text);
                        char[,] matrixArray = matrix.KeyWordFilling(array);
                        string word = matrix.Matrix2DDecoder(matrixArray, correctword);
                        textBox7.Text = word;
                    }
                    else
                    {
                        MessageBox.Show("Vstupní řetězec musí být větší jak 2 znaky", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Zadejte vstupní řetěžec", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve zadejte klíč", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
