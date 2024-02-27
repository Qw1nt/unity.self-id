using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.Editor.SelfIds
{
    [CustomPropertyDrawer(typeof(Id))]
    public class SelfIdPropertyDrawer : PropertyDrawer
    {
        private const float EmpiricCalculatedHeight = 125f;
        private string[] _idsPrefixes;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var offset = property.FindPropertyRelative("_usePrefix").boolValue ? 0f : 20f;
            return EmpiricCalculatedHeight - offset;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var id = property.FindPropertyRelative("_id");
            var hash = property.FindPropertyRelative("_hash");
            var usePrefix = property.FindPropertyRelative("_usePrefix");
            var prefix = property.FindPropertyRelative("_prefix");
            var content = property.FindPropertyRelative("_content");

            var drawPrefix = usePrefix.boolValue;

            EditorGUI.BeginProperty(position, label, property);

            var idLabelRect = new Rect(position.x, position.y + 7.5f, position.width, GetPropertyHeight(id, 10f));
            var usePrefixRect = CalculateRect(position, idLabelRect, usePrefix, 55f);
            var prefixRect = CalculateRect(position, usePrefixRect, prefix);
            var contentRect = CalculateRect(position, drawPrefix == true ? prefixRect : usePrefixRect, content);

            DrawIdPart(idLabelRect, id, hash);
            DrawUsePrefixProperty(usePrefixRect, usePrefix);

            if (drawPrefix == true)
                DrawPrefixPart(prefixRect, prefix);

            DrawContentPart(contentRect, content);

            id.stringValue = drawPrefix == true 
                ? $"{prefix.stringValue}:{content.stringValue}" 
                : content.stringValue;

            hash.intValue = id.stringValue.GetHashCode() ^ id.stringValue.Length;
            
            EditorGUI.EndProperty();
        }

        private Rect CalculateRect(Rect position, Rect lastProperty, SerializedProperty property, float offset = 20f)
        {
            return new Rect(position.x, lastProperty.y + offset, position.width, GetPropertyHeight(property));
        }

        private int GetPrefixIndex(string prefix)
        {
            for (int i = 0; i < IdsPrefixes.PrefixesArray.Length; i++)
            {
                if (IdsPrefixes.PrefixesArray[i] == prefix)
                    return i;
            }

            return -1;
        }

        private void DrawIdPart(Rect position, SerializedProperty id, SerializedProperty hash)
        {
            var style = new GUIStyle("Box")
            {
                padding = new RectOffset(0, 0, 5, 5),
                fontSize = 13,
                fontStyle = FontStyle.Normal,
                normal =
                {
                    textColor = new Color(0.85f, 0.85f, 0.85f)
                }
            };

            EditorGUI.LabelField(position, id?.stringValue, style);

            position.y += 25f;
            EditorGUI.LabelField(position, hash.intValue.ToString(), style);

            if (EditorGUI.LinkButton(position, "") == false)
                return;

            GUIUtility.systemCopyBuffer = id?.stringValue;
            EditorWindow.focusedWindow.ShowNotification(new GUIContent("Copied to clipboard"), 0.35f);
        }

        private void DrawHashProperty(Rect position, SerializedProperty property)
        {
            EditorGUI.LabelField(position, property.intValue.ToString());
        }
        
        private void DrawUsePrefixProperty(Rect position, SerializedProperty property)
        {
            var value = EditorGUI.Toggle(position, "Use Prefix:", property.boolValue);

            if (value != property.boolValue)
                property.boolValue = value;
        }

        private void DrawPrefixPart(Rect position, SerializedProperty prefix)
        {
            var oldIndex = GetPrefixIndex(prefix.stringValue);
            var newIndex = EditorGUI.Popup(position, "Prefix", GetPrefixIndex(prefix.stringValue),
                IdsPrefixes.PrefixesArray);

            if (oldIndex == newIndex)
                return;

            prefix.stringValue = IdsPrefixes.PrefixesArray[newIndex];
        }

        private void DrawContentPart(Rect position, SerializedProperty content)
        {
            EditorGUI.PropertyField(position, content, new GUIContent("Content"));
        }

        private float GetPropertyHeight(SerializedProperty property, float offset = 0f)
        {
            if (property == null)
                return 20f + offset;

            return EditorGUI.GetPropertyHeight(property) + offset;
        }
    }
}