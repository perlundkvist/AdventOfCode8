using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2024;

internal class Day16 : DayBase
{
    public int Cost = int.MaxValue;

    private List<Position> _allRoutes = [];

    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_16");

        #region Part 1

        var map = input.ToCharArray();

        var map2 = new int[input.Count, input[0].Length];
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                map2[x, y] = input[y][x] == '#' ? 1 : 0;
            }
        }

        Print(map);
        //Print(map2);

        //GetCheapest(map);
        Console.WriteLine($"Cost: {Cost}.");

        Cost = 83432; // Cheat, user answer from Part 1
        GetRoutes(map);

        Console.WriteLine($"Positions on routes: {_allRoutes.Count}. ");

        #endregion

    }


    private void GetCheapest(char[,] map)
    {
        var startPos = new Position(map.GetLength(0) - 2, 1);
        var endPos = new Position(1, map.GetLength(1) - 2);
        map[endPos.Line, endPos.Col] = '.';
        if (Logg.DoLog)
            Print(map);
        var visited = new Dictionary<Position<Direction>, long>();
        var moves = new Stack<(Position<Direction> startPos, Position endPos, int cost)>();

        moves.Push((new Position<Direction>(startPos, Direction.Up), endPos, 1000));
        moves.Push((new Position<Direction>(startPos, Direction.Left), endPos, 1000));
        moves.Push((new Position<Direction>(startPos, Direction.Down), endPos, 1000));
        moves.Push((new Position<Direction>(startPos, Direction.Right), endPos, 0));
        while (moves.Any())
        {
            var move = moves.Pop();
            var toAdd = Move(move.startPos, move.endPos, move.cost, map, visited);
            toAdd.ForEach(m => moves.Push(m));
        }
    }

    private List<(Position<Direction> startPos, Position endPos, int cost)> Move(Position<Direction> startPos, Position endPos, int cost, char[,] map, Dictionary<Position<Direction>, long> visited)
    {
        var moves = new List<(Position<Direction> startPos, Position endPos, int steps)>();
        if (startPos.Line == endPos.Line && startPos.Col == endPos.Col)
        {
            //var cost2 = GetCost(visited);
            //if (cost != cost2)
            //    Console.WriteLine($"Found diff: {cost}. Cost: {cost2}");
            Cost = Math.Min(cost, Cost);
            Console.WriteLine($"Found end: {cost}. Cost: {Cost}");
            DrawMap(map, startPos, visited.Select(v => v.Key).ToHashSet());
            return moves;
        }
        if (cost > Cost)
            return moves;
        //DrawMap2(map, startPos, visited);

        visited[startPos] = cost;

        var surrounding = startPos.GetSurrounding(map);
        var nextDirection = NextDirection(startPos.Value, Direction.Left);
        var next = GetNext(nextDirection, surrounding);
        if (next != null && ShouldTry(next, map))
        {
            if (!visited.TryGetValue(next, out var value) || value > cost)
                moves.Add(new(next, endPos, cost + 1001));
        }

        nextDirection = NextDirection(startPos.Value, Direction.Right);
        next = GetNext(nextDirection, surrounding);
        if (next != null && ShouldTry(next, map))
        {
            if (!visited.TryGetValue(next, out var value) || value > cost)
                moves.Add(new(next, endPos, cost + 1001));
        }

        next = GetNext(startPos.Value, startPos.GetSurrounding(map));
        if (next != null && ShouldTry(next, map))
            moves.Add(new(next, endPos, cost + 1));
        return moves;
    }

    private void GetRoutes(char[,] map)
    {
        var startPos = new Position(map.GetLength(0) - 2, 1);
        var endPos = new Position(1, map.GetLength(1) - 2);
        map[endPos.Line, endPos.Col] = '.';
        if (Logg.DoLog)
            Print(map);
        var visited = new Dictionary<Position<Direction>, long>();
        var moves = new Stack<(Position<Direction> startPos, int cost, List<Position> route)>();

        moves.Push((new Position<Direction>(startPos, Direction.Up), 1000, []));
        moves.Push((new Position<Direction>(startPos, Direction.Left), 1000, []));
        moves.Push((new Position<Direction>(startPos, Direction.Down), 1000, []));
        moves.Push((new Position<Direction>(startPos, Direction.Right), 0, []));
        while (moves.Any())
        {
            var move = moves.Pop();
            var toAdd = Move(move.startPos, endPos, move.cost, map, visited, move.route);
            toAdd.ForEach(m => moves.Push(m));
        }
    }

    private List<(Position<Direction> startPos, int cost, List<Position> route)> Move(Position<Direction> startPos, Position endPos, int cost, char[,] map, Dictionary<Position<Direction>, long> visited, List<Position> route)
    {
        var moves = new List<(Position<Direction> startPos, int cost, List<Position> route)>();

        route.Add(startPos.GetPosition());
        if (cost > Cost)
            return [];
        if (startPos.Line == endPos.Line && startPos.Col == endPos.Col)
        {
            if (cost < Cost)
            {
                _allRoutes.Clear();
                _allRoutes.AddRange(route);
                Cost = cost;
                Console.WriteLine($"Found new shortest: {cost}.");
            }
            else
            {
                _allRoutes = _allRoutes.Union(route).ToList();
                Console.WriteLine($"Found shortest again: {cost}.");
            }
            DrawMap2(map, startPos, _allRoutes.ToHashSet());
            return moves;
        }
        //DrawMap2(map, startPos, visited);

        visited[startPos] = cost;

        var surrounding = startPos.GetSurrounding(map);
        var nextDirection = NextDirection(startPos.Value, Direction.Left);
        var next = GetNext(nextDirection, surrounding);
        if (next != null && ShouldTry(next, map))
        {
            if (!visited.TryGetValue(next, out var value) || value > cost)
                moves.Add(new(next, cost + 1001, [.. route]));
        }

        nextDirection = NextDirection(startPos.Value, Direction.Right);
        next = GetNext(nextDirection, surrounding);
        if (next != null && ShouldTry(next, map))
        {
            if (!visited.TryGetValue(next, out var value) || value > cost)
                moves.Add(new(next, cost + 1001, [.. route]));
        }

        next = GetNext(startPos.Value, startPos.GetSurrounding(map));
        if (next != null && ShouldTry(next, map))
            moves.Add(new(next, cost + 1, [.. route]));

        return moves;

    }

    private Direction NextDirection(Direction direction, Direction turn)
    {
        if (turn == Direction.Left)
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                _ => throw new ArgumentOutOfRangeException()
            };
        if (turn == Direction.Right)
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Right => Direction.Down,
                _ => throw new ArgumentOutOfRangeException()
            };
        throw new NotImplementedException();
    }

    //private int GetCost(HashSet<Position<Direction>> visited)
    //{
    //    var cost = 0;
    //    var direction = Direction.Right;
    //    var current = new Position(visited.Max(v => v.Line), 1);
    //    while (true)
    //    {
    //        var next = GetNext(direction, current.GetSurrounding(visited));
    //        if (next != null)
    //        {
    //            cost++;
    //            current = next;
    //            continue;
    //        }
    //        next = GetNext(direction, Direction.Left, current.GetSurrounding(visited));
    //        if (next != null)
    //        {
    //            cost += 1001;
    //            current = next;
    //            direction = direction switch
    //            {
    //                Direction.Up => Direction.Left,
    //                Direction.Down => Direction.Right,
    //                Direction.Left => Direction.Down,
    //                Direction.Right => Direction.Up,
    //                _ => throw new ArgumentOutOfRangeException()
    //            };
    //            continue;
    //        }
    //        next = GetNext(direction, Direction.Right, current.GetSurrounding(visited));
    //        if (next != null)
    //        {
    //            cost += 1001;
    //            current = next;
    //            direction = direction switch
    //            {
    //                Direction.Up => Direction.Right,
    //                Direction.Down => Direction.Left,
    //                Direction.Left => Direction.Up,
    //                Direction.Right => Direction.Down,
    //                _ => throw new ArgumentOutOfRangeException()
    //            };
    //            continue;
    //        }
    //        break;
    //    }
    //    return cost + 1;
    //}

    //private static long GetCost(List<Position> visited)
    //{
    //    var cost = 0L;
    //    var direction = Pathfind.Directions[2]; // Right
    //    var current = visited.First();
    //    foreach (var next in visited.Skip(1))
    //    {
    //        var diff = next - current;
    //        if (diff == direction)
    //        {
    //            cost++;
    //        }
    //        else
    //        {
    //            cost += 1001;
    //            direction = diff;
    //        }
    //        current = next;
    //    }

    //    return cost;
    //}

    private Position<Direction>? GetNext(Direction direction, (Position? up, Position? down, Position? left, Position? right) surrounding)
    {
        return direction switch
        {
            Direction.Up => surrounding.up == null ? null : new Position<Direction>(surrounding.up, Direction.Up),
            Direction.Down => surrounding.down == null ? null : new Position<Direction>(surrounding.down, Direction.Down),
            Direction.Left => surrounding.left == null ? null : new Position<Direction>(surrounding.left, Direction.Left),
            Direction.Right => surrounding.right == null ? null : new Position<Direction>(surrounding.right, Direction.Right),
            _ => null
        };
    }

    private static bool ShouldTry(Position<Direction> next, char[,] map)
    {
        var c = map[next.Line, next.Col];
        var tryIt = c switch
        {
            '.' => true,
            _ => false
        };

        //if (tryIt)
        //    Logg.WriteLine($"Try {next}");
        return tryIt;
    }

    private static void DrawMap(char[,] map, Position? current = null, HashSet<Position<Direction>>? visited = null)
    {
        if (!Logg.DoLog)
            return;
        var lines = map.GetLength(0);
        var cols = map.GetLength(1);
        for (var l = 0; l < lines; l++)
        {
            for (var c = 0; c < cols; c++)
            {
                var v = visited?.FirstOrDefault(v => v.Line == l && v.Col == c);
                var draw = v != null ? 'O' : current != null && current.Line == l && current.Col == c ? 'x' : map[l, c];
                var fg = Console.ForegroundColor;
                if (draw == 'x')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (draw == 'O')
                    Console.ForegroundColor = ConsoleColor.Green;
                Logg.Write($"{draw}");
                Console.ForegroundColor = fg;
            }
            Logg.WriteLine();
        }
        Logg.WriteLine();
    }

    private void DrawMap2(char[,] map, Position current, HashSet<Position> visited)
    {
        Thread.Sleep(2000);
        Console.Clear();
        var start = Math.Max(0, current.Line - 15);
        var lines = Math.Min(map.GetLength(0), current.Line + 15);
        var cols = map.GetLength(1);
        for (var l = start; l < lines; l++)
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
                Console.Write($"{draw}");
                Console.ForegroundColor = fg;
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        //Console.ReadKey(true);
    }


}