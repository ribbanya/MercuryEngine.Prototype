using System.Collections.Generic;

namespace MercuryEngine.Prototype.EntityComponentSystem {
  /// <summary>
  /// A naive implementation of a job scheduler.
  /// Single-threaded, hard-coded.
  /// </summary>
  public class SystemManager {
    private readonly Queue<ISystem> updateQueue;

    public SystemManager(params ISystem[] systems) {
      this.updateQueue = new Queue<ISystem>(systems);
    }

    public void Update() {
      foreach (var system in this.updateQueue) {
        system.Update();
      }
    }
  }
}