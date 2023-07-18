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
    /// Rotate the object by a angle a in a X axis based in origin
    /// </summary>
    /// <param name="cosa">Cosine of the rotation angle</param>
    /// <param name="sina">Sine of the rotation angle</param>
    T RotateX(float cosa, float sina);

    /// <summary>
    /// Rotate the object by a angle a in a Y axis based in origin
    /// </summary>
    /// <param name="a">The angle of rotation</param>
    T RotateY(float cosa, float sina);

    /// <summary>
    /// Rotate the object by a angle a in a Z axis based in origin
    /// </summary>
    /// <param name="a">The angle of rotation</param>
    T RotateZ(float cosa, float sina);

    /// <summary>
    /// Scale the object based in the origin with (x, y, z) scale values
    /// </summary>
    /// <param name="x">The x component of scale</param>
    /// <param name="y">The y component of scale</param>
    /// <param name="z">The z component of scale</param>
    T Scale(float x, float y, float z);

    T RotateX(float x, float y, float z, float cosa, float sina) =>
        Translate(-x, -y, -z)
        .RotateX(cosa, sina)
        .Translate(x, y, z);

    T RotateY(float x, float y, float z, float cosa, float sina) =>
        Translate(-x, -y, -z)
        .RotateY(cosa, sina)
        .Translate(x, y, z);

    T RotateZ(float x, float y, float z, float cosa, float sina) =>
        Translate(-x, -y, -z)
        .RotateZ(cosa, sina)
        .Translate(x, y, z);
    
    T RotateX(Vertex p, float cosa, float sina) =>
        RotateX(p.x, p.y, p.z, cosa, sina);

    T RotateY(Vertex p, float cosa, float sina) =>
        RotateY(p.x, p.y, p.z, cosa, sina);

    T RotateZ(Vertex p, float cosa, float sina) =>
        RotateZ(p.x, p.y, p.z, cosa, sina);
}