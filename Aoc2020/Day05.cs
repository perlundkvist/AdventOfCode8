namespace AdventOfCode8.Aoc2020
{
    class Day05 : DayBase
    {
        public void Run()
        {
            var puzzleInput = GetInput("2020_05");
            var highest = 0;
            var seats = new List<int>();
            foreach (var line in puzzleInput)
            {
                var binary = line.Replace('F', '0').Replace('B', '1').Replace('R', '1').Replace('L', '0');
                var seat = Convert.ToInt32(binary, 2);
                Console.WriteLine($"Line {line}: seat  {seat}");
                highest = Math.Max(seat, highest);
                seats.Add(seat);
            }
            Console.WriteLine($"Highest {highest}");

            for (int i = 1; i < 996; i++)
            {
                if (seats.Contains(i))
                    continue;
                Console.WriteLine($"Missing: {i}");
            }

        }
    }
}
