namespace Sharped;

/// <summary>
/// Represents a tridimensional vector with axes x, y and z.
/// </summary>
public record Vector(float x, float y, float z)
{
    public float mod => x * x + y * y + z * z;

    public override string ToString()
        => $"P = ({x:N3}, {y:N3}, {z:N3})";

    public static Vector operator +(Vector v)
        => new (v.x, v.y, v.z);
    
    public static Vector operator -(Vector v)
        => new (-v.x, -v.y, -v.z);

    public static Vector operator +(Vector v, Vector u)
        => new (v.x + u.x, v.y + u.y, v.z + u.z);
    
    public static Vector operator -(Vector v, Vector u)
        => new (v.x - u.x, v.y - u.y, v.z - u.z);
    
    public static Vector operator *(Vector v, float a)
        => new (v.x * a, v.y * a, v.z * a);
    
    public static Vector operator *(float a, Vector v)
        => new (a * v.x, a * v.y, a * v.z);
    
    public static Vector operator /(Vector v, float a)
        => new (v.x / a, v.y / a, v.z / a);
    
    public static Vector operator *(Vector u, Vector v)
        => new (
        u.y * v.z - u.z * v.y, 
        u.z * v.x - u.x * v.z,
        u.x * v.y - u.y * v.x
    );
    
    public static implicit operator Vector((float x, float y, float z) tuple)
        => new (tuple.x, tuple.y, tuple.z);
    
    public static readonly Vector Empty;
    public static readonly Vector i;
    public static readonly Vector j;
    public static readonly Vector k;
    static Vector()
    {
        Empty = new(0, 0, 0);
        i = new(1, 0, 0);
        j = new(0, 1, 0);
        k = new(0, 0, 1);
    }
}