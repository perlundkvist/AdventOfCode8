using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day02 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2023_02");

            int sum = 0;
            foreach (var line in input)
            {
                sum += GetNumber2(line);
            }

            Console.WriteLine($"Sum: {sum}");


        }

        private int GetNumber(string line)
        {
            var split = line.Split(':');
            var game = int.Parse(split[0].Split(' ')[1]);
            var sets = split[1].Split(";");
            foreach (var set in sets) 
            {
                var colors = GetColorNumber(set);
                if (colors.Red > 12)
                    return 0;
                if (colors.Green > 13)
                    return 0;
                if (colors.Blue > 14)
                    return 0;
            }
            return game;
        }

        private int GetNumber2(string line)
        {
            var split = line.Split(':');
            var game = int.Parse(split[0].Split(' ')[1]);
            var sets = split[1].Split(";");
            var red = 0;
            var green = 0;
            var blue = 0;
            foreach (var set in sets)
            {
                var colors = GetColorNumber(set);
                red = Math.Max(red, colors.Red);
                green = Math.Max(green, colors.Green);
                blue = Math.Max(blue, colors.Blue);
            }

            return red * green * blue;
        }

        private Colors GetColorNumber(string set)
        {
            var split = set.Split(",");
            var red = 0;
            var green = 0;
            var blue = 0;
            foreach (var cubes in split)
            {
                red += GetValue("red", cubes);
                green += GetValue("green", cubes);
                blue += GetValue("blue", cubes);
            }
            return new Colors(red, green, blue);
        }

        private int GetValue(string color,  string cubes)
        {
            if (!cubes.Contains(color))
                return 0;
            return int.Parse(cubes.Trim().Split(' ')[0].Trim());
        }

        private record Colors(int Red, int Green, int Blue) { }
    }
}
