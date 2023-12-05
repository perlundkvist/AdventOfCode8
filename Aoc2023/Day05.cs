using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day05 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2023_05");

            var seeds = GetSeeds(input.First());
            var maps = GetMaps(input[2..]);

            var lowest = GetLowest(seeds, maps);
            Console.WriteLine($"Lowest: {lowest}");

            var seeds2 = GetSeeds2(input.First());
            lowest = GetLowest2(seeds2, maps);
            Console.WriteLine($"Lowest2: {lowest}");
        }

        private List<long> GetSeeds(string input)
        {
            // seeds: 79 14 55 13
            var split = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
            return split.Select(long.Parse).ToList();
        }

        private List<Tuple<long, long>> GetSeeds2(string input)
        {
            // seeds: 79 14 55 13
            var split = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..];
            var pairs = split.Chunk(2).ToList();
            var seeds = pairs.Select(p => new Tuple<long, long>(long.Parse(p.First()), long.Parse(p.Last()))).ToList();
            return seeds;
        }

        private ImmutableList<Map> GetMaps(List<string> input)
        {

            //seed - to - soil map:
            //50 98 2
            //52 50 48
            var maps = new List<Map>();

            var idx = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    idx++;
                    continue;
                }
                if (!char.IsDigit(line.First()))
                    continue;

                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var map = new Map
                {
                    Index = idx,
                    Source = long.Parse(split[1]),
                    Destination = long.Parse(split[0]),
                    Length = long.Parse(split[2])
                };
                maps.Add(map);
            }

            return maps.ToImmutableList<Map>();
        }

        private static long GetLowest(List<long> seeds, ImmutableList<Map> maps)
        {
            var locations = seeds.Select(s => new { Seed = s, Location = GetLocation(s, maps) }).ToList();
            return locations.Min(l => l.Location);
        }

        private static long GetLowest2(List<Tuple<long, long>> seeds, ImmutableList<Map> maps)
        {
            var lowest = long.MaxValue;
            foreach (var seed in seeds)
            {
                var lastSeed = seed.Item1 + seed.Item2;
                for (var i = seed.Item1; i <= lastSeed ; i++)
                {
                    var location = GetLocation(i, maps);
                    if (location < lowest)
                        lowest = location;
                    if (i % 100000 == 0)
                        Console.WriteLine($"i: {i} ({lastSeed})");
                }
            }
            return lowest;
        }

        private static long GetLocation(long seed, ImmutableList<Map> maps)
        {
            var value = seed;
            for (var i = 0; i < 7; i++)
            {
                var map = maps.FirstOrDefault(m => m.Index == i && value >= m.Source && value <= m.Source + m.Length);
                value += map?.Diff ?? 0;
            }

            return value;
        }
    }

    public class Map
    {
        public int Index{ get; set; }

        public long Source { get; set; }
        public long Destination { get; set; }
        public long Length { get; set; }
        public long Diff => Destination - Source;

    }
}
