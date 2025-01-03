namespace AdventOfCode8.Aoc2020
{
    class Day10 : DayBase
    {
        public void Run()
        {
            var puzzleInput = GetInput("2020_10");

            var jolts = new List<int>() { 0 };

            foreach (var line in puzzleInput)
            {
                jolts.Add(int.Parse(line));
            }

            jolts.Add(jolts.Max() + 3);
            var diff1 = 0;
            var diff3 = 0;
            jolts = jolts.OrderBy(j => j).ToList();
            for (int i = 0; i < jolts.Count-1; i++)
            {
                var diff = jolts[i + 1] - jolts[i];
                if (diff == 1)
                    diff1++;
                else if (diff == 3)
                    diff3++;
            }

            Console.WriteLine($"Diff1: {diff1}, Diff3: {diff3}. Mult: {diff1*diff3}");

            var adapters = new List<Adapter>();

            jolts.ForEach(j => adapters.Add(new Adapter(j)));

            foreach (var adapter in adapters)
            {
                adapter.connectsTo.AddRange(adapters.Where(a => a.jolt > adapter.jolt && a.jolt <= adapter.jolt + 3).ToList());
            }


            var w2e = adapters.First().WaysToEnd();

            //Print(adapters.First());
            //long count = 0;
            //foreach (var connectTo in adapters.First().connectsTo)
            //{
            //    Print(connectTo);
            //    count += Count(connectTo);
            //}
            Console.WriteLine($"Count {w2e}");

        }

        private long Count(Adapter adapter)
        {
            if (!adapter.connectsTo.Any())
                return 1;

            long count = adapter.connectsTo.Count();
            foreach (var connectTo in adapter.connectsTo)
            {
                count *= Count(connectTo);
            }
            return count;
        }

        private void Print(Adapter adapter)
        {
            Console.Write($"{adapter.connectsTo.Count()}");
            if (!adapter.connectsTo.Any())
            {
                Console.WriteLine();
                return;
            }
            Console.Write("-");
            foreach (var connectTo in adapter.connectsTo)
            {
                Print(connectTo);
            }
        }

        protected class Adapter
        {
            public int jolt;
            public readonly List<Adapter> connectsTo = new List<Adapter>();

            private long? waysToEnd = null;

            public Adapter(int jolt)
            {
                this.jolt = jolt;
            }

            public long WaysToEnd()
            {
                if (waysToEnd.HasValue)
                    return waysToEnd.Value;

                if (!connectsTo.Any())
                {
                    waysToEnd = 1;
                    return 1;
                }
                waysToEnd = connectsTo.Sum(c => c.WaysToEnd());
                return waysToEnd.Value;
            }
        }
    }
}
