﻿using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes
{
    internal class SerializedId : SerializedBase
    {
        private SerializedProperty _editorFullName;
        private SerializedProperty _name;
        private SerializedProperty _indexInSubgroup;

        private SerializedProperty _group;
        private SerializedProperty _subgroup;
        private SerializedProperty _item;
        
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

        public ushort IndexInSubgroup
        {
            get => (ushort)_indexInSubgroup.uintValue;
            set => _indexInSubgroup.uintValue = value;
        }

        public byte Group
        {
            get => (byte)_group.uintValue;
            set => _group.uintValue = value;
        }

        public byte Subgroup
        {
            get => (byte)_subgroup.uintValue;
            set => _subgroup.uintValue = value;
        }

        public ushort Item
        {
            get => (ushort)_item.uintValue;
            set => _item.uintValue = value;
        }
        
        public int Hash
        {
            get => _hash.intValue;
            set => _hash.intValue = value;
        }
        
        protected override void OnSetSource(SerializedProperty source)
        {
            _editorFullName = source.FindPropertyRelative("_editorFullName");
            _name = source.FindPropertyRelative("_id");
            _indexInSubgroup = source.FindPropertyRelative("_indexInSubgroup");

            _group = source.FindPropertyRelative("_group");
            _subgroup = source.FindPropertyRelative("_subgroup");
            _item = source.FindPropertyRelative("_item");
            
            _hash = source.FindPropertyRelative("_hash");
        }

        public static string GenerateEditorFullName(SerializedIdGroup group, SerializedSubgroup subgroup, SerializedId id)
        {
            return $"{group.Name}/{subgroup.Name}/{id.Name}";
        }    
    }
}