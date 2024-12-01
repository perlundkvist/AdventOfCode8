using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode8.Aoc2024
{
    internal class Day02 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2024_02");

            var numbers1 = new List<int>();
            var numbers2 = new List<int>();

            foreach (var line in input)
            {
                var numbers = line.Split("   ").Select(int.Parse).ToList();
                numbers1.Add(numbers[0]);
                numbers2.Add(numbers[1]);
            }

            numbers1.Sort();
            numbers2.Sort();

            var distance = 0;
            var similarity = 0;
            for (var i = 0; i < numbers1.Count; i++)
            {
                distance += Math.Abs(numbers1[i] - numbers2[i]);
                similarity += numbers1[i] * numbers2.Count(n => n == numbers1[i]);
            }

            Console.WriteLine($"Distance: {distance}");
            Console.WriteLine($"Similarity: {similarity}");

        }

    }
}
