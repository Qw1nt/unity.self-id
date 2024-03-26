using Qw1nt.SelfIds.Editor.Scripts.Interfaces;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedIdGroup : SerializedBase, ISerializedWithSource
    {
        public SerializedProperty Source => Reference;
        
        public SerializedProperty Name { get; private set; }
        
        public SerializedProperty Id { get; private set; }
        
        public SerializedArray<SerializedSubgroup> Subgroups { get; private set; }

        protected override void OnSetSource(SerializedProperty property)
        {
            Name = property.FindPropertyRelative("_name");
            Id = property.FindPropertyRelative("_id");
            
            var subgroups = property.FindPropertyRelative("_subgroups");
            Subgroups = new SerializedArray<SerializedSubgroup>(Owner, subgroups);
        }
    }
}