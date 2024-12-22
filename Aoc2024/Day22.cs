using System.Diagnostics;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2024;

internal class Day22 : DayBase
{


    internal void Run()
    {
        Logg.DoLog = false;

        var input = GetInput("2024_22");

        var sum = 0L;

        foreach (var line in input)
        {
            var secret = long.Parse(line);
            var result = Calculate(secret, 2000);
            Logg.WriteLine($"{line}: {result}");
            sum += result;
        }

        Console.WriteLine($"Sum {sum}");

        var keyLists = new List<Dictionary<Key , int>>();
        foreach (var line in input)
        {
            var keyList = GetChanges(long.Parse(line));
            keyLists.Add(keyList);
        }

        var bananas = GetBananas(keyLists);
        Console.WriteLine($"Bananas {bananas}. 1400 too low");
    }

    private long GetBananas(List<Dictionary<Key, int>> keyLists)
    {
        var result = 0L;
        var keys = keyLists.SelectMany(l => l.Select(k => k.Key).Distinct()).Distinct().ToList();
        foreach (var key in keys)
        {
            var bananas = GetBananas(keyLists, key);
            result = Math.Max(result, bananas);
        }

        return result;
    }

    private long GetBananas(List<Dictionary<Key, int>> keyLists, Key key)
    {
        var result = 0L;
        foreach (var keyList in keyLists)
        {
            if (keyList.TryGetValue(key, out var bananas))
                result += bananas;
        }

        return result;
    }

    private record Key(int K1, int K2, int K3, int K4);

    private Dictionary<Key, int> GetChanges(long secret)
    {
        var changes = new List<(int price, int change)>();
        var keys = new Dictionary<Key, int>();
        var price = (int)(secret % 10);
        for (var i = 0; i < 2000; i++)
        {
            secret = Calculate(secret);
            var price2 = (int)(secret % 10);
            changes.Add((price2, price2 - price));
            Logg.WriteLine($"{secret}: {price2} ({price2 - price})");
            price = price2;
            if (i > 2)
            {
                var key = new Key(changes[i - 3].change, changes[i - 2].change, changes[i - 1].change,
                    changes[i].change);
                keys.TryAdd(key, price);
            }
        }

        //changes.ForEach(c => Console.WriteLine(c));
        return keys;
    }

    private long Calculate(long secret, int times)
    {
        for (var i = 0; i < times; i++)
        {
            secret = Calculate(secret);
        }

        return secret;
    }

    private long Calculate(long secret)
    {
        var result = secret * 64;
        secret ^= result;
        secret %= 16777216;
        result = secret / 32;
        secret ^= result;
        secret %= 16777216;
        result = secret * 2048;
        secret ^= result;
        secret %= 16777216;

        return secret;
    }

}