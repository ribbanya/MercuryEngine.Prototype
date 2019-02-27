using MercuryEngine.Prototype.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace MercuryEngine.Prototype.Components {
  [ComponentData]
  public struct CircleRenderData {
    public float Radius;
    public Color Color;
    public bool Fill;
    public Vector2 Scale;
  }
}