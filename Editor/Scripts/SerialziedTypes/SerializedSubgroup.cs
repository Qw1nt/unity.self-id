using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedSubgroup : SerializedBase
    {
        public SerializedProperty Name { get; private set; }
        
        public SerializedProperty Id { get; private set; }
        
        public SerializedProperty Ids { get; private set; }
        
        protected override void OnSetSource(SerializedProperty property)
        {
            Name = Reference.FindPropertyRelative("_name");
            Id = Reference.FindPropertyRelative("_id");
            Ids = Reference.FindPropertyRelative("_ids");
        }
    }
}