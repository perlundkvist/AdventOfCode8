using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2024;

internal class Day04 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_04");

        //input = ["ASAMXS"];

        var sum = 0;
        var y = 0;
        foreach (var line in input)
        {
            var indexes = line.AllIndexesOf("X");
            foreach (var index in indexes)
            {
                sum += Xmas(index, y, input);
            }
            y++;
        }

        Console.WriteLine($"Sum: {sum}");

        sum = 0;
        y = 0;
        foreach (var line in input)
        {
            var indexes = line.AllIndexesOf("A");
            foreach (var index in indexes)
            {
                sum += MasX(index, y, input);
            }
            y++;
        }

        Console.WriteLine($"Sum2: {sum}");

    }

    private int Xmas(int index, int y, List<string> input)
    {
        var sum = 0;
        if (FindLeft(index, y, input, "MAS"))
            sum++;
        if (FindRight(index, y, input, "MAS"))
            sum++;
        if (FindDown(index, y, input, "MAS"))
            sum++;
        if (FindUp(index, y, input, "MAS"))
            sum++;
        if (FindLeftUp(index, y, input, "MAS"))
            sum++;
        if (FindRightUp(index, y, input, "MAS"))
            sum++;
        if (FindLeftDown(index, y, input, "MAS"))
            sum++;
        if (FindRightDown(index, y, input, "MAS"))
            sum++;
        if (sum > 0 && input.Count < 11)
            Console.WriteLine($"{sum} XMAS at {y + 1},{index + 1}");
        return sum;
    }


    private int MasX(int index, int y, List<string> input)
    {
        var sum = 0;
        if (FindLeftUp(index, y, input, "M") && FindRightDown(index, y, input, "S"))
        {
            if (FindLeftDown(index, y, input, "M") && FindRightUp(index, y, input, "S"))
                sum++;
            if (FindLeftDown(index, y, input, "S") && FindRightUp(index, y, input, "M"))
                sum++;
        }
        if (FindLeftUp(index, y, input, "S") && FindRightDown(index, y, input, "M"))
        {
            if (FindLeftDown(index, y, input, "M") && FindRightUp(index, y, input, "S"))
                sum++;
            if (FindLeftDown(index, y, input, "S") && FindRightUp(index, y, input, "M"))
                sum++;
        }
        if (FindRightUp(index, y, input, "M") && FindLeftDown(index, y, input, "S"))
        {
            if (FindLeftUp(index, y, input, "M") && FindRightDown(index, y, input, "S"))
                sum++;
            if (FindLeftUp(index, y, input, "S") && FindRightDown(index, y, input, "M"))
                sum++;
        }
        if (FindRightUp(index, y, input, "S") && FindLeftDown(index, y, input, "M"))
        {
            if (FindLeftUp(index, y, input, "M") && FindRightDown(index, y, input, "S"))
                sum++;
            if (FindLeftUp(index, y, input, "S") && FindRightDown(index, y, input, "M"))
                sum++;
        }

        sum /= 2;
        if (sum > 0 && input.Count < 11)
            Console.WriteLine($"{sum} X-MAS at {y + 1},{index + 1}");
        return sum;
    }

    private bool FindLeft(int index, int i, List<string> input, string rest)
    {
        if (index== 0)
            return false;
        if (input[i][index - 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindLeft(index - 1, i, input, rest[1..]);
    }

    private bool FindRight(int index, int i, List<string> input, string rest)
    {
        if (index == input[0].Length - 1)
            return false;
        if (input[i][index + 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindRight(index + 1, i, input, rest[1..]);
    }

    private bool FindUp(int index, int i, List<string> input, string rest)
    {
        if (i == 0)
            return false;
        if (input[i-1][index] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindUp(index, i - 1, input, rest[1..]);
    }

    private bool FindDown(int index, int i, List<string> input, string rest)
    {
        if (i == input.Count - 1)
            return false;
        if (input[i + 1][index] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindDown(index, i + 1, input, rest[1..]);
    }

    private bool FindLeftUp(int index, int i, List<string> input, string rest)
    {
        if (index == 0)
            return false;
        if (i == 0)
            return false;
        if (input[i - 1][index - 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindLeftUp(index - 1, i - 1, input, rest[1..]);
    }

    private bool FindLeftDown(int index, int i, List<string> input, string rest)
    {
        if (index == 0)
            return false;
        if (i == input.Count - 1)
            return false;
        if (input[i + 1][index - 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindLeftDown(index - 1, i + 1, input, rest[1..]);
    }

    private bool FindRightUp(int index, int i, List<string> input, string rest)
    {
        if (index == input[0].Length - 1)
            return false;
        if (i == 0)
            return false;
        if (input[i - 1][index + 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindRightUp(index + 1, i - 1, input, rest[1..]);
    }

    private bool FindRightDown(int index, int i, List<string> input, string rest)
    {
        if (index == input[0].Length - 1)
            return false;
        if (i == input.Count - 1)
            return false;
        if (input[i + 1][index + 1] != rest[0])
            return false;
        if (rest.Length == 1)
            return true;
        return FindRightDown(index + 1, i + 1, input, rest[1..]);
    }

}