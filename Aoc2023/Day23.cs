﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Aoc2023
{
    internal class Day23 : DayBase
    {

        private int longest = 0;
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_23");

            var map = GetMap(input);
            DrawMap(map);

            var startPos = new Position(0, 1);
            var endPos = new Position(map.GetLength(0) - 1, map.GetLength(1) - 2);
            var visited = new HashSet<Position>();
            //DrawMap(map, startPos, visited);
            //DrawMap(map, endPos);
            Move(startPos, endPos, 0, map, visited);

            Console.WriteLine($"Longest route: {longest}");

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private void Move(Position startPos, Position endPos, int steps, char[,] map, HashSet<Position> visited)
        {
            if (startPos == endPos) 
            {
                Console.WriteLine($"Found end: {steps}");
                longest = Math.Max(steps, longest);
                return;
            }
            DrawMap(map, startPos, visited);
            visited.Add(startPos);
            var (up, down, left, right) = startPos.GetSurrounding(map);
            var next = right;
            if (ShouldTry(next, Direction.Right, map, visited))
                 Move(next, endPos, steps + 1, map, [.. visited]);
            next = down;
            if (ShouldTry(next, Direction.Down, map, visited))
                Move(next, endPos, steps + 1, map, [.. visited]);
            next = left;
            if (ShouldTry(next, Direction.Left, map, visited))
                Move(next, endPos, steps + 1, map, [.. visited]);
            next = up;
            if (ShouldTry(next, Direction.Up, map, visited))
                Move(next, endPos, steps + 1, map, [.. visited]);
        }

        private void Move2(Position startPos, Position endPos, int steps, char[,] map, HashSet<Position> visited)
        {
            var currPos = startPos;
            while (true)
            {
                if (currPos == endPos)
                {
                    Console.WriteLine($"Found end: {steps}");
                    longest = Math.Max(steps, longest);
                    continue;
                }
                DrawMap(map, currPos, visited);
                visited.Add(startPos);
                var (up, down, left, right) = startPos.GetSurrounding(map);
                var next = right;
                if (ShouldTry(next, Direction.Right, map, visited))
                    Move(next, endPos, steps + 1, map, [.. visited]);
                next = down;
                if (ShouldTry(next, Direction.Down, map, visited))
                    Move(next, endPos, steps + 1, map, [.. visited]);
                next = left;
                if (ShouldTry(next, Direction.Left, map, visited))
                    Move(next, endPos, steps + 1, map, [.. visited]);
                next = up;
                if (ShouldTry(next, Direction.Up, map, visited))
                    Move(next, endPos, steps + 1, map, [.. visited]);
            }
        }

        private bool ShouldTry(Position? next, Direction direction, char[,] map, HashSet<Position> visited)
        {
            if (next == null || !IsPath(next, map))
                return false;
            if (visited.Contains(next))
                return false;
            var c = map[next.Line, next.Col];
            var tryIt = c switch
            {
                '.' => true,
                '>' when direction == Direction.Right => true,
                '<' when direction == Direction.Left => true,
                '^' when direction == Direction.Up => true,
                'v' when direction == Direction.Down  => true,
                _ => false
            };

            if (!tryIt)
                return false;

            Logg.WriteLine($"Try {next}");
            return true;
        }

        private bool IsPath(Position pos, char[,] map)
        {
            var c = map[pos.Line, pos.Col];
            return c switch
            {
                '.' or '<' or '>' or '^' or 'v' => true,
                _ => false
            };
        }

        private void DrawMap(char[,] map, Position? current = null, HashSet<Position>? visited = null)
        {
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            for (var l = 0; l < lines; l++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var draw = visited != null && visited.Contains(new Position(l, c)) ? 'O' :
                        current != null && current.Line == l && current.Col == c ? 'x' :
                        map[l, c];
                    Logg.Write($"{draw}");
                }
                Logg.WriteLine();
            }
            Logg.WriteLine();
        }

        private char[,] GetMap(List<string> input)
        {
            var map = new char[input.Count, input.First().Length];
            var lines = map.GetLength(0);
            var cols = map.GetLength(1);
            for (var l = 0; l < lines; l++)
            {
                var line = input[l];
                for (var c = 0; c < cols; c++)
                {
                    map[l, c] = line[c];
                }
            }
            return map;
        }
    }
}
