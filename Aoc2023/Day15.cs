using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day15 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_15");
            var hash = GetHash("HASH");
            Console.WriteLine($"Hash: {hash}.");

            var sequence = input[0].Split(',').ToList();
            var sum = sequence.Sum(s => GetHash(s));
            Console.WriteLine($"Sum: {sum}.");

            sum = GetSum2(sequence);
            Console.WriteLine($"Sum2: {sum}.");

            Console.WriteLine($"{DateTime.Now - start}");
        }

        private int GetSum2(List<string> sequence)
        {
            var boxes = new Box[256];

            foreach (var operation in sequence)
            {
                var splitter = operation.Contains("=") ? "=" : "-";
                var split = operation.Split(splitter);
                var lensId = split[0];
                var boxId = GetHash(lensId);
                var box = boxes[boxId] ?? new Box();
                boxes[boxId] = box;
                var lens = box.Lenses.FirstOrDefault(l => l.Id == lensId);
                switch (splitter)
                {
                    case "=":
                        var focal = int.Parse(split[1]);
                        if (lens != null)
                            lens.Focal = focal;
                        else
                            box.Lenses.Add(new Lens(lensId, focal));
                        break;
                    case "-":
                        if (lens != null) 
                            box.Lenses.Remove(lens);
                        break;
                }
                
            }

            var sum = 0;

            for (var i = 0; i < boxes.Length; i++)
            {
                var box = boxes[i];
                if (box == null) 
                    continue;
                var focal = 0;
                var l = 1;
                foreach (var lens in box.Lenses)
                {
                    focal += (i + 1) * l *lens.Focal;
                    l++;
                }
                sum += focal;
            }

            return sum;
        }

        private int GetHash(string input)
        {
            var hash = 0;
            foreach (var c in input)
            {
                hash += (int)c;
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }

        private class Box
        {
            public List<Lens> Lenses { get; } = new();
        }

        private class Lens(string id, int focal)
        {
            public string Id { get; } = id;
            public int Focal { get; set; } = focal;
        }
    }
}
