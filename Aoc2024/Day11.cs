using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode8.Aoc2024;

internal class Day11 : DayBase
{
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
        var count = 0L;
        foreach (var item in input)
        {
            var result = long.Parse(item);
            count += Blink2(long.Parse(item), 25);
            Console.WriteLine($"{item}: {count}");
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
        if (blink == 0)
            return 0;
        if (input == 0)
        {
            result = 1 + Blink2(1, blink-1);
        }
        else if (input.ToString().Length % 2 == 0)
        {
            var item = input.ToString();
            input =  long.Parse(item[..(item.Length / 2)]);
            result = Blink2(input, blink-1);
            input =  long.Parse(item[(item.Length / 2)..]);
            result += Blink2(input, blink-1);
        }
        else
        {
            result = 1 + Blink2(input * 2024, blink - 1);
        }

        return result;
    }
}