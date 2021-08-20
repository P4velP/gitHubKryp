using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA_sifra
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string RepairEncryptedWord(string inputWord)
        {
            string word = "";

            for (int j = 0; j < inputWord.Length; j++)
            {
                if (Convert.ToInt32(inputWord[j]) >= 48 && Convert.ToInt32(inputWord[j]) <= 57)
                {
                    word += inputWord[j];
                }
                else if (Convert.ToInt32(' ') == Convert.ToInt32(inputWord[j]))
                {
                    word += ' ';
                }
            }

            return word;
        }
        public string RepairWord(string word)
        {
            string repairword = "";
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(word);
            repairword = System.Text.Encoding.UTF8.GetString(tempBytes);
            return repairword;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                    BigInteger p = RandomInteger();
                    BigInteger q = RandomInteger();
                    while (q == p)
                    {
                        q = RandomInteger();
                    }
                    textBox1.Text = Convert.ToString(p);
                    textBox2.Text = Convert.ToString(q);
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numb = e.KeyChar;

            if (!(Char.IsDigit(numb)) && numb != 8)
            {
                e.Handled = true;
                MessageBox.Show("Prosím zadejte pouze čísla", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        { 
            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
                {

                    if (IsPrime(BigInteger.Parse(textBox1.Text)))
                    {
                        if (IsPrime(BigInteger.Parse(textBox2.Text)))
                        {
                            
                            BigInteger p = BigInteger.Parse(textBox1.Text);
                            BigInteger q = BigInteger.Parse(textBox2.Text);
                            RSA_Sifra rsa = new RSA_Sifra(p, q);
                            if (rsa.GetN() >= 4294967296)
                            {
                                string word = RepairWord(textBox5.Text);
                                textBox10.Text = Convert.ToString(rsa.GetN());
                                textBox9.Text = Convert.ToString(rsa.GetD());
                                textBox4.Text = Convert.ToString(rsa.GetN());
                                textBox3.Text = Convert.ToString(rsa.GetE());
                                textBox6.Text = textBox8.Text = encodeFunction(rsa.GetN(), rsa.GetE(), word);
                            }
                            else
                            {
                                MessageBox.Show("Zadejte větší hodnoty parametrů nebo kratší vstupní řetězec","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Parametr q není prvno číslo", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Parametr p není prvočíslo", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Musíte vyplnit oba parametry", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve vyplňte vstupní řetězec", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox8.Text))
            {
                if (!string.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrEmpty(textBox9.Text))
                {
                    BigInteger n = BigInteger.Parse(textBox10.Text);
                    BigInteger d = BigInteger.Parse(textBox9.Text);
                    string word = RepairEncryptedWord(textBox8.Text);
                    textBox7.Text = decodeFunction(n, d, word);
                }
                else
                {
                    MessageBox.Show("Zadejte privátní klíč", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nejprve vyplňte vstupní řetězec", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsPrime(BigInteger numb)
        {
            double sqrt = Math.Exp(BigInteger.Log(numb) / 2);
            
            if(numb == 0 || numb == 1)
            {
                return false;
            }
            else if(numb == 2)
            {
                return true;
            }
            else if (numb % 2 == 0)
            {
                return false;
            }
            else
            {   
                for (int a = 3; a <= sqrt; a += 2)
                {
                    if (numb % a == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public BigInteger RandomInteger() // to generate a random number
        {
            byte[] bytes = new byte[4];
            BigInteger numb;
            do
            {
                var rng = new RNGCryptoServiceProvider();
                rng.GetBytes(bytes);

                numb = new BigInteger(bytes);
                numb = BigInteger.Abs(numb);
                if (BigInteger.ModPow(numb, 1, 2) == 0)
                {
                    numb = BigInteger.Subtract(numb, 1);
                }
                rng.Dispose();
            }
            while (!IsPrime(numb));
            
            return numb;
        }


        private string encodeFunction(BigInteger n, BigInteger e, string word)
        {
            byte[] array = new byte[4];
            string code = "";

            for (int i = 0; i < word.Length;)           // cyklus na delku  vstupniho retězce
            {
                for (int j = 0; j < 4; j++, i++)        // vnitřní cyklus načítá znaky ze vstupu po 4
                {
                    if (i < word.Length)
                    {
                        array[j] = Convert.ToByte(word[i]);     // j je index pro ukladání do pole 
                    }                                           // i je index na procházení vstupního řetězce
                    else
                    {
                        array[j] = Convert.ToByte(null);        // když dojde vstupni řetězec a já nemam ještě 4 Byty vkládám hodnotu null
                    }
                }
                BigInteger m = new BigInteger(array);           // pole Bytů převedu na BigArray
                BigInteger c = BigInteger.ModPow(m, e, n);      // samotné kodování
                code += c + " ";                                // uložení do stringu code a přidání mezery pro oddělení jednotlivných "slov"

            }

            return code;                            // navrácení zakodované zprávy
        }

        private string decodeFunction(BigInteger n, BigInteger d, string word)
        {
            byte[] array = new byte[4];
            string decode = "";
            string mCrypted = "";
            for (int i = 0; i < word.Length; i++)       // cyklus na načítání znaků z textboxu
            {
                if (word[i] != ' ')                     // koduji po 4 bytech oddělené mezerou
                {

                    mCrypted += word[i];                // načítám tak dlouho čísla než naleznu mezeru

                }
                else                                    // kdyz naleznu mezeru
                {                                       
                    BigInteger c = BigInteger.Parse(mCrypted);      // převedu řetězec na BigInteger
                    BigInteger m = BigInteger.ModPow(c, d, n);      // provedu dekodovani
                    array = m.ToByteArray();                        // převedu na byte

                    for (int j = 0; j < array.Length; j++)          // cyklus na převod čísel na znaky z pole bytů
                    {
                        decode += Convert.ToChar(array[j]);         // převádí jednotlivé byty na znaky
                        
                    }
                    mCrypted = "";                                  // vypraznění proměné pro načtení dalšího zakodovaného řetězce
                }
               
            }

            return decode;          // navrácení dekodovaného řetězce
        }
        
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
   
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }


    }
}
