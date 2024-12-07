using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2024;

internal class Day07 : DayBase
{
    internal void Run()
    {

        //var test = Correct(12, new List<int>() { 6, 6 , 1});

        var input = GetInput("2024_07");
        long correct = 0;
        long correct2 = 0;
        foreach (var line in input)
        {
            Console.WriteLine(line);

            // With split
            //var parts = line.Split(' ', ':');
            //var result = long.Parse(parts[0]);
            //var numbers = parts[2..].Select(long.Parse).ToList();

            // With regex
            var matches = Regex.Matches(line, @"(\d+)").Select(m => m.Value).ToList();
            var result = long.Parse(matches[0]);
            var numbers = matches[1..].Select(long.Parse).ToList();

            if (Correct(result, numbers))
                correct += result;
            if (Correct2(result, numbers))
            {
                correct2 += result;
                Console.WriteLine("true");
            }
        }


        Console.WriteLine($"Sum: {correct}");
        Console.WriteLine($"Sum2: {correct2}");

    }

    private bool Correct(long result, List<long> numbers)
    {
        var value = numbers[0] + numbers[1];
        if (numbers.Count == 2 && value == result)
            return true;
        var newNumbers = new List<long> { value };
        if (numbers.Count > 2)
        {
            newNumbers.AddRange(numbers[2..]);
            if (Correct(result, newNumbers))
                return true;
        }

        value = numbers[0] * numbers[1];
        if (numbers.Count == 2)
            return value == result;
        newNumbers = [value];
        newNumbers.AddRange(numbers[2..]);
        if (Correct(result, newNumbers))
            return true;

        return false;
    }

    private bool Correct2(long result, List<long> numbers)
    {
        var value = numbers[0] + numbers[1];
        if (numbers.Count == 2 && value == result)
            return true;
        var newNumbers = new List<long> { value };
        if (numbers.Count > 2)
        {
            newNumbers.AddRange(numbers[2..]);
            if (Correct2(result, newNumbers))
                return true;
        }

        value = long.Parse($"{numbers[0]}{numbers[1]}");
        if (numbers.Count == 2 && value == result)
            return true;
        if (numbers.Count > 2)
        {
            newNumbers = [value];
            newNumbers.AddRange(numbers[2..]);
            if (Correct2(result, newNumbers))
                return true;
        }

        value = numbers[0] * numbers[1];
        if (numbers.Count == 2)
            return value == result;
        newNumbers = [value];
        newNumbers.AddRange(numbers[2..]);
        if (Correct2(result, newNumbers))
            return true;

        return false;
    }
}