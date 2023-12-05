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

            var maps = GetMaps(input);

            var lowest = GetLowest(input);
            Console.WriteLine($"Lowest: {lowest}");

            //sum = GetSum2(input);
            //Console.WriteLine($"Sum2: {sum}");
        }

        private ImmutableList<Map> GetMaps(List<string> input)
        {
            var maps = new List<Map>();

            foreach (var map in maps)
            {

            }
            
            return maps.ToImmutableList<Map>();
        }

        private object GetLowest(List<string> input)
        {
            throw new NotImplementedException();
        }
    }

    public class Map
    {
        public enum MapType { Seed2Soil, Soil2Fertilizer, Fertilizer2Water, Water2Light, Light2Temp, Temp2Humidity, Humidity2Location}
        public MapType Type { get; set; }

        public long Source { get; set; }
        public long Destination { get; set; }
        public long Length { get; set; }
    }
}
