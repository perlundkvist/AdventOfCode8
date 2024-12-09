using static System.Reflection.Metadata.BlobBuilder;

namespace AdventOfCode8.Aoc2024;

internal class Day09 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_09").First();
        var numbers = input.Select(n =>  int.Parse(n.ToString())).ToList();
        var addFile = true;
        var blocks = new List<int>();
        var files = new List<KeyValuePair<int, int>>();
        var id = 0;
        foreach (var number in numbers)
        {
            for (var i = 0; i < number; i++)
            {
                blocks.Add(addFile ? id : -1);
            }
            if (number > 0)
                files.Add(new KeyValuePair<int, int>(addFile ? id : -1, number));

            if (addFile)
                id++;
            addFile = !addFile;
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
        Console.WriteLine();

        files = MoveLast(files);

        if (input.Length < 100)
        {
            Console.WriteLine();
            Console.WriteLine("00992111777.44.333....5555.6666.....8888..");
            Print(files);
        }

        blocks = GetBlocks(files);
        if (input.Length < 100)
            Print(blocks);

        Console.WriteLine();
        Console.WriteLine($"{GetCheckSum(blocks)} 6327183633722 too high");

    }

    private List<KeyValuePair<int, int>> MoveLast(List<KeyValuePair<int, int>> files)
    {
        var reversed = files.ToList();
        reversed.Reverse();
        foreach (var file in reversed)
        {
            if (file.Key == -1)
                continue;
            Console.WriteLine($"Checking {file}");
            var moveFrom = files.LastIndexOf(file);
            var fileTo = files.FirstOrDefault(f => f.Key == -1 && f.Value >= file.Value);
            if (fileTo.Key == 0)
                continue;
            var moveTo = files.IndexOf(fileTo);
            if (moveFrom < moveTo)
                continue;
            if (files.Count < 30)
            {
                Console.WriteLine($"Moving {file}");
                Console.WriteLine($"Before: {string.Join(' ', files)}");
            }

            files[moveFrom] = new KeyValuePair<int, int>(-1, file.Value);
            files[moveTo] = file;
            if (fileTo.Value > file.Value)
                files.Insert(moveTo + 1, new KeyValuePair<int, int>(-1, fileTo.Value - file.Value));
            if (files.Count < 30)
                Console.WriteLine($"After : {string.Join(' ', files)}");

            files = CompressEmpty(files);
            if (files.Count < 30)
            {
                Console.WriteLine($"After2: {string.Join(' ', files)}");
                Print(files);
            }
        }
        return files;
    }

    private List<KeyValuePair<int, int>> CompressEmpty(List<KeyValuePair<int, int>> files)
    {
        var compressed = new List<KeyValuePair<int, int>>();
        for (var i = 0; i < files.Count; i++)
        {
            if (i < files.Count - 1 && files[i].Key == -1 && files[i + 1].Key == -1)
            {
                compressed.Add(new KeyValuePair<int, int>(-1, files[i].Value + files[i + 1].Value));
                i++;
                continue;
            }

            compressed.Add(files[i]);
        }

        return compressed;
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

    private List<int> GetBlocks(List<KeyValuePair<int, int>> files)
    {
        var blocks = new List<int>();
        foreach (var file in files)
        {
            for (var i = 0; i < file.Value; i++)
            {
                blocks.Add(file.Key);
            }
        }
        return blocks;
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