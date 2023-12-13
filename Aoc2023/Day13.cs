using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day13 : DayBase
    {
        internal void Run()
        {

            var start = DateTime.Now;

            var input = GetInput("2023_13s").ToImmutableList();
            var records = GetMaps(input);

            var sum = GetSum(records);
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private object GetSum(List<List<string>> records)
        {
            long sum = 0;
            foreach(var record in records)
            {
                sum += GetSum(record);
            }
            return sum;
        }

        private long GetSum(List<string> record)
        {
            var sum = 0;
            for (int l = 0; l < record.Count; l++)
            {
                var line = record[l];
                var allMatch = true;
                for (int c = 1; c < line.Length - 1; c++)
                {
                    if (!ColMirrors(line, c))
                    {
                        allMatch = false;
                        break;
                    }
                }
                if (!allMatch)
                    continue;
            }
            return sum;
        }

        private bool ColMirrors(string line, int col)
        {
            var left = line.Substring(0, col);
            left.Reverse();
            var right = line.Substring(col);

            var shortest = left.Count() < right.Count() ? left : right;
            var longest = shortest == left ? right : left;
            for (int c = 0; c < shortest.Count(); c++)
            {
                if (shortest[c] != longest[c])
                    return false;
            }

            return true;
        }

        private List<List<string>> GetMaps(ImmutableList<string> input)
        {
            var maps = new List<List<string>>();
            var map = new List<string>();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    maps.Add(map);
                    map = new List<string>();
                    continue;
                }
                map.Add(line);
            }
            return maps;
        }
    }
}
