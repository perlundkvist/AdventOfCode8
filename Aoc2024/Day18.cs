using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day18 : DayBase
{
    private int _shortest = int.MaxValue;

    internal void Run()
    {
        Logg.DoLog = true;

        var input = GetInput("2024_18s");

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
        var endPos = new Position(map.GetLength(0), map.GetLength(1));
        var visited = new HashSet<Position>();
        //DrawMap(map, startPos, visited);
        //DrawMap(map, endPos);

        //Move(startPos, endPos, 0, map, visited);
        //Console.WriteLine($"Longest route: {longest}");

        _shortest = 0;
        var moves = new Stack<(Position startPos, Position endPos, int steps, HashSet<Position> visited)>();

        moves.Push((startPos, endPos, 0, visited));
        while (moves.Any())
        {
            var move = moves.Pop();
            var toAdd = Move(move.startPos, move.endPos, move.steps, map, move.visited);
            toAdd.ForEach(m => moves.Push(m));
        }

        Console.WriteLine($"Shortest route: {_shortest}.");
    }

    private List<(Position startPos, Position endPos, int steps, HashSet<Position> visited)> Move(Position startPos, Position endPos, int steps, char[,] map, HashSet<Position> visited)
    {
        var moves = new List<(Position startPos, Position endPos, int steps, HashSet<Position> visited)>();
        return moves;
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
                var fg = Console.ForegroundColor;
                if (draw == 'x')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (draw == 'O')
                    Console.ForegroundColor = ConsoleColor.Green;
                Logg.Write($"{(draw == 0 ? '.' : draw)}");
                Console.ForegroundColor = fg;
            }
            Logg.WriteLine();
        }
        Logg.WriteLine();
    }

}