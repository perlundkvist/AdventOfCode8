using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2020
{
    class Day08 : DayBase
    {
        public void Run()
        {
            var input = GetInput("2020_08");
            var instructions = new List<Instruction>();
            foreach (var line in input)
            {
                var parts = line.Split(" ");
                var instruction = new Instruction(parts[0], int.Parse(parts[1]));
                instructions.Add(instruction);
            }

            var acc = 0;
            var index = 0;
            while (true)
            {
                var instruction = instructions[index];
                if (instruction.Tested)
                    break;
                instruction.Tested = true;
                switch (instruction.Name)
                {
                    case "nop":
                        index++;
                        break;
                    case "acc":
                        acc += instruction.Value;
                        index++;
                        break;
                    case "jmp":
                        index += instruction.Value;
                        break;
                }
            }

            Console.WriteLine($"Acc {acc}");


            foreach (var change in instructions.Where(i => i.Name == "nop" || i.Name == "jmp"))
            {
                instructions.ForEach(i => i.Tested = false);
                change.Name = change.Name == "nop" ? "jmp" : "nop";
                acc = 0;
                index = 0;
                while (true)
                {
                    if (index == instructions.Count)
                    {
                        Console.WriteLine($"Acc {acc}");
                        return;
                    }
                    if (index < 0 || index >= instructions.Count)
                        break;
                    var instruction = instructions[index];
                    if (instruction.Tested)
                        break;
                    instruction.Tested = true;
                    switch (instruction.Name)
                    {
                        case "nop":
                            index++;
                            break;
                        case "acc":
                            acc += instruction.Value;
                            index++;
                            break;
                        case "jmp":
                            index += instruction.Value;
                            break;
                    }
                }
                change.Name = change.Name == "nop" ? "jmp" : "nop";
            }

        }

        class Instruction(string name, int value)
        {
            public string Name { get; set; } = name;
            public int Value { get; } = value;
            public bool Tested { get; set; } = false;
        }
    }
}
