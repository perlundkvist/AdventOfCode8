using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day19 : DayBase
{
    private int _shortest = int.MaxValue;
    private Dictionary<Position, int> _costs = new ();

    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_19");
        var patterns = input[0].Split(", ").ToList();
        patterns = patterns.OrderByDescending(t => t.Length).ToList();

        var correct = 0;
        var idx = 1;
        foreach (var design in input[2..])
        {
            Console.WriteLine($"Design {idx++} ({input.Count - 2})");
            if (Possible(design, patterns))
                correct++;
            else
                Console.WriteLine($"Failed: {design}");
        }

        Console.WriteLine($"Correct: {correct}. 306 too low");
    }

    private bool Possible(string design, List<string> patterns)
    {
        if (design == "")
            return true;
        var tryPatterns = patterns.Where(t => t[0] == design[0] && t.Length <= design.Length).ToList();
        while (tryPatterns.Any())
        {
            var pattern = tryPatterns.First();
            tryPatterns.Remove(pattern);
            if (!design.StartsWith(pattern)) 
                continue;
            //Console.WriteLine($"{pattern}, {design}");
            if (Possible(design[pattern.Length..], patterns))
                return true;
            //var noPatterns = tryPatterns.Where(t => t.StartsWith(pattern)).ToList();
            //tryPatterns = tryPatterns.Where(t => !noPatterns.Contains(t)).ToList();
        }
        //Console.WriteLine($"Failed: {design}");
        return false;
    }
}