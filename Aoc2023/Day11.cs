using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day11 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_11").ToImmutableList();
            var universe = GetUniverse(input);
            var galaxies = GetGalaxies(universe);
            var galaxies2 = GetGalaxies2(input, 1000000);

            var sum = GetSum(galaxies);
            Console.WriteLine($"Sum: {sum}");

            sum = GetSum(galaxies2);
            Console.WriteLine($"Sum: {sum}");


            Console.WriteLine($"{DateTime.Now - start}");

        }

        private object GetSum(List<LongPoint> galaxies)
        {
            long sum = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                var galaxy = galaxies[i];
                for (int j = i +1; j < galaxies.Count; j++)
                {
                    var dest = galaxies[j];
                    sum += galaxy.ManhattanDistance(dest);
                }
            }

            //foreach (var galaxy in galaxies)
            //{
            //    var idx = galaxies
            //    foreach (var dest in galaxies.Where(g => g))
            //    {
            //        sum += galaxy.ManhattanDistance(dest) * 2 + 1;
            //    }
            //}
            return sum;
        }

        private ImmutableList<string> GetUniverse(ImmutableList<string> input)
        {
            var universe = new List<StringBuilder>();

            foreach (var line in input)
            {
                universe.Add(new StringBuilder(line));
                if (line.All(c => c == '.'))
                    universe.Add(new StringBuilder(line));
            }
            var idx = 0;
            while (true)
            {
                if (idx >= universe[0].Length)
                    break;
                if (universe.All(l => l[idx] == '.')) 
                {
                    universe.ForEach(s => s.Insert(idx, '.'));
                    idx++;
                }
                idx++;
            }

            return universe.Select(s => s.ToString()).ToImmutableList();
        }

        private static List<LongPoint> GetGalaxies(ImmutableList<string> galaxy)
        {
            var points = new List<LongPoint>();
            var y = 0;
            foreach (var line in galaxy)
            {
                var x = 0;  
                foreach (var space in line)
                {
                    if (space == '#')
                        points.Add(new LongPoint(x, y));
                    x++;
                }
                y++;
            }
            return points;
        }

        private static List<LongPoint> GetGalaxies2(ImmutableList<string> universe, int expansion)
        {
            var points = new List<LongPoint>();
            long y = 0;
            foreach (var line in universe)
            {
                if (line.All(c => c == '.'))
                    y += expansion - 1;
                long x = 0;
                var idx = 0;
                foreach (var space in line)
                {
                    if (space == '#')
                        points.Add(new LongPoint(x, y));
                    if (universe.All(l => l[idx] == '.'))
                        x += expansion - 1;
                    x++;
                    idx++;
                }
                y++;
            }
            return points;
        }
    }
}
