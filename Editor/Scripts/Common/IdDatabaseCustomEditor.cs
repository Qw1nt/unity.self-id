using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    [CustomEditor(typeof(IdsDatabase))]
    public class IdDatabaseCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            return;
            
            if(GUILayout.Button("Generate") == false)
                return;
            
            Undo.RecordObject(target, "IdsDatabase");

            var records = serializedObject.FindProperty("_records");

            for (int i = 0; i < records.arraySize; i++)
            {
                var group = records.GetArrayElementAtIndex(i);
                var groupId = group.FindPropertyRelative("_id").uintValue;
                var groupName = group.FindPropertyRelative("_name").stringValue;
                var subgroups = group.FindPropertyRelative("_subgroups");

                for (int j = 0; j < subgroups.arraySize; j++)
                {
                    var subgroup = subgroups.GetArrayElementAtIndex(j);
                    var subgroupId = subgroup.FindPropertyRelative("_id").uintValue;
                    var subgroupName = subgroup.FindPropertyRelative("_name").stringValue;
                    var ids = subgroup.FindPropertyRelative("_ids");

                    for (int k = 0; k < ids.arraySize; k++)
                    {
                        var id = ids.GetArrayElementAtIndex(k);
                        id.FindPropertyRelative("_group").uintValue = groupId;
                        id.FindPropertyRelative("_subgroup").uintValue = subgroupId;
                        var item = (uint) k + 1;

                        id.FindPropertyRelative("_item").uintValue = item;
                        id.FindPropertyRelative("_hash").ulongValue = Id.Build((ushort)groupId, (ushort)subgroupId, item);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}