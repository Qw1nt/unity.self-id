using System;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal abstract class SerializedBase : IEquatable<SerializedProperty>
    {
        public SerializedObject Owner { get; private set; }
     
        protected SerializedProperty Reference { get; private set; }

        public SerializedBase SetOwner(SerializedObject owner)
        {
            Owner = owner;
            return this;
        }

        public SerializedBase SetSelf(SerializedProperty self)
        {
            Reference = self;
            OnSetSource(Reference);

            return this;
        }

        public void ApplyModifiers()
        {
            Owner.ApplyModifiedProperties();
        }
        
        protected abstract void OnSetSource(SerializedProperty source);
        
        public bool Equals(SerializedProperty other)
        {
            return Reference == other;
        }
    }
}