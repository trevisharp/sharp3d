namespace Sharped;

/// <summary>
/// Represents a tridimensional point with axes x, y and z.
/// </summary>
public record Point(float x, float y, float z) : ITransformable<Point>
{
    public Point Translate(float x, float y, float z)
        => new (this.x + x, this.y + y, this.z + z);
    
    public Point Scale(float x, float y, float z)
        => new (this.x * x, this.y * y, this.z * z);

    public Point RotateX(float cosa, float sina)
        => new (x, y * cosa - z * sina, y * sina + z * cosa);

    public Point RotateY(float cosa, float sina)
        => new (x * cosa + z * sina, y, z * cosa - y * sina);

    public Point RotateZ(float cosa, float sina)
        => new (x * cosa - y * sina, y * cosa + x * sina, z);

    public override string ToString()
        => $"({x:N3}, {y:N3}, {z:N3})";

    public static Point operator +(Point p, Vector v)
        => new (p.x + v.x, p.y + v.y, p.z + v.z);
    
    public static Point operator +(Vector v, Point p)
        => new (p.x + v.x, p.y + v.y, p.z + v.z);

    public static Vector operator -(Point p, Point q)
        => new (p.x - q.x, p.y - q.y, p.z - q.z);
        
    public static implicit operator Point((float x, float y, float z) tuple)
        => new (tuple.x, tuple.y, tuple.z);
    
    public static readonly Point Origin;
    static Point()
        => Origin = new(0, 0, 0);
}