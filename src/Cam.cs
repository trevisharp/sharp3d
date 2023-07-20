using System.Linq;
using System.Drawing;
using System.Collections.Generic;

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
    private float f;
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
    public float Focal
    {
        get => f;
        set
        {
            f = value;
            update();
        }
    }
    public int ScreenWidth { get; init; }
    public int ScreenHeight { get; init; }
    public int MaxDistance { get; set; } = int.MaxValue;

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
        var objs = new List<(PointF[] pts, Material mat, int dis)>();
        g.Clear(Color.Black);
        foreach (var mesh in scene.Meshes)
        {
            foreach (var face in mesh.faces)
            {
                if (!needDraw(face.p) && !needDraw(face.q) && !needDraw(face.r))
                    continue;

                var distP = dist(face.p);
                var distQ = dist(face.q);
                var distR = dist(face.r);
                var minDist = distP < distQ ?
                    (distP < distR ? distP : distR) :
                    (distQ < distR ? distQ : distR);
                
                if (minDist > MaxDistance)
                    continue;
                
                var obj = new PointF[]
                {
                    transform(face.p),
                    transform(face.q),
                    transform(face.r),
                };

                objs.Add((obj, mesh.Material, (int)minDist));
            }
        }

        var ordered = 
            from obj in objs
            orderby obj.dis
            select (obj.pts, obj.mat);
        
        foreach (var obj in ordered)
        {
            g.DrawPolygon(Pens.White, obj.pts);
        }
    }

    public void Draw(Graphics g)
    {
        g.DrawImage(
            this.bmp,
            PointF.Empty
        );
    }
    
    float parametricNumerator = 0;
    float dirLocProd = 0;
    float deltaCenterX = 0;
    float deltaCenterY = 0;
    float deltaCenterZ = 0;
    float nxSolutionDiv = 0;
    float nySolutionDiv = 0;
    float nzSolutionDiv = 0;

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
        dirLocProd = v.x * Location.x + v.y * Location.y + v.z * Location.z;
        parametricNumerator = -(d - f + dirLocProd);
        deltaCenterX = Location.x - renderCenter.x;
        deltaCenterY = Location.y - renderCenter.y;
        deltaCenterZ = Location.z - renderCenter.z;

        nxSolutionDiv = m.y - m.x * n.y / n.x;
        nySolutionDiv = m.z - m.y * n.z / n.y;
        nzSolutionDiv = m.x - m.z * n.x / n.z;
    }

    private float dist(Vertex p)
    {
        var dx = p.x - Location.x;
        var dy = p.y - Location.y;
        var dz = p.z - Location.z;
        return dx * dx + dy * dy + dz * dz;
    }

    private bool needDraw(Vertex q)
        => v.x * q.x + v.y * q.y + v.z * q.z + d > 0;

    private PointF transform(Vertex P)
    {
        // ax + by + cz + d - f = 0
        // (C - P) * t + C

        // C.x + t * (P.x - C.x) = x
        // C.y + t * (P.y - C.y) = y
        // C.z + t * (P.z - C.z) = z
        // a * x + b * y + c * z + d - f = 0

        // a * (C.x + t * (P.x - C.x)) + b * (C.y + t * (P.y - C.y)) + c * (C.z + t * (P.z - C.z)) + d + f = 0
        // t = -(d - f + a * C.x + b * C.y + z * C.z) / (a * (P.x - C.x) + b * (P.y - C.y) + c * (P.z - C.z))
        float t = parametricNumerator / (v.x * P.x + v.y * P.y + v.z * P.z - dirLocProd);
        
        float x = t * (P.x - Location.x) + deltaCenterX;
        float y = t * (P.y - Location.y) + deltaCenterY;
        float z = t * (P.z - Location.z) + deltaCenterZ;

        // a * nx + b * mx = x
        // a * ny + b * my = y
        // a * nz + b * mz = z
        
        float a, b;
        if (n.x != 0)
        {
            // a = (x - b * mx) / nx
            // (x - b * mx) * ny / nx + b * my = y
            // b = (y - x * ny / nx) / (my - mx * ny / nx)
            b = (y - x * n.y / n.x) / nxSolutionDiv;
            a = (x - b * m.x) / n.x;
        }
        else if (n.y != 0)
        {
            // a = (y - b * my) / ny
            // (y - b * my) * nz / ny + b * mz = z
            // b = (z - y * nz / ny) / (mz - my * nz / ny)
            b = (z - y * n.z / n.y) / nySolutionDiv;
            a = (y - b * m.y) / n.y;
        }
        else
        {
            // a = (z - b * mz) / nz
            // (z - b * mz) * nx / nz + b * mx = x
            // b = (x - z * nx / nz) / (mx - mz * nx / nz)
            b = (x - z * n.x / n.z) / nzSolutionDiv;
            a = (z - b * m.z) / n.z;
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