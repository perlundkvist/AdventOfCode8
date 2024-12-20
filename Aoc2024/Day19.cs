using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day19 : DayBase
{
    private readonly HashSet<(string, string)> _failed = new ();
    private readonly Dictionary<(string, string), long> _variants = new ();

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
            //_tested.Clear();
            Console.WriteLine($"Design {idx++} ({input.Count - 2})");
            if (Possible(design, patterns))
                correct++;
            else
                Console.WriteLine($"Failed: {design}");
        }

        Console.WriteLine($"Correct: {correct}");
        Console.WriteLine();

        idx = 1;
        var variants = 0L;
        foreach (var design in input[2..])
        {
            //Console.WriteLine($"Design {idx++} ({input.Count - 2})");
            var value = Possible2(design, patterns);
            Console.WriteLine($"Design {design}: {value}");
            variants += value;
        }

        Console.WriteLine($"Variants: {variants} 95809698 too low");
    }

    private bool Possible(string design, List<string> patterns)
    {
        if (design == "")
            return true;
        var tryPatterns = patterns.Where(t => t[0] == design[0] && t.Length <= design.Length).ToList();
        foreach (var pattern in tryPatterns)
        {
            if (_failed.Contains((design, pattern)))
                continue;
            if (!design.StartsWith(pattern)) 
                continue;
            //Console.WriteLine($"{pattern}, {design}");
            if (Possible(design[pattern.Length..], patterns))
                return true;
            //var noPatterns = tryPatterns.Where(t => t.StartsWith(pattern)).ToList();
            //tryPatterns = tryPatterns.Where(t => !noPatterns.Contains(t)).ToList();
        }
        //Console.WriteLine($"Failed: {design}");
        tryPatterns.ForEach(t => _failed.Add((design, t)));
        return false;
    }

    private long Possible2(string design, List<string> patterns)
    {
        if (design == "")
            return 1;
        var tryPatterns = patterns.Where(t => t[0] == design[0] && t.Length <= design.Length).ToList();
        var variants = 0L;
        foreach (var pattern in tryPatterns)
        {
            if (_failed.Contains((design, pattern)))
                continue;
            if (!_variants.TryGetValue((design, pattern), out var value))
            {
                if (!design.StartsWith(pattern))
                    continue;
                //Console.WriteLine($"{pattern}, {design}");
                value = Possible2(design[pattern.Length..], patterns);
                _variants.Add((design, pattern), value);
            }
            variants += value;
        }
        //Console.WriteLine($"Failed: {design}");
        return variants;
    }
}