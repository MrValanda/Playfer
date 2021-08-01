using System;
using System.Linq;

namespace ShifrPleyfera
{
    class Program
    {
        static void Main(string[] args)
        {
          
            int itemMenu;
            do
            {
                Console.WriteLine("1) Зашифровать");
                Console.WriteLine("2) Расшифровать");
                Console.WriteLine("3) Выход");
                do
                {
                    itemMenu = Convert.ToInt32(Console.ReadLine());
                } while (itemMenu < 0 || itemMenu > 3);

                switch (itemMenu)
                {
                    case 1:
                    {
                        Console.WriteLine("Введите ключ:");
                        string key = new string(Console.ReadLine().ToUpper().Where(x => char.IsLetter(x)).Distinct()
                            .ToArray());
                        string alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                        Console.WriteLine("Введите текст:");
                        string str = new string((key + alphabet).Distinct().ToArray());
                        string text = new string(Console.ReadLine().Where(x => char.IsLetter(x)).ToArray()).ToUpper();
                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text[i] == text[i + 1])
                                if (text[i] != 'Я')
                                    text = text.Insert(i + 1, "Я");
                                else
                                    text = text.Insert(i + 1, "Ъ");
                        }
                        string temp = EncryptText(text, str);

                        if (text.Length % 2 != 0)
                            if (text[text.Length - 1] == 'Я')
                            {
                                text += "Ъ";
                            }
                            else text += "Я";

                        for (int i = 0; i < text.Length-1; i+=2)
                        {
                            Console.Write(text[i].ToString()+text[i+1].ToString()+" ");
                        }

                        Console.WriteLine();
                        for (int i = 0; i < temp.Length-1; i+=2)
                        {
                            Console.Write(temp[i].ToString()+temp[i+1].ToString()+" ");
                        }

                        Console.WriteLine();
                        Console.WriteLine(temp);
                    }
                        break;
                    case 2:
                    {
                        Console.WriteLine("Введите ключ:");
                        string key = new string(Console.ReadLine().ToUpper().Where(x => char.IsLetter(x)).Distinct()
                            .ToArray());
                        string alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                        Console.WriteLine("Введите текст:");
                        string str = new string((key + alphabet).Distinct().ToArray());
                        string text = new string(Console.ReadLine().Where(x => char.IsLetter(x)).ToArray()).ToUpper();
                        string temp = DecryptText(text, str);
                        for (int i = 0; i < text.Length-1; i+=2)
                        {
                            Console.Write(text[i].ToString()+text[i+1].ToString()+" ");
                        }

                        Console.WriteLine();
                        for (int i = 0; i < temp.Length-1; i+=2)
                        {
                            Console.Write(temp[i].ToString()+temp[i+1].ToString()+" ");
                        }

                        Console.WriteLine();
                        Console.WriteLine(temp);
                    }
                        break;
                }
            } while (itemMenu!=3);
        }

        static string EncryptText(string text,string tempStr)
        {
            char[,] encryptTable=new char[8,4];
            string encryptText = "";
            for (int i = 0; i < encryptTable.GetLength(0); i++)
            {
                for (int j = 0; j < encryptTable.GetLength(1); j++)
                {
                    encryptTable[i, j] = tempStr[i * encryptTable.GetLength(1) + j];
                    Console.Write(encryptTable[i,j]);
                }

                Console.WriteLine();
            }
             
            for (int i = 0; i < text.Length-1; i+=2)
            {
                var firstElementIndexs = FindIndexs(encryptTable,text[i]);
                var secondElementIndexs = FindIndexs(encryptTable,text[i+1]);
                if (firstElementIndexs.Item2 != secondElementIndexs.Item2)
                {
                    if (firstElementIndexs.Item1 == secondElementIndexs.Item1) 
                        encryptText +=encryptTable[Mod(firstElementIndexs.Item1,encryptTable.GetLength(0)), Mod(firstElementIndexs.Item2 + 1,encryptTable.GetLength(1))].ToString() +
                                      encryptTable[Mod(secondElementIndexs.Item1,encryptTable.GetLength(0)),Mod(secondElementIndexs.Item2 + 1,encryptTable.GetLength(1))].ToString();
                            
                    else
                    {
                        encryptText +=
                            encryptTable[Mod(firstElementIndexs.Item1, encryptTable.GetLength(0)), Mod(secondElementIndexs.Item2, encryptTable.GetLength(1))]
                                .ToString() + encryptTable[Mod(secondElementIndexs.Item1, encryptTable.GetLength(0)),
                                Mod(firstElementIndexs.Item2, encryptTable.GetLength(1))].ToString();
                    }
                }
                else
                {
                    encryptText +=
                        encryptTable[Mod(firstElementIndexs.Item1 + 1, encryptTable.GetLength(0)), Mod(firstElementIndexs.Item2, encryptTable.GetLength(1))].ToString() +
                        encryptTable[Mod(secondElementIndexs.Item1 + 1, encryptTable.GetLength(0)), Mod(secondElementIndexs.Item2, encryptTable.GetLength(1))].ToString();
                }
            }
            return encryptText;
        }

        static string DecryptText(string text, string tempStr)
        {
            char[,] decryptTable = new char[8, 4];
            string decryptText = "";
            for (int i = 0; i < decryptTable.GetLength(0); i++)
            {
                for (int j = 0; j < decryptTable.GetLength(1); j++)
                {
                    decryptTable[i, j] = tempStr[i * decryptTable.GetLength(1) + j];
                    Console.Write(decryptTable[i, j]);
                }

                Console.WriteLine();
            }


            for (int i = 0; i < text.Length - 1; i += 2)
            {
                var firstElementIndexs = FindIndexs(decryptTable, text[i]);
                var secondElementIndexs = FindIndexs(decryptTable, text[i + 1]);
                if (firstElementIndexs.Item2 != secondElementIndexs.Item2)
                {
                    if (firstElementIndexs.Item1 == secondElementIndexs.Item1)
                        decryptText +=
                            decryptTable[Mod(firstElementIndexs.Item1, decryptTable.GetLength(0)), Mod(firstElementIndexs.Item2 - 1, decryptTable.GetLength(1))]
                                .ToString() +
                            decryptTable[Mod(secondElementIndexs.Item1, decryptTable.GetLength(0)), Mod(secondElementIndexs.Item2 - 1, decryptTable.GetLength(1))]
                                .ToString();

                    else //Прямоугольник
                    {
                        decryptText +=
                            decryptTable[Mod(firstElementIndexs.Item1, decryptTable.GetLength(0)), Mod(secondElementIndexs.Item2, decryptTable.GetLength(1))]
                                .ToString() + decryptTable[Mod(secondElementIndexs.Item1, decryptTable.GetLength(0)),
                                Mod(firstElementIndexs.Item2, decryptTable.GetLength(1))].ToString();
                    }
                }
                else //находятся в 1 столбце 
                {
                    decryptText +=
                        decryptTable[Mod(firstElementIndexs.Item1 - 1, decryptTable.GetLength(0)), Mod(firstElementIndexs.Item2, decryptTable.GetLength(1))]
                            .ToString() +
                        decryptTable[Mod(secondElementIndexs.Item1 - 1, decryptTable.GetLength(0)), Mod(secondElementIndexs.Item2, decryptTable.GetLength(1))]
                            .ToString();
                }
            }

            return decryptText;
        }

        static int Mod(int first, int second) => first % second < 0 ? second + first % second : first % second; // нормальный %
        
        static (int, int) FindIndexs(char[,] table,char symbol)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] == symbol)
                        return (i, j);
                }
            }

            return (0, 0);
        }
    }
}
