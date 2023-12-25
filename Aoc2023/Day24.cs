using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode8.DayBase;

namespace AdventOfCode8.Aoc2023
{
    internal class Day24
    {
        internal void Run()
        {
            var start = DateTime.Now;

            //Logg.DoLog = false;

            //var p1 = new DPoint(2, 12);
            //var p2 = new DPoint(2, 2);
            //var l1 = new Line(p2, p1);

            //var l1 = new Line(double.PositiveInfinity, 3);
            //var l2 = new Line(double.PositiveInfinity, 6);
            //var p1 = l1.GetIntersectionWith(l2);

            var input = GetInput("2023_24");
            var trajectories = GetTrajectories(input);
            //var sum = GetIntersections(trajectories, 7, 27);
            var sum = GetIntersections(trajectories, 200000000000000, 400000000000000);

            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now - start}");
        }

        private List<Line> GetLines((List<DPoint> points, List<DPoint> velocities) trajectories)
        {
            throw new NotImplementedException();
        }

        private object GetIntersections(List<Trajectory> trajectories, long min, long max)
        {
            var sum = 0;
            var lines = trajectories.Select(t => new Line(t.Start, t.Move(1))).ToList();

            lines.ForEach(Logg.WriteLine);
            //lines.ForEach(l => Logg.WriteLine(l.ToEquationString()));

            for (int i = 0; i < trajectories.Count; i++)
            {
                var line = lines[i];
                for (int j = i + 1; j < lines.Count; j++)
                {
                    var line2 = lines[j];
                    var iPoint = line.GetIntersectionWith(line2);
                    if (iPoint == null || iPoint.X < min || iPoint.X > max || iPoint.Y < min || iPoint.Y > max)
                        continue;

                    var trajectory1 = trajectories[i];
                    var trajectory2 = trajectories[j];
                    var t1 = trajectory1.GetTimeForX(iPoint.X);
                    var t2 = trajectory1.GetTimeForY(iPoint.Y);
                    var t3 = trajectory2.GetTimeForX(iPoint.X);
                    var t4 = trajectory2.GetTimeForY(iPoint.Y);

                    if (t1 < 0 || t2 < 0 || t3 < 0 || t4 < 0) 
                    {
                        Logg.WriteLine("In the past");
                        continue;
                    }
                    sum++;
                }
            }
            return sum;
        }

        private List<Trajectory> GetTrajectories(List<string> input)
        {
            var trajectories = new List<Trajectory>();
            foreach (var item in input)
            {
                var split = item.Split(" @ ");
                var split1 = split[0].Split(", ");
                var split2 = split[1].Split(", ");
                var p = new DPoint(double.Parse(split1[0]), double.Parse(split1[1]));
                var v = new DPoint(double.Parse(split2[0]), double.Parse(split2[1]));
                trajectories.Add(new Trajectory(p, v));
                Logg.WriteLine(trajectories.Last());
            }
            return trajectories;
        }
    }
}
