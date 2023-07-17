using System.Drawing;

namespace Sharped;

/// <summary>
/// Represents a source of light with a 3 dimensional
/// point as location, a rgb color and a force of light.
/// </summary>
public record Ligth(Point p, Color color, float force);