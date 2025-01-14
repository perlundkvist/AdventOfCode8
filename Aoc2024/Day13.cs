﻿using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day13 : DayBase
{

    internal void Run()
    {
        var input = GetInput("2024_13");

        List<(Position A, Position B, Position Prize)> machines = new();
        Position? a = null;
        Position? b = null;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var numbers = Regex.Matches(line, @"\d+");
            var x = int.Parse(numbers[0].Value);
            var y = int.Parse(numbers[1].Value);
            if (line.StartsWith("Button A:"))
                a = new Position(x, y);
            if (line.StartsWith("Button B:"))
                b = new Position(x, y);
            if (line.StartsWith("Prize:"))
            {
                var prize = new Position(x, y);
                if (a != null && b != null)
                    machines.Add((a, b, prize));
            }
        }

        var cost = 0L;

        //machines.ForEach(m => Console.WriteLine(m));

        //foreach (var machine in machines)
        //{
        //    //Console.WriteLine(machine);
        //    cost += GetCost(machine);
        //}


        var cost2 = 0L;
        foreach (var machine in machines)
        {
            cost += GetCost2(machine, 0);
            cost2 += GetCost2(machine, 10000000000000);
        }

        Console.WriteLine($"Cost: {cost}");
        Console.WriteLine($"Cost 2: {cost2}. 35743889833877 too low. Diff {cost2 - 35743889833877}");

    }

    private long GetCost((Position A, Position B, Position Prize) machine)
    {
        var cost = 0L;

        for (var a = 0; a < Math.Min(100, machine.Prize.Col/machine.A.Col); a++)
        {
            var rest = machine.Prize.Col - a * machine.A.Col;
            var mod  = rest % machine.B.Col;
            if (mod != 0)
                continue;
            var b = rest / machine.B.Col;
            //Console.WriteLine($"a={a}, b={b}");
            var priceLine = a * machine.A.Line + b * machine.B.Line;
            if (priceLine != machine.Prize.Line)
                continue;
            var machineCost = a * 3 + b;
            //if (cost != 0)
            //    Console.WriteLine($"Following cost={cost}");
            cost = cost == 0 ? machineCost : Math.Min(cost, machineCost);
            //Console.WriteLine($"a={a}, b={b}, cost={cost}");
        }
        //Console.WriteLine();

        return cost;
    }

    private long GetCost2((Position A, Position B, Position Prize) machine, long addition)
    {
        var cost = 0L;

        double targetCol = machine.Prize.Col + addition;
        double targetLine = machine.Prize.Line + addition;
        
        //Console.WriteLine(machine);

        var start = new DPoint(0, targetLine / machine.B.Line);
        var end = new DPoint(targetLine / machine.A.Line, 0);
        var l1 = new Line(start, end);

        start = new DPoint(0, targetCol / machine.B.Col);
        end = new DPoint(targetCol / machine.A.Col, 0);
        var l2 = new Line(start, end);

        var intersection = l1.GetIntersectionWith(l2);
        if (intersection == null)
            return cost;

        var a = (long)Math.Round(intersection.X);
        var b = (long)Math.Round(intersection.Y);
        if (machine.A.Line * a + machine.B.Line * b != (long) targetLine)
            return cost;
        if (machine.A.Col * a + machine.B.Col * b != (long)targetCol)
            return cost;

        cost = (long)(a * 3 + b);
        return cost;

    }

}