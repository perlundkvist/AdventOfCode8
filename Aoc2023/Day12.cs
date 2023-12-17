using System;
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
            var sum = 0L;
            foreach (var group in record.Groups)
            {
                Logg.WriteLine($"{group}");
                var splits = GetSplits(group);
            }
            return sum;
        }

        private List<List<int>> GetSplits(string group)
        {
            var splits = new List<List<int>>();
            if (group.All(c => c == '#')) 
            {
                splits.Add(new List<int> { group.Length });
                return splits;
            }

            //var fix = group.AllIndexesOf("#");
            for (var i = 0; i < group.Length; i++)
            {
                //if (fix.Contains(i))
                //    continue;
                var first = group[..(i + 1)];
                var rest = group[(i + 1)..];
                if (rest.StartsWith("#"))
                    continue;
            }

            return splits;
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
