using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode8
{
    public partial class DayBase
    {
        public enum Direction { Left, Up, Right, Down }

        public static Logger Logg = new Logger();

        public static List<string> GetInput(string name)
        {
            try
            {
                var match = Regex.Match(name, "(\\d{4})_");
                if (match.Success)
                    name = $"{match.Groups[1]}\\{name}";
                return File.ReadAllLines($"..\\..\\..\\input\\{name}.txt").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<string>();
            }
        }


        public static List<string> GetInput(int day)
        {
            return GetInput(day.ToString());
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static void Print(char[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void Print(int[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] == 0 ? '.' : '#');
                }
                Console.WriteLine();
            }
        }

        public static void DrawMap(char[,] map, Position? current = null, HashSet<Position>? visited = null, int sleep = 0, bool clear = true)
        {
            if (!Logg.DoLog)
                return;
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            if (clear)
                Console.Clear();
            for (var l = 0; l < lines; l++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var v = visited?.SingleOrDefault(v => v.Line == l && v.Col == c);
                    var draw = v != null ? 'O' : current != null && current.Line == l && current.Col == c ? 'x' : map[l, c];
                    var fg = Console.ForegroundColor;
                    if (draw == 'x')
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (draw == 'O')
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{(draw == 0 ? '.' : draw)}");
                    Console.ForegroundColor = fg;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            if (sleep > 0)
                Thread.Sleep(sleep);
        }

        public static T[,] Rotate<T>(T[,] array)
        {
            var result = new T[array.GetLength(1), array.GetLength(0)];
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    result[j, array.GetLength(0) - i - 1] = array[i, j];
                }
            }
            return result;
        }

        public class Logger
        {
            public bool DoLog { get; set; } = true;

            internal void Write(FormattableString text)
            {
                if (DoLog)
                    Console.Write(text.ToString());
            }

            internal void Write(object text)
            {
                if (DoLog)
                    Console.Write(text.ToString());
            }

            internal void WriteLine(FormattableString text)
            {
                if (DoLog)
                    Console.WriteLine(text.ToString());
            }

            internal void WriteLine(object text)
            {
                if (DoLog)
                    Console.WriteLine(text.ToString());
            }

            internal void WriteLine()
            {
                if (DoLog)
                    Console.WriteLine();
            }
        }

        public record LongPoint(long X, long Y)
        {
            public long ManhattanDistance(LongPoint p2)
            {
                return Math.Abs(X - p2.X) + Math.Abs(Y - p2.Y);
            }
        }

    }

    public static class Extensions
    {
        public static bool Contains(this Tuple<int,int> interval, int value)
        {
            return value >= interval.Item1 && value <= interval.Item2;
        }

        public static int ManhattanDistance(this Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
        {
            var minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + 1);
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static char[,] ToCharArray(this List<string> input)
        {
            var result = new char[input.Count, input[0].Length];
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    result[i, j] = input[i][j];
                }
            }
            return result;
        }

        public static List<long> Primes(this long value)
        {
            var primes = new List<long>();
            for (var i = 2L; i < Math.Sqrt(value); i++)
            {
                if (value % i == 0)
                    primes.Add(i);
            }
            if (!primes.Any())
                primes.Add(value);

            return primes;
        }

    }


}
