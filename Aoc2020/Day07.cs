using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2020
{
    class Day07 : DayBase
    {
        public void Run()
        {
            var input = GetInput("2020_07");
            var bags = new List<Bag>();
            foreach (var line in input)
            {
                var parts = line.Split(" bags contain ");
                var bag = new Bag(parts[0]);
                bags.Add(bag);
                if (parts[1] == "no other bags.")
                {
                    bag.BagCount = 1;
                    continue;
                }
                var contains = parts[1].Split(", ");
                foreach (var contain in contains)
                {
                    var match = Regex.Match(contain, "(\\d+) (.+) bag");
                    var count = int.Parse(match.Groups[1].Value);
                    var name = match.Groups[2].Value;
                    bag.Contains.Add(name, count);
                }
            }
            //var tested = new List<Bag>();
            var goldBags = new List<string>();
            foreach (var bag in bags)
            {
                if (CanContainGoldBag(bag, bags))
                    goldBags.Add(bag.Name);
            }

            Console.WriteLine($"Bags {goldBags.Distinct().Count()}");

            while (bags.Any(b => b.BagCount == -1))
            {
                foreach (var bag in bags.Where(b => b.BagCount == -1))
                {
                    var containedBags = bag.Contains.Select(c => bags.First(b => b.Name == c.Key)).ToList();
                    if (containedBags.Any(b => b.BagCount == -1))
                        continue;
                    bag.BagCount = 1;
                    foreach (var containedBag in containedBags)
                    {
                        var count = bag.Contains[containedBag.Name];
                        bag.BagCount += count * containedBag.BagCount;
                    }
                }
            }

            Console.WriteLine($"Shiny gold bag contains {bags.First(b => b.Name == "shiny gold").BagCount - 1}");

        }

        private bool CanContainGoldBag(Bag bag, List<Bag> bags)
        {
            foreach (var contain in bag.Contains)
            {
                var subBag = bags.First(b => b.Name == contain.Key);
                if (subBag.Name == "shiny gold")
                    return true;
                if (CanContainGoldBag(subBag, bags))
                    return true;
            }
            return false;
        }

        class Bag(string name)
        {
            public string Name { get; init; } = name;
            public Dictionary<string, int> Contains { get; } = new();
            public int BagCount { get; set; } = -1;
        }
    }
}
