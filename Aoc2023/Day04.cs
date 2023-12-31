﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day04 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2023_04");

            var sum = GetSum(input);
            Console.WriteLine($"Sum: {sum}");

            sum = GetSum2(input);
            Console.WriteLine($"Sum2: {sum}");
        }

        private object GetSum(List<string> input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                sum += GetPoints(line);
            }
            return sum;
        }

        private object GetSum2(List<string> input)
        {
            var cards = new List<Card>();
            for (var i = 0; i < input.Count; i++)
            {
                var card = GetCard(i, cards);
                var winners = GetWinners(input[i]);
                for (var j = 0; j < winners; j++)
                {
                    var card2 = GetCard(i + j + 1, cards);
                    card2.Count += card.Count;
                }
            }

            return cards.Sum(c => c.Count);
        }

        private static Card GetCard(int i, ICollection<Card> cards)
        {
            var card = cards.FirstOrDefault(c => c.Id == i);
            if (card != null) return card;
            card = new Card(i);
            cards.Add(card);
            return card;
        }


        private static int GetPoints(string line)
        {
            var split = line.Split(':', '|');
            var winning = split[1].Trim().Split(' ').ToList();
            winning.Where(x => x == "").ToList().ForEach(x => winning.Remove(x));
            var numbers = split[2].Trim().Split(' ').ToList();
            numbers.Where(x => x == "").ToList().ForEach(x => numbers.Remove(x));
            var winners = winning.Intersect(numbers);

            return winners.Any() ? (int)Math.Pow(2, winners.Count() - 1) : 0;
        }

        private static int GetWinners(string line)
        {
            var split = line.Split(':', '|');
            var winning = split[1].Trim().Split(' ').ToList();
            winning.Where(x => x == "").ToList().ForEach(x => winning.Remove(x));
            var numbers = split[2].Trim().Split(' ').ToList();
            numbers.Where(x => x == "").ToList().ForEach(x => numbers.Remove(x));
            var winners = winning.Intersect(numbers);
            return winners.Count();
        }

        private class Card(int id)
        {
            public int Id => id;
            public int Count { get; set; } = 1;
        }
    }
}
