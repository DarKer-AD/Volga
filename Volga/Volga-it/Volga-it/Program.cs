using System;
using System.IO;

namespace Volga_it
{
    class Program
    {
        static void Main(string[] args)
        {

            string s;
            string[] words = new string[1000];
            int i = 0, count = 0;
            FileStream f = new FileStream("F://file1.html", FileMode.Open); //Изменить путь к файлу
            StreamReader r = new StreamReader(f);

            while (!r.EndOfStream)
            {
                s = r.ReadLine();
                s = SpecSimbol(s);                              //Вызов подпрораммы удаления спецсимволов
                s = Rem(s);                                     //Вызов подпрограммы удаления тегов
                string[] word = s.Split(new char[] { ' ' });    //Создание массива слов строки
                for (int j = 0; j < word.Length; j++)
                {
                    if (word[j] != "")
                    {
                        words[i] = word[j];                     //Создание массива всех слов
                        i++;
                    }
                }
            }

            for (i = 0; i < words.Length; i++)
            {
                if (words[i] != null)
                    count++;                                    //Посчет количества слов
            }

            string[] Words = new string[count];                 //Создание строкового массива нужной величины 

            for (i = 0; i < count; i++)
            {
                Words[i] = words[i].ToLower();                  //Занесение слов в новый массив с изменением регистра
            }

            Words = RemSimbol(Words);                           //Вызов подпрограммы для удаления символов таких как знаки препинания и прочие
            string[,] Itog = new string[count, 2];
            Itog = Podschet(Words, Itog);                       //Подсчет количества уникальных слов
            Itog = Poryadok(Itog, count);                       //Составление правильного порядка слов(Сортировка)

            Console.WriteLine("Уникальные слова и их количество в порядке убывания:");
            Console.WriteLine();

            for (i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < 2; j++)
                        Console.Write("{0,3}", Itog[i, j]);     //Вывод итоговой таблицы уникальных слов
                Console.WriteLine();
            }
            r.Close();
            Console.ReadKey();
        }

        //Удаление спецсимволов
        static string SpecSimbol(string s)
        {
            int Count = 0, i;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i] == '&')                            //Поиск начала спецсимвола
                {
                    string d = s.Remove(0, i);
                    for (int m = 0; m < d.Length; m++)
                    {
                        if (d[m] == ';')                    //Поиск конца спецсимвола
                        {
                            Count = m;
                            break;
                        }
                    }
                    s = s.Remove(i + 1, Count);
                }
            }
            s = s.Replace("&", " ");
            return s;
        }

        //Удаление HTML тегов
        static string Rem(string s)
        {
            int CountI = 0, CountJ = 0, i, j = 0;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i] == '<')            //Поиск начала тега
                {
                    j = i;
                    CountI++;
                }
                if (s[i] == '>')            //Поиск конца тега
                    CountJ++;
                if ((CountI > 0) && (CountJ > 0))
                {
                    s = s.Remove(j, i - j + 1);
                    s = Rem(s);
                    break;
                }
            }
            return s;
        }

        //Удаление символов(Знаки припинания и прочие)
        static string[] RemSimbol(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                for (int j = 0; j < word.Length; j++)
                    if (word[j] == ',' || word[j] == '.' || word[j] == '?' || word[j] == '!' || word[j] == ':' || word[j] == '"' || word[j] == '(' || word[j] == ')' || word[j] == ';' || word[j] == '[' || word[j] == ']' || word[j] == '\n' || word[j] == '\r' || word[j] == '\t')
                        word = word.Remove(j, 1);
                words[i] = word;
            }
            return words;
        }

        //Подсчет количества уникальных слов
        static string[,] Podschet(string[] words, string[,] Itog)
        {
            for (int i = 0; i < words.Length; i++)
            {
                int Count = 0;
                if (words[i] != null)
                {
                    string word = words[i];
                    for (int j = 0; j < words.Length; j++)
                        if (word == words[j])
                        {
                            Itog[i, 0] = word;
                            Count++;
                            words[j] = null;
                        }
                    Itog[i, 1] = Convert.ToString(Count);
                }
            }
            return Itog;
        }

        //Составление правильного порядка(сортировка по убыванию)
        static string[,] Poryadok(string[,] Itog, int count)
        {
            string itog, itog2;
            for (int i = 0; i < count; i++)
            {
                int temp = Convert.ToInt32(Itog[i, 1]);
                for (int j = 0; j < count; j++)
                {
                    if(temp >= Convert.ToInt32(Itog[j, 1]))
                    {
                        itog = Itog[i, 0];
                        itog2 = Itog[i, 1];
                        Itog[i, 0] = Itog[j, 0];
                        Itog[i, 1] = Itog[j, 1];
                        Itog[j, 0] = itog;
                        Itog[j, 1] = itog2;
                    }
                }
            }
            return Itog;
        }
    }
}