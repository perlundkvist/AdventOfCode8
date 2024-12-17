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

        Execute([0, 1], 0);

        const bool example = true;
        Registers[0] = example ? 729 : 30886132;
        //Registers[1] = example ? 5 : 0;

        List<int> program = example
            ? [0, 1, 5, 4, 3, 0]
            : [2, 4, 1, 1, 7, 5, 0, 3, 1, 4, 4, 4, 5, 5, 3, 0];

        #region Part 1

        var idx = 0;
        while (idx >= 0 && idx < program.Count)
        {
            idx = Execute(program, idx);
        }


        Console.WriteLine($"Output: {string.Join(',', Output)}");

        #endregion

    }

    private int Execute(List<int> program, int idx)
    {
        var opcode = program[idx];
        var literal = program[idx + 1];
        var combo = literal <= 3 ? literal : Registers[literal - 4];
        switch (opcode)
        {
            case 0: // adv
                Registers[0] /= (int) Math.Pow(2, combo);
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
                return idx + 2;
            case 6: // bdv
                Registers[1] /= (int)Math.Pow(2, combo);
                return idx + 2;
            case 7: // cdv
                Registers[2] /= (int)Math.Pow(2, combo);
                return idx + 2;
        }

        throw new ArgumentException($"Illegal value {opcode} for opcode");
    }
}