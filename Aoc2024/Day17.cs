using AdventOfCode8.Aoc2023;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day17 : DayBase
{
    public int[] Registers = new int[3];
    public List<int> Output = [];


    internal void Run()
    {
        Logg.DoLog = true;

        Registers[0] = 0;
        Registers[1] = 0;
        Registers[2] = 9;
        Execute([2, 6]);

        Registers[0] = 10;
        Registers[1] = 0;
        Registers[2] = 0;
        Execute([5, 0, 5, 1, 5, 4]);

        Registers[0] = 2024;
        Registers[1] = 0;
        Registers[2] = 0;
        Execute([0, 1, 5, 4, 3, 0]);

        Registers[0] = 0;
        Registers[1] = 29;
        Registers[2] = 0;
        Execute([1, 7]);

        Registers[0] = 0;
        Registers[1] = 2024;
        Registers[2] = 43690;
        Execute([4, 0]);

        Registers[0] = 729;
        Registers[1] = 0;
        Registers[2] = 0;
        Execute([0, 1, 5, 4, 3, 0]);

        #region Part 1

        Registers[0] = 30886132;
        Registers[1] = 0;
        Registers[2] = 0;
        Execute([2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 4, 5, 5, 3, 0]);

        #endregion

        #region Part 2

        Registers[0] = 117440;
        Registers[1] = 0;
        Registers[2] = 0;
        Execute([0, 3, 5, 4, 3, 0]);
        //Execute2([0, 3, 5, 4, 3, 0]);

        Execute2([2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 4, 5, 5, 3, 0]);


        #endregion

    }

    private void Execute2(List<int> program)
    {
        Output.Clear();
        for (var i = 1; i < int.MaxValue; i++)
        {
            if (i % 1000000 == 0)
                Logg.WriteLine($"{i}/{int.MaxValue}");
            Output.Clear();
            Registers[0] = i;
            Registers[1] = 0;
            Registers[2] = 0;
            try
            {
                var idx = 0;
                while (idx >= 0 && idx < program.Count)
                {
                    idx = Execute(program, idx, true);
                }

                //Logg.WriteLine($"Program: {string.Join(',', program)}");
                //Logg.WriteLine($"Output:  {string.Join(',', Output)}");
                if (Output.Count > 0 && Output[0] == 2) 
                    Console.WriteLine($"Found: {i} {Convert.ToString(i, 2)}");
                //if (Output.SequenceEqual(program))
                //{
                //    Console.WriteLine($"Found: {i}");
                //    break;
                //}
            }
            catch
            {
                // ignored
            }
        }
    }

    private void Execute(List<int> program)
    {
        Output.Clear();
        Logg.WriteLine($"Registers {Registers[0]},{Registers[1]},{Registers[2]}");
        var idx = 0;
        while (idx >= 0 && idx < program.Count)
        {
            idx = Execute(program, idx);
        }
        Logg.WriteLine($"Registers {Registers[0]},{Registers[1]},{Registers[2]}");
        Logg.WriteLine($"Output: {string.Join(',', Output)}");
        Logg.WriteLine();
    }

    private int Execute(List<int> program, int idx, bool compare = false)
    {
        var opcode = program[idx];
        var literal = program[idx + 1];
        var combo = literal is <= 3 or 7 ? literal : Registers[literal - 4];
        switch (opcode)
        {
            case 0: // adv
                Registers[0] /= (int)Math.Pow(2, combo);
                return idx + 2;
            case 1: // bxl
                Registers[1] ^= literal;
                return idx + 2;
            case 2: // bst
                Registers[1] = combo % 8;
                return idx + 2;
            case 3: // jnz
                Registers[1] = combo % 8;
                return Registers[0] == 0 ? idx + 2 : literal;
            case 4: // bxc
                Registers[1] ^= Registers[2];
                return idx + 2;
            case 5: // out
                Output.Add(combo % 8);
                if (compare)
                {
                    return 9999;
                }

                return idx + 2;
            case 6: // bdv
                Registers[1] = Registers[0] / (int)Math.Pow(2, combo);
                return idx + 2;
            case 7: // cdv
                Registers[2] = Registers[0] / (int)Math.Pow(2, combo);
                return idx + 2;
        }

        throw new ArgumentException($"Illegal value {opcode} for opcode");
    }
}