using System.Linq;
using MercuryEngine.Prototype.Components;
using MercuryEngine.Prototype.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace MercuryEngine.Prototype.Systems {
  public class CircleRenderSystem : ISystem {
    public void Update() {
      var context = EntityManager.GetReadOnlyComponentDictionary<ShapeRenderContext>().Values.First();

      var count = EntityManager.Count;

      var cameraData = EntityManager.GetReadOnlyComponentDictionary<CameraData>();
      var transformations = EntityManager.GetReadOnlyComponentDictionary<Transformations>();
      for (var i = 0; i < count; i++) {
        if (!cameraData.ContainsKey(i)) continue;
        if (!transformations.TryGetValue(i, out var t)) continue;
        context.Effect.World = t.World;
        context.Effect.View = t.View;
        context.Effect.Projection = t.Projection;
      }

      var circleRenderData = EntityManager.GetReadOnlyComponentDictionary<CircleRenderData>();
      var position = EntityManager.GetReadOnlyComponentDictionary<Position>();
      var visible = EntityManager.GetReadOnlyComponentDictionary<Visible>();
      for (var i = 0; i < count; i++) {
        if (!circleRenderData.TryGetValue(i, out var crd)) continue;
        if (!position.TryGetValue(i, out var p)) continue;
        if (!visible.TryGetValue(i, out var v)) continue;

        context.ShapeBatch.DrawScalableCircle(
          new Vector2(p.X, p.Y),
          crd.Radius, crd.Color,
          crd.Fill, crd.Scale,
          context.Effect
        );
      }
    }
  }
}