using AdventOfCode8;
// See https://aka.ms/new-console-template for more information

if (args.Any())
{
    //ElseResults.Create(@"C:\Projekt\AdventOfCode3\Input\Else.json");
    var json = args.First() == "Web" ? ElseResults.GetTopList(args[1]) : File.ReadAllText(args.First());
    if (json != "")
        ElseResults.Create(json, args.Any(a => a.Equals("times", StringComparison.CurrentCulture)));

    Console.WriteLine();
    Console.WriteLine($"Source: {args.First()}");
    return;
}

new AdventOfCode8.Aoc2020.Day12().Run();
