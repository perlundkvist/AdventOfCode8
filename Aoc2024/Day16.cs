using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day16 : DayBase
{
    public int Cost = int.MaxValue;

    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_16");

        #region Part 1

        var map = input.ToCharArray();

        Print(map);

        GetCheapest(map);


        Console.WriteLine($"Cost: {Cost}");

        #endregion

    }

    private void GetCheapest(char[,] map)
    {
        var startPos = new Position(map.GetLength(0) - 2, 1);
        var endPos = new Position(1, map.GetLength(1) - 2);
        map[endPos.Line, endPos.Col] = '.';
        if (Logg.DoLog)
            Print(map);
        var visited = new HashSet<Position>();
        var moves = new Stack<(Position startPos, Position endPos, int cost, Direction direction, HashSet<Position> visited)>();

        moves.Push((startPos, endPos, 0, Direction.Right, visited));
        while (moves.Any())
        {
            var move = moves.Pop();
            var toAdd = Move(move.startPos, move.endPos, move.cost, move.direction, map, move.visited);
            toAdd.ForEach(m => moves.Push(m));
        }
    }

    private List<(Position startPos, Position endPos, int cost, Direction direction, HashSet<Position> visited)> Move(Position startPos, Position endPos, int cost, Direction direction, char[,] map, HashSet<Position> visited)
    {
        var moves = new List<(Position startPos, Position endPos, int steps, Direction direction, HashSet<Position> visited)>();
        if (startPos == endPos)
        {
            var cost2 = GetCost(visited);
            if (cost != cost2)
                Console.WriteLine($"Found diff: {cost}. Cost: {cost2}");
            Cost = Math.Min(cost, Cost);
            Console.WriteLine($"Found end: {cost}. Cost: {Cost}");
            DrawMap(map, startPos, visited);
            return moves;
        }
        if (cost > Cost)
            return moves;
        //DrawMap2(map, startPos, visited);

        visited.Add(startPos);
        var next = GetNext(direction, Direction.Left, startPos.GetSurrounding(map));
        if (next != null && ShouldTry(next, map, visited))
            moves.Add(new(next, endPos, cost + 1001, NextDirection(direction, Direction.Left), [.. visited]));
        next = GetNext(direction, Direction.Right, startPos.GetSurrounding(map));
        if (next != null && ShouldTry(next, map, visited))
            moves.Add(new(next, endPos, cost + 1001, NextDirection(direction, Direction.Right), [.. visited]));
        next = GetNext(direction, startPos.GetSurrounding(map));
        if (next != null && ShouldTry(next, map, visited))
            moves.Add(new(next, endPos, cost + 1, direction, [.. visited]));
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

    private int GetCost(HashSet<Position> visited)
    {
        var cost = 0;
        var direction = Direction.Right;
        var current = new Position(visited.Max(v => v.Line), 1);
        while (true)
        {
            var next = GetNext(direction, current.GetSurrounding(visited));
            if (next != null)
            {
                cost++;
                current = next;
                continue;
            }
            next = GetNext(direction, Direction.Left, current.GetSurrounding(visited));
            if (next != null)
            {
                cost += 1001;
                current = next;
                direction = direction switch
                {
                    Direction.Up => Direction.Left,
                    Direction.Down => Direction.Right,
                    Direction.Left => Direction.Down,
                    Direction.Right => Direction.Up,
                    _ => throw new ArgumentOutOfRangeException()
                };
                continue;
            }
            next = GetNext(direction, Direction.Right, current.GetSurrounding(visited));
            if (next != null)
            {
                cost += 1001;
                current = next;
                direction = direction switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    Direction.Right => Direction.Down,
                    _ => throw new ArgumentOutOfRangeException()
                };
                continue;
            }
            break;
        }
        return cost + 1;
    }

    private Position? GetNext(Direction direction, (Position? up, Position? down, Position? left, Position? right) surrounding)
    {
        return direction switch
        {
            Direction.Up => surrounding.up,
            Direction.Down => surrounding.down,
            Direction.Left => surrounding.left,
            Direction.Right => surrounding.right,
            _ => null
        };
    }

    private Position? GetNext(Direction direction, Direction turn, (Position? up, Position? down, Position? left, Position? right) surrounding)
    {
        if (turn == Direction.Left)
            return direction switch
            {
                Direction.Up => surrounding.left,
                Direction.Down => surrounding.right,
                Direction.Left => surrounding.down,
                Direction.Right => surrounding.up,
                _ => null
            };
        if (turn == Direction.Right)
            return direction switch
            {
                Direction.Up => surrounding.right,
                Direction.Down => surrounding.left,
                Direction.Left => surrounding.up,
                Direction.Right => surrounding.down,
                _ => null
            };

        return null;
    }

    private static bool ShouldTry(Position? next, char[,] map, HashSet<Position> visited)
    {
        if (next == null)
            return false;
        if (visited.Contains(next))
            return false;
        var c = map[next.Line, next.Col];
        var tryIt = c switch
        {
            '.' => true,
            _ => false
        };

        if (tryIt)
            Logg.WriteLine($"Try {next}");
        return tryIt;
    }

    private static void DrawMap(char[,] map, Position? current = null, HashSet<Position>? visited = null)
    {
        if (!Logg.DoLog)
            return;
        var lines = map.GetLength(0);
        var cols = map.GetLength(1);
        for (var l = 0; l < lines; l++)
        {
            for (var c = 0; c < cols; c++)
            {
                var v = visited?.SingleOrDefault(v => v.Line == l && v.Col == c);
                var draw = v != null ? 'O' : current != null && current.Line == l && current.Col == c ? 'x' : map[l, c];
                Logg.Write($"{draw}");
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