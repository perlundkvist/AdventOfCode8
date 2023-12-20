using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day20 : DayBase
    {
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_20s");



            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        public abstract class Module(string name)
        {
            public string Name { get; } = name;
            public abstract void HandlePulse(bool state, List<Module> modules);
        }

        public class FlipFlop(string name) : Module(name)
        {
            public bool State{ get; private set; } = false;

            public override void HandlePulse(bool state, List<Module> modules)
            {
                throw new NotImplementedException();
            }
        }

        public class Broadcaster(string name, List<string> receivers) : Module(name)
        {
            public override void HandlePulse(bool state, List<Module> modules)
            {
                foreach (var receiver in receivers)
                {
                    modules.First(m => m.Name == receiver).HandlePulse(state, modules);
                }
            }
        }
    }
}
