namespace Sharped;

/// <summary>
/// Represents a tridimensional triangular polygon with tree points.
/// </summary>
public record Face(Vertex p, Vertex q, Vertex r) : ITransformable<Face>
{
    public Face RotateX(float cosa, float sina) =>
        new Face(
            p.RotateX(cosa, sina),
            q.RotateX(cosa, sina),
            r.RotateX(cosa, sina)
        );

    public Face RotateY(float cosa, float sina) =>
        new Face(
            p.RotateY(cosa, sina),
            q.RotateY(cosa, sina),
            r.RotateY(cosa, sina)
        );

    public Face RotateZ(float cosa, float sina) =>
        new Face(
            p.RotateZ(cosa, sina),
            q.RotateZ(cosa, sina),
            r.RotateZ(cosa, sina)
        );

    public Face Scale(float x, float y, float z) =>
        new Face(
            p.Scale(x, y, z),
            q.Scale(x, y, z),
            r.Scale(x, y, z)
        );

    public Face Translate(float x, float y, float z) =>
        new Face(
            p.Translate(x, y, z),
            q.Translate(x, y, z),
            r.Translate(x, y, z)
        );

    public override string ToString()
        => $"{{{p}, {q}, {r}}}";
}