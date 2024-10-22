using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Qw1nt.SelfIds.Editor.Scripts.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedArray<T>
        where T : SerializedBase, new()
    {
        private readonly SerializedObject _serializedObject;
        private readonly SerializedProperty _source;
        private readonly EditorList<T> _items;
        private ListView _view;

        public event Action Changed;
        
        public SerializedArray(SerializedObject serializedObject, SerializedProperty source)
        {
            if (source.isArray == false)
                throw new ArgumentException();

            _serializedObject = serializedObject;
            
            _source = source;
            _items = new EditorList<T>(_source.arraySize);

            var arraySize = _source.arraySize;

            for (int i = 0; i < arraySize; i++)
            {
                var item = new T();
                
                item.SetOwner(_serializedObject)
                    .SetSelf(_source.GetArrayElementAtIndex(i));
                
                _items.Add(item);
            }
        }

        public int Count => _source.arraySize;

        public bool IsReadOnly => false;

        public T this[int index] => _items[index];

        public void BindView(ListView view)
        {
            _view = view;
            view.itemsSource = (List<T>) _items;
            // _view.itemsSource = new List<T>();
        }

        public ValueTuple<SerializedProperty, T> CreateElement(Action<T> onCreate = null, Action onComplete = null)
        {
            _source.arraySize += 1;
            
            var item = new T();
            var itemSource = _source.GetArrayElementAtIndex(_source.arraySize - 1);

            item.SetOwner(_serializedObject)
                .SetSelf(itemSource);

            _items.Add(item);

            onCreate?.Invoke(item);
            onComplete?.Invoke();

            Changed?.Invoke();
            return new ValueTuple<SerializedProperty, T>(itemSource, item);
        }

        public void Add(T element)
        {
            _source.arraySize += 1;
            _items.Add(element);
            
            Changed?.Invoke();
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
            _view.RefreshItems();
            
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index > Count)
                throw new ArgumentException();

            if (index < 0)
                throw new ArgumentException();

            _view.itemsSource = null;
            
            _items.Clear();

            _source.DeleteArrayElementAtIndex(index);
            _serializedObject.ApplyModifiedProperties();

            for (int i = 0; i < _source.arraySize; i++)
            {
                var item = new T();
                item.SetOwner(_serializedObject).SetSelf(_source.GetArrayElementAtIndex(i));
                _items.Add(item);
            }

            _view.itemsSource = (List<T>) _items;
            _view.RefreshItems();
            
            Changed?.Invoke();
        }

        public void Clear()
        {
            _source.arraySize = 0;
            _items.Clear();
            
            Changed?.Invoke();
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