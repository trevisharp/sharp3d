using System.Drawing;
using System.Windows.Forms;

using Sharped;

Scene scene = new Scene
{
    MainCamera = new Cam(Vertex.Origin, Vector.Empty, 0, 0, 0),
    new (

    )
};

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
        Application.DoEvents();
    }
};

Application.Run(form);