﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
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

            var input = GetInput("2023_13").ToImmutableList();
            var records = GetMaps(input);

            //var sum = GetSum(records[3]);

            var sum = GetSum(records);

            Console.WriteLine($"Sum: {sum}. 39815 is too low");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private object GetSum(List<List<string>> records)
        {
            int sum = 0;
            var idx = 0;
            foreach(var record in records)
            {
                var s = GetSum(record);
                sum += s;
                Console.WriteLine($"Record {idx} sum: {s}");
                PrintRecord(record, s);
                idx++;
            }
            return sum;
        }

        private void PrintRecord(List<string> record, int mirror)
        {
            var colSplit = mirror >= 100 ? 0 : mirror;
            var lineSplit = mirror >= 100 ? mirror/ 100 : 0;

            var idx = 0;
            var head = "   12345678901234567890"[..(record[0].Length + 3)];
            Console.WriteLine(colSplit > 0 ? $"{head[..colSplit]} {head[colSplit..]}" : head);
            foreach (var line in record)
            {
                if (lineSplit > 0 && idx == lineSplit)
                    Console.WriteLine("   ".PadRight(line.Length + 3, '-'));

                Console.Write($"{(idx + 1):00} ");
                if (colSplit == 0 && lineSplit == 0)
                    Console.WriteLine("line");

                if (colSplit > 0)
                    Console.WriteLine($"{line[..colSplit]}|{line[colSplit..]}");

                if (lineSplit > 0)
                    Console.WriteLine(line);

                idx++;
            }
            Console.WriteLine();
        }

        private int GetSum(List<string> record)
        {
            var sum = 0;

            var cols = record[0].Length;
            var lines = record.Count;

            for (var c = 1; c < cols - 1; c++)
            {
                var allMatch = true;
                foreach (var line in record)
                {
                    if (Mirrors(line, c))
                        continue;
                    
                    allMatch = false;
                    break;
                }
                if (!allMatch)
                    continue;
                sum = c;
                break;
            }

            if (sum > 0)
                return sum;

            for (var l = 0; l < lines - 1; l++)
            {
                if (!MirrorsDown2(l, record))
                    continue;
                sum = l * 100;
                break;
            }
            return sum;
        }

        private bool MirrorsDown(int start, List<string> lines)
        {
            //Console.WriteLine($"MirrorsDown: {start}"); 
            var idx = start;
            var matchIdx = start + 1;
            while (idx >= 0 && matchIdx < lines.Count)
            {
                var thisLine = lines[idx];
                var matchLine = lines[matchIdx];

                //Console.WriteLine($"idx: {idx}, matchIdx: {matchIdx} {Environment.NewLine}{thisLine}{Environment.NewLine}{matchLine}");

                if (thisLine != matchLine)
                    return false;
                idx--;
                matchIdx++;
            }
            return true;
        }

        private bool MirrorsDown2(int start, List<string> lines)
        {
            //Console.WriteLine($"MirrorsDown2: {start}");
            if (start == 0)
                return false;
            var above = lines[..start];
            above.Reverse();
            var below = lines[start..];
            var shortest = above.Count < below.Count ? above : below;
            var longest = shortest == above ? below : above;
            longest = longest[..shortest.Count];
            var equal = shortest.SequenceEqual(longest);
            return equal;
        }

        private bool Mirrors(string line, int col)
        {
            var left = line.Substring(0, col);
            left = new string(left.Reverse().ToArray());
            var right = line.Substring(col);

            var shortest = left.Count() < right.Count() ? left : right;
            var longest = shortest == left ? right : left;

            longest = longest.Substring(0, shortest.Count());
            return shortest == longest;
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
            if (map.Any())
                maps.Add(map);
            return maps;
        }
    }
}
