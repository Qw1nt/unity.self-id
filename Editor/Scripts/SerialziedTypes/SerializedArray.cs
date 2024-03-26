using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedArray<T>
        where T : SerializedBase, new()
    {
        private readonly SerializedObject _owner;
        private readonly SerializedProperty _source;
        private readonly List<T> _items;
        
        public SerializedArray(SerializedObject owner, SerializedProperty source)
        {
            if (source.isArray == false)
                throw new ArgumentException();

            _owner = owner;
            _source = source;
            _items = new List<T>(_source.arraySize);

            var arraySize = _source.arraySize;

            for (int i = 0; i < arraySize; i++)
            {
                var item = new T().SetSource(_source.GetArrayElementAtIndex(i));
                _items.Add((T) item);
            }
        }

        public int Count => _source.arraySize;

        public bool IsReadOnly => false;

        public IReadOnlyList<T> Items => _items;

        public T this[int index] => _items[index];

        public void Add(T element)
        {
            _source.arraySize += 1;
            
            _source.InsertArrayElementAtIndex(_source.arraySize - 1);
            _items.Add(element);
            
            _owner.ApplyModifiedProperties();
        }

        public bool Remove(T item)
        {
            var arraySize = _source.arraySize;
            var indexToRemove = -1;
            
            for (int i = 0; i < arraySize; i++)
            {
                if(item.Equals(_source.GetArrayElementAtIndex(i)) == false)
                    continue;

                indexToRemove = i;
                break;
            }

            if (indexToRemove == -1)
                return false;

            _source.DeleteArrayElementAtIndex(indexToRemove);
            _items.RemoveAt(indexToRemove);

            _owner.ApplyModifiedProperties();
            
            return true;
        }

        public void Clear()
        {
            _source.arraySize = 0;
            _items.Clear();

            _owner.ApplyModifiedProperties();
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

        public struct Enumerator
        {
            public SerializedArray<T> list;

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