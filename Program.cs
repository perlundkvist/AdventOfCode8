﻿using AdventOfCode8;
// See https://aka.ms/new-console-template for more information

if (args.Any())
{
    //ElseResults.Create(@"C:\Projekt\AdventOfCode3\Input\Else.json");
    var json = ElseResults.GetTopList();
    if (json != "")
        ElseResults.Create(json);
    return;
}

new AdventOfCode8.Aoc2023.Day03().Run();
