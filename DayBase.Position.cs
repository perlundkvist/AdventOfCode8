using System.Drawing;

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

    }

    public record PositionString(int Line, int Col, string Value) : Position(Line, Col)
    {
    }

    public record Position<T>(int Line, int Col, T Value) : Position(Line, Col)
    {
    }

}