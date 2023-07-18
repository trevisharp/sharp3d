using System.Drawing;

namespace Sharped;

/// <summary>
/// Represents a source of light with a tridimensional
/// point as location, a rgb color and a force of light.
/// </summary>
public record Ligth(Vertex p, Color color, float force);