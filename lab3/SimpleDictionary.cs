using System.Collections;
using System.Collections.Generic;

namespace Collections;

public class SimpleDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;
    private ChainNode?[] _buckets;
    private int _count;
    private int _version;

    private class ChainNode
    {
        public KeyValuePair<TKey, TValue> Item { get; set; }
        public ChainNode? Next { get; set; }

        public ChainNode(KeyValuePair<TKey, TValue> item)
        {
            Item = item;
        }
    }

    public SimpleDictionary()
    {
        _buckets = new ChainNode?[DefaultCapacity];
        _count = 0;
        _version = 0;
    }

    public SimpleDictionary(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));
        int initialCapacity = GetNextPrime(capacity);
        _buckets = new ChainNode?[initialCapacity];
        _count = 0;
        _version = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue? value))
                return value;
            throw new KeyNotFoundException($"Ключ '{key}' не найден в словаре.");
        }
        set
        {
            Insert(key, value, false);
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            var keys = new List<TKey>();
            foreach (var kvp in this)
            {
                keys.Add(kvp.Key);
            }
            return keys;
        }
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    public ICollection<TValue> Values
    {
        get
        {
            var values = new List<TValue>();
            foreach (var kvp in this)
            {
                values.Add(kvp.Value);
            }
            return values;
        }
    }

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    public int Count => _count;
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        if (!Insert(key, value, true))
        {
            throw new ArgumentException($"Элемент с таким ключом уже добавлен. Ключ: '{key}'");
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = null;
        }
        _count = 0;
        _version++;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (TryGetValue(item.Key, out TValue? value))
        {
            return EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Массив назначения недостаточно длинный.");
        int index = arrayIndex;
        foreach (var kvp in this)
        {
            array[index++] = kvp;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        int version = _version;
        for (int i = 0; i < _buckets.Length; i++)
        {
            var current = _buckets[i];
            while (current != null)
            {
                if (version != _version)
                    throw new InvalidOperationException("Коллекция была изменена во время перечисления.");
                yield return current.Item;
                current = current.Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        var node = _buckets[bucketIndex];
        if (node == null)
            return false;
        if (EqualityComparer<TKey>.Default.Equals(node.Item.Key, key))
        {
            _buckets[bucketIndex] = node.Next;
            _count--;
            _version++;
            return true;
        }
        var current = node;
        while (current.Next != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(current.Next.Item.Key, key))
            {
                current.Next = current.Next.Next;
                _count--;
                _version++;
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (Contains(item))
        {
            return Remove(item.Key);
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        var node = _buckets[bucketIndex];
        while (node != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(node.Item.Key, key))
            {
                value = node.Item.Value;
                return true;
            }
            node = node.Next;
        }
        value = default(TValue)!;
        return false;
    }

    private bool Insert(TKey key, TValue value, bool addOnly)
    {
        if (_count >= _buckets.Length * LoadFactor)
        {
            Resize();
        }
        return InsertWithoutResize(key, value, addOnly);
    }

    private void Resize()
    {
        var items = new List<KeyValuePair<TKey, TValue>>();
        foreach (var kvp in this)
        {
            items.Add(kvp);
        }
        int newCapacity = GetNextPrime(_buckets.Length * 2);
        _buckets = new ChainNode?[newCapacity];
        _count = 0;
        foreach (var kvp in items)
        {
            InsertWithoutResize(kvp.Key, kvp.Value, false);
        }
    }

    private bool InsertWithoutResize(TKey key, TValue value, bool addOnly)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        int bucketIndex = GetBucketIndex(key);
        var node = _buckets[bucketIndex];
        if (node == null)
        {
            _buckets[bucketIndex] = new ChainNode(new KeyValuePair<TKey, TValue>(key, value));
            _count++;
            _version++;
            return true;
        }
        if (EqualityComparer<TKey>.Default.Equals(node.Item.Key, key))
        {
            if (addOnly)
                return false;
            node.Item = new KeyValuePair<TKey, TValue>(key, value);
            _version++;
            return true;
        }
        var current = node;
        while (current.Next != null)
        {
            if (EqualityComparer<TKey>.Default.Equals(current.Next.Item.Key, key))
            {
                if (addOnly)
                    return false;
                current.Next.Item = new KeyValuePair<TKey, TValue>(key, value);
                _version++;
                return true;
            }
            current = current.Next;
        }
        current.Next = new ChainNode(new KeyValuePair<TKey, TValue>(key, value));
        _count++;
        _version++;
        return true;
    }

    private int GetBucketIndex(TKey key)
    {
        int hashCode = key.GetHashCode();
        return Math.Abs(hashCode) % _buckets.Length;
    }

    private static int GetNextPrime(int min)
    {
        if (min <= 2) return 2;
        if (min <= 3) return 3;
        if (min <= 5) return 5;
        if (min <= 7) return 7;
        if (min <= 11) return 11;
        if (min <= 13) return 13;
        if (min <= 17) return 17;
        if (min <= 19) return 19;
        if (min <= 23) return 23;
        for (int i = min | 1; i < int.MaxValue; i += 2)
        {
            if (IsPrime(i))
                return i;
        }
        return min;
    }

    private static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;
        int sqrt = (int)Math.Sqrt(n);
        for (int i = 3; i <= sqrt; i += 2)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }
}

