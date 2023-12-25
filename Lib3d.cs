using static AdventOfCode8.Lib3d;

namespace AdventOfCode8
{
    public class Lib3d
    {
        public record Point3d(long X, long Y, long Z) 
        {
            public Point3d Move(Point3d velocity) => new(X + velocity.X, Y + velocity.Y, Z + velocity.Z);
        }

        public record Cuboid(Point3d Corner1, Point3d Corner2) 
        {
            public Point3d MinCorner => new(Math.Min(Corner1.X, Corner2.X), Math.Min(Corner1.Y, Corner2.Y), Math.Min(Corner1.Z, Corner2.Z));
            public Point3d MaxCorner => new(Math.Max(Corner1.X, Corner2.X), Math.Max(Corner1.Y, Corner2.Y), Math.Max(Corner1.Z, Corner2.Z));

            public Cuboid Move(Point3d velocity) => new(new Point3d(Corner1.X + velocity.X, Corner1.Y + velocity.Y, Corner1.Z + velocity.Z),
                new Point3d(Corner2.X + velocity.X, Corner2.Y + velocity.Y, Corner2.Z + velocity.Z));

            public override string ToString()
            {
                return $"{Corner1.X},{Corner1.Y},{Corner1.Z}~{Corner2.X},{Corner2.Y},{Corner2.Z}";
            }
        }

    }
}
