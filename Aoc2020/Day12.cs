namespace AdventOfCode8.Aoc2020
{
    class Day12 : DayBase
    {
        internal void Run()
        {
            var puzzleInput = GetInput("2020_12");

            var ship = new Position<Direction>(0, 0, Direction.Right);

            foreach (var line in puzzleInput)
            {
                var action = line[0];
                var value = int.Parse(line[1..]);
                switch (action)
                {
                    case 'N':
                        ship = Move(ship, Direction.Up, value);
                        break;
                    case 'S':
                        ship = Move(ship, Direction.Down, value);
                        break;
                    case 'E':
                        ship = Move(ship, Direction.Right, value);
                        break;
                    case 'W':
                        ship = Move(ship, Direction.Left, value);
                        break;
                    case 'L':
                        ship = Turn(ship, Direction.Left, value);
                        break;
                    case 'R':
                        ship = Turn(ship, Direction.Right, value);
                        break;
                    case 'F':
                        ship = Move(ship, ship.Value, value);
                        break;

                }
            }

            Console.WriteLine($"Distance = {ship.GetPosition().ManhattanDistance(new Position(0, 0))}");

            ship = new Position<Direction>(0, 0, Direction.Right);
            var waypoint = new Position<Direction>(-1, 10, Direction.Up);

            foreach (var line in puzzleInput)
            {
                var action = line[0];
                var value = int.Parse(line[1..]);
                switch (action)
                {
                    case 'N':
                        waypoint = Move(waypoint, Direction.Up, value);
                        break;
                    case 'S':
                        waypoint = Move(waypoint, Direction.Down, value);
                        break;
                    case 'E':
                        waypoint = Move(waypoint, Direction.Right, value);
                        break;
                    case 'W':
                        waypoint = Move(waypoint, Direction.Left, value);
                        break;
                    case 'L':
                        waypoint = TurnWaypoint(waypoint, Direction.Left, value);
                        break;
                    case 'R':
                        waypoint = TurnWaypoint(waypoint, Direction.Right, value);
                        break;
                    case 'F':
                        ship = Move(ship, waypoint, value);
                        break;

                }
            }

            Console.WriteLine($"Distance 2 = {ship.GetPosition().ManhattanDistance(new Position(0, 0))}");
        }

        private Position<Direction> Move(Position<Direction> ship, Position<Direction> velocity, int value)
        {
            for (var i = 0; i < value; i++)
            {
                ship = ship.Move(velocity);
            }
            return ship;
        }

        private Position<Direction> TurnWaypoint(Position<Direction> waypoint, Direction direction, int value)
        {
            for (var i = 0; i < value/90; i++)
            {
                var line = direction == Direction.Right ? waypoint.Col : -waypoint.Col;
                var col = direction == Direction.Right ? -waypoint.Line : waypoint.Line;
                waypoint = new Position<Direction>(line, col, waypoint.Value);
            }
            return waypoint;
        }

        private Position<Direction> Turn(Position<Direction> ship, Direction direction, int value)
        {
            if (direction == Direction.Left)
                value = -value;
            var newDirection = (Direction)((((int)ship.Value + value / 90) + 4) % 4);
            return new Position<Direction>(ship.Line, ship.Col, newDirection);
        }

        private Position<Direction> Move(Position<Direction> ship, Direction direction, int value)
        {
            var velocity = direction switch
            {
                Direction.Up => new Position(-value, 0),
                Direction.Down => new Position(value, 0),
                Direction.Left => new Position(0, -value),
                Direction.Right => new Position(0, value),
                _ => throw new Exception("Invalid direction")
            };

            return ship.Move(velocity);
        }
    }
}
