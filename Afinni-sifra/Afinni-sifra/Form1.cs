using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Afinni_sifra
{
    public partial class Form1 : Form
    {
        public Form1() 
        {
            InitializeComponent();
        }
        
        bool Euklid_Algoritm(int a1)
        {
            int b = 26;
            int a = a1;
            int mod;                                                            // promena mod slouzi pro swap, a pozdeji zbytek po deleni
            
            if (a < 0)
                a = -a;

            if (a > b)  
            {
                mod = b;    // swap
                b = a;
                a = mod;
            }
            while (true)
            {
                if (a > 0)
                {
                    mod = b % a;
                    if (mod == 1)
                    {
                        return true;
                    }
                    else
                    {
                        b = a;
                        a = mod;
                    }
                }
                else
                {
                    break;
                } 
            }
            return false;
        }

        private int MMI(int a)
        {
            
            for (int i = 1; i < 26; i++)
            {
                int calculation = (a * i) % 26;
                if (calculation == 1)
                {
                    return i;
                }
            }
            return 0;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void repairChar(string[] array)
        {
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                switch (textBox1.Text.ToLower()[i])
                {
                    case 'č':
                        array[i] = "C";
                        break;
                    case 'ě':
                        array[i] = "E";
                        break;
                    case 'š':
                        array[i] = "S";
                        break;
                    case 'ř':
                        array[i] = "R";
                        break;
                    case 'ž':
                        array[i] = "Z";
                        break;
                    case 'ý':
                        array[i] = "Y";
                        break;
                    case 'á':
                        array[i] = "A";
                        break;
                    case 'í':
                        array[i] = "I";
                        break;
                    case 'é':
                        array[i] = "E";
                        break;
                    case 'ď':
                        array[i] = "D";
                        break;
                    case 'ň':
                        array[i] = "N";
                        break;
                    case 'ú':
                        array[i] = "U";
                        break;
                    case 'ů':
                        array[i] = "U";
                        break;
                    default:
                        array[i] = Convert.ToString(textBox1.Text.ToUpper()[i]);
                        break;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)  // kodovani
        {
            char znak;
            int asciiZnaku;
            int codeZnak;
            string encodeText = "";
            
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string[] textArray = new string[textBox1.Text.Length];
                repairChar(textArray);
             
                if (!String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox3.Text))
                {
                    int a = Convert.ToInt32(textBox2.Text);
                    int b = Convert.ToInt32(textBox3.Text);

                    if (Euklid_Algoritm(a))
                    {
                        for (int i = 0; i < textBox1.Text.Length; i++)
                        {
                            if(i > 0 && i % 5 == 0)
                            {
                                encodeText += " ";
                            }
                            znak = Convert.ToChar(textArray[i]);
                            asciiZnaku = Convert.ToInt32(znak);
                            if (asciiZnaku >= 65 && asciiZnaku <= 90)
                            {
                                asciiZnaku -= 65;
                            //    Console.WriteLine("asci znak " + asciiZnaku +" = "+ znak);
                                codeZnak = (a * asciiZnaku + b) % 26;
                          //      Console.WriteLine("code znak " + codeZnak);
                                codeZnak += 65;
                                encodeText += Convert.ToString(Convert.ToChar(codeZnak));

                                // zakodovat 
                            }
                            else if (asciiZnaku == 32)
                            {
                                encodeText += "MEZERA";
                            }
                        }
                        textBox4.Text = encodeText;
                    }
                    else
                    {
                        MessageBox.Show("Parametr a je soudělný!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Prosím vyplnte oba parametry!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("SORRY JAKO, ale prázdný řetězec nešifruji", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numb = e.KeyChar;
            if(!(Char.IsDigit(numb)) && numb != 8)
            {
                e.Handled = true;
                MessageBox.Show("Prosím zadejte pouze čísla", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numb = e.KeyChar;
            if (!(Char.IsDigit(numb)) && numb != 8)
            {
                e.Handled = true;
                MessageBox.Show("Prosím zadejte pouze čísla","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)    // dekodovani
        {
            char znak;
            int asciiZnaku;
            int decodeZnak;
            string decodeText = "";

            if (!String.IsNullOrEmpty(textBox8.Text))
            {
                if (!String.IsNullOrEmpty(textBox7.Text) && !String.IsNullOrEmpty(textBox6.Text))
                {
                    int a = Convert.ToInt32(textBox7.Text);
                    int b = Convert.ToInt32(textBox6.Text);
                    if (Euklid_Algoritm(a))
                    { 
                        int invA = MMI(a);

                        for (int i = 0; i < textBox8.Text.Length; i++)
                        {
                            znak = textBox8.Text[i];
                            asciiZnaku = Convert.ToInt32(znak);
                            
                            if (textBox8.Text[i] == 'M' && textBox8.Text[i+1] == 'E' && textBox8.Text[i+2] == 'Z' && textBox8.Text[i+3] == 'E' 
                                && textBox8.Text[i+4] == 'R' && textBox8.Text[i+5] == 'A')
                            {
                                decodeText += " ";
                                i += 5;
                            }
                            else if (asciiZnaku >= 65 && asciiZnaku <= 90)
                            {
                                asciiZnaku -= 65;
                                decodeZnak = ((asciiZnaku - b) * invA) % 26;
                                if (decodeZnak < 0)
                                {
                                    decodeZnak += 26;
                                }
                                decodeZnak += 65;
                                decodeText += Convert.ToString(Convert.ToChar(decodeZnak));
                            }
                        }
                        textBox5.Text = decodeText;
                    }
                    else
                    {
                        MessageBox.Show("Parametr a je soudělný!", "Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Prosím vyplnte oba parametry!", "Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("SORRY JAKO, ale prázdný řetězec nedešifruji", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char numb = e.KeyChar;
            if (!(Char.IsDigit(numb)) && numb != 8)
            {
                e.Handled = true;
                MessageBox.Show("Prosím zadejte pouze čísla", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numb = e.KeyChar;
            if (!(Char.IsDigit(numb)) && numb != 8)
            {
                e.Handled = true;
                MessageBox.Show("Prosím zadejte pouze čísla", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
