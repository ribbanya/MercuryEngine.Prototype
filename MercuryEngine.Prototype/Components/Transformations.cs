using MercuryEngine.Prototype.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace MercuryEngine.Prototype.Components {
  [ComponentData]
  public struct Transformations {
    public Matrix World, View, Projection;
  }
}