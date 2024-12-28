﻿namespace AdventOfCode8.Aoc2023
{
    internal class Day17 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_17s");
            var map = GetMap(input);
            
            Logg.DoLog = false;

            var cost = GetCost(map);
            Console.WriteLine($"Cost: {cost}"); // 48975 too high

            //sum = GetSum2(input);
            //Console.WriteLine($"Sum2: {sum}.");


            Console.WriteLine($"{DateTime.Now - start}"); 
        }

        private int[, ] GetMap(List<string> layout)
        {
            var lines = layout.Count;
            var cols = layout[0].Length;
            var map = new int[lines, cols];
            for (var l = 0; l < lines; l++)
            {
                var line = layout[l];
                for (var c = 0; c < lines; c++)
                {
                    map[l, c] = int.Parse(line[c].ToString());
                }
            }
            return map;
        }
        private object GetCost(int[,] map)
        {
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            if (lines != cols)
                throw new NotImplementedException();

            var minCost = 0;
            for (int i = 1; i < lines; i++)
            {
                minCost += map[i-1, i] + map[i, i];
            }
            var start = new Position<int>(0, 0, 0);
            var visited = new HashSet<Visited>();
            minCost = Math.Min(minCost, Move(start, 1, Direction.Right, 0, minCost, map, visited));
            minCost =  Math.Min(minCost, Move(start, 1, Direction.Down, 0, minCost, map, visited));

            return minCost;
        }

        private int Move(Position<int> pos, int straightMoves, Direction direction, int costSoFar, int minCost, int[,] map, HashSet<Visited> visited)
        {
            var next = GetNext(pos, direction, map);
            if (next == null || costSoFar + next.Value > minCost)
                return minCost;

            var visit = new Visited(pos.Line, pos.Col, direction, straightMoves);
            if (visited.Contains(visit))
                return minCost;
            visited.Add(visit);

            Logg.WriteLine($"{next}. Straight moves {straightMoves}, cost {costSoFar}");

            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            if (next.Line == lines - 1 && next.Col == cols - 1)
            {
                Logg.WriteLine($"Found end at {next}. Cost: {costSoFar + next.Value}");
                return costSoFar + next.Value;
            }

            var newCost = minCost;
            var newMoves = direction == Direction.Right ? straightMoves + 1 : 0;
            if (newMoves < 3 && direction != Direction.Left)
                newCost = Math.Min(newCost, Move(next, newMoves, Direction.Right, costSoFar + next.Value, minCost, map, [.. visited]));

            newMoves = direction == Direction.Down? straightMoves + 1 : 0;
            if (newMoves < 3 && direction != Direction.Up)
                newCost = Math.Min(newCost, Move(next, newMoves, Direction.Down, costSoFar + next.Value, minCost, map, [.. visited]));

            newMoves = direction == Direction.Up ? straightMoves + 1 : 0;
            if (newMoves < 3 && direction != Direction.Down)
                newCost = Math.Min(newCost, Move(next, newMoves, Direction.Up, costSoFar + next.Value, minCost, map, [.. visited]));

            newMoves = direction == Direction.Left? straightMoves + 1 : 0;
            if (newMoves < 3 && direction != Direction.Right)
                newCost = Math.Min(newCost, Move(next, newMoves, Direction.Left, costSoFar + next.Value, minCost, map, [.. visited]));

            return costSoFar + newCost;
        }

        private Position<int>? GetNext(Position pos, Direction nextDirection, int[,] map)
        {
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            var line = pos.Line;
            var col = pos.Col;
            switch (nextDirection)
            {
                case Direction.Up:
                    if (pos.Line == 0)
                        return null;
                    line--;
                    break;
                case Direction.Down:
                    if (pos.Line == lines - 1)
                        return null;
                    line++;
                    break;
                case Direction.Right:
                    if (pos.Col== cols - 1)
                        return null;
                    col++;
                    break;
                case Direction.Left:
                    if (pos.Col == 0)
                        return null;
                    col--;
                    break;
            }
            return new Position<int>(line, col, map[line, col]);
        }

        private record Visited(long Line, long Col, Direction Direction, int Steps) { }
    }
}
