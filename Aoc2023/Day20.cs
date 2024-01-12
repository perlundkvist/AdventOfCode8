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

            //Logg.DoLog = false;

            var input = GetInput("2023_20");

            var modules = input.Select(Module.Create).ToList();
            modules.ForEach(m => m.SetInputs(modules));
            modules.OrderBy(m => m.Name).ToList().ForEach(Logg.WriteLine);

            var sum = PressButton(modules);
            Console.WriteLine($"Total: {sum}");

            modules = input.Select(Module.Create).ToList();
            modules.ForEach(m => m.SetInputs(modules));
            sum = PressButton2(modules);
            Console.WriteLine($"Button presses: {sum}. 884831197 is too low");

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private object PressButton(List<Module> modules)
        {
            var timesPressed = 0;
            var lowSent = 0;
            var highSent = 0;
            while (true)
            {
                //if (timesPressed > 0 && modules.All(m => !m.GetState()))
                //{
                //    break;
                //}
                modules.ForEach(Logg.WriteLine);
                var jobs = new List<(string from, string to, bool state)> { ("button", "broadcaster", false) };
                lowSent++;
                while (jobs.Any())
                {
                    var job = jobs.First();
                    jobs.RemoveAt(0);
                    Logg.WriteLine($"{job.from} -{(job.state ? "high" : "low")}-> {job.to}");
                    var module = modules.FirstOrDefault(m => m.Name == job.to);
                    if (module == null)
                        continue;
                    var newJobs = module.HandlePulse(job.state, job.from, modules);
                    //modules.ForEach(Logg.WriteLine);
                    lowSent += newJobs.Count(j => !j.state);
                    highSent += newJobs.Count(j => j.state);
                    jobs.AddRange(newJobs);
                }
                timesPressed++;
                if (timesPressed == 1000)
                    break;
            }
            return lowSent * highSent;
        }

        private object PressButton2(List<Module> modules)
        {
            var timesPressed = 0L;
            while (true)
            {
                modules.ForEach(Logg.WriteLine);
                var rxFound = 0;
                var jobs = new List<(string from, string to, bool state)> { ("button", "broadcaster", false) };
                while (jobs.Any())
                {
                    var job = jobs.First();
                    jobs.RemoveAt(0);
                    Logg.WriteLine($"{job.from} -{(job.state ? "high" : "low")}-> {job.to}");
                    var module = modules.FirstOrDefault(m => m.Name == job.to);
                    if (module == null)
                        continue;
                    var newJobs = module.HandlePulse(job.state, job.from, modules);
                    if (newJobs.Any(j => j is { to: "rx", state: false }))
                        rxFound++;
                    //modules.ForEach(Logg.WriteLine);
                    jobs.AddRange(newJobs);
                }
                timesPressed++;
                if (timesPressed % 1000000 == 0)
                    Console.WriteLine($"{timesPressed}");
                if (rxFound == 1)
                    break;
            }
            return timesPressed;
        }

        public abstract class Module(string name)
        {
            public string Name { get; } = name;
            protected readonly List<string> Inputs = new();
            protected readonly List<string> Receivers = new();

            public static Module Create(string config) 
            {
                var split = config.Split(" -> ");
                var type = split[0][0];
                Module module = type switch
                {
                    '%' => new FlipFlop(split[0][1..]),
                    '&' => new Conjunction(split[0][1..]),
                    _ => new Broadcaster(split[0])
                };
                module.Receivers.AddRange(split[1].Split(", ").ToList());
                return module;
            }

            public virtual void SetInputs(List<Module> modules)
            {
                Inputs.Clear();
                Inputs.AddRange(modules.Where(m => m.Receivers.Contains(Name)).Select(m => m.Name).Distinct().ToList());
            }

            public abstract string GetModuleType();
            public abstract bool GetState();
            public abstract List<(string from, string to, bool state)> HandlePulse(bool pulse, string sender, List<Module> modules);
            public abstract object Clone();

            public override string ToString()
            {
                return $"{GetModuleType()}{Name}, state: {GetState()}. {string.Join(", ", Inputs)} -> {string.Join(", ", Receivers)}";
            }
        }

        public class FlipFlop(string name) : Module(name)
        {
            private bool _state;

            public override string GetModuleType()
            {
                return "%";
            }

            public override bool GetState()
            {
                return _state;
            }

            public override List<(string from, string to, bool state)> HandlePulse(bool pulse, string sender,
                List<Module> modules)
            {
                if (pulse)
                    return new List<(string from, string to, bool state)>();
                _state = !_state;
                var jobs = Receivers.Select(r => (Name, r, _state)).ToList();
                return jobs;
            }
            public override object Clone()
            {
                return new FlipFlop(Name) { _state = _state };
            }
        }

        public class Conjunction(string name) : Module(name)
        {
            private readonly Dictionary<string, bool> _states = new();

            public override string GetModuleType()
            {
                return "&";
            }
            
            public override bool GetState()
            {
                return _states.Any() && _states.Values.All(v => v);
            }

            public override List<(string from, string to, bool state)> HandlePulse(bool pulse, string sender,
                List<Module> modules)
            {
                if (!_states.TryAdd(sender, pulse))
                    _states[sender] = pulse;
                var state = _states.Values.Any(v => !v);
                var jobs = Receivers.Select(r => (Name, r, state)).ToList();
                return jobs;
            }

            public override void SetInputs(List<Module> modules)
            {
                base.SetInputs(modules);
                _states.Clear();
                foreach (var module in Inputs)
                {
                    _states.TryAdd(module, false);
                }
            }            

            public override object Clone()
            {
                var conjunction = new Conjunction(Name);
                foreach (var state in _states)
                    conjunction._states.TryAdd(state.Key, state.Value);
                return conjunction;
            }
        }

        public class Broadcaster(string name) : Module(name)
        {
            private bool _state;
            public override string GetModuleType()
            {
                return "";
            }
            
            public override bool GetState()
            {
                return _state;
            }

            public override List<(string from, string to, bool state)> HandlePulse(bool pulse, string sender,
                List<Module> modules)
            {
                _state = pulse;
                var jobs = Receivers.Select(r => (Name, r, pulse)).ToList();
                return jobs;
            }

            public override object Clone()
            {
                return new Broadcaster(Name) {_state = _state};
            }
        }

    }
}
