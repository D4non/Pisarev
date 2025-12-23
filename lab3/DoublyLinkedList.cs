using System.Collections;

namespace Collections;

public class DoublyLinkedList : IEnumerable, ICollection, IList
{
    private Node? _head;
    private Node? _tail;
    private int _count;
    private int _version;

    private class Node
    {
        public object? Value { get; set; }
        public Node? Previous { get; set; }
        public Node? Next { get; set; }

        public Node(object? value)
        {
            Value = value;
        }
    }

    public DoublyLinkedList()
    {
        _head = null;
        _tail = null;
        _count = 0;
        _version = 0;
    }

    public int Count => _count;
    public bool IsReadOnly => false;
    public bool IsSynchronized => false;
    public object SyncRoot => this;
    public bool IsFixedSize => false;

    public object? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return GetNodeAt(index).Value;
        }
        set
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));
            GetNodeAt(index).Value = value;
            _version++;
        }
    }

    public int Add(object? value)
    {
        AddLast(value);
        return _count - 1;
    }

    public void AddLast(object? value)
    {
        var newNode = new Node(value);
        if (_tail == null)
        {
            _head = _tail = newNode;
        }
        else
        {
            newNode.Previous = _tail;
            _tail.Next = newNode;
            _tail = newNode;
        }
        _count++;
        _version++;
    }

    public void AddFirst(object? value)
    {
        var newNode = new Node(value);
        if (_head == null)
        {
            _head = _tail = newNode;
        }
        else
        {
            newNode.Next = _head;
            _head.Previous = newNode;
            _head = newNode;
        }
        _count++;
        _version++;
    }

    public void Clear()
    {
        var current = _head;
        while (current != null)
        {
            var next = current.Next;
            current.Previous = null;
            current.Next = null;
            current = next;
        }
        _head = _tail = null;
        _count = 0;
        _version++;
    }

    public bool Contains(object? value)
    {
        return IndexOf(value) >= 0;
    }

    public void CopyTo(Array array, int index)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (array.Length - index < _count)
            throw new ArgumentException("Массив назначения недостаточно длинный.");
        int arrayIndex = index;
        var current = _head;
        while (current != null)
        {
            array.SetValue(current.Value, arrayIndex++);
            current = current.Next;
        }
    }

    public IEnumerator GetEnumerator()
    {
        int version = _version;
        var current = _head;
        while (current != null)
        {
            if (version != _version)
                throw new InvalidOperationException("Коллекция была изменена во время перечисления.");
            yield return current.Value;
            current = current.Next;
        }
    }

    public int IndexOf(object? value)
    {
        int index = 0;
        var current = _head;
        while (current != null)
        {
            if (Equals(current.Value, value))
                return index;
            current = current.Next;
            index++;
        }
        return -1;
    }

    public void Insert(int index, object? value)
    {
        if (index < 0 || index > _count)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index == 0)
        {
            AddFirst(value);
            return;
        }
        if (index == _count)
        {
            AddLast(value);
            return;
        }
        var nodeAt = GetNodeAt(index);
        var newNode = new Node(value)
        {
            Previous = nodeAt.Previous,
            Next = nodeAt
        };
        if (nodeAt.Previous != null)
        {
            nodeAt.Previous.Next = newNode;
        }
        nodeAt.Previous = newNode;
        _count++;
        _version++;
    }

    public void Remove(object? value)
    {
        var current = _head;
        while (current != null)
        {
            if (Equals(current.Value, value))
            {
                RemoveNode(current);
                return;
            }
            current = current.Next;
        }
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _count)
            throw new ArgumentOutOfRangeException(nameof(index));
        var node = GetNodeAt(index);
        RemoveNode(node);
    }

    private void RemoveNode(Node node)
    {
        if (node.Previous != null)
        {
            node.Previous.Next = node.Next;
        }
        else
        {
            _head = node.Next;
        }
        if (node.Next != null)
        {
            node.Next.Previous = node.Previous;
        }
        else
        {
            _tail = node.Previous;
        }
        _count--;
        _version++;
    }

    private Node GetNodeAt(int index)
    {
        if (index < 0 || index >= _count)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (index < _count / 2)
        {
            var current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current!.Next;
            }
            return current!;
        }
        else
        {
            var current = _tail;
            for (int i = _count - 1; i > index; i--)
            {
                current = current!.Previous;
            }
            return current!;
        }
    }
}

