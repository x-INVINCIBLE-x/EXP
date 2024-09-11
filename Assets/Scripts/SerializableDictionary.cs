using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public Dictionary<TKey, TValue> GetDictionary()
    {
        return _dictionary;
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void Clear()
    {
        _dictionary.Clear();
        keys.Clear();
        values.Clear();
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in _dictionary)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        _dictionary.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("Serialization Error: keys and values count do not match. Deserialization aborted.");
            return;
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (!_dictionary.ContainsKey(keys[i]))
            {
                _dictionary.Add(keys[i], values[i]);
            }
            else
            {
                Debug.LogWarning($"Duplicate key found during deserialization: {keys[i]}. Skipping entry.");
            }
        }
    }
}
