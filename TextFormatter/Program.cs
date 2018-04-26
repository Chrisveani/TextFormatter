using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextFormatter
{
    public class TextFormatter
    {
        public string Justify(string text, int width)
        {
            string withEndText = text + " $";
            string[] splittedText = withEndText.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string s = "";
            int sum = 0;
            StringGenerator sg = new StringGenerator();
            List<string> words = new List<string>();
            for (int i = 0; i < splittedText.Length; i++)
            {
                if (splittedText[i] == "$")
                {
                    if (words.Count != 0)
                        if (words.Count == 1)
                            s += words[0];
                        else
                        {
                            for (int j = 0; j < words.Count - 1; j++)
                            {
                                s += words[j] + " ";
                            }
                            s += words[words.Count - 1];
                        }
                }
                else
                {
                    words.Add(splittedText[i]);
                    sum += splittedText[i].Length;
                    if (sum > width - (words.Count - 1))//сумма должна быть меньше, чем указанная длина строки минус минимальное число пробелов. минимальное число пробелов = кол-во слов в строке минус один
                    {
                        if (words.Count > 2)
                        {
                            words.RemoveAt(words.Count - 1);
                            i--;
                            s += sg.MakeString(words, width);
                            sum = 0;
                            words.Clear();
                        }
                        if (words.Count == 2)
                        {
                            words.RemoveAt(words.Count - 1);
                            i--;
                            s += words[0] + "\r\n";//для вывода  в файл
                            //s += words[0] + '\n';
                            sum = 0;
                            words.Clear();
                        }
                        if (words.Count == 1)
                        {
                            s += words[0] + "\r\n";//для вывода  в файл
                            //s += words[0] + '\n';
                            sum = 0;
                            words.Clear();
                        }
                    }
                }
            }
            return s;
        }
    }

    public class StringGenerator//распределитель пробелов в строке
    {
        public string MakeString(List<string> words, int width)
        {
            string s = "";
            int strLength = 0;
            for (int i = 0; i < words.Count; i++)
            {
                strLength += words[i].Length;
            }
            double sepCount = width - strLength;
            double sepLength = sepCount / (words.Count - 1);
            int freeSpace = Convert.ToInt32(Math.Floor(sepLength));
            int moreFreeSpace = Convert.ToInt32(sepCount - ((words.Count - 1) * freeSpace));
            string smallSep = "";
            for (int i = 0; i < freeSpace; i++)
            { smallSep += " "; }
            for (int i = 0; i < words.Count - 1; i++)
            {
                if (i < moreFreeSpace)
                {
                    s += words[i] + smallSep + " ";
                }
                else
                {
                    s += words[i] + smallSep;
                }
            }
            s += words[words.Count - 1];
            return s + "\r\n"; // для вывода в файл 
            //return s + '\n';

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            string tempText = sr.ReadToEnd();
            sr.Close();
            Console.WriteLine("Enter width");
            int n = int.Parse(Console.ReadLine());
            TextFormatter tf = new TextFormatter();
            tempText = tf.Justify(tempText, n);
            StreamWriter sw = new StreamWriter(@"output.txt", false, Encoding.Default);
            sw.WriteLine(tempText);
            sw.Close();
            Console.WriteLine(tempText);
            Console.ReadKey();
        }
    }
}
