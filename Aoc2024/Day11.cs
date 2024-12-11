using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode8.Aoc2024;

internal class Day11 : DayBase
{
    private Dictionary<long, long[]> _results = new();

    internal void Run()
    {
        //var input = GetInput("2024_11s");

        var input = "0 1 10 99 999".Split(" ").ToList();
        input = "125 17".Split(" ").ToList();
        input = "0 37551 469 63 1 791606 2065 9983586".Split(" ").ToList();
        for (int i = 0; i < 25; i++)
        {
            input = Blink(input);
        }
        Console.WriteLine($"Stones 1: {input.Count}");

        input = "0 37551 469 63 1 791606 2065 9983586".Split(" ").ToList();
        //input = "125 17".Split(" ").ToList();
        var count = 0L;
        foreach (var item in input)
        {
            count += Blink2(long.Parse(item), 75);
            //Console.WriteLine($"{item}: {count}");
        }

        Console.WriteLine($"Stones 2: {count}");
    }

    private List<string> Blink(List<string> input)
    {
        var result = new List<string>();
        foreach (var item in input)
        {
            if (item == "0")
            {
                result.Add("1");
            }
            else if (item.Length % 2 == 0)
            {
                result.Add(item[..(item.Length/2)]);
                result.Add(item[(item.Length/2)..].TrimStart('0').PadRight(1, '0'));
            }
            else
            {
                result.Add($"{long.Parse(item)*2024}");
            }
        }

        return result;
    }

    private long Blink2(long input, int blink)
    {
        var result = 0L;
        if (_results.ContainsKey(input) && _results[input][blink] != 0)
        {
            return _results[input][blink];
        }
        if (blink == 0)
            return 1;
        if (input == 0)
        {
            result = Blink2(1, blink-1);
        }
        else if (input.ToString().Length % 2 == 0)
        {
            var item = input.ToString();
            var inp =  long.Parse(item[..(item.Length / 2)]);
            result = Blink2(inp, blink - 1);
            inp =  long.Parse(item[(item.Length / 2)..]);
            result += Blink2(inp, blink - 1);
        }
        else
        {
            result = Blink2(input * 2024, blink - 1);
        }
        if (!_results.ContainsKey(input))
            _results[input] = new long[76];
        _results[input][blink] = result;
        return result;
    }
}