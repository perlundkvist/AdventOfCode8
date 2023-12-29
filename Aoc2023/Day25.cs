using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day25 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_25");

            var connections = GetConnections(input);
            var permutations = connections.GetPermutations(3).ToList();

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private List<(string component1, string component2)> GetConnections(List<string> input)
        {
            var connections = new List<(string component1, string component2)>();
            foreach (var line in input)
            {
                var split = line.Split(": ");
                var component1 = split[0];    
                var components = split[1].Split(' ');
                foreach (var component in components)
                {
                    var c1 = component.CompareTo(component1) < 0 ? component : component1;
                    var c2 = component.CompareTo(component1) < 0 ? component1 : component;
                    var connection = (c1, c2);
                    if (!connections.Contains(connection))
                        connections.Add(connection);
                }
            }
            return connections;
        }
    }
}
