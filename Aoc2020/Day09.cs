namespace AdventOfCode8.Aoc2020
{
    class Day09 : DayBase
    {
        internal void Run()
        {
            var puzzleInput = GetInput("2020_09");
            var numbers = puzzleInput.Select(i => long.Parse(i)).ToList();

            for (int i = 25; i < numbers.Count; i++)
            {
                var preamble = numbers.GetRange(i - 25, 25);
                var number = numbers[i];
                if (ValidNumber(number, preamble))
                    continue;
                Console.WriteLine($"Invalid: {number}");
                break;
            }
        }

        internal void Run2()
        {
            var puzzleInput = GetInput("2020_9");
            var numbers = puzzleInput.Select(i => long.Parse(i)).ToList();

            var invalid = 393911906;

            for (int i = 0; i < numbers.Count; i++)
            {
                long sum = 0;
                var set = new List<long>();
                for (int j = i; j < numbers.Count; j++)
                {
                    sum += numbers[j];
                    if (sum > invalid)
                        break;
                    set.Add(numbers[j]);
                    if (sum < invalid)
                        continue;
                    Console.WriteLine($"Smallest = {set.Min()}, largest = {set.Max()}, sum {set.Min()+ set.Max()} ");
                    return;
                }
            }

        }

        private bool ValidNumber(long number, List<long> preamble)
        {
            for (int i = 0; i < preamble.Count-1; i++)
            {
                var rest = preamble.Skip(i + 1);
                if (rest.Any(r => r + preamble[i] == number))
                    return true;
            }
            return false;
        }
    }
}
