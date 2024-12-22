using System.Diagnostics;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2024;

internal class Day21 : DayBase
{
    private static readonly List<Position<char>> Keypad =
    [
        new Position<char>(0, 0, '7'), new Position<char>(0, 1, '8'), new Position<char>(0, 2, '9'),
        new Position<char>(1, 0, '4'), new Position<char>(1, 1, '5'), new Position<char>(1, 2, '6'),
        new Position<char>(2, 0, '1'), new Position<char>(2, 1, '2'), new Position<char>(2, 2, '3'),
        new Position<char>(3, 0, ' '), new Position<char>(3, 1, '0'), new Position<char>(3, 2, 'A'),
    ];

    private static readonly List<Position<char>> Keypad2 =
    [
        new Position<char>(0, 0, ' '), new Position<char>(0, 1, '^'), new Position<char>(0, 2, 'A'),
        new Position<char>(1, 0, '<'), new Position<char>(1, 1, 'v'), new Position<char>(1, 2, '>'),
    ];

    internal void Run()
    {
        Logg.DoLog = true;

        var input = GetInput("2024_21s");
        var complexity = 0L;

        var d = RunRobot("v<<A>>^A<A>AvA<^AA>A<vAAA>^A", Keypad2, false);
        Console.WriteLine($"{d.Length} {d}");
        d = RunRobot(d, Keypad, false);
        Console.WriteLine($"{d.Length} {d}");
        //RunRobots("<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A");

        var dir = GetDirections("379A");
        dir = GetDirections("379A");
        RunRobots(dir);
        Console.WriteLine($"{dir.Length}");

        foreach (var line in input)
        {
            var directions = GetDirections(line);
            var value = int.Parse(line[..3]);
            Console.WriteLine($"{directions.Length}*{value}");
            complexity += directions.Length * value;
        }

        Console.WriteLine($"complexity {complexity}");


    }

    private void RunRobots(string direction)
    {
        direction = RunRobot(direction, Keypad2, false);
        direction = RunRobot(direction, Keypad2);
        direction = RunRobot(direction, Keypad);
    }

    private string RunRobot(string dir, List<Position<char>> keypad, bool avoidGap = true)
    {
        var start = keypad.First(k => k.Value == 'A');
        var output = "";
        foreach (var move in dir)
        {
            if (avoidGap && start.Value == ' ')
                Console.WriteLine("Moved to gap");

            switch (move)
            {
                case '^':
                    start = keypad.First(k => k.Line == start.Line - 1 && k.Col == start.Col);
                    break;
                case 'v':
                    start = keypad.First(k => k.Line == start.Line + 1 && k.Col == start.Col);
                    break;
                case '<':
                    start = keypad.First(k => k.Line == start.Line && k.Col == start.Col - 1);
                    break;
                case '>':
                    start = keypad.First(k => k.Line == start.Line && k.Col == start.Col + 1);
                    break;
                case 'A':
                    output += start.Value;
                    break;
                default:
                    throw new ArgumentException(move.ToString());
            }
        }
        Console.WriteLine($"Output: {output}");
        return output;
    }

    private string GetDirections(string result)
    {
        var direction = GetDirections(result, Keypad);
        direction = GetDirections(direction, Keypad2);
        direction = GetDirections(direction, Keypad2, false);
        return direction;
    }

    private string GetDirections(string result, List<Position<char>> keypad, bool avoidGap = true)
    {
        var direction = "";
        var start = keypad.First(k => k.Value == 'A');
        var lineA = start.Line;
        foreach (var next in result)
        {
            var end = keypad.First(k => k.Value == next);
            var lines = end.Line - start.Line;
            var cols = end.Col - start.Col;
            bool preferLines;
            var last = direction == "" ? ' ' : direction.Last();
            if (avoidGap && start.Col == 0 && end.Line == lineA)
                preferLines = true;
            else if (avoidGap && end.Col == 0 && start.Line == lineA)
                preferLines = false;
            else
                preferLines = last is '<' or '>';
            if (preferLines)
            {
                direction = direction.PadRight(direction.Length + Math.Abs(cols), cols < 0 ? '<' : '>');
                direction = direction.PadRight(direction.Length + Math.Abs(lines), lines < 0 ? '^' : 'v');
            }
            else
            {
                direction = direction.PadRight(direction.Length + Math.Abs(lines), lines < 0 ? '^' : 'v');
                direction = direction.PadRight(direction.Length + Math.Abs(cols), cols < 0 ? '<' : '>');
            }

            direction += 'A';
            start = end;
        }
        Console.WriteLine($"Direction: {direction}");
        return direction;
    }

    private List<string> GetDirections2(string result)
    {
        var directions = GetDirections2(result, Keypad);
        return directions;
    }

    private List<string> GetDirections2(string result, List<Position<char>> keypad, bool avoidGap = true)
    {
        var directions = new List<string>();
        var direction = "";
        var start = keypad.First(k => k.Value == 'A');
        var lineA = start.Line;
        foreach (var next in result)
        {
            var end = keypad.First(k => k.Value == next);
            var lines = end.Line - start.Line;
            var cols = end.Col - start.Col;

            bool preferLines;
            var last = direction == "" ? ' ' : direction.Last();
            if (avoidGap && start.Col == 0 && end.Line == lineA)
                preferLines = true;
            else if (avoidGap && end.Col == 0 && start.Line == lineA)
                preferLines = false;
            else
                preferLines = last is '<' or '>';
            if (preferLines)
            {
                direction = direction.PadRight(direction.Length + Math.Abs(cols), cols < 0 ? '<' : '>');
                direction = direction.PadRight(direction.Length + Math.Abs(lines), lines < 0 ? '^' : 'v');
            }
            else
            {
                direction = direction.PadRight(direction.Length + Math.Abs(lines), lines < 0 ? '^' : 'v');
                direction = direction.PadRight(direction.Length + Math.Abs(cols), cols < 0 ? '<' : '>');
            }

            direction += 'A';
            start = end;
        }
        Console.WriteLine($"Direction: {direction}");
        return directions;
    }
}