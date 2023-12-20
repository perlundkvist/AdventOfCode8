using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day16 : DayBase
    {
        private static int BeamId = 0;

        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_16");

            Logg.DoLog = false;
            var sum = GetSum(input, new(0, 0, Direction.Right, input));
            Console.WriteLine($"Sum: {sum}. 7059 too low");

            Logg.DoLog = false;
            sum = GetSum2(input);
            Console.WriteLine($"Sum2: {sum}.");


            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int GetSum2(List<string> layout)
        {
            var sum = 0;

            var lines = layout.Count;
            var cols = layout[0].Length;
            for ( var l = 0; l < lines; l++)
            {
                Logg.WriteLine($"Line {l}");
                sum = Math.Max(sum, GetSum(layout, new (l, 0, Direction.Right, layout)));
                sum = Math.Max(sum, GetSum(layout, new(l, cols - 1, Direction.Left, layout)));
            }

            for (var c = 0; c < cols; c++)
            {
                Logg.WriteLine($"Col {c}");
                sum = Math.Max(sum, GetSum(layout, new(0, c, Direction.Down, layout)));
                sum = Math.Max(sum, GetSum(layout, new(lines -1, c, Direction.Up, layout)));
            }

            return sum;
        }

        private int GetSum(List<string> layout, Beam firstBeam)
        {
            var lines = layout.Count;
            var cols = layout[0].Length;
            var map = new int[lines, cols];
            var triedPositions = new List<Position>();
            var beams = new List<Beam> { firstBeam };

            while (true)
            {
                var beam = beams.FirstOrDefault();
                if (beam == null)
                    break;

                map[beam.Line, beam.Col]++;
                Logg.Write($"{beam} -> ");
                var newBeam = beam.Move();
                if (beam.Line < 0 || beam.Line >= lines || beam.Col < 0 || beam.Col >= cols)
                {
                    Logg.WriteLine($"Done!");
                    Logg.WriteLine();
                    beams.Remove(beam);
                }
                else {
                    var tried = triedPositions.FirstOrDefault(p => p.Line == beam.Line && p.Col == beam.Col);
                    if (tried != null && tried.Directions.Contains(beam.Direction))
                    {
                        Logg.WriteLine($"Tried before {beam}");
                        Logg.WriteLine();
                        beams.Remove(beam);
                    }
                    else
                    {
                        if (tried == null)
                        {
                            tried = new Position(beam);
                            triedPositions.Add(tried);
                        }
                        Logg.WriteLine($"{beam}");
                        tried.Directions.Add(beam.Direction);
                    }
                }

                if (newBeam != null)
                {
                    beams.Add(newBeam);
                    Logg.WriteLine($"Added {newBeam}");
                }
            }

            //DrawMap(map);

            var sum = 0;
            foreach (var item in map)
            {
                sum += item > 0 ? 1 : 0;
            }
            return sum;
        }

        private void DrawMap(int[,] map)
        {
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            for (var l = 0; l < lines; l++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var item = map[l, c];
                    Console.Write(item > 0 ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        private class Beam(int line, int col, Direction direction, List<string> layout)
        {
            private readonly int id = Day16.BeamId++;
            public int Line { get; private set; } = line;
            public int Col { get; private set; } = col;
            
            public Direction Direction { get; private set; } = direction;

            public Beam? Move()
            {
                var tile = layout[Line][Col];
                Beam? newBeam = null;
                Logg.Write($" {tile} ");
                switch (Direction) {
                    case Direction.Left:
                        switch (tile)
                        {
                            case '|':
                                if (Line > 0)
                                    newBeam = new Beam(Line - 1, Col, Direction.Up, layout);
                                Line++;
                                Direction = Direction.Down;
                                break;
                            case '\\':
                                Line--;
                                Direction = Direction.Up;
                                break;
                            case '/':
                                Line++;
                                Direction = Direction.Down;
                                break;
                            default:
                                Col--;
                                break;
                        }
                        break;
                    case Direction.Right:
                        switch (tile)
                        {
                            case '|':
                                if (Line > 0)
                                    newBeam = new Beam(Line - 1, Col, Direction.Up, layout);
                                Line++;
                                Direction = Direction.Down;
                                break;
                            case '\\':
                                Line++;
                                Direction = Direction.Down;
                                break;
                            case '/':
                                Line--;
                                Direction = Direction.Up;
                                break;
                            default:
                                Col++;
                                break;
                        }
                        break;
                    case Direction.Up:
                        switch (tile)
                        {
                            case '-':
                                if (Col > 0)
                                    newBeam = new Beam(Line, Col - 1, Direction.Left, layout);
                                Col++;
                                Direction = Direction.Right;
                                break;
                            case '\\':
                                Col--;
                                Direction = Direction.Left;
                                break;
                            case '/':
                                Col++;
                                Direction = Direction.Right;
                                break;
                            default:
                                Line--;
                                break;
                        }
                        break;
                    case Direction.Down:
                        switch (tile)
                        {
                            case '-':
                                if (Col > 0)
                                    newBeam = new Beam(Line, Col - 1, Direction.Left, layout);
                                Col++;
                                Direction = Direction.Right;
                                break;
                            case '\\':
                                Col++;
                                Direction = Direction.Right;
                                break;
                            case '/':
                                Col--;
                                Direction = Direction.Left;
                                break;
                            default:
                                Line++;
                                break;
                        }
                        break;
                };
                return newBeam;
            }

            public override string ToString()
            {
                return $"{id}: Line {Line + 1}, col {Col + 1} {Direction}";
            }
        }

        private class Position(Beam beam)
        {
            public int Line { get; } = beam.Line;
            public int Col { get; } = beam.Col;

            public readonly List<Direction> Directions = [];
        }
    }
}
