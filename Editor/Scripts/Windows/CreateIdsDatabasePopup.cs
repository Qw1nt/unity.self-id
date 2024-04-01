using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Windows
{
    public class CreateIdsDatabasePopup : EditorWindow
    {
        [MenuItem("Examples/ShowPopup Example")]
        static void Init()
        {
            var window = CreateInstance<CreateIdsDatabasePopup>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
            window.ShowPopup();
        }

        void CreateGUI()
        {
            var label = new Label("This is an example of EditorWindow.ShowPopup");
            rootVisualElement.Add(label);

            var button = new Button();
            button.text = "Agree!";
            button.clicked += () => this.Close();
            rootVisualElement.Add(button);
        }
    }
}