using System.Globalization;

namespace AdventOfCode8;

public partial class DayBase
{
    //public class Position<T1>(T1 col, T1 line)
    //{
    //    public T1 Col { get; } = col;
    //    public T1 Line { get; } = line;
    //}

    //public class Position<T1, T2>(T1 col, T1 line, T2 value)
    //{
    //    public T1 Col { get; } = col;
    //    public T1 Line { get; } = line;
    //    public T2 Value { get; } = value;
    //}

    public record Position(int Line, int Col)
    {
        public int ManhattanDistance(Position p2) => Math.Abs(Line - p2.Line) + Math.Abs(Col - p2.Col);

        public static int ShoelaceArea(List<Position> area)
        {
            var n = area.Count;
            var a = 0;
            for (var i = 0; i < n - 1; i++)
            {
                a += area[i].Col * area[i + 1].Line - area[i + 1].Col * area[i].Line;
            }
            return Math.Abs(a + area[n - 1].Col * area[0].Line - area[0].Col * area[n - 1].Line) / 2;
        }

        public (Position? up, Position? down, Position? left, Position? right) GetSurrounding(IEnumerable<Position> map)
        {
            return new(map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col),
                map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col),
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col - 1),
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col + 1));
        }

        public (Position<T>? up, Position<T>? down, Position<T>? left, Position<T>? right) GetSurrounding<T>(IEnumerable<Position<T>> map)
        {
            return new(
                map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col), // up
                map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col), // down
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col - 1), // left
                map.FirstOrDefault(p => p.Line == Line && p.Col == Col + 1)); // right
        }

        /// <summary>
        /// Returns the 4 positions surrounding the current position in a 45 degree angle.
        /// It's not possible to get the 8 surrounding positions due to a limit of max 7 items in a ValueTuple.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public (Position<T>? upLeft, Position<T>? downLeft, Position<T>? upRight, Position<T>? downRight) GetSurrounding45<T>(IEnumerable<Position<T>> map)
        {
            return new(
                map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col - 1), // upLeft
                map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col - 1), // downLeft
                map.FirstOrDefault(p => p.Line == Line - 1 && p.Col == Col + 1), // upRight
                map.FirstOrDefault(p => p.Line == Line + 1 && p.Col == Col + 1)); // downRight
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

    public static double ShoelaceArea(List<DPoint> v)
    {
        int n = v.Count;
        double a = 0.0;
        for (int i = 0; i < n - 1; i++)
        {
            a += v[i].X * v[i + 1].Y - v[i + 1].X * v[i].Y;
        }
        return Math.Abs(a + v[n - 1].X * v[0].Y - v[0].X * v[n - 1].Y) / 2.0;
    }

}