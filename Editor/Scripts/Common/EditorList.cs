using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    public class EditorList<T> where T : new()
    {
        private readonly List<T> _items;

        public EditorList(int capacity = 8)
        {
            _items = new List<T>(8);
        }

        public int Count => _items.Count;

        public T this[int index] => _items[index];

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

            _items.RemoveAt(index);
        }

        public void Clear()
        {
            _items.Clear();
            _items.Capacity = 0;
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

        public static implicit operator List<T>(EditorList<T> editorList)
        {
            return editorList._items;
        }

        public struct Enumerator
        {
            public EditorList<T> list;

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