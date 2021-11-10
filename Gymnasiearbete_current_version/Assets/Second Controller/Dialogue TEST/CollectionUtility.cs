using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Utilities
{
    public static class CollectionUtility
    {
        public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictonary, K key, V value)
        {
            if (serializableDictonary.ContainsKey(key))
            {
                serializableDictonary[key].Add(value);
                return;
            }

            serializableDictonary.Add(key, new List<V> { value });
        }
    }
}
