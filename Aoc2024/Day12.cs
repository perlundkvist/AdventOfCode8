using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode8.Aoc2024;

internal class Day12 : DayBase
{

    internal void Run()
    {
        var input = GetInput("2024_12s");

        List<Position<char>> map = new();
        for (var line = 0; line < input.Count; line++)
        {
            for (var col = 0; col < input[line].Length; col++)
            {
                map.Add(new Position<char>(line, col, input[line][col]));
            }
        }

        var cost = 0;
        while (map.Count > 0)
        {
            var start = map[0];
            var region = GetRegion(start, map);
            //var regionCost = region.Count * GetCost(region);
            var regionCost = region.Count * GetCost2(region);
            Console.WriteLine($"{start.Value}: {region.Count} - {regionCost/region.Count}");
            cost += regionCost;
        }

        Console.WriteLine($"Cost: {cost}");
    }

    private int GetCost(List<Position<char>> region)
    {
        var cost = 0;
        foreach (var position in region)
        {
            var around = position.GetSurrounding(region);
            if (around.up == null)
            {
                cost++;
            }
            if (around.down == null)
            {
                cost++;
            }
            if (around.left == null)
            {
                cost++;
            }
            if (around.right == null)
            {
                cost++;
            }
        }
        return cost;
    }

    private int GetCost2(List<Position<char>> region)
    {
        Position.Print(region);
        var cost = 0;
        var fences = new List<Position>();
        foreach (var position in region)
        {
            var around = position.GetSurrounding(region);
            if (around.up == null)
            {
                fences.Add(new Position(position.Line-1, position.Col));
            }
            if (around.down == null)
            {
                fences.Add(new Position(position.Line + 1, position.Col));
            }
            if (around.left == null)
            {
                fences.Add(new Position(position.Line, position.Col - 1));
            }
            if (around.right == null)
            {
                fences.Add(new Position(position.Line, position.Col + 1));
            }
        }
        fences = fences.Distinct().ToList();

        Position.Print(fences, '*');

        while (fences.Count > 0)
        {
            cost++;
            var position = fences[0];
            fences.Remove(position);
            fences = RemoveAbove(position, fences);
            fences = RemoveBelow(position, fences);
            fences = RemoveLeft(position, fences);
            fences = RemoveRight(position, fences);
        }
        return cost;
    }

    private List<Position> RemoveRight(Position position, List<Position> fences)
    {
        var right = position.GetSurrounding(fences).right;
        if (right != null)
        {
            fences.Remove(right);
            fences = RemoveRight(right, fences);
        }
        return fences;
    }

    private List<Position> RemoveLeft(Position position, List<Position> fences)
    {
        var left = position.GetSurrounding(fences).left;
        if (left != null)
        {
            fences.Remove(left);
            fences = RemoveLeft(left, fences);
        }
        return fences;
    }

    private List<Position> RemoveBelow(Position position, List<Position> fences)
    {
        var down = position.GetSurrounding(fences).down;
        if (down != null)
        {
            fences.Remove(down);
            fences = RemoveBelow(down, fences);
        }
        return fences;
    }

    private List<Position> RemoveAbove(Position position, List<Position> fences)
    {
        var up = position.GetSurrounding(fences).up;
        if (up != null)
        {
            fences.Remove(up);
            fences = RemoveAbove(up, fences);
        }
        return fences;
    }

    private List<Position<char>> GetRegion(Position<char> start, List<Position<char>> map)
    {
        var region = new List<Position<char>> { start };
        map.Remove(start);
        var around = start.GetSurrounding<char>(map);
        var neighbour = around.up;
        if (neighbour?.Value == start.Value)
        {
            region.Add(neighbour);
            map.Remove(neighbour);
            region.AddRange(GetRegion(neighbour, map));
        }
        neighbour = around.down;
        if (neighbour?.Value == start.Value)
        {
            region.Add(neighbour);
            map.Remove(neighbour);
            region.AddRange(GetRegion(neighbour, map));
        }
        neighbour = around.left;
        if (neighbour?.Value == start.Value)
        {
            region.Add(neighbour);
            map.Remove(neighbour);
            region.AddRange(GetRegion(neighbour, map));
        }
        neighbour = around.right;
        if (neighbour?.Value == start.Value)
        {
            region.Add(neighbour);
            map.Remove(neighbour);
            region.AddRange(GetRegion(neighbour, map));
        }
        return region.Distinct().ToList();
    }
}