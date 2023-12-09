using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode8.Aoc2023.Day08;

namespace AdventOfCode8.Aoc2023
{
    internal class Day08 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_08");

            var instructions = input[0];

            var nodes = input[2..].Select(GetNode).ToList();
            var nodes2 = nodes.ToImmutableDictionary(n => n.Id, n => n);

            //var steps = GetSteps2(instructions, nodes);
            var steps = GetSteps3(instructions, nodes2); // 63568204859 too low

            // 18625484023687 Correct answer
            Console.WriteLine($"Steps: {steps}");

            Console.WriteLine($"{DateTime.Now - start}");

        }

        private long GetSteps(string instructions, List<Node> nodes)
        {
            long steps = 0;
            var node = nodes.First(n => n.Id == "AAA");
            while (true)
            {
                var idx = (int) steps % instructions.Length;
                var direction = instructions[idx];
                node = nodes.First(n => n.Id == (direction == 'L' ?  node.Left : node.Right));
                if (node.Id == "ZZZ")
                    break;
                steps++;
            }
            return steps + 1;
        }

        private long GetSteps2(string instructions, List<Node> nodes)
        {
            // Brute force first two series
            long steps = 0;
            var starts = nodes.Where(n => n.Id.EndsWith("A")).ToList()[0..2].ToList();
            starts.ForEach(Console.WriteLine);
            while (true)
            {
                var idx = (int)steps % instructions.Length;
                var direction = instructions[idx];
                var newStarts = new List<Node>();
                foreach (var node in starts)
                {
                    newStarts.Add(nodes.First(n => n.Id == (direction == 'L' ? node.Left : node.Right)));
                }
                //if (steps % 100000 == 0)
                //    Console.WriteLine($"{steps}");
                if (newStarts.All(node => node.Id.EndsWith("Z")))
                {
                    Console.WriteLine($"{steps+1}");
                    break;
                }
                starts = newStarts;
                steps++;
            }
            return steps + 1;
        }

        private long GetSteps3(string instructions, ImmutableDictionary<string, Node> nodes)
        {
            var start = DateTime.Now;
            var starts = nodes.Where(n => n.Key.EndsWith("A")).Select(n => n.Value).ToList();
            var factors = new List<long>();
            for (var i = 0; i < starts.Count; i++)
            {
                var pattern = GetPattern(instructions, starts[i], nodes);

                // Get prime factors
                factors.AddRange(pattern.SelectMany(p => PrimeFactors(p.Steps)));

                Console.WriteLine($"Serie {i}: {starts[i]}");
                pattern.ForEach(n => Console.WriteLine(n));
                Console.WriteLine();
            }

            var steps = factors.Distinct().Aggregate(1L, (x,y) => x * y);

            return steps;
        }

        private List<PatternNode> GetPattern(string instructions, Node node, ImmutableDictionary<string, Node> nodes)
        {
            var patterns = new List<PatternNode>();
            long steps = 0;
            while (true)
            {
                var idx = (int)steps % instructions.Length;
                var direction = instructions[idx];
                var key = direction == 'L' ? node.Left : node.Right;
                var nextNode = nodes[key];
                if (steps > 0 && steps % 10000000 == 0)
                    Console.WriteLine($"{steps}");
                if (nextNode.Id.EndsWith("Z"))
                {
                    if (patterns.Any(p => p.Index == idx && p.Node == node))
                        break;
                    patterns.Add(new PatternNode(node, steps + 1, idx));
                    //Console.WriteLine(patterns.Last());
                }
                node = nextNode;
                steps++;
            }
            return patterns;
        }

        private Node GetNode(string line)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Node(split[0], split[2][1..4], split[3][0..3]);
        }

        public record Node(string Id, string Left, string Right)
        {
            public override string ToString()
            {
                return $"{Id} ({Left} {Right})";
            }
        }

        private record PatternNode(Node Node, long Steps, int Index) 
        {
            public override string ToString()
            {
                return $"{Node}, idx {Index}, steps {Steps}"; 
            }
        }

        // A function to print all prime  
        // factors of a given number n 
        public static List<long> PrimeFactors(long n)
        {
            var factors = new List<long>();
            // Print the number of 2s that divide n 
            while (n % 2 == 0)
            {
                factors.Add(2);
                n /= 2;
            }

            // n must be odd at this point. So we can 
            // skip one element (Note i = i +2) 
            for (long i = 3; i <= Math.Sqrt(n); i += 2)
            {
                // While i divides n, print i and divide n 
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                }
            }

            // This condition is to handle the case when 
            // n is a prime number greater than 2 
            if (n > 2)
                factors.Add(n);
            return factors;
        }

    }
}
