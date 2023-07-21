namespace Sharped.Meshes;

public record Cube : Mesh
{
    private Vertex location;
    private float size;

    public Vertex Location
    {
        get => location;
        set
        {
            location = value;
            update();
        }
    }
    public float Size
    {
        get => size;
        set
        {
            size = value;
            update();
        }
    }

    public Cube(Vertex loc, float siz)
    {
        this.location = loc;
        this.size = siz;
        this.faces = new Face[48];
        update();
    }

    void update()
    {
        makeFace(Vector.i * size, Vector.j * size, Vector.k * size, 0);
        makeFace(-Vector.i * size, Vector.j * size, Vector.k * size, 8);
        makeFace(Vector.j * size, Vector.i * size, Vector.k * size, 16);
        makeFace(-Vector.j * size, Vector.i * size, Vector.k * size, 24);
        makeFace(Vector.k * size, Vector.j * size, Vector.i * size, 32);
        makeFace(-Vector.k * size, Vector.j * size, Vector.i * size, 40);
    }

    void makeFace(Vector u, Vector tp, Vector lf, int baseIndex)
    {
        var center = location - u;

        this.faces[baseIndex++] = new Face(
            center + lf + tp,
            center + lf,
            center + tp
        );

        this.faces[baseIndex++] = new Face(
            center,
            center + lf,
            center + tp
        );

        this.faces[baseIndex++] = new Face(
            center - lf - tp,
            center - lf,
            center - tp
        );

        this.faces[baseIndex++] = new Face(
            center,
            center - lf,
            center - tp
        );

        this.faces[baseIndex++] = new Face(
            center - lf + tp,
            center - lf,
            center + tp
        );

        this.faces[baseIndex++] = new Face(
            center,
            center - lf,
            center + tp
        );

        this.faces[baseIndex++] = new Face(
            center + lf - tp,
            center + lf,
            center - tp
        );

        this.faces[baseIndex++] = new Face(
            center,
            center + lf,
            center - tp
        );

    }
}
