using System.Collections.Generic;
using UnityEngine;

namespace MISC
{
    public static class TransformExtensions
    {
        public static void ClearChildren(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                Object.Destroy(child);
            }
        }
        
        public static IEnumerable<GameObject> FindObjectsWithTag(this Transform parent, string tag)
        {
            var taggedGameObjects = new List<GameObject>();
 
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                
                if (child.CompareTag(tag))
                    taggedGameObjects.Add(child.gameObject);

                if (child.childCount > 0)
                    taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
            return taggedGameObjects;
        }
    }
}