﻿using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day18 : DayBase
{
    private int _shortest = int.MaxValue;
    private readonly Dictionary<Position, int> _costs = new ();

    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_18");

        var length = input.Count < 30 ? 6 : 70;
        var map = new char[length + 1, length + 1];
        var stop = input.Count < 30 ? 12 : 1024;
        foreach (var line in input[..stop])
        {
            var numbers = Regex.Match(line, "(\\d+),(\\d+)");
            map[int.Parse(numbers.Groups[2].Value), int.Parse(numbers.Groups[1].Value)] = '#';
        }


        //for (var i = 0; i < length; i++)
        //{
        //    for (var j = 0; j < length; j++)
        //    {
        //        if (map[i, j] == 0)
        //            map[i, j] = '.';
        //    }
        //}

        DrawMap(map);

        GetShortest(map);
    }

    private void GetShortest(char[,] map)
    {
        var startPos = new Position(0, 0);
        var endPos = new Position(map.GetLength(0) - 1, map.GetLength(1) - 1);
        var visited = new HashSet<Position>();
        //DrawMap(map, startPos, visited);
        //DrawMap(map, endPos);

        //Move(startPos, endPos, 0, map, visited);
        //Console.WriteLine($"Longest route: {longest}");

        _shortest = int.MaxValue;
        //var moves = new Stack<(Position from, Position startPos, Position endPos, int steps, HashSet<Position> visited)>();

        //moves.Push((startPos, startPos, endPos, 0, visited));
        //while (moves.Any())
        //{
        //    var move = moves.Pop();
        //    var toAdd = Move(move.from, move.startPos, move.endPos, move.steps, map, move.visited, moves);
        //    toAdd.ForEach(m => moves.Push(m));
        //}

        var moves = new List<(int distance, Position start, HashSet<Position> visited)>();

        moves.Add((startPos.ManhattanDistance(endPos), startPos, visited));
        while (moves.Count > 0)
        {
            var move = moves.OrderBy(m => m.distance).First();
            var toAdd = GetCost2(move.start, endPos, visited, map);

            foreach (var add in toAdd)
            {
            }

            //toAdd.ForEach(m => moves.Enqueue(m, m.Item1.ManhattanDistance(endPos)));
        }

        Console.WriteLine($"Shortest route: {_costs.First(c => c.Key == startPos)}");
    }

    private List<(Position, Position, HashSet<Position>)> GetCost2(Position start, Position end, HashSet<Position> visited, char[,] map)
    {
        throw new NotImplementedException();
    }

    private int GetCost(char[,] map, Position startPos, Position endPos, HashSet<Position> visited, int cost)
    {
        if (startPos == endPos)
        {
            _shortest = Math.Min(visited.Count, _shortest);
            Console.WriteLine($"Found end. Cost {cost}. Shortest: {_shortest}");
            DrawMap(map, startPos, visited);
            return cost;
        }

        if (_costs.TryGetValue(startPos, out var foundCost))
            return foundCost == int.MaxValue ? foundCost : cost + foundCost;

        if (visited.Count + 1 > _shortest)
            return int.MaxValue;

        DrawMap(map, startPos, visited);
        visited.Add(startPos);
        var (up, down, left, right) = startPos.GetSurrounding(map);
        Position? next;
        next = left;
        var nextCost = int.MaxValue;
        if (ShouldTry(next, map, visited))
            nextCost = Math.Min(nextCost, GetCost(map, next, endPos, [.. visited],  1));
        next = up;
        if (ShouldTry(next, map, visited))
            nextCost = Math.Min(nextCost, GetCost(map, next, endPos, [.. visited],  1));
        next = right;
        if (ShouldTry(next, map, visited))
            nextCost = Math.Min(nextCost, GetCost(map, next, endPos, [.. visited], 1));
        next = down;
        if (ShouldTry(next, map, visited))
            nextCost = Math.Min(nextCost, GetCost(map, next, endPos, [.. visited], 1));
        _costs.Add(startPos, nextCost);
        if (nextCost == int.MaxValue)
            return nextCost;
        return cost + nextCost;
    }

    private List<(Position from, Position startPos, Position endPos, int steps, HashSet<Position> visited)> 
        Move(Position from, Position startPos, Position endPos, int steps, char[,] map, HashSet<Position> visited,
        Stack<(Position from, Position startPos, Position endPos, int steps, HashSet<Position> visited)> stack)
    {
        var moves = new List<(Position from, Position startPos, Position endPos, int steps, HashSet<Position> visited)>();
        if (startPos == endPos)
        {
            _shortest = Math.Min(steps, _shortest);
            var log = Logg.DoLog;
            Console.WriteLine($"Found end: {steps}. Shortest: {_shortest}");
            Logg.DoLog = true;
            DrawMap(map, startPos, visited);
            foreach (var v in visited.Reverse())
            {
                var move = stack.Where(m => m.from == v).ToList();
            }
            Logg.DoLog = log;
            return moves;
        }
        if (steps > _shortest)
            return moves;

        DrawMap(map, startPos, visited, 0);
        visited.Add(startPos);
        var (up, down, left, right) = startPos.GetSurrounding(map);
        Position? next;
        next = left;
        if (ShouldTry(next, map, visited))
            moves.Add(new(startPos, next, endPos, steps + 1, [.. visited]));
        next = up;
        if (ShouldTry(next, map, visited))
            moves.Add(new(startPos, next, endPos, steps + 1, [.. visited])); 
        next = right;
        if (ShouldTry(next, map, visited))
            moves.Add(new(startPos, next, endPos, steps + 1, [.. visited]));
        next = down;
        if (ShouldTry(next, map, visited))
            moves.Add(new(startPos, next, endPos, steps + 1, [.. visited]));
        
        return moves;
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
            (char)0 => true,
            '.' => true,
            _ => false
        };

        if (!tryIt)
            return false;

        Logg.WriteLine($"Try {next}");
        return true;
    }


    private static void DrawMap(char[,] map, Position? current = null, HashSet<Position>? visited = null, int sleep = 0)
    {
        if (!Logg.DoLog)
            return;
        var lines = map.GetLength(0);
        var cols = map.GetLength(1);
        Console.Clear();
        for (var l = 0; l < lines; l++)
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
                Console.Write($"{(draw == 0 ? '.' : draw)}");
                Console.ForegroundColor = fg;
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();
        if (sleep > 0)
            Thread.Sleep(sleep);
    }

}