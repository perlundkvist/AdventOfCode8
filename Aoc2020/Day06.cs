namespace AdventOfCode8.Aoc2020
{
    class Day06 : DayBase
    {
        public void Run()
        {
            var puzzleInput = GetInput("2020_06");
            var lines = new List<string>();
            var answers = 0;
            foreach (var line in puzzleInput)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    //var groupAnswers = string.Join("", lines);
                    //var distinct = groupAnswers.Distinct();

                    var distinct = lines.First().Distinct().ToArray();

                    foreach (var line2 in lines)
                    {
                        distinct = distinct.Intersect(line2.ToArray()).ToArray();
                    }
                    
                    answers += distinct.Count();
                    lines.Clear();
                    continue;
                }
                lines.Add(line);
            }
            Console.WriteLine($"Answers {answers}");
        }
    }
}
