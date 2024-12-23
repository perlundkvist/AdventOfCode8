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

        var fileName = "2024_23";
        var input = GetInput(fileName);
        var start = DateTime.Now;

        var connections = new List<Tuple<string, string>>();
        foreach (var line in input)
        {
            var split = line.Split("-");
            connections.Add(new Tuple<string, string>(split[0], split[1]));
        }

        connections = connections.OrderBy(c => c.Item1).ToList();
        var maps = GetMaps(connections);

        foreach (var map in maps.OrderBy(m => m[0]))
        {
            Console.WriteLine(string.Join(',', map));
        }

        Console.WriteLine($"Sets {maps.Count(m => m.Any(c => c[0] == 't'))}");
        Console.WriteLine($"{DateTime.Now-start}");

        var computers = maps.Select(m => m[0]).ToList().Distinct().ToList();
        Console.WriteLine(string.Join(',', computers));
        var comp = string.Join(',', computers);


    }

    private List<string> GetLinks(List<string> computers, string end, List<Tuple<string, string>> connections, List<Tuple<string, string>> usedConnections)
    {
        var last = computers.Last();
        var myConnections = connections.Where(c => !usedConnections.Contains(c) && (c.Item1 == last || c.Item2 == last)).ToList();
        if (myConnections.Count == 0)
            return computers.Last() == end ? computers : [];
        foreach (var connection in myConnections)
        {
            var next = connection.Item1 == last ? connection.Item2 : connection.Item1;
            if (computers.Contains(next))
                continue;
            if (next == end)
            {
                computers.Add(next);
                return computers;
            }
            computers.Add(next);
            var used = usedConnections.ToList();
            used.Add(connection);
            computers.AddRange(GetLinks(computers[..], end, connections, used));
        }
        return computers;
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