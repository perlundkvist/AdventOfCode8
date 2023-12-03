using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day03 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2023_03");

            var sum = GetSum(input);
            Console.WriteLine($"Sum: {sum}");

            sum = GetSum2(input);
            Console.WriteLine($"Sum: {sum}");
        }

        private object GetSum(List<string> input)
        {
            input.Insert(0, "".PadLeft(input.Count, '.'));
            input.Add("".PadLeft(input.Count, '.'));
            var sum = 0;
            for (int i = 1; i < input.Count - 1; i++)
            {
                string above = $".{input[i - 1]}.";
                string below = $".{input[i + 1]}."; ;
                string line = $".{input[i]}."; ;

                var number = "";
                var adjacent = false;
                for (int j = 1; j < line.Length; j++)
                {
                    var c = line[j];
                    if (c == '.' || !char.IsDigit(c))
                    {
                        if (adjacent)
                            sum += int.Parse(number);
                        adjacent = false;
                        number = "";
                        continue;
                    }

                    number += c;
                    if (adjacent)
                        continue;
                    var compareTo = above[j - 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = above[j];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = above[j + 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = line[j - 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = line[j + 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    if (j == line.Length - 1)
                        continue;

                    compareTo = below[j - 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = below[j];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                    compareTo = below[j + 1];
                    adjacent |= !char.IsDigit(compareTo) && compareTo != '.';

                }
            }
            return sum;

        }

        private object GetSum2(List<string> input)
        {
            input.Insert(0, "".PadLeft(input.Count, '.'));
            input.Add("".PadLeft(input.Count, '.'));
            var gearParts = new List<GearPart>();
            for (int i = 1; i < input.Count - 1; i++)
            {
                string above = $".{input[i - 1]}.";
                string below = $".{input[i + 1]}."; ;
                string line = $".{input[i]}."; ;

                var number = "";
                Point? adjacentPoint = null;
                for (int j = 1; j < line.Length; j++)
                {
                    var c = line[j];
                    if (c == '.' || !char.IsDigit(c))
                    {
                        if (adjacentPoint != null)
                            gearParts.Add(new GearPart(int.Parse(number), adjacentPoint.Value));
                        adjacentPoint = null;
                        number = "";
                        continue;
                    }

                    number += c;
                    adjacentPoint = GetAdjacentPoint(above[j - 1], i - 1, j - 1, adjacentPoint);
                    adjacentPoint = GetAdjacentPoint(above[j],     i - 1, j,     adjacentPoint);
                    adjacentPoint = GetAdjacentPoint(above[j + 1], i - 1, j + 1, adjacentPoint);

                    adjacentPoint = GetAdjacentPoint(line[j - 1], i, j - 1, adjacentPoint);
                    adjacentPoint = GetAdjacentPoint(line[j + 1], i, j + 1, adjacentPoint);

                    if (j == line.Length - 1)
                        continue;
                    adjacentPoint = GetAdjacentPoint(below[j - 1], i + 1, j - 1, adjacentPoint);
                    adjacentPoint = GetAdjacentPoint(below[j],     i + 1, j,     adjacentPoint);
                    adjacentPoint = GetAdjacentPoint(below[j + 1], i + 1, j + 1, adjacentPoint);

                }
            }
            long sum = 0;

            foreach (var gearPart in gearParts)
            {
                var otherParts = gearParts.Where(p => p != gearPart && p.Adjacent == gearPart.Adjacent).ToList();
                if (!otherParts.Any())
                    continue;

                if (otherParts.Count() > 1)
                    Console.WriteLine("Doubles!");

                var otherPart = otherParts.First();
                if (otherPart.Used)
                    continue;

                sum += (gearPart.Value * otherPart.Value);
                gearPart.Used = true;
                otherPart.Used = true;
            }
                
            return sum;

        }

        private Point? GetAdjacentPoint(char compareTo, int i, int j, Point? adjacentPoint)
        {
            if (compareTo != '*')
                return adjacentPoint;

            if (adjacentPoint != null && adjacentPoint.Value.X == i && adjacentPoint.Value.Y == j)
                return adjacentPoint;

            if (adjacentPoint != null)
                Console.WriteLine($"Duplicate *. Line {i}, col {j}. Previous {adjacentPoint}");

            return new Point(i, j);
        }

        private record GearPart(int Value, Point Adjacent)
        {
            public bool Used { get; set; } = false;
        };

    }
}
