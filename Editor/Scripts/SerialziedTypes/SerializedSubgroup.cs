using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedSubgroup : SerializedBase
    {
        private SerializedProperty _groupId;
        private SerializedProperty _name;
        private SerializedProperty _id;

        public ushort GroupId
        {
            get => (ushort) _groupId.uintValue;
            set => _groupId.uintValue = value;
        }

        public string Name
        {
            get => _name.stringValue;
            set => _name.stringValue = value;
        }

        public ushort Id
        {
            get => (ushort) _id.uintValue;
            set => _id.uintValue = value;
        }
        
        public SerializedArray<SerializedId> Ids { get; private set; }
        
        protected override void OnSetSource(SerializedProperty property)
        {
            _groupId = Reference.FindPropertyRelative("_groupId");
            _name = Reference.FindPropertyRelative("_name");
            _id = Reference.FindPropertyRelative("_id");
            Ids = new SerializedArray<SerializedId>(Owner, Reference.FindPropertyRelative("_ids"));
        }
    }
}