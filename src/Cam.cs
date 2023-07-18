using System.Drawing;

namespace Sharped;

/// <summary>
/// Represents a tridimensional camera with location, a vector v direction,
/// a f focal distance, and a (wid, hei) renderization distance.
/// </summary>
public class Cam
{
    private Bitmap bmp;
    private Graphics g;

    public Vertex Location { get; set; }
    public Vector Vector { get; set; }
    public float Focal { get; set; }
    public int ScreenWidth { get; init; }
    public int ScreenHeight { get; init; }

    public Cam(Vertex p, Vector v, float f, int wid, int hei)
    {
        Location = p;
        Vector = v;
        Focal = f;
        ScreenWidth = wid;
        ScreenHeight = hei;

        bmp = new Bitmap(ScreenWidth, ScreenHeight);
        g = Graphics.FromImage(bmp);
    }

    public void Render(Scene scene)
    {

    }

    public void Draw(Graphics g)
    {
        g.DrawImage(
            this.bmp,
            PointF.Empty
        );
    }
}