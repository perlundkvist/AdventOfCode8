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

            var sum = GetSum(galaxies);
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine($"{DateTime.Now - start}");

        }

        private object GetSum(List<Point> galaxies)
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

        private List<Point> GetGalaxies(ImmutableList<string> galaxy)
        {
            var points = new List<Point>();
            var y = 0;
            foreach (var line in galaxy)
            {
                var x = 0;  
                foreach (var space in line)
                {
                    if (space == '#')
                        points.Add(new Point(x, y));
                    x++;
                }
                y++;
            }
            return points;
        }
    }
}
