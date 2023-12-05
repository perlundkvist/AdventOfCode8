using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day06 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_06s");

            //var seeds = GetSeeds(input.First());
            //var maps = GetMaps(input[2..]);

            //var lowest = GetLowest(seeds, maps);
            //Console.WriteLine($"Lowest: {lowest}");

            //var maps2 = InitMaps2(maps);
            //var seeds2 = GetSeeds2(input.First());
            //lowest = GetLowest2(seeds2, maps, maps2);
            //Console.WriteLine($"Lowest2: {lowest}");

            Console.WriteLine($"{DateTime.Now - start}");
        }
    }
}
