using AdventOfCode8.Aoc2023;
using System.Diagnostics;
using System.Xml.Linq;
using static AdventOfCode8.DayBase;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode8.Aoc2024;

internal class Day24 : DayBase
{


    internal void Run()
    {
        Logg.DoLog = true;

        var input = GetInput("2024_24f");
        var start = DateTime.Now;

        var gates = new Dictionary<string, Gate>();
        foreach (var line in input)
        {
            if (line.Contains(":"))
            {
                var split = line.Split(": ");
                var gate = new Gate(split[0], Gate.Operation.None)
                {
                    Value = int.Parse(split[1])
                };
                gates.Add(gate.Name, gate);
            }
            else if (line.Contains("->"))
            {
                var split = line.Split(" ");
                var operation = split[1] switch
                {
                    "AND" => Gate.Operation.And,
                    "OR" => Gate.Operation.Or,
                    "XOR" => Gate.Operation.Xor,
                    _ => Gate.Operation.None
                };
                var gate = new Gate(split[4], operation);
                gate.Inputs.Add(split[0]);
                gate.Inputs.Add(split[2]);

                gates.Add(gate.Name, gate);
            }
        }

        while (true)
        {
            var gate = gates.Values.FirstOrDefault(g => !g.Value.HasValue && g.Inputs.All(i => gates[i].Value.HasValue));
            if (gate == null)
                break;
            gate.Calculate(gates);
        }

        var output = GetValue('z', gates);
        Console.WriteLine($"Output   {output}");
        //Console.WriteLine($"{DateTime.Now - start}");

        // Part 2
        var x = GetValue('x', gates);
        var y = GetValue('y', gates);
        var expected = x + y;
        Console.WriteLine($"Expected {expected}");
        Console.WriteLine($"Diff {expected-output}");
        Console.WriteLine();
        Console.WriteLine("              4         3         2         1");
        Console.WriteLine("         5432109876543210987654321098765432109876543210");
        Console.WriteLine($"Output   {Convert.ToString(output, 2)}");
        Console.WriteLine($"Expected {Convert.ToString(expected, 2)}");

        // https://circuitverse.org/simulator

        var changedGates = new List<string> { "kth", "z12", "gsd", "z26", "tbt", "z32", "vpm", "qnf" };
        Console.WriteLine($"Swapped wires  {string.Join(',', changedGates.Order())}");

    }

    private static long GetValue(char id, Dictionary<string, Gate> gates)
    {
        var output = 0L;
        var power = 0;
        var zGates = gates.Where(g => g.Key.StartsWith(id)).OrderBy(g => g.Key);
        foreach (var gate in zGates)
        {
            output += (gate.Value.Value ?? 0) * (long)Math.Pow(2, power);
            power++;
        }
        return output;
    }

    private class Gate(string name, Gate.Operation operation)
    {
        public enum Operation
        {
            And,
            Or,
            Xor,
            None
        }

        public string Name { get; set; } = name;
        public Operation Op{ get; set; } = operation;
        public int? Value { get; set; }
        public readonly List<string> Inputs = [];

        public void Calculate(Dictionary<string, Gate> gates)
        {
            if (Value.HasValue)
                return;
            if (Inputs.Count != 2)
                return;
            Value = operation switch
            {
                Operation.And => gates[Inputs[0]].Value & gates[Inputs[1]].Value,
                Operation.Or => gates[Inputs[0]].Value | gates[Inputs[1]].Value,
                Operation.Xor => gates[Inputs[0]].Value ^ gates[Inputs[1]].Value,
                _ => 0
            };
        }
    }

}