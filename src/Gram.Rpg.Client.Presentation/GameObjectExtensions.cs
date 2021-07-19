using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation
{
    internal static class GameObjectExtensions
    {
        internal static IInstance GetInstance(this GameObject go)
        {
            return go.GetComponent<Instance.Instance>();
        }

        internal static T GetOrAdd<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();

            return component == null && !typeof(T).IsAbstract ? go.AddComponent<T>() : component;
        }
    }
}
