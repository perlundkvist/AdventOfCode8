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

        var input = GetInput("2024_24");
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

        if (Logg.DoLog)
        {
            var wires = new List<string>();
            var zGates = gates.Where(g => g.Key.StartsWith('z')).OrderBy(g => g.Key);
            foreach (var gate in zGates)
            {
                if (gate.Value.Op != Gate.Operation.Xor)
                {
                    Logg.WriteLine($"{gate.Key}: {gate.Value.Value} {gate.Value.Inputs[0]} {gate.Value.Op} {gate.Value.Inputs[1]}");
                    wires.Add(gate.Value.Inputs[0]);
                    wires.Add(gate.Value.Inputs[1]);
                }
            }
            Console.WriteLine();
            Console.WriteLine(string.Join(',', wires.Order()));
            // Not correct: bkh,nng,psw,qtn,vdn,vtg,x26,y26

            //var xGates = gates.Where(g => g.Key.StartsWith('x')).OrderBy(g => g.Key);
            //foreach (var gate in xGates)
            //{
            //    PrintChain(gate.Key, gates);
            //}
        }

        var output = GetValue('z', gates);
        Console.WriteLine($"Output {output}");
        Console.WriteLine($"{DateTime.Now - start}");

        //var xValue = GetValue('x', gates);
        //Console.WriteLine($"x: {xValue}");
        //var yValue = GetValue('y', gates);
        //Console.WriteLine($"y: {yValue}");

        //Console.WriteLine($"x + y: {xValue + yValue}");

        //PrintChain2("x00", gates, "");
        //PrintChain2("x01", gates, "");
        ReverseChange("z01", gates, "", 0, 3);

    }

    private void PrintChain(string id, Dictionary<string, Gate> gates)
    {
        while (true)
        {
            Console.Write(id);
            if (id.StartsWith("z"))
                break;
            var possible = gates.Where(g => g.Value.Inputs.Any(i => i == id)).ToList();
            id = gates.First(g => g.Value.Inputs.Any(i => i == id)).Key;
            Console.Write("->");
        }
        Console.WriteLine();
    }

    private void PrintChain2(string id, Dictionary<string, Gate> gates, string soFar)
    {
        while (true)
        {
            soFar += id;
            if (id.StartsWith("z") || soFar.Length > 20)
            {
                Console.WriteLine(soFar);
                break;
            }
            var possible = gates.Where(g => g.Value.Inputs.Any(i => i == id)).ToList();
            foreach (var next in possible)
            {
                PrintChain2(next.Key, gates, soFar + "->");
            }
        }
    }

    private void ReverseChange(string id, Dictionary<string, Gate> gates, string soFar, int depth, int maxDepth)
    {
        var gate = gates[id];
        if (depth > maxDepth || id.StartsWith('x'))
        {
            Console.WriteLine(soFar);
            return;
        }

        soFar += id;
        foreach (var input in gate.Inputs)
        {
            var inputGate = gates[input];
            ReverseChange(input, gates, $"{soFar} {inputGate.Op} ", depth + 1, maxDepth);
        }

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