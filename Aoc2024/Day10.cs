﻿using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode8.Aoc2024;

internal class Day10 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_10");

        List<Position<int>> map = new();

        for (var i = 0; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var pos = input[i][j];
                if (char.IsDigit(pos))
                    map.Add(new Position<int>(i, j, int.Parse(pos.ToString())));
            }
        }

        var score = 0;
        foreach (var position in map.Where(p => p.Value == 0))
        {
            var visited = GetHeads(position, map);
            score += visited.Count;
        }

        Console.WriteLine($"{score}");

        score = 0;
        var score2 = 0;
        foreach (var position in map.Where(p => p.Value == 0))
        {
            var visited = GetHeads2(position, map);
            score += visited.Distinct().ToList().Count;
            score2 += visited.Count;
        }

        Console.WriteLine($"Score 1: {score}");
        Console.WriteLine($"Score 2: {score2}");
    }

    private HashSet<Position> GetHeads(Position<int> position, List<Position<int>> map)
    {
        var visited = new HashSet<Position>();
        var (up, down, left, right) = position.GetSurrounding(map);
        Move(position, up, map, visited);
        Move(position, down, map, visited);
        Move(position, left, map, visited);
        Move(position, right, map, visited);
        return visited;
    }

    private void Move(Position<int> from, Position<int>? to, List<Position<int>> map, HashSet<Position> visited)
    {
        if (to == null)
            return;
        var value = to.Value;
        if (value - from.Value != 1)
            return;
        if (value == 9)
        {
            visited.Add(to);
            return;
        }

        var (up, down, left, right) = to.GetSurrounding(map);
        Move(to, up, map, visited);
        Move(to, down, map, visited);
        Move(to, left, map, visited);
        Move(to, right, map, visited);
    }

    private List<Position> GetHeads2(Position<int> position, List<Position<int>> map)
    {
        var visited = new List<Position>();
        var (up, down, left, right) = position.GetSurrounding(map);
        Move(position, up, map, visited);
        Move(position, down, map, visited);
        Move(position, left, map, visited);
        Move(position, right, map, visited);
        return visited;
    }

    private void Move(Position<int> from, Position<int>? to, List<Position<int>> map, List<Position> visited)
    {
        if (to == null)
            return;
        var value = to.Value;
        if (value - from.Value != 1)
            return;
        if (value == 9)
        {
            visited.Add(to);
            return;
        }

        var (up, down, left, right) = to.GetSurrounding(map);
        Move(to, up, map, visited);
        Move(to, down, map, visited);
        Move(to, left, map, visited);
        Move(to, right, map, visited);
    }
}