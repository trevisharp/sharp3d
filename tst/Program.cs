using System;
using System.Drawing;
using System.Windows.Forms;

using Sharped;

Cam cam = null;
Scene scene = Scene.Create(
    new Mesh(
        new Face(
            (10, 0, 0),
            (10, 0, 5),
            (10, 5, 0)
        ),
        new Face(
            (10, 0, 0),
            (10, 0, 5),
            (20, 0, 0)
        ),
        new Face(
            (10, 0, 5),
            (20, 0, 0),
            (20, 0, 5)
        ),
        new Face(
            (20, 0, 0),
            (20, 0, 5),
            (20, 5, 0)
        ),
        new Face(
            (10, 0, 0),
            (10, 5, 0),
            (20, 0, 0)
        ),
        new Face(
            (10, 5, 0),
            (20, 0, 0),
            (20, 5, 0)
        ),
        new Face(
            (10, 0, 5),
            (10, 5, 0),
            (20, 5, 0)
        ),
        new Face(
            (10, 0, 5),
            (20, 0, 5),
            (20, 5, 0)
        )
    )
);

bool isRunning = true;

ApplicationConfiguration.Initialize();

var pb = new PictureBox();
pb.Dock = DockStyle.Fill;

var form = new Form();
form.WindowState = FormWindowState.Maximized;
form.FormBorderStyle = FormBorderStyle.None;
form.Controls.Add(pb);

Bitmap bmp = null;
Graphics g = null;

form.Load += delegate
{
    cam = new Cam(Vertex.Origin, Vector.i, Vector.j, 20f, pb.Width, pb.Height);
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    pb.Image = bmp;
};

form.KeyDown += (o, e) =>
{
    switch (e.KeyCode)
    {
        case Keys.Escape:
            isRunning = false;
            Application.Exit();
            break;
    }
};

PointF? desloc = null;
PointF cursor = PointF.Empty;
bool isDown = false;

pb.MouseDown += (o, e) => isDown = true;

pb.MouseUp += (o, e) => isDown = false;

pb.MouseMove += (o, e) => cursor = e.Location;

pb.MouseWheel += (o, e) =>
{
    var newFocal = cam.Focal + 2 * e.Delta
        / SystemInformation.MouseWheelScrollLines;

    if (newFocal < 1)
        newFocal = 1;
    
    if (newFocal > 1000)
        newFocal = 1000;
    
    cam.Focal = newFocal;
};

Application.Idle += delegate
{
    while (isRunning)
    {
        if (isDown && desloc is null)
            desloc = cursor;
        else if (isDown && desloc is not null)
        {
            var dx = cursor.X - desloc.Value.X;
            var dy = cursor.Y - desloc.Value.Y;

            cam.Translate(dx, dy);

            desloc = cursor;
        }
        else if (!isDown)
        {
            desloc = null;
        }
        
        scene.Meshes[0].RotateX(
            MathF.Cos(.01f),
            MathF.Sin(.01f)
        );

        cam?.Render(scene);
        cam?.Draw(g);
        pb?.Refresh();
        Application.DoEvents();
    }
};

Application.Run(form);