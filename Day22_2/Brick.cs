using System.Drawing;

namespace Day22_1;

internal class Brick
{
    public Brick(int id, int x1, int y1, int z1, int x2, int y2, int z2)
    {
        Id = id;
        X1 = x1;
        Y1 = y1;
        Z1 = z1;
        X2 = x2;
        Y2 = y2;
        Z2 = z2;
    }

    public int Id { get; }
    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int Z1 { get; set; }
    public int X2 { get; set; }
    public int Y2 { get; set; }    
    public int Z2 { get; set; }

    public int MinZ => Math.Min(Z1, Z2);

    public int MaxZ => Math.Max(Z1, Z2);

    public bool Intersects(Brick other)
    {
        var rect1 = new Rectangle(X1, Y1, X2 - X1 + 1, Y2 - Y1 + 1);
        var rect2 = new Rectangle(other.X1, other.Y1, other.X2 - other.X1 + 1, other.Y2 - other.Y1 + 1);
        return rect1.IntersectsWith(rect2);
    }

    public override string ToString()
    {
        return $"({X1},{Y1},{Z1})-({X2},{Y2},{Z2})";
    }
}
