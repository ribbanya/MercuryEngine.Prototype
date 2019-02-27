using System.Linq;
using MercuryEngine.Prototype.Components;
using MercuryEngine.Prototype.EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MercuryEngine.Prototype.Systems {
  public class CameraTransformationSystem : ISystem {
    public static readonly Matrix HalfPixelOffset =
      Matrix.CreateTranslation(-0.5f, -0.5f, 0);

    public void Update() {
      var platformInfo = EntityManager.GetReadOnlyComponentDictionary<PlatformInfo>().Values.First();
      var viewport = platformInfo.Viewport;
      var projection = HalfPixelOffset
                       * Matrix.CreateOrthographicOffCenter(
                         0, viewport.Width,
                         0, viewport.Height,
                         viewport.MinDepth, viewport.MaxDepth
                       );

      var count = EntityManager.Count;
      var position = EntityManager.GetReadOnlyComponentDictionary<Position>();
      var cameraData = EntityManager.GetReadOnlyComponentDictionary<CameraData>();
      var transformations = EntityManager.GetComponentDictionary<Transformations>();

      for (var i = 0; i < count; i++) {
        if (!cameraData.TryGetValue(i, out var cd)) continue;
        if (!position.TryGetValue(i, out var p)) continue;

        var pVector2 = new Vector2(p.X, p.Y);

        transformations[i] = new Transformations {
          World = Matrix.Identity,
          View = Matrix.CreateTranslation(new Vector3(-(pVector2 - cd.Origin), 0.0f))
                 * Matrix.CreateTranslation(new Vector3(-cd.Origin, 0.0f))
                 * Matrix.CreateRotationZ(cd.Rotation)
                 * Matrix.CreateScale(new Vector3(cd.Scale, 1))
                 * Matrix.CreateTranslation(new Vector3(cd.Origin, 0.0f)),
          Projection = projection
        };

        // Break after first camera is found
        break;
      }
    }
  }
}