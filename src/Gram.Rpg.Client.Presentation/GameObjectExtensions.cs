using System.Collections.Generic;
using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation
{
    internal static class GameObjectExtensions
    {
        internal static void Destroy(this GameObject go)
        {
            if (go == null)
                return;

            Object.Destroy(go);
        }

        internal static IInstance GetInstance(this GameObject go)
        {
            return go.GetComponent<Instance.Instance>();
        }

        internal static GameObject[] GetDescendants(this GameObject go)
        {
            var children = new List<GameObject>();

            foreach (Transform t in go.transform)
            {
                children.Add(t.gameObject);

                if (t.childCount > 0)
                    children.AddRange(GetDescendants(t.gameObject));
            }

            return children.ToArray();
        }

        internal static T GetOrAdd<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();

            return component == null && !typeof(T).IsAbstract ? go.AddComponent<T>() : component;
        }


        internal static void StopDefaultAnimation(this GameObject go)
        {
            var anim = go.GetComponent<UnityEngine.Animation>();

            if (anim != null)
                anim.Stop();
        }
    }
}
