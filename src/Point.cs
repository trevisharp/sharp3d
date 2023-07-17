namespace Sharped;

public record Point(float x, float y, float z)
{
    public static Point operator +(Point p, Vector v)
        => new (p.x + v.x, p.y + v.y, p.z + v.z);
    
    public static Point operator +(Vector v, Point p)
        => new (p.x + v.x, p.y + v.y, p.z + v.z);

    public static Vector operator -(Point p, Point q)
        => new (p.x - q.x, p.y - q.y, p.z - q.z);
}