﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day12 : DayBase
    {
        internal void Run()
        {

            var start = DateTime.Now;

            var input = GetInput("2023_12s").ToImmutableList();
            var records = input.Select(i => new CondictionRecord(i)).ToList();

            var sum = GetSum(records);
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private long GetSum(List<CondictionRecord> records)
        {
            var sum = 0L;
            foreach (var record in records)
            {
                sum += GetSum(record);
            }
            return sum;
        }

        private long GetSum(CondictionRecord record)
        {
            foreach (var group in record.Groups)
            {
                Logg.WriteLine($"{group}");
                for (var i = 0; i < group.Length; i++)
                {
                    var first = group[..(i + 1)];
                    var last = group[(i + 2)..];
                }
            }
            return 0;
        }

        public class CondictionRecord
        {
            public List<string> Groups { get; } = [];
            public List<int> Corrects { get; } = [];

            public CondictionRecord(string input)
            {
                var split = input.Split(' ');
                Groups = split[0].Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                Corrects = split[1].Split(",").Select(i => int.Parse(i)).ToList();
            }
        }
    }
}
