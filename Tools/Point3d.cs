namespace Tools;

public record struct Point3d(int X, int Y, int Z)
{
    public static Point3d operator +(Point3d a, Point3d b) => new Point3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    public static Point3d operator -(Point3d a, Point3d b) => new Point3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public Point3d Normalize() => new Point3d(X != 0 ? X / Math.Abs(X) : 0, Y != 0 ? Y / Math.Abs(Y) : 0, Z != 0 ? Z / Math.Abs(Z) : 0);

    public static implicit operator Point3d((int X, int Y, int Z) tuple) => new Point3d(tuple.X, tuple.Y, tuple.Z);

    public int ManhattanDistance(Point3d b) => Math.Abs(X - b.X) + Math.Abs(Y - b.Y) + Math.Abs(Z - b.Z);
}