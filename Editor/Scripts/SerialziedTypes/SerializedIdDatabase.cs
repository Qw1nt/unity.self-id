using Qw1nt.SelfIds.Editor.Scripts.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal class SerializedIdDatabase : SerializedBase, ISerializedWithSource
    {
        public SerializedArray<SerializedIdGroup> Records { get; private set; }
        
        public SerializedProperty Source => Reference;

        protected override void OnSetSource(SerializedProperty source)
        {
            Records = new SerializedArray<SerializedIdGroup>(Owner, source);
        }
        
        public static implicit operator SerializedObject(SerializedIdDatabase database)
        {
            return database.Owner;
        }
    }
}