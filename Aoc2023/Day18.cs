using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day18 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            //Logg.DoLog = false;

            var input = GetInput("2023_18s");
            var plan = GetPlan(input);
            var map = GetMap(plan);
            DrawMap(map);

            var plan2 = GetPlan2(input);
            var trenchLenght = plan2.Sum(p => p.start.ManhattanDistance(p.end));
            var nodes = plan2.Select(p => p.start).ToList();
            var area = Position.ShoelaceArea(nodes);

            DrawMap(map);
            Logg.WriteLine();

            var filled = FillMap(map);

            Console.WriteLine($"Contains: {filled}. 48975 too low");
            Console.WriteLine($"Shoelace: {trenchLenght + area}. ");
            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int FillMap(List<PositionString> map)
        {
            var filled = map.Count;
            var minLine = map.Min(p => p.Line);
            var maxLine = map.Max(p => p.Line);
            var doLog = Logg.DoLog;
            //Logg.DoLog = true;
            for (var line = minLine; line <= maxLine; line++)
            {
                var filling = false;
                var lastDirection = Direction.Down;
                var posOnLine = map.Where(p => p.Line == line).ToList();
                DrawMap(posOnLine);
                var minCol = posOnLine.Min(p => p.Col);
                var maxCol = posOnLine.Max(p => p.Col);
                Logg.WriteLine($"Line: {line} ({maxLine}), minCol: {minCol}, maxCol: {maxCol}. Filled {filled}");
                for (var col = minCol; col <= maxCol; col++)
                {
                    var pos = posOnLine.FirstOrDefault(p => p.Line == line && p.Col == col);
                    if (pos == null)
                    {
                        Logg.Write($"{(filling ? '#' : ' ')}");
                        if (filling)
                            filled++;
                        continue;
                    }

                    Logg.Write($"#");

                    var (up, down, left, right) = pos.GetSurrounding(map);
                    if (left == null)
                    {
                        if (right == null) // " # "
                        {
                            filling = !filling;
                            continue;
                        }
                        lastDirection = up != null ? Direction.Up : Direction.Down;
                        continue;
                    }

                    if (right != null)
                        continue;
                    var newDirection = up != null ? Direction.Up : Direction.Down;
                    if (newDirection != lastDirection)
                        filling = !filling;
                    lastDirection = newDirection;
                }
                Logg.WriteLine();
                Logg.WriteLine($"After {filled}");
            }
            Logg.DoLog = doLog;
            return filled;
        }

        private int FillMap2(List<PositionString> map)
        {
            var minLine = map.Min(p => p.Line);
            var maxLine = map.Max(p => p.Line);
            var doLog = Logg.DoLog;
            Logg.DoLog = true;
            for (var line = minLine; line <= maxLine; line++)
            {
                var filling = false;
                var posOnLine = map.Where(p => p.Line == line).ToList();
                DrawMap(posOnLine);
                var minCol = posOnLine.Min(p => p.Col);
                var maxCol = posOnLine.Max(p => p.Col);
                Logg.WriteLine($"Line: {line} ({maxLine}), minCol: {minCol}, maxCol: {maxCol}");
                for (var col = minCol; col <= maxCol; col++)
                {
                    var pos = posOnLine.FirstOrDefault(p => p.Line == line && p.Col == col);
                    if (filling)
                    {
                        if (pos == null)
                        {
                            if (!map.Any(p => p.Line == line && p.Col == col))
                                map.Add(new PositionString(line, col, ""));
                        }
                        else
                            filling = false;
                        continue;
                    }

                    if (pos == null)
                    {
                        continue;
                    }

                    var next = map.FirstOrDefault(p => p.Line == line && p.Col == col + 1);
                    filling = next == null;
                }
                posOnLine = map.Where(p => p.Line == line).ToList();
                DrawMap(posOnLine);
                Logg.WriteLine();
            }
            Logg.DoLog = doLog;
            DrawMap(map);
            return map.Count;
        }

        private void DrawMap(List<PositionString> map)
        {
            if (Logg.DoLog == false)
                return;
            for (var line = map.Min(p => p.Line); line <= map.Max(p => p.Line); line++)
            {
                for (var col = map.Min(p => p.Col); col <= map.Max(p => p.Col); col++)
                {
                    var pos = map.FirstOrDefault(p => p.Line == line && p.Col == col); 
                    Logg.Write($"{(pos != null ? '#' : ' ')}");
                }
                Logg.WriteLine();
            }
        }
        

        private List<PositionString> GetMap(List<(Direction direction, int steps, string color)> plan)
        {
            var map = new List<PositionString>();
            var pos = new PositionString(0, 0, plan.First().color);
            map.Add(pos);
            foreach (var (direction, steps, color) in plan)
            {
                for (var i = 0; i < steps; i++)
                {
                    pos = direction switch
                    {
                        Direction.Up => new PositionString(pos.Line - 1, pos.Col, color),
                        Direction.Down => new PositionString(pos.Line + 1, pos.Col, color),
                        Direction.Left => new PositionString(pos.Line, pos.Col - 1, color),
                        Direction.Right => new PositionString(pos.Line, pos.Col + 1, color),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    if (!map.Any(p => p.Line == pos.Line && p.Col == pos.Col))
                        map.Add(pos);
                }
            }
            return map;
        }

        private List<(Direction direction, int steps, string color)> GetPlan(List<string> input)
        {
            var plan = new List<(Direction direction, int steps, string color)>();
            foreach (var line in input)
            {
                var split = line.Split(' ');
                var direction = split[0] == "U" ? Direction.Up : split[0] == "D" ? Direction.Down : split[0] == "L" ? Direction.Left : Direction.Right;
                var steps = int.Parse(split[1]);   
                plan.Add((direction, steps, split[2]));
            }
            return plan;
        }

        private List<(Position start, Position end)> GetPlan2(List<string> input, bool useHex = false)
        {
            var plan = new List<(Position start, Position end)>();
            var start = new Position(0, 0);
            foreach (var line in input)
            {
                var split = line.Split(' ');
                var direction = split[0].First();
                var steps = int.Parse(split[1]);
                var end = direction switch
                {
                    'U' => new Position(start.Line - steps, start.Col),
                    'D' => new Position(start.Line + steps, start.Col),
                    'L' => new Position(start.Line, start.Col - steps),
                    'R' => new Position(start.Line, start.Col + steps),
                    _ => throw new ArgumentOutOfRangeException()
                };
                plan.Add((start, end));
                start = end;
            }
            return plan;
        }
    }
}
