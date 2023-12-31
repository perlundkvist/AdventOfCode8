﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode8.Aoc2023
{
    internal class Day01 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2023_01");

            int sum = 0;
            int sum3 = 0;
            foreach (var line in input)
            {
                //sum += GetNumber(line);
                sum += GetNumber2(line);
                sum3 += GetNumber3(line);
                if (sum != sum3)
                    Console.WriteLine();
            }

            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Sum3: {sum3}");


        }

        private int GetNumber(string line)
        {
            var n1 = line.First(c => IsNumber(c));
            var reverse = line.ToArray().Reverse();
            var n2 = reverse.First(c => IsNumber(c));

            var value = $"{n1}{n2}";
            return int.Parse(value);
        }

        private bool IsNumber(char c)
        {
            return c >= '0' && c <= '9';
        }

        private int GetNumber2(string line)
        {
            var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            var firstIdx = line.Length;
            var number1 = 0;
            var lastIdx = -1;
            var number2 = 0;

            foreach (var number in numbers)
            {
                var idx = line.IndexOf(number);
                if (idx > -1 && idx < firstIdx)
                {
                    firstIdx = idx;
                    number1 = Array.IndexOf(numbers, number) + 1;
                    if (number1 > 9)
                        number1 -= 9; 
                }

                idx = line.LastIndexOf(number);
                if (idx > -1 && idx > lastIdx)
                {
                    lastIdx = idx;
                    number2 = Array.IndexOf(numbers, number) + 1;
                    if (number2 > 9)
                        number2 -= 9;
                }
            }
            Console.WriteLine($"{number1} - {number2}");
            return number1 * 10 + number2;
        }

        private int GetNumber3(string line)
        {
            var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            var indexed = numbers.Select(n => new { value = (Array.IndexOf(numbers, n) % 9) + 1, idx = line.IndexOf(n) }).ToList();
            var indexed2 = numbers.Select(n => new { value = (Array.IndexOf(numbers, n) % 9) + 1, idx = line.LastIndexOf(n) }).ToList();
            var number1 = indexed.Where(i => i.idx != -1).OrderBy(i => i.idx).First().value;
            if (number1 > 9)
                number1 -= 9;
            var number2 = indexed2.OrderByDescending(i => i.idx).First().value;
            if (number2 > 9)
                number2 -= 9;
            Console.WriteLine($"{number1} - {number2}");
            return number1 * 10 + number2;
        }
    }
}
