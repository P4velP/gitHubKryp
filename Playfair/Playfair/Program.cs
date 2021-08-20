using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playfair
{
    public class Matrix2DArray
    {

        private string keyArray;
        private string stringArray;


        public Matrix2DArray(string KeyArray, string StringArray) // konsturktor
        {
            keyArray = KeyArray;
            stringArray = StringArray;
        }
        public string RepairWord(string inputWord)
        {
            string word = "";
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(inputWord);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            asciiStr = asciiStr.ToUpper();
            asciiStr = asciiStr.Replace('W', 'V');

            asciiStr = asciiStr.Replace("1", "XJQV");
            asciiStr = asciiStr.Replace("2", "XDQV");
            asciiStr = asciiStr.Replace("3", "XTQV");
            asciiStr = asciiStr.Replace("4", "XCQV");
            asciiStr = asciiStr.Replace("5", "XPQV");
            asciiStr = asciiStr.Replace("6", "XSQV");
            asciiStr = asciiStr.Replace("7", "XQSV");
            asciiStr = asciiStr.Replace("8", "XOQV");
            asciiStr = asciiStr.Replace("9", "XNQV");
            asciiStr = asciiStr.Replace("0", "XQDV");


            for (int j = 0; j < asciiStr.Length; j++)
            {
                if (Convert.ToInt32(asciiStr[j]) >= 65 && Convert.ToInt32(asciiStr[j]) <= 90)
                {
                    word += asciiStr[j];
                }
                else if (Convert.ToInt32(' ') == Convert.ToInt32(asciiStr[j]))
                {
                    word += "XQV";
                }
            }

            return SeparateWord(word);
        } // oprava vstupniho retezce

        private string SeparateWord(string word)
        {

            int delka = word.Length;
            string correctWord = "";
            for (int i = 0; i < delka - 1; i++)
            {

                if (word[i] == word[i + 1] && word[i] != 'X')
                {
                    correctWord += word[i];
                    correctWord += "X";
                }
                else if (word[i] == word[i + 1] && word[i] == 'X')
                {
                    correctWord += word[i];
                    correctWord += "Q";
                }
                else
                {
                    correctWord += word[i];
                    correctWord += word[i + 1];

                    i++;
                }

                if (delka - 2 == i)
                {
                    correctWord += word[i + 1];
                }
            }

            if (correctWord.Length % 2 == 1)
            {
                if (correctWord[correctWord.Length - 1] != 'X')
                {
                    correctWord += 'X';
                }
                else
                {
                    correctWord += 'Q';
                }
            }
            return correctWord;
        }
        public char[,] KeyWordFilling(string keyword) // doplnieni znaku do klice
        {
            int delka = keyword.Length;
            for (int i = 65; i <= 90; i++)
            {
                if (i == 87)
                {
                    i++;
                }
                if (keyword.Length < 26)
                {
                    for (int j = 0; j < delka; j++)
                    {
                        if (keyword[j] == Convert.ToChar(i))
                        {
                            break;
                        }
                        else if (delka - 1 == j)
                        {
                            keyword += Convert.ToChar(i);
                            break;
                        }

                    }
                }
            }
            char[,] arrayTable = new char[5, 5];
            for (int i = 0; i < arrayTable.GetLength(0); i++)
            {
                for (int j = 0; j < arrayTable.GetLength(1); j++)
                {
                    arrayTable[i, j] = Convert.ToChar(keyword.Substring(arrayTable.GetLength(0) * i + j, 1));
                }
            }
            return arrayTable;
        }

        public void CreateMatrix(char[,] Mat2DArray, DataGridView dataGridView)
        {


            int rows = Mat2DArray.GetLength(0);
            int columns = Mat2DArray.GetLength(1);

            dataGridView.ColumnCount = columns;

            for (int r = 0; r < rows; r++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                for (int c = 0; c < columns; c++)
                {
                    row.Cells[c].Value = Mat2DArray[r, c];
                }

                dataGridView.Rows.Add(row);
            }
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToAddRows = false;

        } // vytvoreni datagridu

        public string RepairKeyWord(string word) // oprava kličového slova
        {
            string keyword = "";
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(word);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            asciiStr = asciiStr.ToUpper();
            asciiStr = asciiStr.Replace('W', 'V');

            for (int i = 0; i < asciiStr.Length; i++)
            {
                if (Convert.ToInt32(asciiStr[i]) >= 65 && Convert.ToInt32(asciiStr[i]) <= 90)
                {
                    if (keyword.Length == 0)
                    {
                        keyword += asciiStr[i];
                    }
                    else
                    {
                        for (int j = 0; j < keyword.Length; j++)
                        {
                            if (keyword[j] == asciiStr[i])
                            {
                                break;
                            }
                            else if (keyword.Length - 1 == j)
                            {
                                keyword += asciiStr[i];
                            }
                        }
                    }
                }
                else
                {
                    i++;
                }
            }
            return keyword;
        }

        public string Matrix2DEncoder(char[,] mat2DArray, string word)
        {
            // char[,] mat2DArray = GetMat2DArray();
            int iter = 0;
            string encodeWord = "";


            while (iter < word.Length)
            {
                char znak1 = word[iter];
                char znak2 = word[iter + 1];
                int xZnak1 = -1;
                int yZnak1 = -1;
                int xZnak2 = -1;
                int yZnak2 = -1;
                bool z1 = false;
                bool z2 = false;

                for (int i = 0; i < mat2DArray.GetLength(0); i++)  // for rows
                {
                    for (int j = 0; j < mat2DArray.GetLength(1); j++) // for columns
                    {
                        if (znak1 == mat2DArray[i, j])
                        {
                            xZnak1 = j;
                            yZnak1 = i;
                            z1 = !z1;
                        }

                        if (znak2 == mat2DArray[i, j])
                        {
                            xZnak2 = j;
                            yZnak2 = i;
                            z2 = !z2;
                        }
                        if (z1 && z2)
                        {
                            break;
                        }
                    }
                    if (z1 && z2)
                    {
                        break;
                    }
                }

                if (xZnak1 == xZnak2)
                {
                    // columnCode
                    encodeWord += mat2DArray[(yZnak1 + 1) % 5, xZnak1];
                    encodeWord += mat2DArray[(yZnak2 + 1) % 5, xZnak2];
                }
                else if (yZnak1 == yZnak2)
                {
                    // rowCode
                    encodeWord += mat2DArray[yZnak1, (xZnak1 + 1) % 5];
                    encodeWord += mat2DArray[yZnak2, (xZnak2 + 1) % 5];

                }
                else
                {
                    // squareCode
                    encodeWord += mat2DArray[yZnak1, xZnak2];
                    encodeWord += mat2DArray[yZnak2, xZnak1];
                }
                iter += 2;
            }
            return encodeWord;
        }

        public string Matrix2DDecoder(char[,] mat2DArray, string word) //
        {
            int iter = 0;
            string decodeWord = "";

            while (iter < word.Length)
            {
                char znak1 = word[iter];
                char znak2 = word[iter + 1];
                int xZnak1 = -1;
                int yZnak1 = -1;
                int xZnak2 = -1;
                int yZnak2 = -1;
                bool z1 = false;
                bool z2 = false;

                for (int i = 0; i < mat2DArray.GetLength(0); i++)  // for rows
                {
                    for (int j = 0; j < mat2DArray.GetLength(1); j++) // for columns
                    {
                        if (znak1 == mat2DArray[i, j])
                        {
                            xZnak1 = j;
                            yZnak1 = i;
                            z1 = !z1;
                        }

                        if (znak2 == mat2DArray[i, j])
                        {
                            xZnak2 = j;
                            yZnak2 = i;
                            z2 = !z2;
                        }
                        if (z1 && z2)
                        {
                            break;
                        }
                    }
                    if (z1 && z2)
                    {
                        break;
                    }
                }

                if (xZnak1 == xZnak2)
                {
                    // columnCode
                    int y1 = (yZnak1 - 1) % 5;
                    if (y1 < 0)
                    {
                        y1 += 5;
                    }
                    int y2 = (yZnak2 - 1) % 5;
                    if (y2 < 0)
                    {
                        y2 += 5;
                    }
                    decodeWord += mat2DArray[y1, xZnak1];
                    decodeWord += mat2DArray[y2, xZnak2];
                }
                else if (yZnak1 == yZnak2)
                {
                    // rowCode
                    int x1 = (xZnak1 - 1) % 5;
                    if (x1 < 0)
                    {
                        x1 += 5;
                    }
                    int x2 = (xZnak2 - 1) % 5;
                    if (x2 < 0)
                    {
                        x2 += 5;
                    }
                    decodeWord += mat2DArray[yZnak1, x1];
                    decodeWord += mat2DArray[yZnak2, x2];

                }
                else
                {
                    // squareCode
                    decodeWord += mat2DArray[yZnak1, xZnak2];
                    decodeWord += mat2DArray[yZnak2, xZnak1];
                }
                iter += 2;
            }
            return RStSE(decodeWord);
        }
        private string RStSE(string word)// replace string to something else
        {
            string renewWord = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == 'X' && word.Length - 1 >= (i + 2))
                {
                    if (word[i + 1] == 'Q' && word[i + 2] == 'V')
                    {
                        renewWord += " ";
                        i += 2;
                    }else if(word[i] == 'X' && word.Length -1 >= (i + 3))
                    {
                        if(word[i+1] == 'J' && word[i+2] == 'Q' && word[i+3] == 'V')
                        {
                            renewWord += '1';
                            i += 3;
                        }
                        else if (word[i + 1] == 'D' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '2';
                            i += 3;
                        }
                        else if (word[i + 1] == 'T' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '3';
                            i += 3;
                        }
                        else if (word[i + 1] == 'C' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '4';
                            i += 3;
                        }
                        else if (word[i + 1] == 'P' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '5';
                            i += 3;
                        }
                        else if (word[i + 1] == 'S' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '6';
                            i += 3;
                        }
                        else if (word[i + 1] == 'Q' && word[i + 2] == 'S' && word[i + 3] == 'V')
                        {
                            renewWord += '7';
                            i += 3;
                        }
                        else if (word[i + 1] == 'O' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '8';
                            i += 3;
                        }
                        else if (word[i + 1] == 'N' && word[i + 2] == 'Q' && word[i + 3] == 'V')
                        {
                            renewWord += '9';
                            i += 3;
                        }
                        else if (word[i + 1] == 'Q' && word[i + 2] == 'D' && word[i + 3] == 'V')
                        {
                            renewWord += '0';
                            i += 3;
                        }
                        else
                        {
                            renewWord += word[i];
                        }
                    }
                    else
                    {
                        renewWord += word[i];
                    }

                }
                else
                {
                    renewWord += word[i];
                }
            }
            if(renewWord[renewWord.Length-2] == 'X' && renewWord[renewWord.Length - 1] == 'Q')
            {
                renewWord = renewWord.Remove(renewWord.Length - 1,1);
            }

            return renewWord;
        }
    }







    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
