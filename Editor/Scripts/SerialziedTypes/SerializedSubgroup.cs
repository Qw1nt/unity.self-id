using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal sealed class SerializedSubgroup : SerializedBase
    {
        private SerializedProperty _groupId;
        private SerializedProperty _name;
        private SerializedProperty _id;

        public ulong GroupId
        {
            get => _groupId.ulongValue;
            set => _groupId.ulongValue = value;
        }

        public string Name
        {
            get => _name.stringValue;
            set => _name.stringValue = value;
        }

        public ulong Id
        {
            get =>  _id.ulongValue;
            set => _id.ulongValue = value;
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