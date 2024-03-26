using System;
using System.Runtime.CompilerServices;
using Qw1nt.SelfIds.Editor.Scripts.Interfaces;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedIdGroup : SerializedBase, ISerializedWithSource
    {
        private SerializedProperty _name;
        private SerializedProperty _id;
        
        public SerializedProperty Source => Reference;

        public string Name
        {
            get => _name.stringValue;
            set => _name.stringValue = value;
        }

        public uint Id
        {
            get => _id.uintValue;
            set => _id.uintValue = value;
        }
        
        public SerializedArray<SerializedSubgroup> Subgroups { get; private set; }

        protected override void OnSetSource(SerializedProperty property)
        {
            _name = property.FindPropertyRelative("_name");
            _id = property.FindPropertyRelative("_id");

            var subgroups = property.FindPropertyRelative("_subgroups");
            Subgroups = new SerializedArray<SerializedSubgroup>(Owner, subgroups);
        }
    }
}