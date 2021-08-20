using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        string adressFile;
        int maxStringSize;

    private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (!radioButton1.Checked)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
        }


       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP Image | *.bmp";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                adressFile = ofd.FileName;
            }

            if (adressFile != null)
            {
                pictureBox1.Load(adressFile);
                textBox2.Text = adressFile;
                if (pictureBox1.Image != null)
                {
                    maxStringSize = pictureBox1.Image.Width * pictureBox1.Image.Height / 8 - 1;
                    textBox7.Text = Convert.ToString(pictureBox1.Image.Width + " x " + pictureBox1.Image.Height);
                    textBox11.Text = Convert.ToString(pictureBox1.Image.Width * pictureBox1.Image.Height);
                    textBox13.Text = Convert.ToString(maxStringSize + " znaků");

                }
            }
            else
            {
                MessageBox.Show("Musíte vybrat obrázek", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        
        private void button3_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                if (textBox1.Text.Length != 0)
                {
                    if (maxStringSize >= textBox1.Text.Length)
                    {
                        kodovani(adressFile);
                    }
                    else
                    {
                        MessageBox.Show("Zpráva je příliš dlouhá nebo obrázek je příliš malý na ukrytí této zprávy!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Musíte zadat vstupní řetězec","Chyba", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (radioButton2.Checked == true)
            {
                dekodovani(adressFile);
            }
            else
            {
                MessageBox.Show("Nevybrali jste žádnou funkci programu", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        byte bitArrayToByte(BitArray myBitArray)
        {
            byte[] myByteArray = new byte[1];

            myBitArray.CopyTo(myByteArray, 0);  

            return myByteArray[0];
        }
        private void kodovani(string adressFile)
        {
            BitArray myBitArray = new BitArray(0);
            int asciHodnota;
            Bitmap myBitmap = new Bitmap(adressFile);
            Color pixel;
            int intValue;
            int i = 0;
            int err = 0;
            int numbOfLoadChar = 0;

            for (int y = 0; y < myBitmap.Height; y++)   // cyklus pro vertikalni pokus v Bitmap
            {
                for (int x = 0; x < myBitmap.Width; x++, i++)   // cylkus pro horizontalni posun v Bitmap
                {
                    if (numbOfLoadChar < textBox1.Text.Length)  // podminka která kontroluje počet zakodovaných znaku : zbyvajícím znakům v textBoxu
                    {
                        asciHodnota = Convert.ToInt32(textBox1.Text[numbOfLoadChar]); // převede znak na čislo 
                        myBitArray = DecConvertToBin(asciHodnota);              // volani funkce která převede číslo na pole bitu
                        pixel = myBitmap.GetPixel(x, y);                
                        intValue = pixel.B;                                
                        if (intValue % 2 == 0 && myBitArray[i] == true) // slozena podminka jestli cislo je sude a budeme vkladat 1 jako posledni bit
                        {
                            intValue += 1;
                        }
                        else if ((intValue % 2) == 1 && myBitArray[i] == false) // slozena podminka jestli cislo je liche a budeme vkladat 0 jako posledni bit
                        {
                            intValue -= 1;
                        }

                        myBitmap.SetPixel(x, y, Color.FromArgb(pixel.R, pixel.G, intValue)); // ulozi upraveny bit z5 do bitmapy
                    }
                    else if (numbOfLoadChar == textBox1.Text.Length)  // podminka pokud jsem ulozil vsechny znaky v rezezci
                    {                                                 // prida znak null na konec
                        asciHodnota = Convert.ToInt32(null);
                        myBitArray = DecConvertToBin(asciHodnota);

                        err = 1;                                       // promena err nastavuje na 1 abych mohl po ulozený celého znaku opustit for cykly
                        pixel = myBitmap.GetPixel(x, y);
                        intValue = pixel.B;

                        if (intValue % 2 == 0 && myBitArray[i] == true)
                        {
                            intValue += 1;
                        }
                        else if ((intValue % 2) == 1 && myBitArray[i] == false)
                        {
                            intValue -= 1;
                        }

                        myBitmap.SetPixel(x, y, Color.FromArgb(pixel.R, pixel.G, intValue));

                    }

                    if (i == 7 && err == 0)     // podminka která mi nulluje i což je iterator na bity v myBitArray
                    {

                        numbOfLoadChar++;
                        i = -1;
                    }
                    if (i == 7 && err == 1) break; // ulozil jsem posledni bit ze znaku null opouštim cyklus

                }
                if (err != 0) break; // opousim i vnejsi cylkus
            }
            MessageBox.Show("Zpráva ukryta", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information); // oznameni že jsem dokončil ukryvani zpravy
            
                         
            SaveFileDialog file = new SaveFileDialog(); // ulozeni obrazku s ukrytou zpravou
            file.Filter = "BMP Image | *.bmp";

            if (file.ShowDialog() == DialogResult.OK)
            {
                myBitmap.Save(file.FileName);
                pictureBox2.Load(file.FileName);
            }
        }

        private void dekodovani(string adressFile)
        {
            Bitmap myBitmap = new Bitmap(adressFile);
            Color pixel;
            int intValue;
            int i = 0;
            int err = 0;
           
            bool[] myBoolArray = new bool[8];
            string myStringArray = "";

            for (int k = 0; k <= myBitmap.Height - 1; k++)
            {
                for (int j = 0; j <= myBitmap.Width - 1; j++, i++)
                {
                    pixel = myBitmap.GetPixel(j, k);
                    intValue = pixel.B;

                    if ((intValue & 1) == 0)
                    {
                        myBoolArray[i] = false;
                    }
                    else if ((intValue & 1) == 1)
                    {
                        myBoolArray[i] = true;
                    }

                    if (i == 7) // po přečtení 7mi bitu
                    {
                        BitArray myBitArray = new BitArray(myBoolArray);
                        char myChar = Convert.ToChar(Convert.ToInt32(bitArrayToByte(myBitArray)));  // převedu na znak
                        myStringArray += Convert.ToString(myChar);
                        i = -1;                                                                     // hodnota -1 protože for cyklus
                        if (!Convert.ToBoolean(Convert.ToInt32(myChar)))                             // hlida null znak
                        {
                            err = 1;
                            break;
                        }
                    }
                }
                if (err != 0) break;
            }
            textBox4.Text = myStringArray;  // vypis retezce do texBoxu
        }
   
        BitArray DecConvertToBin(int asciHodnota)  
        {

            bool[] myBoolArray = new bool[8];   // funkce pro prevod dekadickeho cisla na binarni "cislo" pole
            int decNumber = asciHodnota;
            for (int i = 0; i < 8; i++)
            {
                if (decNumber > 0)
                {
                    myBoolArray[i] = Convert.ToBoolean(decNumber % 2);
                    decNumber = decNumber / 2;
                }
                else
                {
                    myBoolArray[i] = false;
                }
            }
            BitArray myBitArray = new BitArray(myBoolArray);
            return myBitArray;
        }





        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            ;
        }
    }
}

