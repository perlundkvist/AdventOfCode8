using System.Diagnostics;
using static AdventOfCode8.Aoc2023.Day06;
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

    private Dictionary<(char, char, int), long> _tested = new();

    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_21");
        var complexity = 0L;

        foreach (var line in input)
        {
            var movements = GetMovements(line);
            var value = int.Parse(line[..3]);
            Console.WriteLine($"{movements}*{value}");
            complexity += movements * value;
        }

        Console.WriteLine($"complexity {complexity}");
    }

    private long GetMovements(string result)
    {
        var direction = GetMovements(result, 1);
        return direction;
    }

    private long GetMovements(string keys, int robot)
    {
        if (robot == 27) // All robots have moved
            return keys.Length;

        var currentKey = 'A';
        var length = 0L;

        foreach (var nextKey in keys)
        {
            var movements =  GetMovements(currentKey, nextKey, robot);
            length += movements;
            currentKey = nextKey;
        }

        return length;
    }

    private long GetMovements(char currentKey, char nextKey, int robot) {

        if (nextKey == ' ')
            Console.WriteLine("Space");

        if (_tested.ContainsKey((currentKey, nextKey, robot)))
            return _tested[(currentKey, nextKey, robot)];

        var keypad = robot == 1 ? Keypad : Keypad2;

        var start = keypad.First(k => k.Value == currentKey);
        var end = keypad.First(k => k.Value == nextKey);

        var lines = end.Line - start.Line;
        var cols = end.Col - start.Col;

        var colKeys = new string(cols < 0 ? '<' : '>', Math.Abs(cols));
        var lineKeys = new string(lines < 0 ? '^' : 'v', Math.Abs(lines));

        var movements = long.MaxValue;

        if (keypad.First(k => k.Line == end.Line && k.Col == start.Col).Value != ' ')
        {
            var cost = GetMovements($"{lineKeys}{colKeys}A", robot + 1);
            //Console.WriteLine($" Testing {currentKey} -> {nextKey} {lineKeys}{colKeys}A : {movements} ");
            movements = Math.Min(movements, cost);
        }

        if (keypad.First(k => k.Line == start.Line && k.Col == end.Col).Value != ' ')
        {
            var cost = GetMovements($"{colKeys}{lineKeys}A", robot + 1);
            //Console.WriteLine($" Testing {currentKey} -> {nextKey} {colKeys}{lineKeys}A : {movements} ");
            movements = Math.Min(movements, cost);
        }
        _tested.Add((currentKey, nextKey, robot), movements);
        return movements;
    }
}