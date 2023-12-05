using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day05 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;
            
            var input = GetInput("2023_05");

            var seeds = GetSeeds(input.First());
            var maps = GetMaps(input[2..]);

            var lowest = GetLowest(seeds, maps);
            Console.WriteLine($"Lowest: {lowest}");

            var maps2 = InitMaps2(maps);
            var seeds2 = GetSeeds2(input.First());
            lowest = GetLowest2(seeds2, maps, maps2);
            Console.WriteLine($"Lowest2: {lowest}");

            Console.WriteLine($"{DateTime.Now - start}");
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
            return seeds.OrderBy(s => s.Item1).ToList();
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

        private static long GetLowest2(List<Tuple<long, long>> seeds, ImmutableList<Map> maps, List<Map2> maps2)
        {
            var minLocation = long.MaxValue;
            foreach (var seed in seeds)
            {
                var source = seed.Item1;
                var end = seed.Item1 + seed.Item2 - 1;
                while (true)
                {
                    var soil = maps2.Single(m => m.Index == 0 && source >= m.Start && source <= m.End);
                    minLocation = Math.Min(minLocation, soil.GetLocation(source));
                    if (soil.End == long.MaxValue)
                        break;
                    source = soil.End + 1;
                    if (source >= end)
                        break;
                }
            }
            return minLocation;
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

        private List<Map2> InitMaps2(ImmutableList<Map> maps)
        {
            var maps2 = new List<Map2>();
            for (int i = 6; i >= 0; i--)
            {
                var indexMaps = maps.Where(m => m.Index == i).ToList();
                long start = 0;
                while (true)
                {
                    var map = indexMaps.FirstOrDefault(m => start >= m.Source && start <= m.SourceEnd);
                    var next = indexMaps.Where(m => start < m.Source).MinBy(m => m.Source);
                    var map2 = new Map2 { Index = i, Start = start };
                    if (i == 6)
                    {
                        map2.Location = map?.Destination ?? start;
                        map2.End = map == null && next == null ? long.MaxValue : (map == null ? next.Source - 1 : map.SourceEnd);
                    }
                    else
                    {
                        var destination = map != null ? map.Destination + start - map.Source : start;
                        var location = maps2.Single(m => m.Index == i + 1 && destination >= m.Start && destination <= m.End);
                        map2.Location = location.GetLocation(destination);
                        var end1 = map?.SourceEnd ?? long.MaxValue;
                        var end2 = next?.Source - 1 ?? long.MaxValue;
                        var end3 = location.End == long.MaxValue ? long.MaxValue : start + (location.End - destination);
                        map2.End = Math.Min(end1, Math.Min(end2, end3));
                    }
                    maps2.Add(map2);
                    if (map2.End == long.MaxValue)
                        break;
                    start = map2.End + 1;
                }
            }
            return maps2;
        }
    }

    public class Map
    {
        public int Index{ get; set; }

        public long Source { get; set; }
        public long SourceEnd => Source + Length - 1;
        public long Destination { get; set; }
        public long Length { get; set; }
        public long Diff => Destination - Source;
    }

    public class Map2
    {
        public int Index{ get; set; }
        public long Start{ get; set; }
        public long End { get; set; }
        public long Location { get; set; }
        public long GetLocation(long source) => Location + source - Start;
            
    }
    
}
