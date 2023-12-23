using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day19 : DayBase
    {
        public static char[] PartType = { 'x', 'm', 'a', 's' };

        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_19");
            var split = input.IndexOf(input.First(string.IsNullOrWhiteSpace));
            var workFlows = GetWorkFlows(input[..split]);
            var parts = GetParts(input[(split + 1)..]);

            var sum = Evaluate(parts, workFlows);
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private long Evaluate(List<Part> parts, List<Workflow> workFlows)
        {
            var sum = 0L;

            foreach (var part in parts)
            {
                Evaluate(part, workFlows);
                if (part.Evaluation == "A")
                    sum += part.Values.Sum();
            }

            return sum;
        }

        private void Evaluate(Part part, List<Workflow> workFlows)
        {
            var nextFlow = "in";
            while (true)
            {
                if (nextFlow == "A" || nextFlow == "R")
                {
                    part.Evaluation = nextFlow;
                    break;
                }
                var workFlow = workFlows.First(f => f.Id == nextFlow);
                nextFlow = workFlow.LastResult;
                foreach (var rule in workFlow.Rules)
                {
                    if (!rule.Evalute(part))
                        continue;
                    nextFlow = rule.Result;
                    break;
                }
            }
        }

        private List<Workflow> GetWorkFlows(List<string> list)
        {
            var workFlows = new List<Workflow>();
            foreach (var item in list)
            {
                var split = item[..^1].Split('{');
                var workFlow = new Workflow(split[0]);
                split = split[1].Split(',');
                foreach (var ruleDesc in split[..^1])
                {
                    var ruleParts = ruleDesc.Split(":");
                    var rule = new Rule(ruleParts[0][0], ruleParts[0][1], int.Parse(ruleParts[0][2..]), ruleParts[1]);
                    workFlow.Rules.Add(rule);
  }
                workFlow.LastResult = split[^1];
                workFlows.Add(workFlow);
            }
            return workFlows;
        }

        private List<Part> GetParts(List<string> ratings)
        {
            var parts = new List<Part>();
            foreach (var rating in ratings)
            {
                var values = rating[1..^1].Split(',');
                var part = new Part();
                foreach (var item in values)
                {
                    var split = item.Split("=");
                    part.SetValue(split[0][0], int.Parse(split[1]));
                }
                parts.Add(part);
            }
            return parts;
        }

        public record Part()
        {
            public string Evaluation = "";
            public int[] Values { get; } = new int[4];
            public void SetValue(char type , int value)
            {
                Values[Array.IndexOf(PartType, type)] = value;
            }
        }

        private record Workflow(string Id)
        {
            public List<Rule> Rules { get; set; } = [];
            public string LastResult { get; set; } = "";
        }

        public record Rule(char Type, char Comparison, int Value, string Result)
        {
            public int TypeIdx { get; init; } = Array.IndexOf(PartType, Type);

            public bool Evalute(Part part)
            {
                var partValue = part.Values[TypeIdx];
                return Comparison switch
                {
                    '<' when partValue < Value => true,
                    '>' when partValue > Value => true,
                    _ => false,
                };
            }
        }
    }
}
