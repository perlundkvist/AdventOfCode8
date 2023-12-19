using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode8
{
    public partial class DayBase
    {
        public enum Direction { Left, Right, Up, Down }

        public static Logger Logg = new Logger();

        public static List<string> GetInput(string name)
        {
            try
            {
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

        public class Logger
        {
            public bool DoLog { get; set; } = true;

            internal void Write(FormattableString text)
            {
                if (DoLog)
                    Console.Write(text.ToString());
            }

            internal void WriteLine(FormattableString text)
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

        public record Position(int Line, int Col)
        {
            public int ManhattanDistance(Position p2) => Math.Abs(Line - p2.Line) + Math.Abs(Col - p2.Col);

            public (Position? up, Position? down, Position? left, Position? right) GetSurrounding(IEnumerable<Position> map)
            {
                return new(map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col),
                    map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col),
                    map.FirstOrDefault(p => p.Line == Line && p.Col == Col - 1),
                    map.FirstOrDefault(p => p.Line == Line && p.Col == Col + 1));
            }
        }

        public record PositionInt(int Line, int Col, int Value) : Position(Line, Col) 
        {
            public PositionInt(int line, int col, string value) : this(line, col, int.Parse(value))
            {

            }
        }

        public record PositionChar(int Line, int Col, char Value) : Position(Line, Col)
        {
        }

        public record PositionString(int Line, int Col, string Value) : Position(Line, Col)
        {
        }

        public record Position<T>(int Line, int Col, T Value) : Position(Line, Col)
        {
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

    }


}
