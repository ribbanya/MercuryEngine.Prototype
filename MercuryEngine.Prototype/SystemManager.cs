using MercuryEngine.Prototype.EntityComponentSystem;

namespace MercuryEngine.Prototype {
  /// <summary>
  /// A naive implementation of a job scheduler.
  /// Single-threaded, hard-coded.
  /// </summary>
  public class SystemManager : ISystem {
    public void Update(EntityManager context) {
      throw new System.NotImplementedException();
    }
  }
}