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
            return EmpiricCalculatedHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fullName = property.FindPropertyRelative("_fullName");
            var id = property.FindPropertyRelative("_id");
            var hash = property.FindPropertyRelative("_hash");

            EditorGUI.BeginProperty(position, label, property);

            GUILayout.BeginHorizontal();
            GUILayout.Label("ID: ");
            
            if (GUILayout.Button(fullName.stringValue, EditorStyles.popup) == true)
            {
                var window = ScriptableObject.CreateInstance<SearchIdWindow>();
                
                window.SetSelectCallback(selectedId =>
                {
                    id.stringValue = selectedId.ToString();
                    fullName.stringValue = selectedId.FullName;
                    hash.uintValue = selectedId;

                    property.serializedObject.ApplyModifiedProperties();
                });

                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), window);
            }
            
            GUILayout.EndHorizontal();

            EditorGUI.EndProperty();
        }
    }
}