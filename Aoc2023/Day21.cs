using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode8;

namespace AdventOfCode8.Aoc2023
{
    internal class Day21 : DayBase
    {

        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_21");

            var map = GetMap(input);
            DrawMap(map);

            var startLine = input.First(l => l.Contains("S"));
            var startPos = new Position(input.IndexOf(startLine), startLine.IndexOf("S"));
            var tried = new HashSet<Try>();
            Move(startPos, 64, map, tried);
            DrawMap(map);

            Console.WriteLine($"Garden plots: {GetCount(map)}");

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int GetCount(char[,] map)
        {
            var count = 0;
            foreach (var c in map)
            {
                count += c switch
                {
                    'O' => 1,
                    _ => 0
                };
            }
            return count;
        }

        private void Move(Position pos, int steps, char[,] map, HashSet<Try> tried)
        {
            if (steps == 0)
            {
                map[pos.Line, pos.Col] = 'O';
                return;
            }
            var (up, down, left, right) = pos.GetSurrounding(map);
            var next = up;
            if (ShouldTry(next, steps, Direction.Up, map, tried))
                Move(next, steps - 1, map, tried);
            next = down;
            if (ShouldTry(next, steps, Direction.Down, map, tried))
                Move(next, steps - 1, map, tried);
            next = left;
            if (ShouldTry(next, steps, Direction.Left, map, tried))
                Move(next, steps - 1, map, tried);
            next = right;
            if (ShouldTry(next, steps, Direction.Right, map, tried))
                Move(next, steps - 1, map, tried);
        }

        private bool ShouldTry(Position? next, int steps, Direction direction, char[,] map, HashSet<Try> tried)
        {
            if (next == null || !IsGarden(next, map))
                return false;
            var tryPos = new Try(next.Line, next.Col, steps, direction);
            if (tried.Contains(tryPos))
                return false;
            Logg.WriteLine($"Try {tryPos}");
            tried.Add(tryPos);
            return true;
        }

        private bool IsGarden(Position pos, char[,] map)
        {
            var c = map[pos.Line, pos.Col];
            return c switch
            {
                'S' or 'O' or '.' => true,
                _ => false
            };
        }

        private void DrawMap(char[,] map)
        {
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            for (var l = 0; l < lines; l++)
            {
                for (var c = 0; c < cols; c++)
                {
                    Logg.Write($"{map[l,c]}");
                }
                Logg.WriteLine();
            }
        }

        private char[,] GetMap(List<string> input)
        {
            var map = new char[input.Count, input.First().Length];
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            for (var l = 0; l < lines; l++)
            {
                var line = input[l];
                for (var c = 0; c < cols; c++)
                {
                    map[l,c] = line[c];
                }
            }
            return map;
        }

        private record Try (int Line, int Col, int Steps, Direction Direction) 
        {
        }
    }
}
