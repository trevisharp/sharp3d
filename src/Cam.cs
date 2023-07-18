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

    public Cam(Vertex p, Vector v, float f, int wid, int hei)
    {
        this.p = p;
        this.v = v;
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
        => d = -(p.x * v.x + p.y * v.y + p.z * v.z);

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

        // ax + by + cz + d + f = 0
        // (C - P) * t + C

        // C.x + t * (C.x - P.x) = x
        // C.y + t * (C.y - P.y) = y
        // C.z + t * (C.z - P.z) = z
        // a * x + b * y + c * z + d + f = 0

        // a * (C.x + t * (C.x - P.x)) + b * (C.y + t * (C.y - P.y)) + c * (C.z + t * (C.z - P.z)) + d + f = 0
        // t = -(d + f + a * C.x + b * C.y + z * C.z) / (a * (C.x - P.x) + b * (C.y - P.y) + c * (C.z - P.z))
        float t = -(d + f + a * C.x + b * C.y + c * C.z) / (a * (C.x - P.x) + b * (C.y - P.y) + c * (C.z - P.z));
        
        float x = - C.x + t * (C.x - P.x);
        float y = - C.x + t * (C.x - P.x);
        float z = - C.x + t * (C.x - P.x);

        return PointF.Empty;
    }
}