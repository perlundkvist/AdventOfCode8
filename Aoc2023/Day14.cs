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
            Print(rocks);
            var sum = GetSum(rocks);
            Console.WriteLine($"Sum: {sum}.");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int GetSum(List<Rock> rocks)
        {
            TiltNorth(rocks);
            Print(rocks);
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
                        rock.Line++;
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
                for (var i = 1; i < rocks.Max(r => r.Col); i++)
                {
                    Console.Write($"{rocks.FirstOrDefault(r => r.Line == l && r.Col == i)?.Shape ?? '.'}");
                }
                Console.WriteLine($"  {l:000}");
            }
            Console.WriteLine();
        }

    }

    

    public class Rock(int line, int col, char shape)
    {
        public int Line { get; set; } = line;
        public int Col { get; } = col;
        public char Shape { get; } = shape;
    }
}
