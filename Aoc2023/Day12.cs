using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode8.Aoc2023.Day06;

namespace AdventOfCode8.Aoc2023
{
    internal partial class Day12 : DayBase
    {
        internal void Run()
        {

            var start = DateTime.Now;

            //var record = new CondictionRecord("?.?????.?????? 1,3,1,3", true);
            //var s = GetSum(record);

            var input = GetInput("2023_12").ToImmutableList();

            var records = input.Select(i => new CondictionRecord(i)).ToList();
            var sum = GetSum(records);
            Console.WriteLine($"Sum: {sum}. 7195 is correct");

            Logg.DoLog = false;
            records = input.Select(i => new CondictionRecord(i, true)).ToList();
            sum = GetSum(records);
            Console.WriteLine($"Sum: {sum}");


            Console.WriteLine($"{DateTime.Now - start}");
        }

        private long GetSum(List<CondictionRecord> records)
        {
            var sum = 0L;
            var idx = 1;
            foreach (var record in records)
            {
                sum += GetSum2(record);
                Console.WriteLine($"{idx++} ({records.Count}) {sum}");
            }
            return sum;
        }

        private long GetSum(CondictionRecord record)
        {
            var sum = 0L;
            var springs = new [] { '#', '?' };
            var rangeList = new List<Range>[record.Corrects.Count];
            var idx = 0;
            var field = record.Field;
            foreach (var correct in record.Corrects)
            {
                var ranges = new List<Range>();
                Logg.WriteLine($"{field}: {correct}");
                var start = 0;
                var used = rangeList.Count(l => l != null);
                if (used > 0)
                {
                    var last = rangeList[used - 1];
                    var len = record.Corrects[used - 1];
                    start = last.First().Start.Value + len + 1;
                }
                while (start <= field.Length)
                {
                    start = field.IndexOfAny(springs, start);
                    //Logg.WriteLine($"Start: {start}");
                    if (start == -1)
                        break;
                    var end = start + correct;
                    if (end > field.Length)
                        break;
                    if (IsPossible(start, end, field))
                        ranges.Add(new Range(start, end));
                    start++;
                }
                Logg.WriteLine($"Indexes: {string.Join(",", ranges)}");
                rangeList[idx++] = ranges;
            }
            //var combinations = GetCombinations(rangeList);
            //sum += TestCombinations(combinations, field);
            //Logg.WriteLine($"{combinations.Count} combinations, {sum} valid");
            rangeList[0] = rangeList[0].Where(r => IsPossible(new Range(0, 0), r, field)).ToList();
            rangeList[record.Corrects.Count - 1] = rangeList[record.Corrects.Count - 1].Where(r => IsPossible(r, new Range(999, 999), field)).ToList();
            for (int i = 1; i < record.Corrects.Count - 1; i++)
            {
                var after = rangeList[i + 1].Last();
                rangeList[i] = rangeList[i].Where(r => r.End.Value < after.Start.Value).ToList();
            }
            sum += GetCombinations2(rangeList, field, 0);
            Logg.WriteLine($"Sum: {sum}");
            Logg.WriteLine();
            return sum;
        }

        private long GetSum2(CondictionRecord record)
        {
            var sum = 0L;
            var springs = new[] { '#', '?' };
            var rangeList = new List<Path>[record.Corrects.Count];
            var idx = 0;
            var field = record.Field;
            foreach (var correct in record.Corrects)
            {
                var paths = new List<Path>();
                Logg.WriteLine($"{field}: {correct}");
                var start = 0;
                var used = rangeList.Count(l => l != null);
                if (used > 0)
                {
                    var last = rangeList[used - 1];
                    var len = record.Corrects[used - 1];
                    start = last.First().Range.Start.Value + len + 1;
                }
                while (start <= field.Length)
                {
                    start = field.IndexOfAny(springs, start);
                    //Logg.WriteLine($"Start: {start}");
                    if (start == -1)
                        break;
                    var end = start + correct;
                    if (end > field.Length)
                        break;
                    if (IsPossible(start, end, field))
                        paths.Add(new Path(new Range(start, end)));
                    start++;
                }
                Logg.WriteLine($"Paths: {string.Join(",", paths)}");
                rangeList[idx++] = paths;
            }
            //var combinations = GetCombinations(rangeList);
            //sum += TestCombinations(combinations, field);
            //Logg.WriteLine($"{combinations.Count} combinations, {sum} valid");
            rangeList[0] = rangeList[0].Where(r => IsPossible(new Range(0, 0), r.Range, field)).ToList();
            rangeList[record.Corrects.Count - 1] = rangeList[record.Corrects.Count - 1].Where(r => IsPossible(r.Range, new Range(999, 999), field)).ToList();
            for (int i = 1; i < record.Corrects.Count - 1; i++)
            {
                var after = rangeList[i + 1].Last();
                rangeList[i] = rangeList[i].Where(r => r.Range.End.Value < after.Range.Start.Value).ToList();
            }
            sum += GetCombinations3(rangeList, field, 0);
            Logg.WriteLine($"Sum: {sum}");
            Logg.WriteLine();
            return sum;
        }


        [GeneratedRegex("#")]
        private static partial Regex HashRegex();

        private long TestCombinations(List<List<Range>> combinations, string field)
        {
            var sum = 0;
            //var matches  = HashRegex().Matches(field).ToList();
            var indexes  = HashRegex().Matches(field).ToList().Select(m => m.Index).ToList();

            foreach (var combination in combinations)
            {
                var valid = true;
                foreach (var index in indexes)
                {
                    if (!combination.Any(r => r.Start.Value <= index && index < r.End.Value))
                        valid = false;
                }
                if (valid)
                {
                    Logg.WriteLine($"{string.Join(",", combination)}");
                    sum++;
                }
            }
            Logg.WriteLine();
            return sum;
        }

        private List<List<Range>> GetCombinations(List<Range>[] rangeList)
        {
            var newCombinations = new List<List<Range>>();

            if (rangeList.Length == 1)
            {
                foreach (var range in rangeList[0])
                {
                    newCombinations.Add([range]);
                }
                return newCombinations;
            }
            foreach (var range in rangeList[0]) 
            {
                var greater = rangeList[1].Where(r => r.Start.Value > range.End.Value).ToList();
                if (greater.Count == 0)
                    break;
                var newList = new List<Range>[rangeList.Length - 1];
                newList[0] = greater;
                if (rangeList.Length > 2)
                    Array.Copy(rangeList, 2, newList, 1, newList.Length - 1);
                var combinations = GetCombinations(newList);
                foreach (var combination in combinations)
                {
                    var newCombination = new List<Range> { range };
                    newCombination.AddRange(combination);
                    newCombinations.Add(newCombination);
                }
            }
            return newCombinations;
        }

        private long GetCombinations2(List<Range>[] rangeList, string field, int depth)
        {
            if (rangeList.Length == 1)
            {
                //Logg.WriteLine($" end: {string.Join(",", rangeList[0])}");
                return rangeList[0].Count;
            }
            var sum = 0L;
            foreach (var range in rangeList[0]) 
            {
                var greater = rangeList[1].Where(r => r.Start.Value > range.End.Value).ToList();
                greater = greater.Where(g => IsPossible(range, g, field)).ToList();
                if (greater.Count == 0)
                    continue;
                //Logg.Write($"{depth}; {range},");
                var newList = new List<Range>[rangeList.Length - 1];
                newList[0] = greater;
                if (rangeList.Length > 2)
                    Array.Copy(rangeList, 2, newList, 1, newList.Length - 1);
                sum += GetCombinations2(newList, field, depth + 1);
            }
            return sum;
        }

        private long GetCombinations3(List<Path>[] pathList, string field, int depth)
        {
            if (pathList.Length == 1)
            {
                pathList[0].ForEach(p => p.Steps = 1);
                return pathList[0].Count;
            }
            var sum = 0L;
            foreach (var path in pathList[0])
            {
                if (path.Steps.HasValue)
                {
                    sum += path.Steps.Value;
                    continue;
                }
                var greater = pathList[1].Where(r => r.Range.Start.Value > path.Range.End.Value).ToList();
                greater = greater.Where(g => IsPossible(path.Range, g.Range, field)).ToList();
                if (greater.Count == 0)
                    continue;
                //Logg.Write($"{depth}; {range},");
                var newList = new List<Path>[pathList.Length - 1];
                newList[0] = greater;
                if (pathList.Length > 2)
                    Array.Copy(pathList, 2, newList, 1, newList.Length - 1);
                path.Steps = GetCombinations3(newList, field, depth + 1);
                sum += path.Steps.Value;
            }
            return sum;
        }


        private bool IsPossible(Range start, Range end, string field)
        {
            var indexes  = HashRegex().Matches(field).ToList().Select(m => m.Index).ToList();
            indexes  = indexes.Where(i => i >= start.End.Value && i < end.Start.Value).ToList();
            return indexes.Count == 0;
        }

        private bool IsPossible(int start, int end, string field)
        {
            var chunk = field[start..end];
            if (chunk.IndexOf('.') != -1)
                return false;
            if (start > 0 && field[start - 1] == '#')
                return false;
            if (end < field.Length && field[end] == '#')
                return false;
            return true;
        }

        public class CondictionRecord
        {
            public string Field { get; }
            public List<string> Groups { get; } = [];
            public List<int> Corrects { get; } = [];

            public CondictionRecord(string input)
            {
                var split = input.Split(' ');
                Field = split[0];
                Groups = split[0].Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                Corrects = split[1].Split(",").Select(i => int.Parse(i)).ToList();
            }

            public CondictionRecord(string input, bool _)
            {
                var split = input.Split(' ');
                Field = split[0];
                Groups = split[0].Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
                Corrects = split[1].Split(",").Select(i => int.Parse(i)).ToList();
                for (int i = 0; i < 4; i++)
                {
                    Field += "?" + split[0];
                    Corrects.AddRange(split[1].Split(",").Select(i => int.Parse(i)).ToList());
                }
                Groups = Field.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        public class Path(Range range)
        {
            public Range Range { get; } = range;
            public long? Steps { get; set; } = null;

            public override string ToString()
            {
                return $"{Range}:{Steps}";
            }
        }

    }
}
