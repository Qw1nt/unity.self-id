using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal class SerializedId : SerializedBase
    {
        private SerializedProperty _editorFullName;
        private SerializedProperty _name;
     
        private SerializedProperty _groupId;
        private SerializedProperty _subgroupId;
        private SerializedProperty _indexInSubgroup;
        
        public string EditorFullName
        {
            get => _editorFullName.stringValue;
            set => _editorFullName.stringValue = value;
        }

        public string Name
        {
            get => _name.stringValue;
            set
            {
                if (value.IsValidName() == false)
                    return;

                _name.stringValue = value;
            }
        }

        public ushort GroupId
        {
            get => (ushort) _groupId.uintValue;
            set => _groupId.uintValue = value;
        }

        public ushort SubgroupId
        {
            get => (ushort) _subgroupId.uintValue;
            set => _subgroupId.uintValue = value;
        }

        public int IndexInSubgroup
        {
            get => _indexInSubgroup.intValue;
            set => _indexInSubgroup.intValue = value;
        }

        protected override void OnSetSource(SerializedProperty source)
        {
            _editorFullName = source.FindPropertyRelative("_fullName");
            _name = source.FindPropertyRelative("_id");

            _groupId = source.FindPropertyRelative("_groupId");
            _subgroupId = source.FindPropertyRelative("_subgroupId");
            _indexInSubgroup = source.FindPropertyRelative("_indexInSubgroup");
        }
    }
}