using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Sharped;
using Sharped.Meshes;

using static Sharped.Vector;

Cam cam = null;
List<Mesh> meshes = new List<Mesh>();

for (int i = -30; i <= 30; i+= 10)
    for (int j = -30; j <= 30; j += 10)
        meshes.Add(Mesh.Cube(i, j, 0, 5f));

Scene scene = Scene.Create(meshes);
scene.Ligths.Add(new Ligth((0, 0, -10), Color.White, 10f));

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
bool rotate = false;

form.Load += delegate
{
    cam = new Cam(Vertex.Origin - 15 * k, i, j, 200f, pb.Width, pb.Height);
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
        
        case Keys.W:
            cam.Translate(1, 0, 0);
            break;
        
        case Keys.S:
            cam.Translate(-1, 0, 0);
            break;
        
        case Keys.D:
            cam.Translate(0, 1, 0);
            break;
        
        case Keys.A:
            cam.Translate(0, -1, 0);
            break;
        
        case Keys.Space:
            cam.Translate(0, 0, -1);
            break;
        
        case Keys.ShiftKey:
            cam.Translate(0, 0, 1);
            break;
    }
};

PointF? desloc = null;
PointF cursor = PointF.Empty;
bool isDown = false;

pb.MouseDown += (o, e) => isDown = true;

pb.MouseUp += (o, e) => isDown = false;

PointF? center = null;
pb.MouseMove += (o, e) =>
{
    cursor = e.Location;

    if (center is not null)
    {
        // var dy = center.Value.Y - cursor.Y;
        // var angle = dy / 100f;
        // var sin = MathF.Sin(angle);
        // var cos = MathF.Cos(angle);
        // cam.RotateY(cos, sin);

        var dy = center.Value.Y - cursor.Y;
        var angle = dy / 100f;
        var sin = MathF.Sin(angle);
        var cos = MathF.Cos(angle);
        cam.RotateZ(cos, sin);
    }

    center = cursor;
};

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

            cam.Move(dx, dy);

            desloc = cursor;
        }
        else if (!isDown)
        {
            desloc = null;
        }

        if (rotate)
            scene.Meshes[0]
                .Translate(-15, 0, 0)
                .RotateX(
                    MathF.Cos(0.01f),
                    MathF.Sin(0.01f)
                )
                // .RotateY(
                //     MathF.Cos(0.01f),
                //     MathF.Sin(0.01f)
                // )
                // .RotateZ(
                //     MathF.Cos(0.01f),
                //     MathF.Sin(0.01f)
                // )
                .Translate(15, 0, 0);


        cam?.Render(scene);
        cam?.Draw(g);
        pb?.Refresh();
        Application.DoEvents();
    }
};

Application.Run(form);