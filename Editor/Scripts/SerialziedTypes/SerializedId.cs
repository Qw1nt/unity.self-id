using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal class SerializedId : SerializedBase
    {
        private SerializedProperty _editorFullName;
        private SerializedProperty _name;
        private SerializedProperty _indexInSubgroup;
        private SerializedProperty _hash;

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
                if(value.IsValidName() == false)
                    return;

                _name.stringValue = value;
            }
        }

        public uint IndexInSubgroup
        {
            get => _indexInSubgroup.uintValue;
            set => _indexInSubgroup.uintValue = value;
        }
        
        public uint Hash
        {
            get => _hash.uintValue;
            set => _hash.uintValue = value;
        }
        
        protected override void OnSetSource(SerializedProperty source)
        {
            _editorFullName = source.FindPropertyRelative("_fullName");
            _name = source.FindPropertyRelative("_id");
            _indexInSubgroup = source.FindPropertyRelative("_indexInSubgroup");
            _hash = source.FindPropertyRelative("_hash");
        }
    }
}