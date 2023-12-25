using System;
using System.Collections.Generic;
using System.Linq;
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

            //Logg.DoLog = false;

            var input = GetInput("2023_22s");
            var cuboids = GetCuboids(input);

            //var below = GetBelow(cuboids[3], cuboids);
            //Logg.WriteLine();
            //below.ForEach(Logg.WriteLine);

            cuboids = Compress(cuboids);


            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
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
                    moveZ = belows.Max(c => c.MaxCorner.Z) - next.MinCorner.Z;
                if (moveZ != 0)
                    grounded.Add(next.Move(new(0, 0, moveZ)));
                else
                    grounded.Add(next);
                cuboids.Remove(next);

            }

            return grounded;
        }

        private List<Cuboid> GetBelow(Cuboid cuboid, List<Cuboid> grounded)
        {
            var minCorner = cuboid.MinCorner;
            var maxCorner = cuboid.MaxCorner;

            var belows = grounded.Where(c => c.MaxCorner.Z < minCorner.Z).ToList();
            belows = belows.Where(c => (minCorner.X >= c.MinCorner.X && minCorner.X <= c.MaxCorner.X) ||
                (maxCorner.X >= c.MinCorner.X && maxCorner.X <= c.MaxCorner.X)).ToList();
            belows = belows.Where(c => (minCorner.Y >= c.MinCorner.Y && minCorner.Y <= c.MaxCorner.Y) ||
                (maxCorner.Y >= c.MinCorner.Y && maxCorner.Y <= c.MaxCorner.Y)).ToList();

            return belows;

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
