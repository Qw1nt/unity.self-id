using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    public class PooledList<T> where T : new()
    {
        private readonly List<T> _items;
        private readonly List<T> _pooled;

        public PooledList(int capacity = 8)
        {
            _items = new List<T>(8);
            _pooled = new List<T>(8);
        }

        public int Count => _items.Count;

        public T this[int index] => _items[index];

        public T TakeFromPoolOrCreate()
        {
            T item = default;
            
            if (_pooled.Count > 0)
            {
                item = _pooled[^1];
                _pooled.RemoveAt(_pooled.Count - 1);
            }
            else
            {
                item = new T();
            }

            _items.Add(item);
            return item;
        }
        
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0)
                throw new ArgumentException();

            if (index >= _items.Count)
                throw new ArgumentException();

            var item = _items[index];
            _items.RemoveAt(index);

            _pooled.Add(item);
        }

        public void Clear()
        {
            _pooled.AddRange(_items);
            _items.Clear();
        }
        
        public Enumerator GetEnumerator()
        {
            var enumerator = new Enumerator
            {
                list = this,
                length = Count,
                current = default,
                index = 0
            };

            return enumerator;
        }

        public static implicit operator List<T>(PooledList<T> pooledList)
        {
            return pooledList._items;
        }

        public struct Enumerator
        {
            public PooledList<T> list;

            public int length;
            public T current;
            public int index;

            public bool MoveNext()
            {
                if (index >= length)
                    return false;

                current = list[index];
                index++;

                return true;
            }

            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => current;
            }
        }
    }
}