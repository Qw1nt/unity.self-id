using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Qw1nt.SelfIds.Editor.Scripts.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedArray<T>
        where T : SerializedBase, new()
    {
        private readonly SerializedObject _owner;
        private readonly SerializedProperty _source;
        private readonly PooledList<T> _items;

        public SerializedArray(SerializedObject owner, SerializedProperty source)
        {
            if (source.isArray == false)
                throw new ArgumentException();

            _owner = owner;
            _source = source;
            _items = new PooledList<T>(_source.arraySize);

            var arraySize = _source.arraySize;

            for (int i = 0; i < arraySize; i++)
            {
                var item = new T().SetSource(_source.GetArrayElementAtIndex(i));
                _items.Add((T) item);
            }
        }

        public int Count => _source.arraySize;

        public bool IsReadOnly => false;

        public T this[int index] => _items[index];

        public void BindView(ListView view)
        {
            view.itemsSource = (List<T>) _items;
        }

        public ValueTuple<SerializedProperty, T> CreateElement(Action<T> onCreate = null, Action onComplete = null)
        {
            _source.arraySize += 1;
            var item = new T();

            _items.Add(item);

            var itemSource = _source.GetArrayElementAtIndex(_source.arraySize - 1);

            item.SetOwner(_owner)
                .SetSource(itemSource);

            onCreate?.Invoke(item);
            onComplete?.Invoke();

            return new ValueTuple<SerializedProperty, T>(itemSource, _items[^1]);
        }

        public void Add(T element)
        {
            _source.arraySize += 1;
        }

        public bool Remove(T item)
        {
            var arraySize = _source.arraySize;
            var indexToRemove = -1;

            for (int i = 0; i < arraySize; i++)
            {
                if (item.Equals(_source.GetArrayElementAtIndex(i)) == false)
                    continue;

                indexToRemove = i;
                break;
            }

            if (indexToRemove == -1)
                return false;

            RemoveAt(indexToRemove);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index > Count)
                throw new ArgumentException();

            if (index < 0)
                throw new ArgumentException();

            _items.Clear();

            _source.DeleteArrayElementAtIndex(index);
            _owner.ApplyModifiedProperties();

            for (int i = 0; i < _source.arraySize; i++)
            {
                _items.TakeFromPoolOrCreate().SetOwner(_owner)
                    .SetSource(_source.GetArrayElementAtIndex(i));
            }
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