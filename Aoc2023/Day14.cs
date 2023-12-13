using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day14 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_14s").ToImmutableList();
            //var records = GetMaps(input);

            //var sum = GetSum(records);
            //Console.WriteLine($"Sum: {sum}.");

            Console.WriteLine($"{DateTime.Now - start}");
        }
    }
}
