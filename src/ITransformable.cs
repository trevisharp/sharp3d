namespace Sharped;

/// <summary>
/// Represents a geometric transfomable object that can translate, rotate and scale
/// </summary>
/// <typeparam name="T">The type of production of each transformation</typeparam>
public interface ITransformable<T>
    where T : ITransformable<T>
{
    /// <summary>
    /// Translate the object by the vector (x, y, z)
    /// </summary>
    /// <param name="x">The value of axis x translation</param>
    /// <param name="y">The value of axis y translation</param>
    /// <param name="z">The value of axis z translation</param>
    T Translate(float x, float y, float z);

    /// <summary>
    /// Rotate the object by a angle a in a axis (ax, ay, az) based in origin.
    /// </summary>
    /// <param name="ax">The x component of rotation axis</param>
    /// <param name="ay">The y component of rotation axis</param>
    /// <param name="az">The z component of rotation axis</param>
    /// <param name="a">The angle of rotation</param>
    T Rotate(float ax, float ay, float az, float a);

    /// <summary>
    /// Scale the object based in the origin with (x, y, z) scale values
    /// </summary>
    /// <param name="x">The x component of scale</param>
    /// <param name="y">The y component of scale</param>
    /// <param name="z">The z component of scale</param>
    T Scale(float x, float y, float z);

    /// <summary>
    /// Rotate the object by a angle a in a axis (ax, ay, az) based in a point (x, y, z)
    /// </summary>
    /// <param name="x">The x component of rotation point</param>
    /// <param name="y">The y component of rotation point</param>
    /// <param name="z">The z component of rotation point</param>
    /// <param name="ax">The x component of rotation axis</param>
    /// <param name="ay">The y component of rotation axis</param>
    /// <param name="az">The z component of rotation axis</param>
    /// <param name="a">The angle of rotation</param>
    T Rotate(float x, float y, float z, float ax, float ay, float az, float a) =>
        Translate(-x, -y, -z)
        .Rotate(ax, ay, az, a)
        .Translate(x, y, z);

    T RotateX(float x, float y, float z, float a) =>
        Rotate(x, y, z, 1, 0, 0, a);

    T RotateY(float x, float y, float z, float a) =>
        Rotate(x, y, z, 0, 1, 0, a);

    T RotateZ(float x, float y, float z, float a) =>
        Rotate(x, y, z, 0, 0, 1, a);
    
    /// <summary>
    /// Rotate the object by a angle a in a axis (ax, ay, az) based in a point (x, y, z)
    /// </summary>
    /// <param name="x">The x component of rotation point</param>
    /// <param name="y">The y component of rotation point</param>
    /// <param name="z">The z component of rotation point</param>
    /// <param name="ax">The x component of rotation axis</param>
    /// <param name="ay">The y component of rotation axis</param>
    /// <param name="az">The z component of rotation axis</param>
    /// <param name="a">The angle of rotation</param>
    T Rotate(Point p, float ax, float ay, float az, float a) =>
        Translate(-p.x, -p.y, -p.z)
        .Rotate(ax, ay, az, a)
        .Translate(p.x, p.y, p.z);

    T RotateX(Point p, float a) =>
        Rotate(p.x, p.y, p.z, 1, 0, 0, a);

    T RotateY(Point p, float a) =>
        Rotate(p.x, p.y, p.z, 0, 1, 0, a);

    T RotateZ(Point p, float a) =>
        Rotate(p.x, p.y, p.z, 0, 0, 1, a);

    T RotateX(float a) =>
        Rotate(0, 0, 0, 1, 0, 0, a);

    T RotateY(float a) =>
        Rotate(0, 0, 0, 0, 1, 0, a);

    T RotateZ(float a) =>
        Rotate(0, 0, 0, 0, 0, 1, a);
}