using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AdventOfCode8.DayBase;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode8.Aoc2024
{
    internal class Day02 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2024_02");

            var safe = 0;
            var safe2 = 0;

            foreach (var line in input)
            {
                var numbers = line.Split(" ").Select(int.Parse).ToList();
                var s = IsSafe(numbers);
                if (s)
                    safe++;
                s = IsSafe2(line);
                if (s)
                    safe2++;
                Console.WriteLine($"{line} Safe: {s}");
            }

            Console.WriteLine($"Safe: {safe}");
            Console.WriteLine($"Safe 2: {safe2} (not 599)");

        }

        private bool IsSafe(List<int> numbers)
        {
            var increaseing = numbers[0] <= numbers[1];
            for (var i = 1; i < numbers.Count; i++)
            {
                if (increaseing && numbers[i] <= numbers[i - 1])
                    return false;
                if (!increaseing && numbers[i] >= numbers[i - 1])
                    return false;
                if (Math.Abs(numbers[i] - numbers[i - 1]) > 3)
                    return false;
            }
            return true;
        }

        private bool IsSafe2(string line)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToList();
            for (var i = 0; i < numbers.Count; i++)
            {
                numbers.RemoveAt(i);
                if (IsSafe(numbers))
                    return true;
                numbers = line.Split(" ").Select(int.Parse).ToList();
            }
            return false;
        }
    }
}
