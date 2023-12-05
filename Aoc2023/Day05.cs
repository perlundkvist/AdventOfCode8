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
            var input = GetInput("2023_05s");

            var seeds = GetSeeds(input.First());
            var maps = GetMaps(input[2..]);

            var lowest = GetLowest(seeds, maps);
            Console.WriteLine($"Lowest: {lowest}");

            var maps2 = InitMaps2(maps);
            var seeds2 = GetSeeds2(input.First());
            lowest = GetLowest2(seeds2, maps, maps2);
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

        private static long GetLowest2(List<Tuple<long, long>> seeds, ImmutableList<Map> maps, List<Map> maps2)
        {
            var minLocation = long.MaxValue;
            foreach (var seed in seeds)
            {
                var source = seed.Item1;
                var length = seed.Item2;
                while (true)
                {
                    var location = GetLocation(0, source, length, maps, maps2);
                    minLocation = Math.Min(minLocation, location.Location + location.Diff ?? location.Source);
                    length = Math.Min(length, location.Length);
                    source += length;
                    if (source > seed.Item2)
                        break;
                }
            }
            return minLocation;
        }

        private static Map GetLocation(int index, long source, long length, ImmutableList<Map> maps, List<Map> maps2)
        {
            var map2 = maps2.FirstOrDefault(m => m.Index == index && source >= m.Source && source <= m.SourceEnd);
            if (map2 != null)
                return map2;

            if (index == 6)
            {
                var map = maps2.FirstOrDefault(m => m.Index == index && source >= m.Source && source <= m.SourceEnd);
                if (map == null)
                {
                    var before = maps2.Where(m => m.Index == index && source > m.SourceEnd).MaxBy(m => m.Source);
                    var after = maps2.Where(m => m.Index == index && source < m.Source).MinBy(m => m.Source);
                    var start = before?.Source ?? 0;
                    var end = after?.Source ?? source;
                    map = new Map { Index = index, Source = start, Destination = end, Length = end - start + 1, Location = start};
                    maps2.Add(map);
                }
                return map;
            }

            var indexMaps = maps.Where(m => m.Index == index).ToList();
            var map1 = indexMaps.FirstOrDefault(m => source >= m.Source && source <= m.SourceEnd);
            source += map1?.Diff ?? 0;

            var location = GetLocation(index + 1, source, length, maps, maps2);
            map2 = new Map { Index = index, Source = source, Destination = location.Destination, Length = location.Length, Location = location.Location };
            maps2.Add(map2);
            return map2;

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

        private List<Map> InitMaps2(ImmutableList<Map> maps)
        {
            var locations = maps.Where(m => m.Index == 6).OrderBy(m => m.Source).ToList();
            locations.ForEach(l => l.Location = l.Destination);
            return locations;
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
        public long? Location { get; set; }
    }
}
