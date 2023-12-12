using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day10 : DayBase
    {

        private enum Direction { Left, Right, Up, Down }

        internal void Run()
        {
            var start = DateTime.Now;

            var input = GetInput("2023_10").ToImmutableList();
            var pipes = GetPipes(input);

            //Print(pipes, input.Count, input[0].Count());
            //return;
            //Console.WriteLine();
            //Print(pipes);

            var distance = GetDistance(pipes);
            Console.WriteLine($"Distance: {distance}");

            var enclosed = GetEnclosed(pipes, input);
            Console.WriteLine($"Enclosed: {enclosed}");

            Console.WriteLine($"{DateTime.Now - start}");

        }

        private int GetEnclosed(List<Pipe> pipes, ImmutableList<string> map)
        {
            SetOutsides(pipes);

            var free = new List<Pipe>();
            var allPipes = new List<Pipe>(pipes);

            for (int l = 0; l < map.Count; l++)
            {
                for (int c = 0; c < map[l].Length; c++)
                {
                    var pipe = new Pipe(c, l, '.');
                    if (allPipes.Contains(pipe))
                        continue;
                    if (c == 0 || c == map[l].Length -1 || l == 0 || l == map.Count - 1)
                    {
                        SetPipeAsFree(pipe);
                        free.Add(pipe);
                        allPipes.Add(pipe);
                        continue;
                    }
                    var (Above, Below, Left, Right) = GetSurrounding(pipe, free);
                    var neighbour = Above ?? Below ?? Left ?? Right;
                    if (neighbour != null)
                    {
                        SetPipeAsFree(pipe);
                        free.Add(pipe);
                        allPipes.Add(pipe);
                        continue;
                    }
                }
            }
            Print(allPipes);
            while (true)
            {
                var toFix = allPipes.Where(p => p.Shape == '.').ToList();
                Console.WriteLine($"Left to fix: {toFix.Count}");
                if (!toFix.Any())
                    break;
                foreach (var pipe in toFix)
                {
                    var (Above, Below, Left, Right) = GetSurrounding(pipe, allPipes);
                    if (Above?.Shape is 'O' || Below?.Shape is 'O' || Left?.Shape is 'O' || Right?.Shape is 'O')
                    {
                        SetPipeAsFree(pipe);
                        continue;
                    }
                    if (Above?.Shape is 'I' || Below?.Shape is 'I' || Left?.Shape is 'I' || Right?.Shape is 'I')
                    {
                        pipe.Shape = 'I';
                        continue;
                    }
                    Print(allPipes, current: pipe);
                    bool onEdge = false;

                    switch (pipe.Shape)
                    {
                        case '-':
                            onEdge = !pipes.Any(p => p.Line == pipe.Line && p.Col < pipe.Col); // None above 
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Up);
                                break;
                            }
                            onEdge = !pipes.Any(p => p.Line == pipe.Line && p.Col > pipe.Col); // None below 
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Down);
                                break;
                            }
                            if (Left != null && Left.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            else if (Left != null && Left.Outsides.Contains(Direction.Down))
                                pipe.Outsides.Add(Direction.Down);
                            else if (Right != null && Right.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            else if (Right != null && Right.Outsides.Contains(Direction.Down))
                                pipe.Outsides.Add(Direction.Down);
                            break;
                        case '|':
                            onEdge = !pipes.Any(p => p.Line < pipe.Line && p.Col == pipe.Col); // None left
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Left);
                                break;
                            }
                            onEdge = !pipes.Any(p => p.Line > pipe.Line && p.Col == pipe.Col); // None right
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Right);
                                break;
                            }
                            if (Above != null && Above.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            else if (Below != null && Below.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            break;
                        case 'F':
                            onEdge = !pipes.Any(p => p.Line == pipe.Line && p.Col < pipe.Col); // None above 
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Up);
                                break;
                            }
                            onEdge = !pipes.Any(p => p.Line < pipe.Line && p.Col == pipe.Col); // None left
                            if (onEdge)
                            {
                                pipe.Outsides.Add(Direction.Left);
                                break;
                            }
                            if (Above != null && Above.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            else if (Below != null && Below.Outsides.Contains(Direction.Up))
                                pipe.Outsides.Add(Direction.Up);
                            break;
                        default:
                            throw new NotImplementedException(pipe.Shape.ToString());
                    }
                }
            }
            return allPipes.Count(p => p.Shape == 'I');
        }

        private void SetOutsides(List<Pipe> pipes)
        {
            var start = pipes.OrderBy(p => p.Line).ThenBy(p => p.Col).First(p => p.Shape == 'F');
            start.Outsides.Add(Direction.Up);
            start.Outsides.Add(Direction.Left);
            var pipe = start;
            var previous = start;
            var idx = 0;
            while (true)
            {
                var (n1, n2) = pipe.GetNeighbours(pipes);
                var next = n1 == previous ? n2 : n1;
                if (next == start)
                    break;

                previous = pipe;
                pipe = next;
                idx++;
            }
            Console.WriteLine($"Start: {start.Col},{start.Line}");
        }

        private void SetPipeAsFree(Pipe pipe)
        {
            pipe.Shape = 'O';
            pipe.Outsides.Add(Direction.Left);
            pipe.Outsides.Add(Direction.Right);
            pipe.Outsides.Add(Direction.Up);
            pipe.Outsides.Add(Direction.Down);
        }

        private void EnclosedOrFree(Pipe pipe, List<Pipe> free, List<Pipe> enclosed, List<Pipe> pipes, List<Pipe> tested)
        {
            var testedHere = tested.ToList();
            Print(pipes, current: pipe);
            var (Above, Below, Left, Right) = GetSurrounding(pipe, pipes);
            var canEscape = false;
            if (Above == null)
            {
                var next = new Pipe(pipe.Col, pipe.Line - 1, '.');
                if (!testedHere.Contains(next))
                {
                    testedHere.Add(next);
                    EnclosedOrFree(next, free, enclosed, pipes, testedHere);
                    testedHere.Remove(next);
                }
            }
            else
            {
                canEscape = CanEscape(Above, pipe, free, enclosed, pipes, Direction.Up);
            }
            if (!canEscape && Below == null)
            {
                var next = new Pipe(pipe.Col, pipe.Line + 1, '.');
                if (!testedHere.Contains(next))
                {
                    testedHere.Add(pipe);
                    EnclosedOrFree(next, free, enclosed, pipes, testedHere);
                    testedHere.Remove(next);
                }
            }
            else if (!canEscape && Below != null)
            {
                canEscape = CanEscape(Below, pipe, free, enclosed, pipes, Direction.Down);
            }
            if (!canEscape && Left == null)
            {
                var next = new Pipe(pipe.Col - 1, pipe.Line, '.');
                if (!testedHere.Contains(next))
                {
                    testedHere.Add(pipe);
                    EnclosedOrFree(next, free, enclosed, pipes, testedHere);
                    testedHere.Remove(next);
                }
            }
            else if (!canEscape && Left != null)
            {
                canEscape = CanEscape(Left, pipe, free, enclosed, pipes, Direction.Left);
            }
            if (Right == null)
            {
                var next = new Pipe(pipe.Col + 1, pipe.Line, '.');
                if (!testedHere.Contains(next))
                {
                    testedHere.Add(pipe);
                    EnclosedOrFree(next, free, enclosed, pipes, testedHere);
                    testedHere.Remove(next);
                }
            }
            else if (!canEscape && Right != null)
            {
                canEscape = CanEscape(Right, pipe, free, enclosed, pipes, Direction.Right);
            }
            pipe.Shape = canEscape ? 'O' : 'I';
            if (pipe.Shape == 'I' && !enclosed.Contains(pipe))
                enclosed.Add(pipe);
            else if (pipe.Shape == 'O' && !free.Contains(pipe))
                free.Add(pipe);
        }

        private bool CanEscape(Pipe neighbour, Pipe pipe, List<Pipe> free, List<Pipe> enclosed, List<Pipe> pipes, Direction direction)
        {
            double c;
            double l;
            switch (direction)
            {
                case Direction.Left:
                    if (neighbour.Shape is '|')
                        return false;
                    if (neighbour.Shape is 'L' or 'F')
                        throw new NotImplementedException($"{direction}");
                    if (neighbour.Shape is 'F')
                    {
                        c = pipe.Col - 0.5;
                        l = Convert.ToDouble(pipe.Line);
                        return CanEscape(c, l, free, pipes, direction);
                    }
                    break;
                case Direction.Right:
                    if (neighbour.Shape is '|')
                        return false;
                    if (neighbour.Shape is 'J' or '7')
                        throw new NotImplementedException($"{direction}");
                    break;
                case Direction.Down:
                    if (neighbour.Shape is '-')
                        return false;
                    if (neighbour.Shape is 'J' or 'L')
                        throw new NotImplementedException($"{direction}");
                    c = neighbour.Shape is 'F' ? pipe.Col - 0.5 : pipe.Col + 0.5; 
                    l = Convert.ToDouble(pipe.Line + 1);
                    return CanEscape(c, l, free, pipes, direction);
                case Direction.Up:
                    if (neighbour.Shape is '-')
                        return false;
                    if (neighbour.Shape is '7' or 'F')
                        throw new NotImplementedException($"{direction}");
                    if (neighbour.Shape is 'L')
                    {
                        c = pipe.Col - 0.5;
                        l = Convert.ToDouble(pipe.Line);
                        return CanEscape(c, l, free, pipes, direction);
                    }
                    if (neighbour.Shape is 'J')
                    {
                        c = pipe.Col + 0.5;
                        l = Convert.ToDouble(pipe.Line);
                        return CanEscape(c, l, free, pipes, direction);
                    }
                    break;
            }
            return false;
        }

        private bool CanEscape(double col, double line, List<Pipe> free, List<Pipe> pipes, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    //if (neighbour.Shape is '|')
                    //    return false;
                    //if (neighbour.Shape is 'L' or 'F')
                    //    throw new NotImplementedException($"{direction}");
                    //if (neighbour.Shape is 'F')
                    //{
                    //    var c = Convert.ToDouble(pipe.Col);
                    //    var l = Convert.ToDouble(pipe.Line) - 0.5;
                    //    return CanEscape(new Point(c, l), free, pipes, direction);
                    //}
                    break;
                case Direction.Right:
                    //if (neighbour.Shape is '|')
                    //    return false;
                    //if (neighbour.Shape is 'J' or '7')
                    //    throw new NotImplementedException($"{direction}");
                    break;
                case Direction.Down:
                    var below1 = pipes.FirstOrDefault(p => p.Line == line + 1 && p.Col == Math.Floor(col));
                    var below2 = pipes.FirstOrDefault(p => p.Line == line + 1 && p.Col == Math.Ceiling(col));
                    
                    var left = pipes.FirstOrDefault(p => p.Line == line && p.Col == Math.Floor(col));
                    var right = pipes.FirstOrDefault(p => p.Line == line && p.Col == Math.Ceiling(col));
                    break;
                case Direction.Up:
                    var above1 = pipes.FirstOrDefault(p => p.Line == line - 1 && p.Col == Math.Floor(col));
                    var above2 = pipes.FirstOrDefault(p => p.Line == line - 1 && p.Col == Math.Ceiling(col));

                    //if (neighbour.Shape is '-')
                    //    return false;
                    //if (neighbour.Shape is '7' or 'F')
                    //    throw new NotImplementedException($"{direction}");
                    break;
            }
            return false;
        }

        private static (Pipe? Above,  Pipe? Below, Pipe? Left, Pipe? Right) GetSurrounding(Pipe pipe, List<Pipe> pipes)
        {
            var above = pipes.FirstOrDefault(p => p.Line == pipe.Line - 1 && p.Col == pipe.Col);
            var below = pipes.FirstOrDefault(p => p.Line == pipe.Line + 1 && p.Col == pipe.Col);
            var left = pipes.FirstOrDefault(p => p.Line == pipe.Line && p.Col == pipe.Col - 1);
            var right = pipes.FirstOrDefault(p => p.Line == pipe.Line && p.Col == pipe.Col + 1);

            return (above, below, left, right);
        }

        private long GetDistance(List<Pipe> pipes)
        {
            return pipes.Count / 2;
        }

        private List<Pipe> GetPipes(ImmutableList<string> map)
        {
            var pipes = new List<Pipe>();

            var startLine = map.First(l => l.Contains("S"));
            var line = map.IndexOf(startLine);
            var col = startLine.IndexOf("S");

            var pipe = new Pipe(col, line, 'S');
            pipes.Add(pipe);
            var previous = pipe;
            while (true)
            {
                var next = pipe.GetNext(previous, map);
                if (next.Shape == 'S')
                    break;
                previous = pipe;
                pipe = next;
                pipes.Add(pipe);
            }
            var sPipe = pipes.First(p => p.Shape == 'S');
            var (Above, Below, Left, Right) = GetSurrounding(sPipe, pipes);
            FixS(sPipe, Above, Below, Left, Right);
            return pipes;
        }

        private void FixS(Pipe pipe, Pipe? above, Pipe? below, Pipe? left, Pipe? right)
        {
            if (above?.Shape is '|' or '7' or 'F' && below?.Shape is '|' or 'L' or 'J')
                pipe.Shape = '|';
            else if (left?.Shape is '-' or 'L' or 'F' && right?.Shape is '-' or '7' or 'J')
                pipe.Shape = '-';
            else if (above?.Shape is '|' or '7' or 'F' && right?.Shape is '-' or '7' or 'J')
                pipe.Shape = 'L';
            else if (above?.Shape is '|' or '7' or 'F' && left?.Shape is '-' or 'L' or 'F')
                pipe.Shape = 'J';
            else if (below?.Shape is '|' or 'L' or 'J' && left?.Shape is '-' or 'L' or 'F')
                pipe.Shape = '7';
            else if (below?.Shape is '|' or 'L' or 'J' && right?.Shape is '-' or '7' or 'J')
                pipe.Shape = 'F';

            Console.WriteLine($"S -> {pipe.Shape}");
        }

        private void Print(List<Pipe> pipes, int lines = 0, int cols = 0, Pipe? current = null, bool pretty = false)
        {
            if (lines == 0)
                lines = pipes.Max(p => p.Line) + 1;
            if (cols == 0)
                cols = pipes.Max(p => p.Col) + 1;
            for (int l = 0; l < lines; l++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var empty = '.';
                    if (!pipes.Any(p => p.Line == l && p.Col < c) ||
                        !pipes.Any(p => p.Line == l && p.Col > c) ||
                        !pipes.Any(p => p.Line < l && p.Col == c) ||
                        !pipes.Any(p => p.Line > l && p.Col == c))
                        empty = ' ';
                    var symbol = current != null && current.Col == c && current.Line == l ? 'X' :
                        pipes.FirstOrDefault(p => p.Col == c && p.Line == l)?.Shape ?? empty;
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        private string GetPretty(char symbol, bool pretty)
        {
            if (!pretty)
                return symbol.ToString();
            return symbol switch
            {
                'F' => "┏",
                'J' => "┛",
                'L' => "┗",
                '7' => "┓",
                '|' => "┃",
                '_' => "━",
                _ => symbol.ToString(),
            };
        }

        private class Pipe(int col, int line, char shape) : IEquatable<Pipe>
        {
            public int Col { get; } = col;
            public int Line { get; } = line;
            public char Shape { get; set; } = shape;
            public List<Direction> Outsides { get; } = [];

            public (Pipe n1, Pipe n2) GetNeighbours(List<Pipe> pipes)
            {
                var (above, below, left, right) = GetSurrounding(this, pipes);
                var (n1, n2) = Shape switch
                {
                    '|' => (above, below),
                    '-' => (left, right),
                    'F' => (below, right),
                    'J' => (above, left),
                    'L' => (above, right),
                    '7' => (below, left),
                    _ => throw new NotImplementedException(Shape.ToString())
                };
                ArgumentNullException.ThrowIfNull(n1);
                ArgumentNullException.ThrowIfNull(n2);
                return (n1, n2);
            }

            public Pipe GetNext(Pipe previous, ImmutableList<string> map)
            {
                var col = Col;
                var line = Line;
                var sameLine = previous.Line == line;
                var sameCol = previous.Col == col;
                switch (Shape)
                {
                    case '|':
                        if (sameLine || !sameCol)
                            throw new ArgumentException(Shape.ToString());
                        line = previous.Line == Line - 1 ? Line + 1 : Line - 1;
                        break;
                    case '-':
                        if (!sameLine || sameCol)
                            throw new ArgumentException(Shape.ToString());
                        col = previous.Col == Col - 1 ? Col + 1 : Col - 1;
                        break;
                    case 'L':
                        if (previous.Line != line && previous.Line != line - 1)
                            throw new ArgumentException(Shape.ToString());
                        if (previous.Col != col && previous.Col != col + 1)
                            throw new ArgumentException(Shape.ToString());
                        line = sameLine ? Line - 1 : Line;
                        col = sameLine ? Col : Col + 1;
                        break;
                    case 'J':
                        if (previous.Line != line && previous.Line != line - 1)
                            throw new ArgumentException(Shape.ToString());
                        if (previous.Col != col && previous.Col != col - 1)
                            throw new ArgumentException(Shape.ToString());
                        line = sameLine ? Line - 1 : Line;
                        col = sameLine ? Col : Col - 1;
                        break;
                    case '7':
                        if (previous.Line != line && previous.Line != line + 1)
                            throw new ArgumentException(Shape.ToString());
                        if (previous.Col != col && previous.Col != col - 1)
                            throw new ArgumentException(Shape.ToString());
                        line = sameLine ? Line + 1 : Line;
                        col = sameLine ? Col : Col - 1;
                        break;
                    case 'F':
                        if (previous.Line != line && previous.Line != line + 1)
                            throw new ArgumentException(Shape.ToString());
                        if (previous.Col != col && previous.Col != col + 1)
                            throw new ArgumentException(Shape.ToString());
                        line = sameLine ? Line + 1 : Line;
                        col = sameLine ? Col : Col + 1;
                        break;
                    case '.':
                        throw new NotImplementedException();
                    case 'S':
                        var above = Line == 0 ? '.' : map[Line - 1][Col];
                        var below = map[Line + 1][Col]; // I know it isn't at the bottom edge
                        var left = Col == 0 ? '.' : map[Line][Col - 1];
                        var right = map[Line][Col + 1]; // I know it isn't at the right edge
                        if (above is '|' or '7' or 'F')
                            line = Line - 1;
                        else if (below is '|' or 'L' or 'J')
                            line = Line + 1;
                        else if (right is '-' or '7' or 'J')
                            col = Col + 1;
                        else if (left is '-' or 'L' or 'F')
                            col = Col - 1;
                        break;
                }
                var pipe = new Pipe(col, line, map[line][col]);
                switch (pipe.Shape)
                {
                    case 'F':
                        break;
                    case '7':
                        pipe.Outsides.Add(Direction.Up);
                        pipe.Outsides.Add(Direction.Right);
                        break;
                    case 'L':
                        pipe.Outsides.Add(Direction.Down);
                        pipe.Outsides.Add(Direction.Left);
                        break;
                    case 'J':
                        pipe.Outsides.Add(Direction.Down);
                        pipe.Outsides.Add(Direction.Right);
                        break;
                }
                return pipe;
            }
            public bool Equals(Pipe? other)
            {
                if (other == this) return true;
                if (other == null) return false;
                return Line == other.Line && Col == other.Col;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

    }
}
