using System.Drawing;
using System.Globalization;

namespace AdventOfCode8;

public partial class DayBase
{
    public record Position(int Line, int Col)
    {
        public int ManhattanDistance(Position p2) => Math.Abs(Line - p2.Line) + Math.Abs(Col - p2.Col);

        public static int ShoelaceArea(List<Position> area)
        {
            var n = area.Count;
            var a = 0;
            for (var i = 0; i < n - 1; i++)
            {
                a += area[i].Line * area[i + 1].Col - area[i + 1].Line * area[i].Col;
            }
            return Math.Abs(a + area[n - 1].Line * area[0].Col - area[0].Line * area[n - 1].Col) / 2;
        }

        public (Position? up, Position? down, Position? left, Position? right) GetSurrounding(IEnumerable<Position> map)
        {
            return new(map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col),
                map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col),
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col - 1),
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col + 1));
        }

        public (Position? up, Position? down, Position? left, Position? right) GetSurrounding<T>(T[,] map)
        {
            var maxLine = map.GetLength(0) - 1;
            var maxCol = map.GetLength(1) - 1;
            return new (
                Line > 0 ? new(Line - 1, Col) : null,
                Line < maxLine  ? new(Line + 1, Col) : null,
                Col > 0 ? new(Line, Col - 1) : null,
                Col < maxCol -1 ? new(Line, Col + 1) : null);
        }

    }

    public record PositionString(int Line, int Col, string Value) : Position(Line, Col)
    {
    }

    public record Position<T>(int Line, int Col, T Value) : Position(Line, Col)
    {
    }

    public record DPoint(double X, double Y)
    {
        public DPoint Move(DPoint velocity) => new(X + velocity.X, Y + velocity.Y);
    }

    public record Line () : IEquatable<Line> 
    {
        public double K { get; }
        public double M { get; }

        public double GetX(double y) => (y - M) / K; // x = (y - m) / k
        public double GetY(double x) => K * x + M; // y = kx + m

        public bool Vertical => double.IsInfinity(K);
        public bool Horizontal => K == 0;

        // y = kx + m
        public Line(double k, double m) : this() 
        {
            K = k;
            M = m;
        }

        public Line(DPoint start, DPoint end) : this()
        {
            K = (end.Y - start.Y) / (end.X - start.X);
            M = double.IsInfinity(K) ? start.X : start.Y - K * start.X;
        }

        public DPoint GetPointForX(double x)
        {
            return new DPoint(x, GetY(x));
        }

        public DPoint GetPointForY(double y)
        {
            // x = (y - m) / k
            return new DPoint(GetX(y), y);
        }

        public DPoint? GetIntersectionWith(Line line2)
        {
            if (line2.Equals(this)) 
                return null;

            if (K == line2.K)
                return null;

            if (Vertical)
                return new DPoint(M, line2.GetY(M));

            if (line2.Vertical)
                return new DPoint(line2.M, GetY(line2.M));

            var x = (line2.M - M) / (K - line2.K);
            return GetPointForX(x);
        }

        public string ToEquationString()
        {
            return $"y = {K.ToString(CultureInfo.InvariantCulture)}x + {M.ToString(CultureInfo.InvariantCulture)}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(K, M);
        }
    }


    public record Trajectory(DPoint Start, DPoint Velocity)
    { 
        public DPoint Move(double time) => new DPoint(Start.X + Velocity.X * time, Start.Y + Velocity.Y * time);

        public double GetTimeForX(double x) => (x - Start.X) / Velocity.X;
        public double GetTimeForY(double y) => (y - Start.Y) / Velocity.Y;
    }

}