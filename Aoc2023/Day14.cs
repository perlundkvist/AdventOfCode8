using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day14 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_14");
            var rocks = GetRocks(input);
            var sum = GetSum(rocks);
            Console.WriteLine($"Sum: {sum}.");

            sum = GetSum2(rocks);
            Console.WriteLine($"Sum: {sum}.");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int GetSum2(List<Rock> rocks)
        {
            var cycles = 0;
            var newRocks = rocks;
            var rocksList = new List<List<Rock>> { rocks };
            var maxLine = rocks.Max(r => r.Line);
            var maxCol = rocks.Max(r => r.Col);

            //var id 

            while (true)
            {
                cycles++;
                newRocks = Cycle(newRocks);
                if (rocksList.Any(l => l.SequenceEqual(newRocks)))
                    break;
                rocksList.Add(newRocks);
                if (cycles % 1000 == 0)
                    Console.WriteLine($"{cycles} cycles.");
            }
            var first = rocksList.First(l => l.SequenceEqual(newRocks));
            var index1 = rocksList.IndexOf(first);
            var cycleLen = rocksList.Count - index1 + 1;
            var index = (1000000000 - index1) % cycleLen + index1;
            Console.WriteLine($"{index - 1} cycles {GetLoad(rocksList[index - 1])}");
            Console.WriteLine($"{index + 1} cycles {GetLoad(rocksList[index + 1])}");
            return GetLoad(rocksList[index]);
        }

        private List<Rock> Cycle(List<Rock> rocks)
        {
            var newRocks = new List<Rock>(rocks.Select(r => r.Clone()).ToList());
            var equals = rocks.SequenceEqual(newRocks);
            TiltNorth(newRocks);
            TiltWest(newRocks);
            TiltSouth(newRocks);
            TiltEast(newRocks);

            return newRocks;
        }

        private int GetSum(List<Rock> rocks)
        {
            TiltNorth(rocks);
            return GetLoad(rocks);
        }

        private static int GetLoad(List<Rock> rocks)
        {
            return rocks.Where(r => r.Shape == 'O').Sum(r => r.Line);
        }

        private void TiltNorth(List<Rock> rocks)
        {
            var maxNorth = rocks.Max(r => r.Line);
            for (var col = 1; col <= rocks.Max(r => r.Col); col++)
            {
                var rocksInCol = rocks.Where(r => r.Col == col).ToList();
                while (true)
                {
                    var moved = false;
                    foreach (var rock in rocksInCol.Where(r => r.Shape == 'O' && r.Line != maxNorth))
                    {
                        if (rocksInCol.Any(r => r.Line == rock.Line + 1))
                            continue;
                        rock.Line++;
                        moved = true;
                    }
                    if (!moved)
                        break;
                }
            }
        }

        private void TiltSouth(List<Rock> rocks)
        {
            for (var col = 1; col <= rocks.Max(r => r.Col); col++)
            {
                var rocksInCol = rocks.Where(r => r.Col == col).ToList();
                while (true)
                {
                    var moved = false;
                    foreach (var rock in rocksInCol.Where(r => r.Shape == 'O' && r.Line != 1))
                    {
                        if (rocksInCol.Any(r => r.Line == rock.Line - 1))
                            continue;
                        rock.Line--;
                        moved = true;
                    }
                    if (!moved)
                        break;
                }
            }
        }

        private void TiltEast(List<Rock> rocks)
        {
            var maxEast = rocks.Max(r => r.Col);
            for (var line = 1; line <= rocks.Max(r => r.Line); line++)
            {
                var rocksInLine = rocks.Where(r => r.Line == line).ToList();
                while (true)
                {
                    var moved = false;
                    foreach (var rock in rocksInLine.Where(r => r.Shape == 'O' && r.Col != maxEast))
                    {
                        if (rocksInLine.Any(r => r.Col == rock.Col + 1))
                            continue;
                        rock.Col++;
                        moved = true;
                    }
                    if (!moved)
                        break;
                }
            }
        }

        private void TiltWest(List<Rock> rocks)
        {
            for (var line = 1; line <= rocks.Max(r => r.Line); line++)
            {
                var rocksInLine = rocks.Where(r => r.Line == line).ToList();
                if (rocksInLine.All(r => r.Shape != 'O'))
                    continue;   
                while (true)
                {
                    var moved = false;
                    foreach (var rock in rocksInLine.Where(r => r.Shape == 'O' && r.Col != 1))
                    {
                        if (rocksInLine.Any(r => r.Col == rock.Col - 1))
                            continue;
                        rock.Col--;
                        moved = true;
                    }
                    if (!moved)
                        break;
                }
            }
        }
        private List<Rock> GetRocks(List<string> input)
        {
            input.Reverse();
            var rocks = new List<Rock>();
            var l = 1;
            foreach (var line in input )
            {
                var c = 1;
                foreach (var rock in line)
                {
                    if (rock == 'O' || rock == '#')
                        rocks.Add(new Rock(l, c, rock));
                    c++;
                }
                l++;
            }
            return rocks;
        }
    
        private void Print(List<Rock> rocks)
        {
            for (int l = rocks.Max(r => r.Line); l > 0; l--)
            {
                for (var i = 1; i <= rocks.Max(r => r.Col); i++)
                {
                    Console.Write($"{rocks.FirstOrDefault(r => r.Line == l && r.Col == i)?.Shape ?? '.'}");
                }
                Console.WriteLine($"  {l,3}");
            }
            Console.WriteLine();
        }

    }

    

    public class Rock(int line, int col, char shape) : IEquatable<Rock>
    {
        public int Line { get; set; } = line;
        public int Col { get; set; } = col;
        public char Shape { get; } = shape;

        public Rock Clone()
        {
            return new Rock(Line, Col, Shape);
        }

        public bool Equals(Rock? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Line == other.Line && Col == other.Col && Shape == other.Shape;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rock)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line, Col, Shape);
        }
    }
}
