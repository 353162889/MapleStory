using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Framework
{
    [Serializable]
    public abstract class AbstractSerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        [SerializeField]
        protected List<TKey> keys;
        [SerializeField]
        protected List<TValue> values;

        public List<TKey> ListKeys
        {
            get
            {
                return keys;
            }
        }
        public List<TValue> ListValues
        {
            get
            {
                return values;
            }
        }

        public AbstractSerializableDictionary()
            : this(10)
        {
        }

        public AbstractSerializableDictionary(int capacity)
            : this(new List<TKey>(capacity), new List<TValue>(capacity))
        {
        }

        public AbstractSerializableDictionary(List<TKey> keys, List<TValue> values)
        {
            this.keys = keys;
            this.values = values;
        }

        private bool TryGetIndex(TKey key, out int index)
        {
            return (index = keys.IndexOf(key)) != -1;
        }

        #region IDictionary implementation
        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }
        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException(string.Format("There's already a key `{0}` defined in the dictionary", key.ToString()));
            }
            if (key == null)
            {
                throw new ArgumentNullException("key can not null");
            }
            keys.Add(key);
            values.Add(value);
        }
        public bool Remove(TKey key)
        {
            int index;
            if (TryGetIndex(key, out index))
            {
                keys.RemoveAt(index);
                values.RemoveAt(index);
                return true;
            }
            return false;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index;
            if (TryGetIndex(key, out index))
            {
                value = values[index];
                return true;
            }
            value = default(TValue);
            return false;
        }
        public TValue this[TKey key]
        {
            get
            {
                int index;
                if (!TryGetIndex(key, out index))
                {
                    throw new KeyNotFoundException(key.ToString());
                }
                return values[index];
            }
            set
            {
                int index;
                if (TryGetIndex(key, out index))
                {
                    values[index] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return keys;
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                return values;
            }
        }
        #endregion
        #region ICollection implementation
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }
        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                array[i] = new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }
        public int Count
        {
            get
            {
                return keys.Count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }
        #endregion
        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

    }
}