using System;
using System.Text;
using Qw1nt.SelfIds.Editor.Scripts.Base;
using Qw1nt.SelfIds.Editor.Scripts.Common;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Windows
{
    internal class EnumSourceGenerationPopup : EditorWindow<EnumSourceGeneratorPopupData>
    {
        private SerializedIdGroup _idGroup;
        private SerializedSubgroup _idSubgroup;

        private TextField _savePathView;
        private Button _selectSavePathButton;

        private TextField _namespaceField;
        
        private Button _generateButton;
        
        public override void SetData(EnumSourceGeneratorPopupData data)
        {
            _idGroup = data.Group;
            _idSubgroup = data.Subgroup;
        }

        private void CreateGUI()
        {
            Resources.Load<VisualTreeAsset>("Popups/EnumSourceGenerationPopup").CloneTree(rootVisualElement);

            _savePathView = rootVisualElement.Q<TextField>("save-path-field");
            _selectSavePathButton = rootVisualElement.Q<Button>("select-save-path-button");

            rootVisualElement.Q<Label>("group-name").text = _idGroup.Name;
            rootVisualElement.Q<Label>("subgroup-name").text = _idSubgroup.Name;

            _generateButton = rootVisualElement.Q<Button>("generate-button");
            _generateButton.SetEnabled(false);

            _namespaceField = rootVisualElement.Q<TextField>("namespace-field");
            _namespaceField.value = "SelfId.GeneratedEnums";
            
            _selectSavePathButton.clicked += OnPathSelect;
            _generateButton.clicked += Generate;
        }

        private void OnPathSelect()
        {
            var setter = new SavePathSetter();
            
            if(setter.TryExecute("Generated Enum", $"{_idGroup?.Name}.{_idSubgroup?.Name}", "cs", out var result) == false)
                return;

            _savePathView.value = result;
            _generateButton.SetEnabled(true);
        }
        
        private void Generate()
        {
            try
            {
                var sourceGenerator = new EnumSourceGenerator(_idGroup, _idSubgroup, _namespaceField.value);
                sourceGenerator.Execute(new StringBuilder(), _savePathView.value);
                EditorUtility.DisplayDialog("Уведомление", $"Файл сгенерирован, путь {_savePathView.value}", "Ок");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Ошибка", "Не удалось сгенерировать enum, см. в консоль", "Ок");
                Debug.LogError(e.ToString());
            }
            finally
            {
                Close();
            }
        }
    }

    internal struct EnumSourceGeneratorPopupData
    {
        public SerializedIdGroup Group;
        public SerializedSubgroup Subgroup;
    }
}