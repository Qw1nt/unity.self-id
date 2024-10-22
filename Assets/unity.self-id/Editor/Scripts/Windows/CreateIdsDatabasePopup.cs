using System;
using Qw1nt.SelfIds.Editor.Scripts.Common;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Windows
{
    public class CreateIdsDatabasePopup : EditorWindow
    {
        private TextField _savePath;
        private Button _setSavePathButton;
        private Button _createButton;

        private void CreateGUI()
        {
            Resources.Load<VisualTreeAsset>("Popups/CreateIdsDatabasePopup").CloneTree(rootVisualElement);

            _savePath = rootVisualElement.Q<TextField>("save-path");
            _setSavePathButton = rootVisualElement.Q<Button>("set-save-path-button");
            _createButton = rootVisualElement.Q<Button>("create-button");

            _setSavePathButton.clicked += SetSavePath;
            _createButton.clicked += CreateData;
        }

        private void CreateData()
        {
            if (string.IsNullOrEmpty(_savePath.value) == true)
            {
                EditorUtility.DisplayDialog("Ошибка", "Не указан путь сохранения", "Ок");
                return;
            }

            AssetDatabase.CreateAsset(CreateInstance<IdsDatabase>(), _savePath.value);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath<IdsDatabase>(_savePath.value);

            AssetDatabase.SetLabels(asset, new[] {IdDatabaseEditorWindow.DatabaseAssetLabel});
            AssetDatabase.Refresh();

            Close();
        }

        private void SetSavePath()
        {
            var setter = new SavePathSetter();
            
            if (setter.TryExecute("Save Id Database", "IdDatabase", "asset", out var path) == false)
                return;

            if (path.Contains("Editor") == false)
            {
                EditorUtility.DisplayDialog("Ошибка", "Сохранение должно происходить в папку \"Editor\"", "Ок");
                return;
            }

            _savePath.value = path;
        }

        private void OnDestroy()
        {
            _setSavePathButton.clicked -= SetSavePath;
            _createButton.clicked -= CreateData;
        }
    }
}