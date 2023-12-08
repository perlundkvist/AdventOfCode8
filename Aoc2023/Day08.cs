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

            var steps = GetSteps3(instructions, nodes2);
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
            long steps = 0;
            var starts = nodes.Where(n => n.Id.EndsWith("A")).ToList();
            while (true)
            {
                var idx = (int)steps % instructions.Length;
                var direction = instructions[idx];
                var newStarts = new List<Node>();
                foreach (var node in starts)
                {
                    newStarts.Add(nodes.First(n => n.Id == (direction == 'L' ? node.Left : node.Right)));
                }
                if (steps % 100000 == 0)
                    Console.WriteLine($"{steps}");
                if (newStarts.All(node => node.Id.EndsWith("Z")))
                    break;
                starts = newStarts;
                steps++;
            }
            return steps + 1;
        }

        private long GetSteps3(string instructions, ImmutableDictionary<string, Node> nodes)
        {
            var start = DateTime.Now;
            long steps = 0;
            var starts = nodes.Where(n => n.Key.EndsWith("A")).Select(n => n.Value);
            while (true)
            {
                var idx = (int)steps % instructions.Length;
                var direction = instructions[idx];
                var newStarts = new List<Node>();
                foreach (var node in starts)
                {
                    var key = direction == 'L' ? node.Left : node.Right;
                    newStarts.Add(nodes[key]);
                }
                if (steps % 10000000 == 0)
                    Console.WriteLine($"{steps}");
                if (newStarts.All(node => node.Id.EndsWith("Z")))
                    break;
                starts = newStarts;
                steps++;
            }
            return steps + 1;
        }

        private Node GetNode(string line)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Node(split[0], split[2][1..4], split[3][0..3]);
        }

        public record Node(string Id, string Left, string Right)
        {

        }
    }
}
