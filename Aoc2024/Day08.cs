using System.Text.RegularExpressions;

namespace AdventOfCode8.Aoc2024;

internal class Day08 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_08");

        var antinodes = new List<Position>();
        var antinodes2 = new List<Position>();
        var antinodes3 = new List<Position>();
        var antennas = new List<Position<char>>();

        for (var line = 0; line < input.Count; line++)
        {
            for (var col = 0; col < input[line].Length; col++)
            {
                if (input[line][col] != '.')
                    antennas.Add(new Position<char>(line, col, input[line][col]));
            }
        }

        //Print(antennas, antinodes, input.Count, input[0].Length);


        //var types = antennas.Select(a => a.Value).Distinct().ToList();
        //foreach (var type in types)
        //{
        //    antinodes.AddRange(GetAntinodes(antennas.Where(a => a.Value == type).ToList(), input.Count, input[0].Length));
        //    antinodes2.AddRange(GetAntinodes2(antennas.Where(a => a.Value == type).ToList(), input.Count, input[0].Length));
        //    antinodes3 = antinodes3.Union(GetAntinodes2(antennas.Where(a => a.Value == type).ToList(), input.Count, input[0].Length)).ToList(); // Test with union
        //}

        var antennaGroups = antennas.GroupBy(a => a.Value).ToList();
        foreach (var group in antennaGroups)
        {
            antinodes.AddRange(GetAntinodes(group.ToList(), input.Count, input[0].Length));
            antinodes2.AddRange(GetAntinodes2(group.ToList(), input.Count, input[0].Length));
            antinodes3 = antinodes3.Union(GetAntinodes2(group.ToList(), input.Count, input[0].Length)).ToList(); // Test with union
        }

        Print(antennas, antinodes2, input.Count, input[0].Length);

        Console.WriteLine($"{antinodes.Distinct().Count()}");
        Console.WriteLine($"{antinodes2.Distinct().Count()}");
        Console.WriteLine($"{antinodes3.Count} {antinodes2.Count}");
    }

    private void Print(List<Position<char>> antennas, List<Position> antinodes, int lines, int cols)
    {
        for (var line = 0; line < lines; line++)
        {
            for (var col = 0; col < cols; col++)
            {
                Console.Write(antinodes.Any(a => a.Line == line && a.Col == col) ? "#" : antennas.Any(a => a.Line == line && a.Col == col) ? antennas.First(a => a.Line == line && a.Col == col).Value : '.');
            }
            Console.WriteLine();
        }
    }

    private List<Position> GetAntinodes(List<Position<char>> antennas, int lines, int cols)
    {
        List<Position> antinodes = [];
        for (var i = 0; i < antennas.Count - 1; i++)
        {
            var antenna = antennas[i];
            foreach (var neighbour in antennas[(i+1)..])
            {
                var line = neighbour.Line - antenna.Line;
                var col = neighbour.Col - antenna.Col;

                var newLine = antenna.Line - line;
                var newCol = antenna.Col - col;
                if (newLine >= 0 && newLine < lines && newCol >= 0 && newCol < cols)
                    antinodes.Add(new Position(newLine, newCol));
                newLine = neighbour.Line + line;
                newCol = neighbour.Col + col;
                if (newLine >= 0 && newLine < lines && newCol >= 0 && newCol < cols)
                    antinodes.Add(new Position(newLine, newCol));
            }
        }
        return antinodes;
    }

    private List<Position> GetAntinodes2(List<Position<char>> antennas, int lines, int cols)
    {
        List<Position> antinodes = [];
        for (var i = 0; i < antennas.Count - 1; i++)
        {
            var antenna = antennas[i];
            antinodes.Add(new Position(antenna.Line, antenna.Col));
            foreach (var neighbour in antennas[(i + 1)..])
            {
                var line = neighbour.Line - antenna.Line;
                var col = neighbour.Col - antenna.Col;

                var newLine = antenna.Line;
                var newCol = antenna.Col;
                while (true)
                {
                    newLine -= line;
                    newCol -= col;
                    if (newLine < 0 || newLine >= lines || newCol < 0 || newCol >= cols)
                        break;
                    antinodes.Add(new Position(newLine, newCol));
                }

                newLine = antenna.Line;
                newCol = antenna.Col;
                while (true)
                {
                    newLine += line;
                    newCol += col;
                    if (newLine < 0 || newLine >= lines || newCol < 0 || newCol >= cols)
                        break;
                    antinodes.Add(new Position(newLine, newCol));
                }
            }
        }
        return antinodes;
    }
}