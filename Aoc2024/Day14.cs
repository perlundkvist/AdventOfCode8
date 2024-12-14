using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day14 : DayBase
{

    internal void Run()
    {
        var input = GetInput("2024_14");

        List<Position> robots = new();
        List<Position<Position>> robots2 = new();
        Position? a = null;
        Position? b = null;
        var steps = 100;

        var cols = input.Count == 12 ? 11 : 101;
        var lines = input.Count == 12 ? 7 : 103;

        var middleX = cols / 2;
        var middleY = lines / 2;

        foreach (var robot in robots2)
        {
            Console.WriteLine($"{robot.Col} {IsInLine(robot, 5, robots2)}");
        }

        foreach (var line in input)
        {
            var numbers = Regex.Matches(line, @"-?\d+");
            var x = int.Parse(numbers[0].Value);
            var y = int.Parse(numbers[1].Value);
            var vx = int.Parse(numbers[2].Value);
            var vy = int.Parse(numbers[3].Value);
            robots2.Add(new Position<Position>(y, x, new Position(vy, vx)));
            x = ((x + vx * steps) % cols + cols) % cols; 
            y = ((y + vy * steps) % lines + lines) % lines;
            robots.Add(new Position(y, x));
        }
        if (lines < 20)
            Print(robots, lines, cols);

        var q1 = robots.Where(r => r.Col < middleX && r.Line < middleY).ToList();
        var q2 = robots.Where(r => r.Col > middleX && r.Line < middleY).ToList();
        var q3 = robots.Where(r => r.Col < middleX && r.Line > middleY).ToList();
        var q4 = robots.Where(r => r.Col > middleX && r.Line > middleY).ToList();

        long sf = q1.Count * q2.Count * q3.Count * q4.Count;

        Console.WriteLine($"Robots: {q1.Count} {q2.Count} {q3.Count} {q4.Count} {sf}");

        var printed = false;
        for (int i = 1; i < 10000; i++)
        {
            var map = robots2.ToList();
            robots2.Clear();
            foreach (var robot in map)
            {
                var line = ((robot.Line + robot.Value.Line) % lines + lines) % lines;
                var col = ((robot.Col + robot.Value.Col) % cols + cols) % cols;
                robots2.Add(new Position<Position>(line, col, robot.Value));
            }

            foreach (var pos in robots2)
            {
                const int length = 5;
                if (IsInLine(pos, length, robots2) && IsInLine(pos.GetSurrounding(robots2).up, length - 2, robots2))
                {
                    Console.WriteLine($"Step {i}");
                    Print(robots2.Select(Position (r) => r).ToList(), lines, cols, pos.Line - length - 3);
                    printed = true;
                    break;
                }
            }
            if (printed)
                break;
        }
    }

    private static bool IsInLine(Position<Position>? pos, int length, List<Position<Position>> map)
    {
        if (pos == null)
            return false;
        var inLine = map.Where(m => m.Line == pos.Line).ToList();
        if (inLine.Count < length)
            return false;
        inLine = inLine.DistinctBy(m => m.GetPosition()).ToList();
        var distance = length / 2;
        if (inLine.Any(m => m.Col == pos.Col - distance - 1 || m.Col == pos.Col + distance + 1))
            return false;
        inLine = inLine.Where(m => m.Col >= pos.Col - distance && m.Col <= pos.Col + distance).ToList();
        return inLine.Count == length;
    }

    private static void Print(List<Position> map, int lines, int cols, int startLine = 0)
    {
        for (var line = startLine; line < lines; line++)
        {
            for (var col = 0; col < cols; col++)
            {
                var pos = map.Count(p => p.Line == line && p.Col == col);
                Console.Write(pos == 0 ? "." : pos.ToString());
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}