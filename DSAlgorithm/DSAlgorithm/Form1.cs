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
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace DSAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string adressFile = "";
        string adressFile1 = "";
        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Message files (*.msg)|*.msg| Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                adressFile = ofd.FileName;
                textBox1.Text = adressFile;
            }
            else
            {
                MessageBox.Show("Nevybrali jste žádný soubor", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(adressFile))
            {
                FileInfo f = new FileInfo(adressFile);
                long size = f.Length;
                string name = f.Name;
                var extens = Path.GetExtension(adressFile);
                string fname = f.FullName;
                var time = f.CreationTime;
                var type = f.Attributes;
                textBox2.Text = "Name: " + name + "\r\nExtension: " + extens + "\r\nSize: " + size + "bytes\r\nCreate time: " 
                                                    + time + "\r\nType: " + type + "\r\nFullname: " + fname;
                ofd.Dispose();
            }
            else
            {
                MessageBox.Show("Tento souboru neexistuje!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(adressFile != "")
            {
                BigInteger n = 1;
                BigInteger d = 1;
                string hash = Sha256();

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Key files (*.priv)|*.priv";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string tmp = "";
                    string text = File.ReadAllText(ofd.FileName);
                    for (int i = 0; i < text.Length; i++) // loading key
                    {
                        if(text[i] != ' ' && text[i] != '\r')
                        {
                            tmp += text[i];
                        }
                        else
                        {
                            if(n == 1)
                            { 
                                n = BigInteger.Parse(tmp);
                                tmp = "";
                            }
                            else
                            {
                                d = BigInteger.Parse(tmp);
                                tmp = "";
                                break;
                            }
                        }
                    }
                    try    // file work
                    {
                        string path = Directory.GetCurrentDirectory() + "\\final";
                        if (Directory.Exists(path))
                        {
                            File.Copy(adressFile, Path.Combine(path, "message.msg"), true);
                        }
                        else
                        {
                            Directory.CreateDirectory(path);
                            File.Copy(adressFile, Path.Combine(path,"message.msg"), true);
                        }
                    
                        File.WriteAllText(Path.Combine(path,"end.sign"), encodeFunction(n, d, hash));
                        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "final.zip")))
                        {
                            try
                            {
                                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "final.zip"));
                            }
                            catch (IOException exception)
                            {
                                MessageBox.Show(exception.Message, "Error");
                                return;
                            }
                        }
                       
                        ZipFile.CreateFromDirectory(path, Directory.GetCurrentDirectory() + "\\final.zip");
                        Directory.Delete(path, true);
                        MessageBox.Show("OK", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Chyba: " + exception.ToString(), "Chyba", MessageBoxButtons.OK,
                                                                     MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nepovedlo se načíst soubor");
                }
            }
            else
            {
                MessageBox.Show("Nejprve vyber vstupní soubor");
            }
        }

        private string Sha256()
        {
            string sha256OfFile = "";
            try
            {
                FileStream myFilestream = File.OpenRead(adressFile);
                SHA256Managed mySHA256Managed = new SHA256Managed();
                byte[] byteArrayOfSHA256 = mySHA256Managed.ComputeHash(myFilestream);
                sha256OfFile = BitConverter.ToString(byteArrayOfSHA256).Replace("-", String.Empty).ToLower();
                myFilestream.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Chyba: " + exception.ToString(), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sha256OfFile;
        }
        private string encodeFunction(BigInteger n, BigInteger d, string word)
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
                BigInteger c = BigInteger.ModPow(m, d, n);      // samotné kodování
                code += c + " ";                                // uložení do stringu code a přidání mezery pro oddělení jednotlivných "slov"

            }

            return code;                            // navrácení zakodované zprávy
        }

        private string decodeFunction(BigInteger n, BigInteger e, string word)
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
                    BigInteger m = BigInteger.ModPow(c, e, n);      // provedu dekodovani
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

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Zip files | *.zip";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                adressFile1 = ofd.FileName;
                textBox1.Text = adressFile1;
                FileInfo f = new FileInfo(adressFile1);
                var extens = Path.GetExtension(adressFile1);
                long size = f.Length;
                string name = f.Name;
                string fname = f.FullName;
                var time = f.CreationTime;
                var type = f.Attributes;
                textBox2.Text = "Name: " + name +"\r\nExtension: "+ extens + "\r\nSize: " + size + " bytes\r\nCreate time: " 
                                                    + time + "\r\nType: " + type + "\r\nFullname: " + fname;
            }
        }

        private void button4_Click(object sender, EventArgs ex)
        {
            if (adressFile1 != "")
            {
                string mes = "";
                string hash = "";
                BigInteger n = 1;
                BigInteger e = 1;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "unzip");

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Key files (*.pub)|*.pub";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string tmp = "";
                    string text = File.ReadAllText(ofd.FileName);
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] != ' ' && text[i] != '\r')
                        {
                            tmp += text[i];
                        }
                        else
                        {
                            if (n == 1)
                            {
                                n = BigInteger.Parse(tmp);
                                tmp = "";
                            }
                            else
                            {
                                e = BigInteger.Parse(tmp);
                                tmp = "";
                                break;
                            }
                        }
                    }
                }
                try
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "unzip"));
                    ZipFile.ExtractToDirectory(adressFile1, path);
                    
                    adressFile = Path.Combine(path, "message.msg");
                    hash = Sha256();
                    string encrypthMes = File.ReadAllText(Path.Combine(path, "end.sign"));
                    mes = decodeFunction(n, e, encrypthMes);

                    Directory.Delete(path, true);
                    ofd.Dispose();

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Chyba: " + exception.ToString(), "Chyba", MessageBoxButtons.OK,
                                                                 MessageBoxIcon.Error);
                }

                if (String.Equals(hash, mes))
                {
                    MessageBox.Show("Původní zpráva","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Falešná zpráva","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nevybrali jste ZIP soubor", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e) // generování klíčů
        {
            RSA myRSA = new RSA();

            try
            {
                FileInfo f = new FileInfo("key.priv");
                StreamWriter w = f.CreateText();
                w.WriteLine("{0} {1}", myRSA.GetN(), myRSA.GetD());
                w.Close();

                FileInfo g = new FileInfo("key.pub");
                StreamWriter wr = g.CreateText();
                wr.WriteLine("{0} {1}", myRSA.GetN(), myRSA.GetE());
                wr.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Chyba: " + exception.ToString(), "Chyba", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Error);
            }
        }
    }
}
