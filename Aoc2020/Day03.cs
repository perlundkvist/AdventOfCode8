namespace AdventOfCode8.Aoc2020
{
    class Day03 : DayBase
    {

        public void Run()
        {
            var puzzleInput = GetInput("2020_03");
            var trees = new int[5];
            var xes = new int[5];
            var moves = new int[5] { 1, 3, 5, 7, 1};

            var even = false;
            foreach (var line in puzzleInput)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == 4 && even)
                        continue;
                    if (line[xes[i]] == '#')
                        trees[i]++;
                    xes[i] += moves[i];
                    xes[i] %= line.Length;
                }
                even = !even;
            }

            Console.WriteLine($"Trees: {string.Join(',', trees)}. Product {trees.Aggregate(1, (x, y) => x * y)}");
        }
    }
}
