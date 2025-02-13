using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MultiThreaded
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"Sigmund-Freud_-_Detskata_dusha_-_8233-b.txt";
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                int wordCount = 0;
                string shortWord = "";
                string longWord = "";
                double avgWordLength = 0;
                List<string> mostCommonWords = new List<string>();
                List<string> leastCommonWords = new List<string>();

                string text = File.ReadAllText(filePath, Encoding.UTF8);
                List<string> words = ExtractWords(text);

                Thread threadCount = new Thread(() => wordCount = words.Count);
                Thread threadShortestWord = new Thread(() => shortWord = ShortestWord(words));
                Thread threadLongestWord = new Thread(() => longWord = LongestWord(words));
                Thread threadCalculatedAvarage = new Thread(() => avgWordLength = CalculateAverage(words));
                Thread threadMostCommonWords = new Thread(() => mostCommonWords = FindMostCommonWords(words, 5));
                Thread threadLeasrCommonWords = new Thread(() => leastCommonWords = FindLeastCommonWords(words, 5));

                threadCount.Start();
                threadShortestWord.Start();
                threadLongestWord.Start();
                threadCalculatedAvarage.Start();
                threadMostCommonWords.Start();
                threadLeasrCommonWords.Start();

                threadCount.Join();
                threadShortestWord.Join();
                threadLongestWord.Join();
                threadCalculatedAvarage.Join();
                threadMostCommonWords.Join();
                threadLeasrCommonWords.Join();

                Console.WriteLine($"Total words: {wordCount}");
                Console.WriteLine($"Shortest word: {shortWord}");
                Console.WriteLine($"Longest word: {longWord}");
                Console.WriteLine($"Average word length: {avgWordLength}");
                Console.WriteLine($"Most common words: {string.Join(", ", mostCommonWords)}");
                Console.WriteLine($"Least common words: {string.Join(", ", leastCommonWords)}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            Console.ReadLine();
        }

        static List<string> ExtractWords(string text)
        {
            List<string> words = new List<string>();
            MatchCollection matches = Regex.Matches(text.ToLower(), @"\b[а-я]{3,}\b");

            foreach (Match match in matches)
            {
                words.Add(match.Value);
            }
            return words;
        }

        static double CalculateAverage(List<string> words)
        {
            int totalLength = 0;
            foreach (string word in words)
            {
                totalLength += word.Length;
            }
            return words.Count > 0 ? (double)totalLength / words.Count : 0;
        }

        static string LongestWord(List<string> words)
        {
            string longest = words[0];
            foreach (string word in words)
            {
                if (word.Length > longest.Length)
                {
                    longest = word;
                }
            }
            return longest;
        }

        static string ShortestWord(List<string> words)
        {
            string shortest = words[0];
            foreach (string word in words)
            {
                if (word.Length <= shortest.Length)
                {
                    shortest = word;
                }
            }
            return shortest;
        }

        static List<string> FindMostCommonWords(List<string> words, int count)
        {
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (frequency.ContainsKey(word))
                {
                    frequency[word]++;
                }
                else
                {
                    frequency[word] = 1;
                }
            }

            List<string> mostCommon = new List<string>();
            foreach (var pair in frequency.OrderByDescending(p => p.Value).Take(count))
            {
                mostCommon.Add(pair.Key);
            }

            return mostCommon;
        }

        static List<string> FindLeastCommonWords(List<string> words, int count)
        {
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (frequency.ContainsKey(word))
                {
                    frequency[word]++;
                }

                else
                {
                    frequency[word] = 1;
                }
            }

            List<string> leastCommon = new List<string>();
            foreach (var pair in frequency.OrderBy(p => p.Value).Take(count))
            {
                leastCommon.Add(pair.Key);
            }

            return leastCommon;
        }
    }
}
