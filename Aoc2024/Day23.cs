using AdventOfCode8.Aoc2023;
using System.Diagnostics;
using System.Xml.Linq;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2024;

internal class Day23 : DayBase
{


    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_23");
        var start = DateTime.Now;

        var connections = new List<Tuple<string, string>>();
        foreach (var line in input)
        {
            var split = line.Split("-");
            connections.Add(new Tuple<string, string>(split[0], split[1]));
        }

        connections = connections.OrderBy(c => c.Item1).ToList();

        var maps = GetMaps(connections);

        //foreach (var map in maps.OrderBy(m => m[0]))
        //{
        //    Console.WriteLine(string.Join(',', map));
        //}

        Console.WriteLine($"Sets {maps.Count(m => m.Any(c => c[0] == 't'))}");
        Console.WriteLine($"{DateTime.Now-start}");

        start = DateTime.Now;
        var groups= maps.GroupBy(m => m[0]).ToList();
        foreach (var group in groups.OrderByDescending(g => g.Count()))
        {
            var list = group.ToList();
            if (!AllConnected(list, connections))
                continue;
            var computers = list.SelectMany(l => l).Distinct().OrderBy(c => c).ToList();
            Console.WriteLine($"Password: {string.Join(',', computers)}. av,dg,ot,di,dw,fa,ge,ax,kh,yw,vz,ki,qw is wrong");
            break;
        }
        Console.WriteLine($"{DateTime.Now - start}");


    }

    private bool AllConnected(List<List<string>> list, List<Tuple<string, string>> connections)
    {
        var computers = list.SelectMany(l => l).Distinct().ToList();
        var idx = 0;
        foreach (var computer1 in computers)
        {
            idx++;
            var rest = computers[idx..];
            foreach (var computer2 in rest)
            {
                if (!connections.Any(c => c.Item1 == computer1 && c.Item2 == computer2 || c.Item1 == computer2 && c.Item2 == computer1))
                    return false;
            }
        }
        return true;
    }

    private List<List<string>> GetMaps(List<Tuple<string, string>> connections)
    {
        var maps = new List<List<string>>();

        var tested = new HashSet<Tuple<string, string>>();

        foreach (var first in connections)
        {
            var seconds = connections.Where(c => !c.Equals(first) && (c.Item1 == first.Item2 || c.Item2 == first.Item2)).ToList();
            foreach (var second in seconds)
            {
                var link = first.Item2 == second.Item1 ? second.Item2 : second.Item1;
                var thirds = connections.Where(c => !c.Equals(first) && !c.Equals(second) && (c.Item1 == link || c.Item2 == link)).ToList();
                foreach (var third in thirds)
                {
                    if (third.Item1 == first.Item1 || third.Item2 == first.Item1)
                    {
                        //var last = third.Item1 == first.Item1 ? third.Item2 : third.Item1;
                        var map = new List<string> { first.Item1, first.Item2, link }.OrderBy(m => m).ToList();
                        if (!maps.Any(m => m.SequenceEqual(map)))
                            maps.Add(map);
                    }
                }
            }
        }

        return maps;
    }
}