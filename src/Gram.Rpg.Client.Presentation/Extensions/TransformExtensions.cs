using System.Collections.Generic;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Extensions
{
    public static class TransformExtensions
    {
        internal static T[] GetChildren<T>(this Transform transform) where T : Instance.Instance
        {
            var children = new List<T>(transform.childCount);

            foreach (Transform t in transform)
                children.Add(t.gameObject.GetOrAdd<T>());

            return children.ToArray();
        }

        internal static T[] GetDescendants<T>(this Transform transform) where T : Instance.Instance
        {
            var descendants = new List<T>();

            
            void Visit(Transform curTransform)
            {
                descendants.AddRange(curTransform.GetChildren<T>());

                foreach (Transform child in curTransform)
                    Visit(child);
            }
            
            
            Visit(transform);

            return descendants.ToArray();
        }
    }
}
