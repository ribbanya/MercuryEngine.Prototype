using System.Collections.Generic;

namespace MercuryEngine.Prototype.EntityComponentSystem {
  public interface IComponentDictionary<T> : IDictionary<int, T> where T : struct { }
}