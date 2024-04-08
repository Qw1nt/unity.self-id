using Qw1nt.SelfIds.Editor.Scripts.Base;
using Qw1nt.SelfIds.Editor.Scripts.Windows;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Utils
{
    internal static class ModalUtils
    {
        public static void Open<T>(string title)
            where T : EditorWindow
        {
            var window = ScriptableObject.CreateInstance<T>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 100);
            window.titleContent = new GUIContent(title);

            window.ShowModal();
        }
        
        public static void Open<TWindow, TData>(string title, TData data)
            where TWindow : EditorWindow<TData>
        {
            var window = ScriptableObject.CreateInstance<TWindow>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 100);
            window.titleContent = new GUIContent(title);

            window.SetData(data);
            window.ShowModal();
        }
    }
}