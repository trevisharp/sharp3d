using System.Drawing;
using System.Windows.Forms;

using Sharped;

var cam = new Cam(Vertex.Origin, Vector.i, 5f, 640, 480);
Scene scene = Scene.Create(
    new Mesh(
        new Face(
            (10, 5, -5),
            (10, 10, 0),
            (10, 5, +5)
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
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
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

Application.Idle += delegate
{
    while (isRunning)
    {
        cam.Render(scene);
        cam.Draw(g);
        Application.DoEvents();
    }
};

Application.Run(form);