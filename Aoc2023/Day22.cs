using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode8.DayBase;
using static AdventOfCode8.Lib3d;

namespace AdventOfCode8.Aoc2023
{
    internal class Day22
    {
        internal void Run()
        {
            var start = DateTime.Now;

            Logg.DoLog = false;

            var input = GetInput("2023_22");
            var cuboids = GetCuboids(input);

            //var below = GetBelow(cuboids[3], cuboids);
            //Logg.WriteLine();
            //below.ForEach(Logg.WriteLine);

            cuboids = Compress(cuboids);
            //Logg.WriteLine("After compress");
            //cuboids.ForEach(Logg.WriteLine);

            var safe = CountBricks(cuboids);
            var sum = safe.Count();
            Console.WriteLine($"Sum: {sum}.");

            sum = CountFalling(cuboids, safe);
            Console.WriteLine($"Sum: {sum}.");


            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private List<Cuboid> CountBricks(List<Cuboid> cuboids)
        {
            var safe = new List<Cuboid>();
            foreach (var cuboid in cuboids)
            {
                Logg.WriteLine($"Testing {cuboid}");
                var above = GetAbove(cuboid, cuboids).Where(c => c.MinCorner.Z == cuboid.MaxCorner.Z + 1).ToList();
                if (above.Count == 0)
                {
                    if (!safe.Contains(cuboid))
                        safe.Add(cuboid);
                    continue;
                }
                var singleSupport = false;
                foreach (var cAbove in above)
                {
                    var below = GetBelow(cAbove, cuboids).Where(c => c.MaxCorner.Z == cAbove.MinCorner.Z - 1).ToList();
                    if (below.Count > 1)
                        continue;
                    singleSupport = true;
                    break;
                }
                if (!singleSupport & !safe.Contains(cuboid))
                    safe.Add(cuboid);
            }
            safe.ForEach(Logg.WriteLine);
            return safe;
        }

        private int CountFalling(List<Cuboid> cuboids, List<Cuboid> safe)
        {
            var sum = 0;
            var toTest = cuboids.Where(c => !safe.Contains(c)).ToList();
            var testCount = toTest.Count();
            var idx = 1;
            foreach(var cuboid in toTest)
            {
                Console.WriteLine($"{idx++} of {testCount}");
                sum += CountFalling(cuboid, cuboids);
            }

            return sum;
        }

        private int CountFalling(Cuboid cuboid, List<Cuboid> cuboids)
        {
            cuboids = cuboids.ToList();
            cuboids.Remove(cuboid);
            var moved = Compress2(cuboids);
            return moved.Count;
        }

        private List<Cuboid> Compress(List<Cuboid> cuboids)
        {
            var grounded = new List<Cuboid>();

            cuboids = [.. cuboids.OrderBy(c => c.MinCorner.Z)];
            while (true)
            {
                if (cuboids.Count == 0)
                    break;  
                var next = cuboids.First();
                var belows = GetBelow(next, grounded);
                var moveZ = 1 - next.MinCorner.Z;
                if (belows.Count != 0)
                    moveZ = belows.Max(c => c.MaxCorner.Z) - next.MinCorner.Z + 1;
                if (moveZ != 0)
                    grounded.Add(next.Move(new(0, 0, moveZ)));
                else
                    grounded.Add(next);
                cuboids.Remove(next);

            }

            return grounded;
        }

        private List<Cuboid> Compress2(List<Cuboid> cuboids)
        {
            var grounded = new List<Cuboid>();
            var moved = new List<Cuboid>();

            cuboids = [.. cuboids.OrderBy(c => c.MinCorner.Z)];
            while (true)
            {
                if (cuboids.Count == 0)
                    break;
                var next = cuboids.First();
                var belows = GetBelow(next, grounded);
                var moveZ = 1 - next.MinCorner.Z;
                if (belows.Count != 0)
                    moveZ = belows.Max(c => c.MaxCorner.Z) - next.MinCorner.Z + 1;
                if (moveZ != 0)
                {
                    grounded.Add(next.Move(new(0, 0, moveZ)));
                    moved.Add(next.Move(new(0, 0, moveZ)));
                }
                else
                    grounded.Add(next);
                cuboids.Remove(next);
            }

            return moved;
        }

        private List<Cuboid> GetAbove(Cuboid cuboid, List<Cuboid> map)
        {
            var minCorner = cuboid.MinCorner;
            var maxCorner = cuboid.MaxCorner;

            var above = map.Where(c => c.MinCorner.Z > maxCorner.Z).ToList();
            above = above.Where(c => minCorner.X <= c.MaxCorner.X && c.MinCorner.X <= cuboid.MaxCorner.X &&
                minCorner.Y <= c.MaxCorner.Y && c.MinCorner.Y <= cuboid.MaxCorner.Y).ToList();

            return above;
        }

        private List<Cuboid> GetBelow(Cuboid cuboid, List<Cuboid> map)
        {
            var minCorner = cuboid.MinCorner;
            var maxCorner = cuboid.MaxCorner;

            var below = map.Where(c => c.MaxCorner.Z < minCorner.Z).ToList();
            below = below.Where(c => minCorner.X <= c.MaxCorner.X && c.MinCorner.X <= cuboid.MaxCorner.X &&
                minCorner.Y <= c.MaxCorner.Y && c.MinCorner.Y <= cuboid.MaxCorner.Y).ToList();

            return below;
        }

        private List<Cuboid> GetCuboids(List<string> input)
        {
            var cuboids = new List<Cuboid>();

            foreach (var item in input)
            {
                var points = item.Split('~');
                var split = points[0].Split(',');
                var p1 = new Point3d(long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2]));
                split = points[1].Split(',');
                var p2 = new Point3d(long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2]));
                cuboids.Add(new Cuboid(p1, p2));
                Logg.WriteLine(cuboids.Last()); 
            }

            return cuboids;
        }
    }
}
