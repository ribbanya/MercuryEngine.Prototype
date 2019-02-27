using MercuryEngine.Prototype.EntityComponentSystem;
using MercuryEngine.Prototype.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MercuryEngine.Prototype.Components {
  [ComponentData]
  public struct ShapeRenderContext {
    public PrimitiveShapeBatch ShapeBatch;
    public BasicEffect Effect;
  }
}