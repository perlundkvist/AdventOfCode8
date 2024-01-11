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

            var input = GetInput("2023_20s");

            var modules = input.Select(Module.Create).ToList();
            modules.ForEach(m => m.SetInputs(modules));

            var sum = PressButton(modules);
            Console.WriteLine($"Total: {sum}");

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
                if (timesPressed > 4000)
                    break;
                if (timesPressed > 0 && modules.All(m => !m.GetState()))
                {
                    break;
                }
                modules.ForEach(Logg.WriteLine);
                var clone = modules.Select(m => (Module)m.Clone()).ToList();
                var broadcaster = clone.First(m => m.Name == "broadcaster");
                broadcaster.HandlePulse(true, "broadcaster");
                foreach (var module in modules)
                {
                    var (lowPulses, highPulses) = module.SendPulses(clone);
                    lowSent += lowPulses;
                    highSent += highPulses;
                }
                modules = clone;
                timesPressed++;
            }
            return lowSent * highSent;
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

            public void SetInputs(List<Module> modules)
            {
                Inputs.Clear();
                Inputs.AddRange(modules.Where(m => m.Receivers.Contains(Name)).Select(m => m.Name).Distinct().ToList());
            }

            public abstract string GetModuleType();
            public abstract bool GetState();
            public abstract void HandlePulse(bool state, string sender);
            public abstract (int lowPulses, int highPulses) SendPulses(List<Module> modules);
            public abstract object Clone();

            public override string ToString()
            {
                return $"{GetModuleType()}{Name}, state: {GetState()}. {string.Join(", ", Inputs)} -> {string.Join(", ", Receivers)}";
            }
        }

        public class FlipFlop(string name) : Module(name)
        {
            private bool _state;
            private bool _sendPulse;

            public override string GetModuleType()
            {
                return "%";
            }

            public override bool GetState()
            {
                return _state;
            }

            public override void HandlePulse(bool state, string sender)
            {
                _sendPulse = !state;
                if (state)
                    return;
                _state = !_state;
            }

            public override (int lowPulses, int highPulses) SendPulses(List<Module> modules)
            {
                if (!_sendPulse)
                    return (0, 0);
                var lowPulses = !_state ? Receivers.Count : 0;
                var highCount = _state ? Receivers.Count : 0;
                foreach (var receiver in Receivers)
                {
                    modules.First(m => m.Name == receiver).HandlePulse(_state, name);
                }
                return (lowPulses, highCount);
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

            public override void HandlePulse(bool state, string sender)
            {
                if (!_states.TryAdd(sender, state))
                    _states[sender] = state;
            }

            public override (int lowPulses, int highPulses) SendPulses(List<Module> modules)
            {
                var state = _states.Values.Any(v => !v);
                var lowPulses = !state ? Receivers.Count : 0;
                var highCount = state ? Receivers.Count : 0;
                foreach (var receiver in Receivers)
                {
                    modules.First(m => m.Name == receiver).HandlePulse(state, name);
                }
                return (lowPulses, highCount);
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

            public override void HandlePulse(bool state, string sender)
            {
                _state = state;
            }

            public override (int lowPulses, int highPulses) SendPulses(List<Module> modules)
            {
                foreach (var receiver in Receivers)
                {
                    modules.First(m => m.Name == receiver).HandlePulse(_state, name);
                }
                var lowPulses = !_state ? Receivers.Count : 0;
                var highCount = _state ? Receivers.Count : 0;
                return (lowPulses, highCount);
            }
            public override object Clone()
            {
                return new Broadcaster(Name) {_state = _state};
            }
        }

    }
}
