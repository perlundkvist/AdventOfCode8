using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day19 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_19s");



            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        public enum PartType {X, M, A, S}

        public class Part ()
        {

        }

        private class Workflow
        {
            public string Id { get; set; }

            public List<Rule> Rules { get; set; } = new List<Rule>();
            public string LastResult { get; set; }
        }

        public class Rule(PartType type, char comparison, int value, string result)
        {

        }
    }
}
