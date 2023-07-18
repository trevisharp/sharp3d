namespace Sharped;

/// <summary>
/// Represents a mutable set of faces/polygons.
/// </summary>
public record Mesh(params Face[] faces) : ITransformable<Mesh>
{
    public Mesh RotateX(float cosa, float sina)
    {
        for (int i = 0; i < faces.Length; i++)
            faces[i] = faces[i].RotateX(cosa, sina);
        return this;
    }

    public Mesh RotateY(float cosa, float sina)
    {
        for (int i = 0; i < faces.Length; i++)
            faces[i] = faces[i].RotateY(cosa, sina);
        return this;
    }

    public Mesh RotateZ(float cosa, float sina)
    {
        for (int i = 0; i < faces.Length; i++)
            faces[i] = faces[i].RotateZ(cosa, sina);
        return this;
    }

    public Mesh Scale(float x, float y, float z)
    {
        for (int i = 0; i < faces.Length; i++)
            faces[i] = faces[i].Scale(x, y, z);
        return this;
    }

    public Mesh Translate(float x, float y, float z)
    {
        for (int i = 0; i < faces.Length; i++)
            faces[i] = faces[i].Translate(x, y, z);
        return this;
    }
}