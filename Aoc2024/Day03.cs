using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2024;

internal class Day03 : DayBase
{
    internal void Run()
    {
        var input = string.Join("", GetInput("2024_03"));
        var inp = GetInput("2024_03");


        //input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        //input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

        var matches = Regex.Matches(input, @"(mul\(\d+,\d+\))");
        
        var sum = 0;

        foreach (Match match in matches)
        {
            var numbers = Regex.Match(match.Value, "(\\d+),(\\d+)");
            sum += int.Parse(numbers.Groups[1].Value) * int.Parse(numbers.Groups[2].Value);
        }
        Console.WriteLine($"Sum: {sum}");

        matches = Regex.Matches(input, @"(mul\(\d+,\d+\))|(do\(\))|(don't\(\))");
        sum = 0;
        var doFound = true;
        foreach (Match match in matches)
        {
            if (match.Value == "do()")
            {
                doFound = true;
                continue;
            }
            else if (match.Value == "don't()")
            {
                doFound = false;
                continue;
            }
            if (!doFound)
                continue;
            var numbers = Regex.Match(match.Value, "(\\d+),(\\d+)");
            sum += int.Parse(numbers.Groups[1].Value) * int.Parse(numbers.Groups[2].Value);
        }
        Console.WriteLine($"Sum2: {sum}");
    }
}