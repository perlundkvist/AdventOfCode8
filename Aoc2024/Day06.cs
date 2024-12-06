namespace AdventOfCode8.Aoc2024;

internal class Day06 : DayBase
{
    internal void Run()
    {
        var input = GetInput("2024_06");

        var obstacles = new List<Position>();

        var x = 0;
        var visited = new List<Position>();
        foreach (var line in input)
        {
            var ys = line.AllIndexesOf("#");
            obstacles.AddRange(ys.Select(y => new Position(x, y)));
            var g = line.IndexOf('^');
            if (g > -1)
                visited.Add(new Position(x, g));
            x++;
        }

        var current = visited.First();
        Move(obstacles, visited, Direction.Up, current, input.Count, input[0].Length);

        Console.WriteLine($"Visited: {visited.Count}");

        //obstacles.Add(new Position(6, 3));
        //Console.WriteLine($"{Move2(obstacles, Direction.Up, current, input.Count, input[0].Length)}");
        //Console.WriteLine($"{Move3(obstacles, Direction.Up, current, input.Count, input[0].Length)}");
        //obstacles.Remove(new Position(6, 3));

        var newObstructions = 0;
        var count = 0;
        foreach (var obstacle in visited)
        {
            Console.WriteLine($"{count++}");
            obstacles.Add(obstacle); 
            //if (!Move2(obstacles, Direction.Up, current, input.Count, input[0].Length))
            //    newObstructions++;
            if (!Move3(obstacles, Direction.Up, current, input.Count, input[0].Length))
                newObstructions++;
            obstacles.Remove(obstacle);
        }

        Console.WriteLine($"New possible: {newObstructions}");

    }

    private void Move(List<Position> obstacles, List<Position> visited, Direction direction, Position current, int lines, int cols)
    {
        while (true)
        {
            if (!visited.Contains(current))
                visited.Add(current);
            var nextCol = current.Col;
            var nextLine = current.Line;
            switch (direction)
            {
                case Direction.Up:
                    nextLine--;
                    if (nextLine < 0) 
                        return;
                    if (!obstacles.Contains(new Position(nextLine, nextCol))) 
                        break;
                    nextLine++;
                    direction = Direction.Right;
                    break;
                case Direction.Down:
                    nextLine++;
                    if (nextLine >= lines)
                        return;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextLine--;
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    nextCol--;
                    if (nextCol < 0)
                        return;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextCol++;
                    direction = Direction.Up;
                    break;
                case Direction.Right:
                    nextCol++;
                    if (nextCol >= cols)
                        return;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextCol--;
                    direction = Direction.Down;
                    break;
            }

            current = new Position(nextLine, nextCol);
        }
    }

    private bool Move2(List<Position> obstacles, Direction direction, Position current, int lines, int cols)
    {
        List<Position<Direction>> visitedDirections = new();
        while (true)
        {
            if (visitedDirections.Contains(new Position<Direction>(current.Line, current.Col, direction)))
                return false;
            visitedDirections.Add(new Position<Direction>(current.Line, current.Col, direction));
            var nextCol = current.Col;
            var nextLine = current.Line;
            switch (direction)
            {
                case Direction.Up:
                    nextLine--;
                    if (nextLine < 0)
                        return true;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextLine++;
                    direction = Direction.Right;
                    break;
                case Direction.Down:
                    nextLine++;
                    if (nextLine >= lines)
                        return true;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextLine--;
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    nextCol--;
                    if (nextCol < 0)
                        return true;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextCol++;
                    direction = Direction.Up;
                    break;
                case Direction.Right:
                    nextCol++;
                    if (nextCol >= cols)
                        return true;
                    if (!obstacles.Contains(new Position(nextLine, nextCol)))
                        break;
                    nextCol--;
                    direction = Direction.Down;
                    break;
            }

            current = new Position(nextLine, nextCol);
        }
    }

    private bool Move3(List<Position> obstacles, Direction direction, Position current, int lines, int cols)
    {
        List<Position<Direction>> visitedDirections = [];
        while (true)
        {
            if (visitedDirections.Contains(new Position<Direction>(current.Line, current.Col, direction)))
                return false;
            visitedDirections.Add(new Position<Direction>(current.Line, current.Col, direction));
            var nextCol = current.Col;
            var nextLine = current.Line;
            switch (direction)
            {
                case Direction.Up:
                    var above = obstacles.OrderBy(o => o.Line).LastOrDefault(o => o.Line < nextLine && o.Col == nextCol);
                    if (above == null)
                        return true;
                    for (; nextLine > above.Line; nextLine--)
                    {
                        visitedDirections.Add(new Position<Direction>(nextLine, nextCol, direction));
                    }
                    nextLine++;
                    direction = Direction.Right;
                    break;
                case Direction.Down:
                    var below = obstacles.OrderBy(o => o.Line).FirstOrDefault(o => o.Line > nextLine && o.Col == nextCol);
                    if (below == null)
                        return true;
                    for (; nextLine < below.Line; nextLine++)
                    {
                        visitedDirections.Add(new Position<Direction>(nextLine, nextCol, direction));
                    }
                    nextLine--;
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    var left = obstacles.OrderBy(o => o.Col).LastOrDefault(o => o.Line == nextLine && o.Col < nextCol);
                    if (left == null)
                        return true;
                    for (; nextCol > left.Col; nextCol--)
                    {
                        visitedDirections.Add(new Position<Direction>(nextLine, nextCol, direction));
                    }
                    nextCol++;
                    direction = Direction.Up;
                    break;
                case Direction.Right:
                    var right = obstacles.OrderBy(o => o.Col).FirstOrDefault(o => o.Line == nextLine && o.Col > nextCol);
                    if (right == null)
                        return true;
                    for (; nextCol < right.Col; nextCol++)
                    {
                        visitedDirections.Add(new Position<Direction>(nextLine, nextCol, direction));
                    }
                    nextCol--;
                    direction = Direction.Down;
                    break;
            }

            current = new Position(nextLine, nextCol);
        }
    }

}