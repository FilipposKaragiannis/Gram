using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Extensions
{
    public static class TransformExtensions
    {
         private static bool TransformNameFound(string name, bool perfectMatch, Transform child)
        {
            return perfectMatch && child.name == name || !perfectMatch && child.name.ToLower().Contains(name.ToLower());
        }

        internal static Transform GetChild(this Transform transform, string name, bool perfectMatch = true)
        {
            foreach (Transform child in transform)
                if (TransformNameFound(name, perfectMatch, child))
                    return child;

            return null;
        }

        internal static T GetChild<T>(this Transform transform, string name, bool perfectMatch = true) where T : Instance.Instance
        {
            foreach (Transform child in transform)
                if (TransformNameFound(name, perfectMatch, child))
                    return child.gameObject.GetOrAdd<T>();

            return default(T);
        }

        internal static Transform[] GetChildren(this Transform transform)
        {
            var children = new List<Transform>(transform.childCount);

            foreach (Transform t in transform)
                children.Add(t);

            return children.ToArray();
        }

        internal static Transform[] GetChildren(this Transform transform, string name, bool perfectMatch)
        {
            return transform.GetChildren()
                            .Where(c => TransformNameFound(name, perfectMatch, c))
                            .ToArray();
        }

        internal static Transform[] GetChildren(this Transform transform, string pattern)
        {
            var regex = new Regex(pattern);
            
            return transform.GetChildren()
                            .Where(c => regex.IsMatch(c.name))
                            .ToArray();
        }

        internal static T[] GetChildren<T>(this Transform transform) where T : Instance.Instance
        {
            var children = new List<T>(transform.childCount);

            foreach (Transform t in transform)
                children.Add(t.gameObject.GetOrAdd<T>());

            return children.ToArray();
        }

        internal static Transform[] GetDescendants(this Transform transform)
        {
            var descendants = new List<Transform>();

            
            void Visit(Transform curTransform)
            {
                descendants.AddRange(curTransform.GetChildren());

                foreach (Transform child in curTransform)
                    Visit(child);
            }
            
            
            Visit(transform);

            return descendants.ToArray();
        }

        internal static Transform[] GetDescendants(this Transform transform, string name, bool perfectMatch)
        {
            var descendants = new List<Transform>();

            
            void Visit(Transform curTransform)
            {
                descendants.AddRange(curTransform.GetChildren(name, perfectMatch));

                foreach (Transform child in curTransform)
                    Visit(child);
            }
            
            
            Visit(transform);

            return descendants.ToArray();
        }

        internal static Transform[] GetDescendants(this Transform transform, string pattern)
        {
            var descendants = new List<Transform>();

            void Visit(Transform curTransform)
            {
                descendants.AddRange(curTransform.GetChildren(pattern));

                foreach (Transform child in curTransform)
                    Visit(child);
            }
            
            
            Visit(transform);

            return descendants.ToArray();
        }

        internal static T GetDescendant<T>(this Transform transform, string name, bool perfectMatch = true) where T : Instance.Instance
        {
            if (TransformNameFound(name, perfectMatch, transform))
                return transform.gameObject.GetOrAdd<T>();


            var descendant = default(T);

            foreach (Transform c in transform)
                if ((descendant = c.GetDescendant<T>(name, perfectMatch)) != null)
                    break;

            return descendant;
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
