using System.Collections.Generic;

namespace MercuryEngine.Prototype.Extensions {
  // ReSharper disable once UnusedMember.Global
  public static class DeconstructorExtensions {
    public static void
      Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> @this, out TKey key, out TValue value) {
      key = @this.Key;
      value = @this.Value;
    }
  }
}