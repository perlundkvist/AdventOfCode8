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
            Console.WriteLine($"Enclosed: {enclosed}. 246 too low"); 

            Console.WriteLine($"{DateTime.Now - start}");

        }

        private int GetEnclosed(List<Pipe> pipes, ImmutableList<string> map)
        {
            SetOutsides(pipes);

            PrintToFile2(pipes, "map4");

            var free = new List<Pipe>();
            var allPipes = new List<Pipe>(pipes);

            for (int l = 0; l < map.Count; l++)
            {
                for (int c = 0; c < map[l].Length; c++)
                {
                    var pipe = new Pipe(c, l, '.');
                    if (allPipes.Contains(pipe))
                        continue;
                    var onEdge = !pipes.Any(p => p.Line == pipe.Line && p.Col < pipe.Col);  // None left 
                    onEdge |= !pipes.Any(p => p.Line == pipe.Line && p.Col > pipe.Col);  // None right
                    onEdge |= !pipes.Any(p => p.Line < pipe.Line && p.Col == pipe.Col); // None above
                    onEdge |= !pipes.Any(p => p.Line > pipe.Line + 1 && p.Col == pipe.Col); // None below
                    if (onEdge)
                    {
                        SetPipeAsFree(pipe);
                        free.Add(pipe);
                    }
                    allPipes.Add(pipe);
                }
            }
            //Print(allPipes);
            //PrintToFile(allPipes, "map2");
            allPipes.Where(p => p.Shape == '.').ToList().ForEach(p => p.SetSurrounding(allPipes));
            while (true)
            {
                var toFix = allPipes.Where(p => p.Shape == '.').ToList();
                Console.WriteLine($"Left to fix: {toFix.Count}");
                if (!toFix.Any())
                    break;
                foreach (var pipe in toFix)
                {
                    var (Above, Below, Left, Right) = pipe.GetSurrounding();
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
                    if (Above != null && IsPipe(Above))
                    {
                        if (CheckAbove(pipe, Above))
                            continue;
                    }
                    if (Below != null && IsPipe(Below))
                    {
                        if (CheckBelow(pipe, Below))
                            continue;
                    }
                    if (Left != null && IsPipe(Left))
                    {
                        if (CheckLeft(pipe, Left))
                            continue;
                        continue;
                    }
                    if (Right != null && IsPipe(Right))
                    {
                        if (CheckRight(pipe, Right))
                            continue;
                        continue;
                    }
                    //Print(allPipes, current: pipe);
                }
            }
            PrintToFile(allPipes, "map3");
            return allPipes.Count(p => p.Shape == 'I');
        }

        private bool CheckAbove(Pipe pipe, Pipe above)
        {
            switch (above.Shape) 
            {
                case '|':
                    throw new NotImplementedException();
                case '-':
                    if (above.Outsides.Any(o => o == Direction.Down))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                case 'J':
                case 'F':
                    if (above.Outsides.Any(o => o == Direction.Right))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                case 'L':
                case '7':
                    if (above.Outsides.Any(o => o == Direction.Left))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool CheckBelow(Pipe pipe, Pipe below)
        {
            switch (below.Shape)
            {
                case '|':
                    throw new NotImplementedException();
                case '-':
                    if (below.Outsides.Any(o => o == Direction.Up))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                case 'L':
                case '7':
                    if (below.Outsides.Any(o => o == Direction.Right))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                case 'J':
                case 'F':
                    if (below.Outsides.Any(o => o == Direction.Left))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        private bool CheckLeft(Pipe pipe, Pipe left)
        {
            switch (left.Shape)
            {
                case '-':
                    throw new NotImplementedException();
                case '|':
                case 'J':
                case '7':
                    if (left.Outsides.Any(o => o == Direction.Right))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool CheckRight(Pipe pipe, Pipe right)
        {
            switch (right.Shape)
            {
                case '-':
                    throw new NotImplementedException();
                case '|':
                case 'L':
                case 'F':
                    if (right.Outsides.Any(o => o == Direction.Left))
                        SetPipeAsFree(pipe);
                    else
                        pipe.Shape = 'I';
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        private void SetOutsides(List<Pipe> pipes)
        {
            var start = pipes.OrderBy(p => p.Line).ThenBy(p => p.Col).First(p => p.Shape == 'F');
            start.Outsides.Add(Direction.Up);
            start.Outsides.Add(Direction.Left);
            var pipe = start;
            var previous = pipe;
            while (true)
            {
                var (n1, n2) = pipe.GetNeighbours();
                var next = n1 == previous ? n2 : n1;
                previous = pipe;
                pipe = next;
                if (pipe == start)
                    break;

                var (above, below, left, right) = pipe.GetSurrounding();
                switch (pipe.Shape)
                {
                    case '|':
                        pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Left : Direction.Right);
                        break;
                    case '-':
                        pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Up) ? Direction.Up : Direction.Down);
                        break;
                    case 'L':
                        if (previous == above)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Down : Direction.Up);
                        }
                        else if (previous == right)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Down) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Down) ? Direction.Down : Direction.Up);
                        }
                        else 
                            throw new NotImplementedException();
                        break;
                    case 'J':
                        if (previous == above)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Up : Direction.Down);
                        }
                        else if (previous == left)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Up) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Up) ? Direction.Up : Direction.Down);
                        }
                        else
                            throw new NotImplementedException();
                        break;
                    case '7':
                        if (previous == below)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Down : Direction.Up);
                        }
                        else if (previous == left)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Down) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Down) ? Direction.Down : Direction.Up);
                        }
                        else
                            throw new NotImplementedException();
                        break;
                    case 'F':
                        if (previous == below)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Left) ? Direction.Up : Direction.Down);
                        }
                        else if (previous == right)
                        {
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Up) ? Direction.Left : Direction.Right);
                            pipe.Outsides.Add(previous.Outsides.Any(o => o == Direction.Up) ? Direction.Up : Direction.Down);
                        }
                        else
                            throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void SetPipeAsFree(Pipe pipe)
        {
            pipe.Shape = 'O';
            pipe.Outsides.Add(Direction.Left);
            pipe.Outsides.Add(Direction.Right);
            pipe.Outsides.Add(Direction.Up);
            pipe.Outsides.Add(Direction.Down);
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
            pipes.ForEach(p => p.SetSurrounding(pipes));
            var sPipe = pipes.First(p => p.Shape == 'S');
            var (Above, Below, Left, Right) = sPipe.GetSurrounding();
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

        private void Print(List<Pipe> pipes, int lines = 0, int cols = 0, Pipe? current = null, bool pretty = true)
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
                    if (pretty && 
                        !pipes.Any(p => p.Line == l && p.Col < c) ||
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

        private void PrintToFile(List<Pipe> pipes, string name)
        {
            var writer = File.CreateText($"..\\..\\..\\input\\{name}.txt");
            var lines = pipes.Max(p => p.Line) + 1;
            var cols = pipes.Max(p => p.Col) + 1;
            for (int l = 0; l < lines; l++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var symbol = pipes.FirstOrDefault(p => p.Col == c && p.Line == l)?.Shape ?? ' ';
                    writer.Write(GetPretty(symbol));
                }
                writer.WriteLine();
            }
            writer.Flush();
            writer.Close();
            Console.WriteLine($"{name} created");
        }

        private void PrintToFile2(List<Pipe> pipes, string name)
        {
            var writer = File.CreateText($"..\\..\\..\\input\\{name}.txt");
            var lines = pipes.Max(p => p.Line) + 1;
            var cols = pipes.Max(p => p.Col) + 1;
            for (int l = 0; l < lines; l++)
            {
                var line1 = "";
                var line2 = "";
                var line3 = "";
                for (int c = 0; c < cols; c++)
                {
                    var pipe = pipes.FirstOrDefault(p => p.Col == c && p.Line == l);
                    if (pipe == null)
                    {
                        line1 += "   ";
                        line2 += "   ";
                        line3 += "   ";
                    }
                    else
                    {
                        var symbol = pipe.GetSymbol();
                        line1 += symbol.line1;
                        line2 += symbol.line2;
                        line3 += symbol.line3;
                    }
                }
                writer.WriteLine(line1);
                writer.WriteLine(line2);
                writer.WriteLine(line3);
            }
            writer.Flush();
            writer.Close();
            Console.WriteLine($"{name} created");
        }

        private static bool IsPipe(Pipe pipe)
        {
            return pipe.Shape switch
            {
                'F' or 'J' or 'L' or '7' or '|' or '_' => true,
                _ => false,
            };
        }

        internal static string GetPretty(char symbol)
        {
            return symbol switch
            {
                'F' => "┏",
                'J' => "┛",
                'L' => "┗",
                '7' => "┓",
                '|' => "┃",
                '-' => "━",
                _ => symbol.ToString(),
            };
        }

        private class Pipe(int col, int line, char shape) : IEquatable<Pipe>
        {
            public int Col { get; } = col;
            public int Line { get; } = line;
            public char Shape { get; set; } = shape;
            public List<Direction> Outsides { get; } = [];

            private Pipe? above;
            private Pipe? below;
            private Pipe? left;
            private Pipe? right;

            public (Pipe n1, Pipe n2) GetNeighbours()
            {
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

            public (Pipe? Above, Pipe? Below, Pipe? Left, Pipe? Right) GetSurrounding()
            {
                return (above, below, left, right);
            }

            public (string line1, string line2, string line3) GetSymbol()
            {
                var line1 = "   ";
                var line2 = " " + Day10.GetPretty(Shape) + " ";
                var line3 = "   ";

                var leftOut = Outsides.Any(o => o == Direction.Left);
                var upOut = Outsides.Any(o => o == Direction.Up);
                switch (Shape)
                {
                    case '|':
                        line1 = (leftOut ? "░" : " ") + '|' + (!leftOut ? "░" : " ");
                        line2 = (leftOut ? "░" : " ") + '|' + (!leftOut ? "░" : " ");
                        line3 = (leftOut ? "░" : " ") + '|' + (!leftOut ? "░" : " ");
                        break;
                }
                
                return (line1, line2, line3);
            }

            public void SetSurrounding(List<Pipe> pipes)
            {
                above = pipes.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col);
                below = pipes.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col);
                left = pipes.FirstOrDefault(p => p.Line == Line && p.Col == Col - 1);
                right = pipes.FirstOrDefault(p => p.Line == Line && p.Col == Col + 1);
                ArgumentNullException.ThrowIfNull(above ?? below ?? left ?? right);
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
