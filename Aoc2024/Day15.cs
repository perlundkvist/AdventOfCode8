using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode8.Aoc2024;

internal class Day15 : DayBase
{

    internal void Run()
    {
        var input = GetInput("2024_15");

        #region Part 1
        var map = new List<Position<char>>();
        var line = 0;
        foreach (var l in input)
        {
            if (!l.StartsWith('#'))
                break;
            var col = 0;
            foreach (var c in l)
            {
                map.Add(new Position<char>(line, col, c));
                col++;
            }
            line++;
        }

        Position.Print(map);

        foreach (var l in input[line..])
        {
            foreach (char c in l)
            {
                //Console.WriteLine(c);
                Move(c, map);
                //Position.Print(map);
            }
            Console.WriteLine("One line done");
        }

        var sum = map.Where(p => p.Value == 'O').Sum(p => p.Line * 100 + p.Col);
        Console.WriteLine($"Sum: {sum}");

        #endregion

        line = 0;
        var map2 = new List<Position<char>>();
        foreach (var l in input)
        {
            if (!l.StartsWith('#'))
                break;
            var col = 0;
            foreach (var c in l)
            {
                switch (c)
                {
                    case '#':
                    case '.':
                        map2.Add(new Position<char>(line, col++, c));
                        map2.Add(new Position<char>(line, col++, c));
                        break;
                    case 'O':
                        map2.Add(new Position<char>(line, col++, '['));
                        map2.Add(new Position<char>(line, col++, ']'));
                        break;
                    case '@':
                        map2.Add(new Position<char>(line, col++, c));
                        map2.Add(new Position<char>(line, col++, '.'));
                        break;
                }
            }
            line++;
        }
        Position.Print(map2);

        foreach (var l in input[line..])
        {
            foreach (char c in l)
            {
                //Console.WriteLine(c);
                Move2(c, map2);
                //Position.Print(map2);
            }
            Console.WriteLine("One line done");
        }

        sum = map2.Where(p => p.Value == '[').Sum(p => p.Line * 100 + p.Col);
        Console.WriteLine($"Sum: {sum}");

    }

    private void Move(char movement, List<Position<char>> map)
    {
        var robot = map.First(p => p.Value == '@');
        List<Position<char>> spaces;
        Position<char>? space = null;
        List<Position<char>> walls;
        Position<char> wall;
        Position<char>? next = null;

        switch (movement)
        {
            case '<':
                spaces = map.Where(p => p.Value == '.' && p.Line == robot.Line && p.Col < robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Col).Last();
                walls = map.Where(p => p.Value == '#' && p.Line == robot.Line && p.Col < robot.Col).ToList();
                wall = walls.OrderBy(p => p.Col).Last();
                if (wall.Col > space.Col)
                    break;
                next = map.First(p => p.Line == robot.Line && p.Col == robot.Col - 1);
                break;

            case '>':
                spaces = map.Where(p => p.Value == '.' && p.Line == robot.Line && p.Col > robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Col).First();
                walls = map.Where(p => p.Value == '#' && p.Line == robot.Line && p.Col > robot.Col).ToList();
                wall = walls.OrderBy(p => p.Col).First();
                if (wall.Col < space.Col)
                    break;
                next = map.First(p => p.Line == robot.Line && p.Col == robot.Col + 1);
                break;

            case '^':
                spaces = map.Where(p => p.Value == '.' && p.Line < robot.Line && p.Col == robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Line).Last();
                walls = map.Where(p => p.Value == '#' && p.Line < robot.Line && p.Col == robot.Col).ToList();
                wall = walls.OrderBy(p => p.Line).Last();
                if (wall.Line > space.Line)
                    break;
                next = map.First(p => p.Line == robot.Line - 1 && p.Col == robot.Col);
                break;

            case 'v':
                spaces = map.Where(p => p.Value == '.' && p.Line > robot.Line && p.Col == robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Line).First();
                walls = map.Where(p => p.Value == '#' && p.Line > robot.Line && p.Col == robot.Col).ToList();
                wall = walls.OrderBy(p => p.Line).First();
                if (wall.Line < space.Line)
                    break;
                next = map.First(p => p.Line == robot.Line + 1 && p.Col == robot.Col);
                break;
        }

        if (next == null || space == null) return;
        var count = map.Count;
        map.Remove(next);
        map.Remove(robot);
        map.Remove(space);
        if (space != next)
        {
            map.Add(new Position<char>(space.Line, space.Col, 'O'));
        }
        map.Add(new Position<char>(next.Line, next.Col, '@'));
        map.Add(new Position<char>(robot.Line, robot.Col, '.'));
        if (map.Count != count)
            throw new Exception("Count changed");
        //Console.WriteLine($"Robot: {map.First(p => p.Value == '@')}");
    }

    private void Move2(char movement, List<Position<char>> map)
    {
        var robot = map.First(p => p.Value == '@');
        List<Position<char>> spaces;
        Position<char>? space = null;
        List<Position<char>> walls;
        Position<char> wall;
        Position<char>? next = null;

        switch (movement)
        {
            case '<':
                spaces = map.Where(p => p.Value == '.' && p.Line == robot.Line && p.Col < robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Col).Last();
                walls = map.Where(p => p.Value == '#' && p.Line == robot.Line && p.Col < robot.Col).ToList();
                wall = walls.OrderBy(p => p.Col).Last();
                if (wall.Col > space.Col)
                    break;
                next = map.First(p => p.Line == robot.Line && p.Col == robot.Col - 1);
                break;

            case '>':
                spaces = map.Where(p => p.Value == '.' && p.Line == robot.Line && p.Col > robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Col).First();
                walls = map.Where(p => p.Value == '#' && p.Line == robot.Line && p.Col > robot.Col).ToList();
                wall = walls.OrderBy(p => p.Col).First();
                if (wall.Col < space.Col)
                    break;
                next = map.First(p => p.Line == robot.Line && p.Col == robot.Col + 1);
                break;

            case '^':
                spaces = map.Where(p => p.Value == '.' && p.Line < robot.Line && p.Col == robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Line).Last();
                walls = map.Where(p => p.Value == '#' && p.Line < robot.Line && p.Col == robot.Col).ToList();
                wall = walls.OrderBy(p => p.Line).Last();
                if (wall.Line > space.Line)
                    break;
                next = map.First(p => p.Line == robot.Line - 1 && p.Col == robot.Col);
                break;

            case 'v':
                spaces = map.Where(p => p.Value == '.' && p.Line > robot.Line && p.Col == robot.Col).ToList();
                if (spaces.Count == 0)
                    break;
                space = spaces.OrderBy(p => p.Line).First();
                walls = map.Where(p => p.Value == '#' && p.Line > robot.Line && p.Col == robot.Col).ToList();
                wall = walls.OrderBy(p => p.Line).First();
                if (wall.Line < space.Line)
                    break;
                next = map.First(p => p.Line == robot.Line + 1 && p.Col == robot.Col);
                break;
        }

        if (next == null || space == null) return;
        if (next.Value == '.')
        {
            map.Remove(next);
            map.Remove(robot);
            map.Add(new Position<char>(next.Line, next.Col, '@'));
            map.Add(new Position<char>(robot.Line, robot.Col, '.'));
            return;
        }
        if (movement == '<')
        {
            map.Remove(next);
            map.Remove(robot);
            map.Remove(space);
            map.Add(new Position<char>(next.Line, next.Col, '@'));
            map.Add(new Position<char>(robot.Line, robot.Col, '.'));
            map.Add(new Position<char>(space.Line, space.Col, '['));

            var boxes = map.Where(p => p.Line == robot.Line && p.Col > space.Col && p.Col < next.Col).ToList();
            foreach (var box in boxes)
            {
                map.Remove(box);
                map.Add(new Position<char>(box.Line, box.Col, box.Value == '[' ? ']' : '['));
            }
            return;
        }
        if (movement == '>')
        {
            map.Remove(next);
            map.Remove(robot);
            map.Remove(space);
            map.Add(new Position<char>(next.Line, next.Col, '@'));
            map.Add(new Position<char>(robot.Line, robot.Col, '.'));
            map.Add(new Position<char>(space.Line, space.Col, ']'));
            //Position.Print(map);
            var boxes = map.Where(p => p.Line == robot.Line && p.Col > next.Col && p.Col < space.Col).ToList();
            foreach (var box in boxes)
            {
                map.Remove(box);
                map.Add(new Position<char>(box.Line, box.Col, box.Value == '[' ? ']' : '['));
            }
            return;
        }

        if (movement == '^' && CanMoveUp(next, map))
        {
            MoveUp(next, map);
            next = map.First(p => p.Line == next.Line && p.Col == next.Col);
            map.Remove(robot);
            map.Remove(next);
            map.Add(new Position<char>(next.Line, next.Col, '@'));
            map.Add(new Position<char>(robot.Line, robot.Col, '.'));
        }

        if (movement == 'v' && CanMoveDown(next, map))
        {
            MoveDown(next, map);
            next = map.First(p => p.Line == next.Line && p.Col == next.Col);
            map.Remove(robot);
            map.Remove(next);
            map.Add(new Position<char>(next.Line, next.Col, '@'));
            map.Add(new Position<char>(robot.Line, robot.Col, '.'));
        }
    }

    private bool CanMoveUp(Position<char> next, List<Position<char>> map)
    {
        var direction = next.Value == ']' ? -1 : 1;
        var next2 =  map.First(p => p.Line == next.Line && p.Col == next.Col + direction);

        if (next.Value == '#' || next2.Value == '#')
            return false;

        var above1 =  map.First(p => p.Line == next.Line - 1 && p.Col == next.Col);
        var above2 =  map.First(p => p.Line == next2.Line - 1 && p.Col == next2.Col);

        if (above1 == null || above2 == null)
            throw new ArgumentException("No above");

        if (above1.Value == '.' && above2.Value == '.')
            return true;

        var canMove = above1.Value == '.' || CanMoveUp(above1, map);
        if (!canMove)
            return false;

        canMove = above2.Value == '.' || CanMoveUp(above2, map);
        return canMove;
    }

    private void MoveUp(Position<char> next, List<Position<char>> map)
    {
        var left = next.Value == '[' ? next : map.First(p => p.Line == next.Line && p.Col == next.Col - 1);
        var right = next.Value == '[' ? map.First(p => p.Line == next.Line && p.Col == next.Col + 1) : next;
        var nextLeft = map.First(p => p.Line == left.Line - 1 && p.Col == left.Col);
        var nextRight = map.First(p => p.Line == right.Line - 1 && p.Col == right.Col);

        if (nextLeft.Value != '.')
            MoveUp(nextLeft, map);
        if (nextRight.Value == '[')
            MoveUp(nextRight, map);

        left = next.Value == '[' ? next : map.First(p => p.Line == next.Line && p.Col == next.Col - 1);
        right = next.Value == '[' ? map.First(p => p.Line == next.Line && p.Col == next.Col + 1) : next;
        nextLeft = map.First(p => p.Line == left.Line - 1 && p.Col == left.Col);
        nextRight = map.First(p => p.Line == right.Line - 1 && p.Col == right.Col);

        map.Remove(left);
        map.Remove(right);
        map.Remove(nextLeft);
        map.Remove(nextRight);
        map.Add(new Position<char>(left.Line, left.Col, '.'));
        map.Add(new Position<char>(right.Line, right.Col, '.'));
        map.Add(new Position<char>(nextLeft.Line, nextLeft.Col, '['));
        map.Add(new Position<char>(nextRight.Line, nextRight.Col, ']'));
    }

    private bool CanMoveDown(Position<char> next, List<Position<char>> map)
    {
        var direction = next.Value == ']' ? -1 : 1;
        var next2 = map.First(p => p.Line == next.Line && p.Col == next.Col + direction);

        if (next.Value == '#' || next2.Value == '#')
            return false;

        var below1 = map.First(p => p.Line == next.Line + 1 && p.Col == next.Col);
        var below2 = map.First(p => p.Line == next2.Line + 1 && p.Col == next2.Col);

        if (below1 == null || below2 == null)
            throw new ArgumentException("No below");

        if (below1.Value == '.' && below2.Value == '.')
            return true;

        var canMove = below1.Value == '.' || CanMoveDown(below1, map);
        if (!canMove)
            return false;

        canMove = below2.Value == '.' || CanMoveDown(below2, map);
        return canMove;
    }

    private void MoveDown(Position<char> next, List<Position<char>> map)
    {
        var left = next.Value == '[' ? next : map.First(p => p.Line == next.Line && p.Col == next.Col - 1);
        var right = next.Value == '[' ? map.First(p => p.Line == next.Line && p.Col == next.Col + 1) : next;
        var nextLeft = map.First(p => p.Line == left.Line + 1 && p.Col == left.Col);
        var nextRight = map.First(p => p.Line == right.Line + 1 && p.Col == right.Col);

        if (nextLeft.Value != '.')
            MoveDown(nextLeft, map);
        if (nextRight.Value == '[')
            MoveDown(nextRight, map);

        left = next.Value == '[' ? next : map.First(p => p.Line == next.Line && p.Col == next.Col - 1);
        right = next.Value == '[' ? map.First(p => p.Line == next.Line && p.Col == next.Col + 1) : next;
        nextLeft = map.First(p => p.Line == left.Line + 1 && p.Col == left.Col);
        nextRight = map.First(p => p.Line == right.Line + 1 && p.Col == right.Col);

        map.Remove(left);
        map.Remove(right);
        map.Remove(nextLeft);
        map.Remove(nextRight);
        map.Add(new Position<char>(left.Line, left.Col, '.'));
        map.Add(new Position<char>(right.Line, right.Col, '.'));
        map.Add(new Position<char>(nextLeft.Line, nextLeft.Col, '['));
        map.Add(new Position<char>(nextRight.Line, nextRight.Col, ']'));
    }


}