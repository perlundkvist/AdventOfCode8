using AdventOfCode8.Aoc2023;
using System.Diagnostics;
using System.Xml.Linq;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2024;

internal class Day25 : DayBase
{
    internal void Run()
    {
        Logg.DoLog = true;

        var input = GetInput("2024_25");
        var start = DateTime.Now;

        var keys = new List<int[]>();
        var locks = new List<int[]>();
        foreach (var block in input.Chunk(8))
        {
            var pins = block[1..6].ToList();
            var heights = new int[5];
            foreach (var pin in pins)
            {
                for (var col = 0; col < 5; col++)
                {
                    heights[col] += pin[col] == '#' ? 1 : 0;
                }

            }

            if (block[0].StartsWith("#"))
                locks.Add(heights);
            else
                keys.Add(heights);
        }

        var pairs = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                var match = true;
                for (var i = 0; i < 5; i++)
                {
                    if (key[i] + @lock[i] <= 5) 
                        continue;
                    match = false;
                    break;
                }
                if (match)
                    pairs++;
            }
        }

        Console.WriteLine($"Pairs {pairs}");
        Console.WriteLine($"{DateTime.Now - start}");

    }

}