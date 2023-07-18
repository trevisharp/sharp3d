using System.Drawing;
using System.Collections.Generic;

namespace Sharped;

public class Scene
{
    public List<Mesh> Meshes { get; private set; } = new();
    public List<Ligth> Ligths { get; private set; } = new();

    public static Scene Create(params Mesh[] meshes)
    {
        Scene scene = new Scene();
        
        scene.Meshes.AddRange(meshes);

        return scene;
    }
}