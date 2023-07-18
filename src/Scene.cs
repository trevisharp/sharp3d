using System.Collections.Generic;

namespace Sharped;

public class Scene
{
    public Cam? MainCamera { get; set; }

    public List<Mesh> Meshes { get; private set; } = new();
    public List<Ligth> Ligths { get; private set; } = new();
    public List<Cam> Cameras { get; private set; } = new();
}