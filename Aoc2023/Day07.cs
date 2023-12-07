using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode8.Aoc2023.Day06;

namespace AdventOfCode8.Aoc2023
{
    internal class Day07 : DayBase
    {

        private static char[] CardRanks = new [] {'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3',  '2' };

        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_07");

            var hands = input.Select(l => new Hand(l)).ToList();

            var winnings = GetWinnings(hands);

            Console.WriteLine($"Winnings: {winnings}");
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private object GetWinnings(List<Hand> hands)
        {
            hands.Sort();
            hands.Reverse();

            long winnings = 0;

            var idx = 1;
            foreach (Hand hand in hands)
            {
                winnings += hand.Bid * idx;
                idx++;
            }

            return winnings;
        }

        private class Hand : IComparable<Hand> 
        {
            public int Type { get; init; }
            public long Bid{ get; init; }
            public string Cards { get; init; }

            public Hand(string cards) 
            {
                Type = GetType(cards.Split(' ')[0]);
                Bid = int.Parse(cards.Split(' ')[1]);
                Cards = cards;
            }

            private int GetType(string cards)
            {
                var card = cards.FirstOrDefault(c => cards.Count(x => x == c) == 5);
                if (card != 0)
                    return 0;

                card = cards.FirstOrDefault(c => cards.Count(x => x == c) == 4);
                if (card != 0)
                    return 1;

                card = cards.FirstOrDefault(c => cards.Count(x => x == c) == 3);
                if (card != 0)
                {
                    var card2 = cards.FirstOrDefault(c => c != card && cards.Count(x => x == c) == 2);
                    return card2 == 0 ? 3 : 2;
                }
                card = cards.FirstOrDefault(c => cards.Count(x => x == c) == 2);
                if (card != 0)
                {
                    var card2 = cards.FirstOrDefault(c => c != card && cards.Count(x => x == c) == 2);
                    return card2 == 0 ? 5 : 4;
                }

                return 6;
            }

            public int CompareTo(Hand other)
            {
                if (Type != other.Type) 
                    return Type.CompareTo(other.Type);
                for (int i = 0; i < Cards.Count(); i++)
                {
                    if (Cards[i].CompareTo(other.Cards[i]) != 0)
                        return Cards[i].CompareTo(other.Cards[i]);
                }
                return 0;
            }
        }
    }
}
