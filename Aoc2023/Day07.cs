namespace AdventOfCode8.Aoc2023
{
    internal class Day07 : DayBase
    {

        private static readonly List<char> CardRanks = new() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3',  '2' };
        private static readonly List<char> CardRanks2 = new() {'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'};

        private static readonly List<string> Types = new()
        {
            "Five of a kind",
            "Four of a kind",
            "Full house",
            "Three of a kind",
            "Two pair",
            "One pair",
            "High card"
        };

        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_07");

            var h1 = new Hand2("QJJJT 678");
            var h2 = new Hand2("J25T9 464");

            //var c = h1.CompareTo(h2);
            //Console.WriteLine($"{h1}");

            var h3 = new Hand2("3J25T 464");
            //var w2 = GetWinnings(new List<Hand2> {h1, h2, h3});

            var hands = input.Select(l => new Hand(l)).ToList();
            var hands2 = input.Select(l => new Hand2(l)).ToList();

            var winnings = GetWinnings(hands);
            var winnings2 = GetWinnings(hands2);

            Console.WriteLine($"Winnings: {winnings}");
            Console.WriteLine($"Winnings: {winnings2}"); 
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private object GetWinnings(List<Hand> hands)
        {
            hands.Sort();
            hands.Reverse();

            long winnings = 0;

            var idx = 1;
            foreach (var hand in hands)
            {
                winnings += hand.Bid * idx;
                idx++;
            }

            return winnings;
        }

        private object GetWinnings(List<Hand2> hands)
        {
            hands.Sort();
            hands.Reverse();
            //hands.ForEach(Console.WriteLine);
            //Console.WriteLine();
            //hands.Where(h => h.JokerCount > 0).ToList().ForEach(Console.WriteLine);

            long winnings = 0;

            var idx = 1;
            foreach (var hand in hands)
            {
                var winning = hand.Bid * idx;
                winnings += winning;
                //Console.WriteLine($"{hand} {idx} {winning,-7} {winnings}");
                idx++;
            }

            return winnings;
        }

        private class Hand2 : IComparable<Hand2> 
        {
            private int Type { get; init; }
            public long Bid{ get; init; }
            private string Cards { get; init; }

            public int JokerCount => Cards?.Count(c => c == 'J') ?? 0;

            public Hand2(string cards) 
            {
                Cards = cards.Split(' ')[0];
                Type = GetType(Cards);
                Bid = int.Parse(cards.Split(' ')[1]);
            }

            public override string ToString()
            {
                return $"{Cards} {Types[Type]} {Bid:000}";
            }

            private int GetType(string cards)
            {
                var jokerCount = JokerCount;
                if (jokerCount == 5)
                    return 0;

                var card = cards.FirstOrDefault(c => c != 'J' && cards.Count(x => x == c) + jokerCount >= 5);
                if (card != 0)
                    return 0;

                card = cards.FirstOrDefault(c => c != 'J' && cards.Count(x => x == c) + jokerCount >= 4);
                if (card != 0)
                    return 1;

                card = cards.FirstOrDefault(c => c != 'J' && cards.Count(x => x == c) + jokerCount >= 3);
                if (card != 0)
                {
                    var card2 = cards.FirstOrDefault(c => c != 'J' && c != card && cards.Count(x => x == c) == 2);
                    return card2 == 0 ? 3 : 2;
                }
                card = cards.FirstOrDefault(c => c != 'J' && cards.Count(x => x == c) + jokerCount >= 2);
                if (card != 0)
                {
                    var card2 = cards.FirstOrDefault(c => c != 'J' && c != card && cards.Count(x => x == c) == 2);
                    return card2 == 0 ? 5 : 4;
                }

                return 6;
            }

            public int CompareTo(Hand2? other)
            {
                if (other == null)
                    return 1;
                if (Type != other.Type) 
                    return Type.CompareTo(other.Type);
                for (var i = 0; i < Cards.Count(); i++)
                {
                    var card = CardRanks2.IndexOf(Cards[i]);
                    var otherCard = CardRanks2.IndexOf(other.Cards[i]);
                    if (card.CompareTo(otherCard) != 0)
                        return card.CompareTo(otherCard);
                }
                return 0;
            }
        }
        private class Hand : IComparable<Hand> 
        {
            private int Type { get; init; }
            public long Bid{ get; init; }
            private string Cards { get; init; }

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
                    var card = CardRanks.IndexOf(Cards[i]);
                    var otherCard = CardRanks.IndexOf(other.Cards[i]);
                    if (card.CompareTo(otherCard) != 0)
                        return card.CompareTo(otherCard);
                }
                return 0;
            }
        }
    }
}
