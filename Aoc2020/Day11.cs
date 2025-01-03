namespace AdventOfCode8.Aoc2020
{
    class Day11 : DayBase
    {
        internal void Run()
        {
            var puzzleInput = GetInput("2020_11s");
            var places = new List<Place>();
            var y = 0;
            foreach (var line in puzzleInput)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    places.Add(new Place { X = x, Y = y, Occupied = line[x] });
                }
                y++;
            }

            PrintPlaces(places);

            while (true)
            {
                var oldPlaces = places.Select(p => (Place) p.Clone()).ToList();
                var changed = false;
                foreach (var place in places)
                {
                    changed |= place.SetOccupied(oldPlaces);
                }
                PrintPlaces(places);
                if (!changed)
                    break;
            }
            Console.WriteLine($"Occuied = {places.Count(p => p.Occupied == '#')}");
        }

        internal void Run2()
        {
            var puzzleInput = GetInput("2020_11");
            var places = new List<Place>();
            var y = 0;
            foreach (var line in puzzleInput)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    places.Add(new Place { X = x, Y = y, Occupied = line[x] });
                }
                y++;
            }

            PrintPlaces(places);

            while (true)
            {
                var oldPlaces = places.Select(p => (Place)p.Clone()).ToList();
                var changed = false;
                foreach (var place in places)
                {
                    changed |= place.SetOccupied2(oldPlaces);
                }
                PrintPlaces(places);
                if (!changed)
                    break;
            }
            Console.WriteLine($"Occuied = {places.Count(p => p.Occupied == '#')}");
        }

        private void PrintPlaces(List<Place> places)
        {
            for (int y = 0; y <= places.Max(p => p.Y); y++)
            {
                foreach (var place in places.Where(p => p.Y == y).OrderBy(p => p.X))
                {
                    Console.Write(place.Occupied);

                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private class Place : ICloneable
        {
            public int X;
            public int Y;
            public char Occupied;

            public object Clone()
            {
                return new Place { X = X, Y = Y, Occupied = Occupied };
            }

            public override bool Equals(object other)
            {
                return ((Place)other).X == X && ((Place)other).Y == Y;
            }

            internal bool SetOccupied(List<Place> oldPlaces)
            {
                if (Occupied == '.')
                    return false;
                var oldOccupied = Occupied;
                var neighbours = GetNeighbours(oldPlaces);
                if (Occupied == 'L' && !neighbours.Any(p => p.Occupied == '#'))
                    Occupied = '#';
                else if (Occupied == '#' && neighbours.Count(p => p.Occupied == '#') >= 4)
                    Occupied = 'L';
                return oldOccupied != Occupied;
            }

            private List<Place> GetNeighbours(List<Place> oldPlaces)
            {
                var neighbours = oldPlaces.Where(p => p.X >= X - 1 && p.X <= X + 1 && p.Y >= Y - 1 && p.Y <= Y + 1 && Occupied != '.' && !p.Equals(this)).ToList();
                return neighbours;
            }

            internal bool SetOccupied2(List<Place> oldPlaces)
            {
                if (Occupied == '.')
                    return false;
                var oldOccupied = Occupied;
                var neighbours = GetNeighbours2(oldPlaces);
                if (Occupied == 'L' && !neighbours.Any(p => p.Occupied == '#'))
                    Occupied = '#';
                else if (Occupied == '#' && neighbours.Count(p => p.Occupied == '#') >= 5)
                    Occupied = 'L';
                return oldOccupied != Occupied;
            }

            private List<Place> GetNeighbours2(List<Place> oldPlaces)
            {
                var neighbours = new List<Place>();
                var chairs = oldPlaces.Where(p => p.Occupied != '.').ToList();
                var chair = chairs.OrderByDescending(p => p.X).FirstOrDefault(p => p.Y == Y && p.X < X); // Right
                if (chair != null)
                    neighbours.Add(chair);
                chair = chairs.OrderBy(p => p.X).FirstOrDefault(p => p.Y == Y && p.X > X); // Left
                if (chair != null)
                    neighbours.Add(chair);
                chair = chairs.OrderByDescending(p => p.Y).FirstOrDefault(p => p.X == X && p.Y < Y); // Up
                if (chair != null)
                    neighbours.Add(chair);
                chair = chairs.OrderBy(p => p.Y).FirstOrDefault(p => p.X == X && p.Y > Y); // Down
                if (chair != null)
                    neighbours.Add(chair);
                chair = GetCornerNeighbour(chairs, -1, -1);
                if (chair != null)
                    neighbours.Add(chair);
                chair = GetCornerNeighbour(chairs, -1, 1);
                if (chair != null)
                    neighbours.Add(chair);
                chair = GetCornerNeighbour(chairs, 1, -1);
                if (chair != null)
                    neighbours.Add(chair);
                chair = GetCornerNeighbour(chairs, 1, 1);
                if (chair != null)
                    neighbours.Add(chair);

                return neighbours;
            }

            private Place GetCornerNeighbour(List<Place> chairs, int xDir, int yDir )
            {
                var x = X + xDir;
                var y = Y + yDir;
                var xMax = chairs.Max(c => c.X);
                var yMax = chairs.Max(c => c.Y);
                while (x >= 0 && x <= xMax && y >= 0 && y <= yMax)
                {
                    var chair = chairs.FirstOrDefault(c => c.X == x && c.Y == y);
                    if (chair != null)
                        return chair;
                    x += xDir;
                    y += yDir;
                }
                return null;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
