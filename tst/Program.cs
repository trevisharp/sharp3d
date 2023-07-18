using System.Windows.Forms;

ApplicationConfiguration.Initialize();

var pb = new PictureBox();
pb.Dock = DockStyle.Fill;

var form = new Form();
form.WindowState = FormWindowState.Maximized;
form.FormBorderStyle = FormBorderStyle.None;
form.Controls.Add(pb);

Application.Run(form);