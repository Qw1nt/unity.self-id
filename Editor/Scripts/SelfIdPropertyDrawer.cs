using Qw1nt.SelfIds.Editor.Scripts.Windows;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts
{
    [CustomPropertyDrawer(typeof(Id))]
    public class SelfIdPropertyDrawer : PropertyDrawer
    {
        private const float EmpiricCalculatedHeight = 9f;
        private string[] _idsPrefixes;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_id"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fullName = property.FindPropertyRelative("_editorFullName");
            var id = property.FindPropertyRelative("_id");

            var group = property.FindPropertyRelative("_group");
            var subgroup = property.FindPropertyRelative("_subgroup");
            var item = property.FindPropertyRelative("_item");

            var hash = property.FindPropertyRelative("_hash");

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, new GUIContent("ID: "));

            position.x += 35f;
            position.xMax -= 32f;
            
            if (GUI.Button(position, fullName.stringValue, EditorStyles.popup) == true)
            {
                var window = ScriptableObject.CreateInstance<SearchIdWindow>();

                window.SetSelectCallback(selectedId =>
                {
                    id.stringValue = selectedId.ToString();
                    fullName.stringValue = selectedId.EditorFullName;

                    group.ulongValue = selectedId.Group;
                    subgroup.ulongValue = selectedId.Subgroup;
                    item.ulongValue = selectedId.Subgroup;

                    hash.ulongValue = selectedId;

                    property.serializedObject.ApplyModifiedProperties();
                });

                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    window);
            }

            EditorGUI.EndProperty();
        }
    }
}