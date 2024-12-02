namespace AdventOfCode8.Aoc2024;

internal class Day02 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_02");

        var safe = 0;
        var safe2 = 0;

        foreach (var line in input)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToList();
            var s = IsSafe(numbers);
            if (s)
                safe++;
            s = IsSafe2(line);
            if (s)
                safe2++;
            Console.WriteLine($"{line} Safe: {s}");
        }

        Console.WriteLine($"Safe: {safe}");
        Console.WriteLine($"Safe 2: {safe2} (not 599)");

    }

    private static bool IsSafe(List<int> numbers)
    {
        var increasing = numbers[0] <= numbers[1];
        for (var i = 1; i < numbers.Count; i++)
        {
            if (increasing && numbers[i] <= numbers[i - 1])
                return false;
            if (!increasing && numbers[i] >= numbers[i - 1])
                return false;
            if (Math.Abs(numbers[i] - numbers[i - 1]) > 3)
                return false;
        }
        return true;
    }

    private static bool IsSafe2(string line)
    {
        var numbers = line.Split(" ").Select(int.Parse).ToList();
        for (var i = 0; i < numbers.Count; i++)
        {
            numbers.RemoveAt(i);
            if (IsSafe(numbers))
                return true;
            numbers = line.Split(" ").Select(int.Parse).ToList();
        }
        return false;
    }
}