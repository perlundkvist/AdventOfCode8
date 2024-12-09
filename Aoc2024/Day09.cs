namespace AdventOfCode8.Aoc2024;

internal class Day09 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_09s").First();
        var numbers = input.Select(n =>  int.Parse(n.ToString())).ToList();
        var file = true;
        var blocks = new List<int>();
        var files = new List<KeyValuePair<int, int>>();
        var id = 0;
        foreach (var number in numbers)
        {
            for (var i = 0; i < number; i++)
            {
                blocks.Add(file ? id : -1);
            } 
            files.Add(new KeyValuePair<int, int>(file ? id : -1, number));

            if (file)
                id++;
            file = !file;
        }

        if (input.Length < 100)
            Print(blocks);

        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == -1)
            {
                var last = blocks.Last(b => b >= 0);
                var l = blocks.LastIndexOf(last);
                if (l <= i)
                    break;
                blocks[l] = -1;
                blocks[i] = last;
            }
        }

        if (input.Length < 100)
            Print(blocks);

        Console.WriteLine($"{GetCheckSum(blocks)}");

        var reverse = files.ToList();
        reverse.Reverse();

        foreach (var move in reverse)
        {
            var moveFrom = files.IndexOf(move);
            var empty = files.FirstOrDefault(f => f.Key == -1 && f.Value >= move.Value);
            if (empty.Key == 0)
                continue;
            var moveTo = files.IndexOf(empty);
            if (moveFrom < moveTo)
            {
                files[moveTo] = move;
                files[moveFrom] = empty;
            }
        }

        if (input.Length < 100)
            Print(files);   

    }

    private long GetCheckSum(List<int> blocks)
    {
        var checksum = 0L;
        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == -1)
                continue;
            checksum += blocks[i] * i;
        }
        return checksum;
    }

    private void Print(List<int> blocks)
    {
        foreach (var block in blocks)
        {
            Console.Write(block == -1 ? '.' : block.ToString());
        }
        Console.WriteLine();
    }

    private void Print(List<KeyValuePair<int, int>> blocks)
    {
        foreach (var block in blocks)
        {
            for (int i = 0; i < block.Value; i++)
            {
                var id = block.Key == -1 ? "." : block.Key.ToString();
                Console.Write(id);
            }
        }
        Console.WriteLine();

    }


}