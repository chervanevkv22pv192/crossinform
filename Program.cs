using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        public static bool IsLetter(char c)
        {
            return ('а' <= c) && (c <= 'я');
        }
        public static int GetIndexForNet(char a1, char a2, char a3)
        {
            return a3 - 'а' + (a2 - 'а') * 33 + (a1 - 'а') * 33 * 33;
        }
        public class StringAndMatrix
        {
            string str;
            TripletAndCount[] triplets;
            public StringAndMatrix(string rowString, ref TripletAndCount[] mat)
            {
                str = rowString;
                triplets = mat;
            }
            public void ProcessLine()
            {
                if (str.Length > 2) //если в строке нет даже 3 символов, скипаем
                {
                    string line = str.ToLower(); //нужно избавиться от верхнего регистра, чтоб в сравнениях символов использовать только строчные буквы 
                    int i = 2;
                    while (i < line.Length) //проходимся по всем триплетам
                    {
                        if (IsLetter(line[i - 2]) && IsLetter(line[i - 1]) && IsLetter(line[i])) //все 3 символа должны быть буквами
                        {
                            triplets[GetIndexForNet(line[i - 2], line[i - 1], line[i])].count++; //нашли триплет - добавили +1 к количеству его вхождений в тексте
                        }
                        i++;
                    }
                }
            }
        }
        public struct TripletAndCount
        {
            public string triplet;
            public int count;
        }
        public static int Compare(TripletAndCount x, TripletAndCount y) //компаратор для метода сортировки
        {
            if (x.count == y.count)
                return 0;
            if (x.count > y.count)
                return 1;
            else
                return -1;
        }
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            TripletAndCount[] triplets = new TripletAndCount[33 * 33 * 33];
            for (int i = 0; i < 33; i++)
                for (int j = 0; j < 33; j++)
                    for (int k = 0; k < 33; k++)
                        triplets[i * 33 * 33 + j * 33 + k].triplet = "" + (char)('а' + i) + (char)('а' + j) + (char)('а' + k);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //if (line.Length > 2)
                        //{
                        //    line = line.ToLower();
                        //    int i = 2;
                        //    while (i < line.Length)
                        //    {
                        //        if (IsLetter(line[i - 2]) && IsLetter(line[i - 1]) && IsLetter(line[i]))
                        //        {
                        //            triplets[GetIndexForNet(line[i - 2], line[i - 1], line[i])].count++;
                        //        }
                        //        i++;
                        //    }
                        //}

                        //Вообще, в задании было сказано, что программа должна быть многопоточной, но многопоточность тут тормозит выполнение программы
                        //Можете раскомменить код выше, а 3 следующие строки, наоборот, закомментить, программа отработает в разы быстрее
                        Thread thread = new Thread(new StringAndMatrix(line, ref triplets).ProcessLine);
                        thread.Start();
                        thread.Join();
                    }
                }
                Array.Sort(triplets, Compare);
                stopwatch.Stop();
                Console.WriteLine("На выполнение затрачено " + stopwatch.ElapsedMilliseconds + " мс.\nНаиболее часто встречающиеся триплеты:");
                for (int i = 0; i < 10; i++)
                    if (triplets[triplets.Length - 1 - i].count > 0)
                        Console.WriteLine(triplets[triplets.Length - 1 - i].triplet + " " + triplets[triplets.Length - 1 - i].count + " вхождений");
            }
            catch(Exception e) { Console.WriteLine(e.Message); }

        }
    }
}
