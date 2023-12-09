using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day09 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_09");

            var sum = GetSum(input);
            Console.WriteLine($"Sum: {sum}");

            sum = GetSum2(input);
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine($"{DateTime.Now - start}");

        }

        private object GetSum(List<string> input)
        {

            var readings = input.Select(l => l.Split(' ').ToList().Select(r => long.Parse(r)).ToList());

            long sum = 0;

            foreach (var reading in readings)
            {
                sum += GetNextValue(reading);
            }
            return sum;
        }

        private object GetSum2(List<string> input)
        {

            var readings = input.Select(l => l.Split(' ').ToList().Select(r => long.Parse(r)).ToList());

            long sum = 0;

            foreach (var reading in readings)
            {
                sum += GetPrevValue(reading);
            }
            return sum;
        }

        private long GetNextValue(List<long> reading)
        {
            var nextLine = new List<long>();
            for (int i = 0; i < reading.Count-1; i++)
            {
                nextLine.Add(reading[i+1] - reading[i]);
            }

            return nextLine.All(v => v == 0) ? reading[^1] : reading[^1] + GetNextValue(nextLine);
        }

        private long GetPrevValue(List<long> reading)
        {
            var nextLine = new List<long>();
            for (int i = 0; i < reading.Count - 1; i++)
            {
                nextLine.Add(reading[i + 1] - reading[i]);
            }

            return nextLine.All(v => v == 0) ? reading[0] : reading[0] - GetPrevValue(nextLine);
        }
    }
}
