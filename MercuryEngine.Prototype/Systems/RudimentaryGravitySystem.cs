using MercuryEngine.Prototype.Components;
using MercuryEngine.Prototype.EntityComponentSystem;
using MercuryEngine.Prototype.Extensions;

namespace MercuryEngine.Prototype.Systems {
  public class RudimentaryGravitySystem : ISystem {
    public void Update() {
      var count = EntityManager.Count;
      var hasGravity = EntityManager.GetReadOnlyComponentDictionary<HasGravity>();
      var position = EntityManager.GetComponentDictionary<Position>();
      for (var i = 0; i < count; i++) {
        if (!hasGravity.ContainsKey(i)) continue;
        if (!position.TryGetValue(i, out var p)) continue;
        p.Y -= 0.5f;
        position[i] = p;
      }
    }
  }
}