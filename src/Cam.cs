using System.Drawing;

namespace Sharped;

/// Represents a tridimensional camera with location, a vector v direction,
/// a f focal distance, and a (wid, hei) renderization distance.
/// </summary>
public class Cam
{
    private Bitmap bmp;
    private Graphics g;
    private float d;
    private Vertex p;
    private Vector v;
    private Vector renderCenter;
    
    private Vector n;
    private Vector m;

    public Vertex Location
    {
        get => p;
        set
        {
            p = value;
            update();
        }
    }
    public Vector Vector
    {
        get => v;
        set
        {
            v = value;
            update();
        }
    }
    public float Focal { get; set; }
    public int ScreenWidth { get; init; }
    public int ScreenHeight { get; init; }

    public Cam(Vertex p, Vector v, Vector u, float f, int wid, int hei)
    {
        this.p = p;
        this.v = v;
        this.n = u;
        this.m = v * u;
        Focal = f;
        ScreenWidth = wid;
        ScreenHeight = hei;
        
        update();

        bmp = new Bitmap(ScreenWidth, ScreenHeight);
        g = Graphics.FromImage(bmp);
    }

    public void Render(Scene scene)
    {
        g.Clear(Color.Black);
        foreach (var mesh in scene.Meshes)
        {
            foreach (var face in mesh.faces)
            {
                if (!needDraw(face.p) && !needDraw(face.q) && !needDraw(face.r))
                    continue;

                g.DrawPolygon(Pens.White, new PointF[]
                {
                    transform(face.p),
                    transform(face.q),
                    transform(face.r),
                });
            }
        }
    }

    public void Draw(Graphics g)
    {
        g.DrawImage(
            this.bmp,
            PointF.Empty
        );
    }
    
    private void update()
    {
        d = -(p.x * v.x + p.y * v.y + p.z * v.z);

        // v.x * t + p.x = cx
        // v.y * t + p.y = cy
        // v.z * t + p.z = cz
        // v.x * c.x + v.y * c.y + v.z * c.z + d + f = 0
        // v.x^2 * t + v.x * p.x + v.y^2 * t + v.y * p.y + v.z^2 * t + v.z * p.z + d + f = 0
        // t = -(v.x * p.x + v.y * p.y + v.z * p.z + d + f) /  (v.x^2 + v.y^2 + v.z^2)]
        var t = -(v.x * p.x + v.y * p.y + v.z * p.z + d - Focal) 
            / (v.x * v.x + v.y * v.y + v.z * v.z);
        renderCenter = new (
            v.x * t + p.x,
            v.y * t + p.y,
            v.z * t + p.z
        );
    }

    private bool needDraw(Vertex q)
        => v.x * q.x + v.y * q.y + v.z * q.z + d > 0;

    private PointF transform(Vertex v)
    {
        // TODO: optmize
        float a = this.v.x,
            b = this.v.y,
            c = this.v.z,
            f = Focal;
        var C = this.Location;
        var P = v;

        // ax + by + cz + d - f = 0
        // (C - P) * t + C

        // C.x + t * (P.x - C.x) = x
        // C.y + t * (P.y - C.y) = y
        // C.z + t * (P.z - C.z) = z
        // a * x + b * y + c * z + d - f = 0

        // a * (C.x + t * (P.x - C.x)) + b * (C.y + t * (P.y - C.y)) + c * (C.z + t * (P.z - C.z)) + d + f = 0
        // t = -(d - f + a * C.x + b * C.y + z * C.z) / (a * (P.x - C.x) + b * (P.y - C.y) + c * (P.z - C.z))
        float t = -(d - f + a * C.x + b * C.y + c * C.z) 
            / (a * (P.x - C.x) + b * (P.y - C.y) + c * (P.z - C.z));
        
        float x = C.x + t * (P.x - C.x);
        float y = C.y + t * (P.y - C.y);
        float z = C.z + t * (P.z - C.z);

        x -= renderCenter.x;
        y -= renderCenter.y;
        z -= renderCenter.z;

        var nx = n.x;
        var ny = n.y;
        var nz = n.z;
        var mx = m.x;
        var my = m.y;
        var mz = m.z;

        // a * nx + b * mx = x
        // a * ny + b * my = y
        // a * nz + b * mz = z
        
        if (nx != 0)
        {
            // a = (x - b * mx) / nx
            // (x - b * mx) * ny / nx + b * my = y
            // b = (y - x * ny / nx) / (my - mx * ny / nx)
            b = (y - x * ny / nx) / (my - mx * ny / nx);
            a = (x - b * mx) / nx;
        }
        else if (ny != 0)
        {
            // a = (y - b * my) / ny
            // (y - b * my) * nz / ny + b * mz = z
            // b = (z - y * nz / ny) / (mz - my * nz / ny)
            b = (z - y * nz / ny) / (mz - my * nz / ny);
            a = (y - b * my) / ny;
        }
        else
        {
            // a = (z - b * mz) / nz
            // (z - b * mz) * nx / nz + b * mx = x
            // b = (x - z * nx / nz) / (mx - mz * nx / nz)
            b = (x - z * nx / nz) / (mx - mz * nx / nz);
            a = (z - b * mz) / nz;
        }
        return new PointF(a + ScreenWidth / 2, b + ScreenHeight / 2);
    }

    public void Translate(float x, float y)
    {
        this.Location = this.Location with
        {
            x = this.Location.x - 10 * (x * n.x + y * m.x) / Focal,
            y = this.Location.y - 10 * (x * n.y + y * m.y) / Focal,
            z = this.Location.z - 10 * (x * n.z + y * m.z) / Focal
        };
    }

    public Cam RotateX(float cosa, float sina)
    {
        throw new System.NotImplementedException();
    }

    public Cam RotateY(float cosa, float sina)
    {
        throw new System.NotImplementedException();
    }

    public Cam RotateZ(float cosa, float sina)
    {
        throw new System.NotImplementedException();
    }

    public Cam Scale(float x, float y, float z)
    {
        throw new System.NotImplementedException();
    }
}