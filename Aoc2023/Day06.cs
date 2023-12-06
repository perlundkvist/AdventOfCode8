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

            var testRaces = new List<Race> { new Race(7, 9), new Race(15, 40), new Race (30, 200)};
            var races = new List<Race> { new Race(56, 334), new Race(71, 1135), new Race(79, 1350), new Race(99, 2430) };

            double time = 56717999;
            double distance = 334113513502430;

            var margin = GetMargin(races);
            Console.WriteLine($"Margin: {margin}");

            margin = GetMargin2(7, 9);
            margin = GetMargin2(71530, 940200);
            Console.WriteLine($"Margin: {margin}");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private long GetMargin(List<Race> races)
        {
            var margin = 1;
            foreach(var race in races)
            {
                margin *= GetMargin(race);
            }
            return margin;
        }

        private int GetMargin(Race race)
        {
            var margin = 0;
            for (int i = 1; i < race.Time;  i++)
            {
                if (GetDistance(i, race.Time) > race.Distance)
                    margin++;
            }
            return margin;
        }

        private long GetMargin2(double time, double distance)
        {
            var maxTime1 = time / 2 + Math.Sqrt(Math.Pow(time / 2, 2) + 200);
            var maxTime2 = time / 2 - Math.Sqrt(Math.Pow(time / 2, 2) + 200);

            return 1;
        }

        private int GetDistance(int time, int maxTime)
        {
            return time * (maxTime - time);
        }


        internal record Race(int Time, int Distance);
    }
}
