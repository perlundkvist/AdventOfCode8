﻿using System;
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

            Print(pipes, input.Count, input[0].Count());
            return;
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
            var free = new List<Pipe>();
            var enclosed = new List<Pipe>();
            var pipe = pipes.First(p => p.Shape == 'S');
            var (Above, Below, Left, Right) = GetSurrounding(pipe, pipes);

            if (Above?.Shape is '|' or '7'  or 'F' && Below?.Shape is '|' or 'L' or 'J') 
                pipe.Shape = '|';
            else if (Left?.Shape is '-' or 'L' or 'F' && Right?.Shape is '-' or '7' or 'J')
                pipe.Shape = '-';
            else if (Above?.Shape is '|' or '7' or 'F' && Right?.Shape is '-' or '7' or 'J')
                pipe.Shape = 'L';
            else if (Above?.Shape is '|' or '7' or 'F' && Left?.Shape is '-' or 'L' or 'F')
                pipe.Shape = 'J';
            else if (Below?.Shape is '|' or 'L' or 'J' && Left?.Shape is '-' or 'L' or 'F')
                pipe.Shape = '7';
            else if (Below?.Shape is '|' or 'L' or 'J' && Right?.Shape is '-' or '7' or 'J')
                pipe.Shape = 'F';

            Console.WriteLine($"S -> {pipe.Shape}");

            for (int l = 0; l < map.Count; l++)
            {
                for (int c = 0; c < map[l].Length; c++)
                {
                    pipe = new Pipe(c, l, 'O');
                    if (pipes.Contains(pipe))
                        continue;
                    (Above, Below, Left, Right) = GetSurrounding(pipe, free);
                    var neighbour = Above ?? Below ?? Left ?? Right;
                    if (neighbour != null)
                    {
                        free.Add(pipe);
                        continue;
                    }
                    (Above, Below, Left, Right) = GetSurrounding(pipe, enclosed);
                    neighbour = Above ?? Below ?? Left ?? Right;
                    if (neighbour != null)
                    {
                        enclosed.Add(pipe);
                        continue;
                    }
                    (Above, Below, Left, Right) = GetSurrounding(pipe, pipes);
                    neighbour = Above ?? Below ?? Left ?? Right;
                    if (neighbour == null)
                    {
                        free.Add(pipe);
                        continue;
                    }
                    var allTiles = free.ToList();
                    allTiles.AddRange(enclosed);
                    allTiles.AddRange(pipes);

                    //Print(allTiles, l + 1);
                    EnclosedOrFree(pipe, free, enclosed, pipes, []);
                }
            }

            return enclosed.Count;
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

        private (Pipe? Above,  Pipe? Below, Pipe? Left, Pipe? Right) GetSurrounding(Pipe pipe, List<Pipe> pipes)
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
            return pipes;
        }

        private void Print(List<Pipe> pipes, int lines = 0, int cols = 0, Pipe? current = null)
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
                    Console.Write(current != null && current.Col == c && current.Line == l ? 'X' :
                        pipes.FirstOrDefault(p => p.Col == c && p.Line == l)?.Shape ?? empty);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        private class Pipe(int col, int line, char shape) : IEquatable<Pipe>
        {
            public int Col { get; } = col;
            public int Line { get; } = line;
            public char Shape { get; set; } = shape;

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