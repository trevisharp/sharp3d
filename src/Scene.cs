using System.Collections;
using System.Collections.Generic;

namespace Sharped;

public class Scene : List<Mesh>
{
    public Cam? MainCamera { get; set; }
    public List<Ligth> Ligths { get; private set; } = new();
}