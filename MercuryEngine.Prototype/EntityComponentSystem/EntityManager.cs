using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MercuryEngine.Prototype.Extensions;

namespace MercuryEngine.Prototype.EntityComponentSystem {
  /// <summary>
  /// A naive, hard-coded implementation of an entity manager.
  /// Not robust, not thread-safe. Holds all the component dictionaries.
  /// </summary>
  public static class EntityManager {
    private static readonly IDictionary<Type, IDictionary> ComponentDictionaries;
    public static int Count { get; private set; }

    public static int GetNewEntityId() {
      return Count++;
    }

    static EntityManager() {
      ComponentDictionaries = new Dictionary<Type, IDictionary>();
      foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
        foreach (var type in asm.GetLoadableTypes()) {
          if (!Attribute.IsDefined(type, typeof(ComponentDataAttribute))) {
            continue;
          }

          var makeMe = typeof(ConcurrentDictionary<,>).MakeGenericType(typeof(int), type);
          var dictionary = Activator.CreateInstance(makeMe);
          ComponentDictionaries.Add(type, (IDictionary) dictionary);
        }
      }
    }

    private static bool ValidateType(Type type, out Exception exception) {
      if (ComponentDictionaries.ContainsKey(type)) {
        exception = null;
        return true;
      }

      exception = new InvalidOperationException($"'{type}' is not a valid component.");
      return false;
    }

    public static IDictionary<int, T> GetComponentDictionary<T>() {
      var type = typeof(T);
      if (!ValidateType(type, out var exception)) throw exception;

      return (IDictionary<int, T>) ComponentDictionaries[type];
    }

    public static IReadOnlyDictionary<int, T> GetReadOnlyComponentDictionary<T>() {
      return new ReadOnlyDictionary<int, T>(GetComponentDictionary<T>());
    }

    public static bool AttachComponent<T>(int id, T value = default) {
      var dictionary = GetComponentDictionary<T>();
      var result = !dictionary.ContainsKey(id);
      dictionary[id] = value;
      return result;
    }

    public static bool DetachComponent<T>(int id) {
      var dictionary = GetComponentDictionary<T>();
      var result = dictionary.ContainsKey(id);
      dictionary.Remove(id);
      return result;
    }

    public static bool DestroyEntity(int id) {
      var result = false;
      foreach (var dictionary in ComponentDictionaries.Values) {
        if (dictionary.Contains(id)) result = true;
        dictionary.Remove(id);
      }
      return result;
    }
  }
}