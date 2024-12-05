namespace AdventOfCode8.Aoc2024;

internal class Day05 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_05");

        var rules = new List<Tuple<int, int>>();
        var updates = new List<List<int>>();
        foreach (var line in input)
        {
            if (line.Contains('|'))
            {
                rules.Add(new Tuple<int, int>(int.Parse(line.Split("|")[0]), int.Parse(line.Split("|")[1])));
            }
            else if (line.Contains(','))
            {
                var split = line.Split(",");
                updates.Add(split.Select(int.Parse).ToList());
            }
        }

        var sum = 0;
        var idx = 0;
        var incorrects = new List<List<int>>();
        foreach (var update in updates)
        {
            idx++;
            if (!Correct(update, rules))
            {
                incorrects.Add(update);
                continue;
            }

            Console.WriteLine($"Correct: {idx}");
            sum += update[update.Count / 2];
        }

        Console.WriteLine($"Sum: {sum}");

        sum = 0;
        foreach (var incorrect in incorrects)
        {
            Fix(incorrect, rules);
            sum += incorrect[incorrect.Count / 2];

        }
        Console.WriteLine($"Sum2: {sum}");

    }

    private void Fix(List<int> update, List<Tuple<int, int>> rules)
    {
        foreach (var rule in rules)
        {
            if (!update.Contains(rule.Item1) || !update.Contains(rule.Item2))
                continue;
            var idx1 = update.IndexOf(rule.Item1);
            var idx2 = update.IndexOf(rule.Item2);
            if (idx1 < idx2)
                continue;
            update[idx1] = rule.Item2;
            update[idx2] = rule.Item1;
            Fix(update, rules);
        }
    }

    private bool Correct(List<int> update, List<Tuple<int, int>> rules)
    {
        foreach (var rule in rules)
        {
            if (!update.Contains(rule.Item1) || !update.Contains(rule.Item2)) 
                continue;
            var idx1 = update.IndexOf(rule.Item1);
            var idx2 = update.IndexOf(rule.Item2);
            if (idx1 > idx2)
                return false;
        }
        return true;
    }
}