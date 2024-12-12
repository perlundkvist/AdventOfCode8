using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day12 : DayBase
{

    internal void Run()
    {
        var input = GetInput("2024_12");

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
        //if (region.First().Value == 'A')
        //    Position.Print(region);
        var cost = 0;
        foreach (var position in region)
        {
            if (IsCornerUpLeft(position, region))
                cost++;
            if (IsCornerUpRight(position, region))
                cost++;
            if (IsCornerDownLeft(position, region))
                cost++;
            if (IsCornerDownRight(position, region))
                cost++;
        }
        return cost;
    }

    private bool IsCornerUpLeft(Position<char> position, List<Position<char>> region)
    {
        var around = position.GetSurrounding(region);
        if ((around.up == null && around.left == null))
            return true;
        var around45 = position.GetSurrounding45(region);
        return around45.upLeft == null && around.up != null && around.left != null;
    }

    private bool IsCornerUpRight(Position<char> position, List<Position<char>> region)
    {
        var around = position.GetSurrounding(region);
        if (around.up == null && around.right == null)
            return true;
        var around45 = position.GetSurrounding45(region);
        return around45.upRight == null && around.up != null && around.right != null;
    }

    private bool IsCornerDownLeft(Position<char> position, List<Position<char>> region)
    {
        var around = position.GetSurrounding(region);
        if (around.down == null && around.left == null)
            return true;
        var around45 = position.GetSurrounding45(region);
        return around45.downLeft == null && around.down != null && around.left != null;
    }

    private bool IsCornerDownRight(Position<char> position, List<Position<char>> region)
    {
        var around = position.GetSurrounding(region);
        if (around.down == null && around.right == null)
            return true;
        var around45 = position.GetSurrounding45(region);
        return around45.downRight == null && around.down != null && around.right != null;
    }

    private List<Position<char>> RemoveRight(Position<char> position, List<Position<char>> fences)
    {
        var right = position.GetSurrounding(fences, position.Value).right;
        if (right != null)
        {
            fences.Remove(right);
            fences = RemoveRight(right, fences);
        }
        return fences;
    }

    private List<Position<char>> RemoveLeft(Position<char> position, List<Position<char>> fences)
    {
        var left = position.GetSurrounding(fences, position.Value).left;
        if (left != null)
        {
            fences.Remove(left);
            fences = RemoveLeft(left, fences);
        }
        return fences;
    }

    private List<Position<char>> RemoveBelow(Position<char> position, List<Position<char>> fences)
    {
        var down = position.GetSurrounding(fences).down;
        if (down != null)
        {
            fences.Remove(down);
            fences = RemoveBelow(down, fences);
        }
        return fences;
    }

    private List<Position<char>> RemoveAbove(Position<char> position, List<Position<char>> fences)
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