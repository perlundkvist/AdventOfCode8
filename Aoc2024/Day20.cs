using System.ComponentModel;
using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day20 : DayBase
{

    internal void Run()
    {
        Logg.DoLog = true;

        var input = GetInput("2024_20");
        var map = input.ToCharArray();

        var line = input.First(l => l.Contains('S'));
        var startPos = new Position(input.IndexOf(line), line.IndexOf('S'));
        line = input.First(l => l.Contains('E'));
        var endPos = new Position(input.IndexOf(line), line.IndexOf('E'));

        var route = GetRoute(map, startPos, endPos);
        //DrawMap(map, endPos, route.ToHashSet());
        Console.WriteLine($"Route: {route.Count}");

        //GetCheats(map, route, 100);
        GetCheats2(map, route, 100, 20);
        //GetCheats(map, route, 100, false);

        Console.WriteLine("Part 2 example 285 cheats");

    }

    private void GetCheats(char[,] map, List<Position> route, int save)
    {
        var cheats = new List<Position>();
        foreach (var position in route)
        {
            var cheatPositions = GetCheatPositions(position, route, map);
            foreach (var cheatPosition in cheatPositions)
            {
                var diff = GetDiff(position, cheatPosition, route);
                if (diff < save)
                    continue;
                cheats.Add(cheatPosition);
            }
        }
        Console.WriteLine($"Cheats: {cheats.ToList().Count}");
    }

    private void GetCheats2(char[,] map, List<Position> route, int save, int cheatTime)
    {
        var cheats = new List<Position<int>>();
        foreach (var position in route)
        {
            var cheatPositions = GetCheatPositions2(position, route, map, cheatTime).ToList();
            //DrawMap(map, position, cheatPositions.ToHashSet());
            var startIdx = route.IndexOf(position);
            foreach (var cheatPosition in cheatPositions)
            {
                //Logg.WriteLine(cheatPosition);
                var cheatCost = cheatPosition.ManhattanDistance(position);
                var saved = route.IndexOf(cheatPosition) - startIdx;
                var diff = saved - cheatCost;
                if (diff < save)
                    continue;
                cheats.Add(new Position<int>(cheatPosition.Line, cheatPosition.Col, diff));
            }
        }
        var grouped = cheats.GroupBy(c => c.Value);
        foreach (var group in grouped.OrderBy(g => g.Key))
        {
            Console.WriteLine($"{group.Count()}-{group.Key}");
        }
        Console.WriteLine($"Cheats: {cheats.ToList().Count}");
    }

    private IEnumerable<Position> GetCheatPositions2(Position position, List<Position> route, char[,] map, int cheatTime)
    {
        if (position == route.Last())
            return new List<Position>();

        var startIdx = route.IndexOf(position) + 1;
        return route[startIdx..].Where(p => p.ManhattanDistance(position) <= cheatTime);
    }

    private IEnumerable<Position> GetCheatPositions(Position position, List<Position> route, char[,] map)
    {
        var cheats = new List<Position>();
        var surrounding = position.GetSurrounding(map);
        if (surrounding.right != null && !route.Contains(surrounding.right))
        {
            var next = surrounding.right.GetSurrounding(map).right;
            if (next != null && route.Contains(next))
                cheats.Add(next);
        }
        if (surrounding.left != null && !route.Contains(surrounding.left))
        {
            var next = surrounding.left.GetSurrounding(map).left;
            if (next != null && route.Contains(next))
                cheats.Add(next);
        }
        if (surrounding.up != null && !route.Contains(surrounding.up))
        {
            var next = surrounding.up.GetSurrounding(map).up;
            if (next != null && route.Contains(next))
                cheats.Add(next);
        }
        if (surrounding.down != null && !route.Contains(surrounding.down))
        {
            var next = surrounding.down.GetSurrounding(map).down;
            if (next != null && route.Contains(next))
                cheats.Add(next);
        }

        return cheats;
    }

    private int GetDiff(Position pos1, Position pos2, List<Position> route)
    {
        return route.IndexOf(pos2) - route.IndexOf(pos1) - 2;
    }

    private List<Position> GetRoute(char[,] map, Position startPos, Position endPos)
    {
        var route = new List<Position> {startPos};
        while (true)
        {
            var surrounding = startPos.GetSurrounding(map);
            var next = GetNext(surrounding.up, route, map) ??
                       GetNext(surrounding.down, route, map) ??
                       GetNext(surrounding.left, route, map) ??
                       GetNext(surrounding.right, route, map);
            if (next == null)
                break;
            route.Add(next);
            if (next == endPos)
                break;
            startPos = next;
        }
        return route;
    }

    private Position? GetNext(Position? next, List<Position> route, char[,] map)
    {
        if (next == null || route.Contains(next))
            return null;
        return map[next.Line, next.Col] is 'E' or '.' ? next : null;
    }
}