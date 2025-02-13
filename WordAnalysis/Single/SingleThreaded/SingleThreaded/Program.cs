using System.Text;
using System.Text.RegularExpressions;

namespace SingleThreaded
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the text file using a stream reader.
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sigmund-Freud_-_Detskata_dusha_-_8233-b.txt");
            Console.OutputEncoding = Encoding.UTF8;

            // Read the stream as a string.
            string text = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            List<string> words = ExtractWords(text);
            double avgWordLength = CalculateAverage(words);
            string longWord = longestWord(words);
            string shortWord = shortestWord(words);

            Console.WriteLine($"The count of the words are:{words.Count}");

            Console.WriteLine($"Longest words is:{longWord}");
            Console.WriteLine($"Shortest words is:{shortWord}");
            Console.WriteLine($"Avarage words length is:{avgWordLength}");

            List<string> mostCommonWords = FindMostCommonWords(words, 5);
            List<string> leastCommonWords = FindLeastCommonWords(words, 5);

            Console.WriteLine($"Most common words:{string.Join(", ", mostCommonWords)}");
            Console.WriteLine($"Least common words:{string.Join(", ", leastCommonWords)}");

            Console.ReadLine();


        }

        static double CalculateAverage(List<string> words)
        {
            int totalLength = 0;
            foreach (string word in words)
            {
                totalLength += word.Length;
            }

            double avarage = (double)totalLength / words.Count;
            return avarage;
        }

        private static string longestWord(List<string> words)
        {
            string longest = words[0];
            foreach (string word in words)
            {
                if (word.Length >= longest.Length)
                {
                    longest = word;
                }
            }
            return longest;
        }

        private static string shortestWord(List<string> words)
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

        static List<string> FindLeastCommonWords(List<string> words, int count)
        {
            Dictionary<string, int> frequency = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (frequency.ContainsKey(word))
                    frequency[word]++;
                else
                    frequency[word] = 1;
            }

            List<string> leastCommon = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string minWord = null;
                int minCount = int.MaxValue;

                foreach (var pair in frequency)
                {
                    if (pair.Value < minCount)
                    {
                        minCount = pair.Value;
                        minWord = pair.Key;
                    }
                }

                if (minWord != null)
                {
                    leastCommon.Add(minWord);
                    frequency.Remove(minWord);
                }
            }
            return leastCommon;
        }

        static List<string> FindMostCommonWords(List<string> words, int count)
        {
            Dictionary<string, int> frequency = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (frequency.ContainsKey(word))
                    frequency[word]++;
                else
                    frequency[word] = 1;
            }

            List<string> mostCommon = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string maxWord = null;
                int maxCount = 0;

                foreach (var pair in frequency)
                {
                    if (pair.Value > maxCount)
                    {
                        maxCount = pair.Value;
                        maxWord = pair.Key;
                    }
                }

                if (maxWord != null)
                {
                    mostCommon.Add(maxWord);
                    frequency.Remove(maxWord);
                }
            }
            return mostCommon;
        }
    }
}
