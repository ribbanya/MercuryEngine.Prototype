using System.Collections.Concurrent;

namespace MercuryEngine.Prototype.EntityComponentSystem {
  public class ComponentDictionary<T> : ConcurrentDictionary<int, T>, IComponentDictionary<T> where T : struct { }
}