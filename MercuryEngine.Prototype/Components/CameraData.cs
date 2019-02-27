using MercuryEngine.Prototype.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace MercuryEngine.Prototype.Components {
  [ComponentData]
  public struct CameraData {
    public Vector2 Origin, Scale;
    public float Rotation;
  }
}