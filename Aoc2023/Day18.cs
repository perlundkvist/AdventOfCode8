using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day18 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_18");
            var plan = GetPlan(input);
            var map = GetMap(plan);
            var area = Position.ShoelaceArea(map);

            var picks = area + map.Count / 2 + 1; // Pick's theorem
            Console.WriteLine($"Contains: {picks}. 49897 is correct"); 

            //DrawMap(map);

            Console.WriteLine();
            plan = GetPlan(input, true);
            var map2 = GetMap2(plan);
            //map2.ForEach(Console.WriteLine);
            var area2 = DayBase.ShoelaceArea(map2);
            var trenchArea = plan.Sum(p => p.steps);
            var picks2 = area2 + trenchArea / 2 + 1;
            Console.WriteLine($"Contains: {picks2}.");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int FillMap(List<Position> map)
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
                Logg.WriteLine();
            }
            Logg.DoLog = doLog;
            return map.Count;
        }

        private void DrawMap(List<Position> map)
        {
            if (Logg.DoLog == false)
                return;
            for (var line = map.Min(p => p.Line); line <= map.Max(p => p.Line); line++)
            {
                for (var col = map.Min(p => p.Col); col <= map.Max(p => p.Col); col++)
                {
                    if (col == 0 && line == 0)
                    {
                        Logg.Write('S');
                        continue;
                    }
                    var pos = map.FirstOrDefault(p => p.Line == line && p.Col == col); 
                    Logg.Write($"{(pos != null ? '#' : ' ')}");
                }
                Logg.WriteLine();
            }
        }
        

        private List<Position> GetMap(List<(Direction direction, long steps)> plan)
        {
            var map = new List<Position>();
            var pos = new Position(0, 0);
            map.Add(pos);
            foreach (var (direction, steps) in plan)
            {
                for (long i = 0; i < steps; i++)
                {
                    pos = direction switch
                    {
                        Direction.Up => new Position(pos.Line - 1, pos.Col),
                        Direction.Down => new Position(pos.Line + 1, pos.Col),
                        Direction.Left => new Position(pos.Line, pos.Col - 1),
                        Direction.Right => new Position(pos.Line, pos.Col + 1),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    if (!map.Any(p => p.Line == pos.Line && p.Col == pos.Col))
                        map.Add(pos);
                }
            }
            return map;
        }

        private List<DPoint> GetMap2(List<(Direction direction, long steps)> plan)
        {

            var map = new List<DPoint>();
            var pos = new DPoint(0, 0);
            map.Add(pos);
            foreach (var (direction, steps) in plan)
            {
                pos = direction switch
                {
                    Direction.Up => new DPoint(pos.X, pos.Y - steps),
                    Direction.Down => new DPoint(pos.X, pos.Y + steps),
                    Direction.Left => new DPoint(pos.X - steps, pos.Y),
                    Direction.Right => new DPoint(pos.X + steps, pos.Y),
                    _ => throw new ArgumentOutOfRangeException()
                    };
                if (!map.Any(p => p.X == pos.X && p.Y == pos.Y))
                    map.Add(pos);
            }
            return map;

            //var directions = new[] { Direction.Right, Direction.Down, Direction.Left, Direction.Up };
            //foreach (var line in input)
            //{
            //    var split = line.Split(' ');
            //    var dir = split[2][7];
            //    var direction = directions[int.Parse(dir.ToString())];
            //    var steps =  Convert.ToInt64(split[2][2..7], 16);
            //    //plan.Add((direction, steps, split[2]));
            //}

            //var map = new List<DPoint>();
            //var pos = new DPoint(0, 0);
            //map.Add(pos);
            //foreach (var (direction, steps, color) in plan)
            //{
            //    var step = Convert.ToInt64(color[2..^1], 16);
            //    for (var i = 0; i < step; i++)
            //    {
            //        pos = direction switch
            //        {
            //            Direction.Up => new DPoint(pos.X, pos.Y - 1),
            //            Direction.Down => new DPoint(pos.X, pos.Y + 1),
            //            Direction.Left => new DPoint(pos.X - 1, pos.Y),
            //            Direction.Right => new DPoint(pos.X + 1, pos.Y),
            //            _ => throw new ArgumentOutOfRangeException()
            //        };
            //        if (!map.Any(p => p.X == pos.X && p.Y == pos.Y))
            //            map.Add(pos);
            //    }
            //}
            return map;
        }

        private List<(Direction direction, long steps)> GetPlan(List<string> input, bool useHex = false)
        {
            var directions = new[] { Direction.Right, Direction.Down, Direction.Left, Direction.Up };
            var plan = new List<(Direction direction, long steps)>();
            foreach (var line in input)
            {
                var split = line.Split(' ');
                Direction direction;
                long steps;
                if (useHex)
                {
                    var dir = split[2][7];
                    direction = directions[int.Parse(dir.ToString())];
                    steps = Convert.ToInt64(split[2][2..7], 16);
                }
                else { 
                    direction = split[0] == "U" ? Direction.Up : split[0] == "D" ? Direction.Down : split[0] == "L" ? Direction.Left : Direction.Right;
                    steps = long.Parse(split[1]);
                }
                plan.Add((direction, steps));
            }
            return plan;
        }

    }
}
