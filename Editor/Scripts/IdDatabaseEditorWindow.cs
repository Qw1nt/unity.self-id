using System;
using System.Collections.Generic;
using System.Linq;
using Qw1nt.SelfIds.Editor.Scripts.Controls;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Qw1nt.SelfIds.Editor.Scripts
{
    public class IdDatabaseEditorWindow : EditorWindow
    {
        public const string DatabaseAssetLabel = "IdDatabase";
        private const string WindowTreeAssetPath = "IdDatabaseWindow";

        private bool _hasListChanges;

        private SerializedIdDatabase _database;
        private TextField _groupNameInputField;

        private ListView _groupView;
        private VisualElement _contentDisplay;

        private void CreateGUI()
        {
            LoadDatabase();
            BuildView();
            InstallBindings();
        }

        private void LoadDatabase()
        {
            var guid = AssetDatabase.FindAssets("l:" + DatabaseAssetLabel).FirstOrDefault();

            if (string.IsNullOrEmpty(guid) == true)
                throw new NullReferenceException();

            var path = AssetDatabase.GUIDToAssetPath(guid);
            var database = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            var serializedObject = new SerializedObject(database);

            _database = new SerializedIdDatabase();
            _database.SetOwner(serializedObject)
                .SetSource(serializedObject.FindProperty("_records"));
        }

        private void BuildView()
        {
            var tree = Resources.Load<VisualTreeAsset>(WindowTreeAssetPath);
            tree.CloneTree(rootVisualElement);

            _groupNameInputField = rootVisualElement.Q<TextField>("group-name-input");

            var panel = new TwoPaneSplitView(0, 250f, TwoPaneSplitViewOrientation.Horizontal);

            _groupView = new ListView()
            {
                virtualizationMethod = CollectionVirtualizationMethod.FixedHeight,
                fixedItemHeight = 48
            };
            _contentDisplay = new VisualElement();

            BuildGroupPart();

            panel.Add(_groupView);
            panel.Add(_contentDisplay);

            _database.Records.BindView(_groupView);

            rootVisualElement.Q<VisualElement>("content").Add(panel);
        }

        private void InstallBindings()
        {
            rootVisualElement.Q<Button>("create-group-button").clicked += () =>
            {
                if (string.IsNullOrEmpty(_groupNameInputField.value) == true)
                {
                    EditorUtility.DisplayDialog("Ошибка", "Название группы не может быть пустым", "Ок");
                    return;
                }

                var lastId = (ushort) _database.Records.Count == 0
                    ? 0u
                    : _database.Records[^1].Id;

                _database.Records.CreateElement(item =>
                {
                    item.Id = lastId + 1u;
                    item.Name = _groupNameInputField.value;
                    item.Subgroups.Clear();
                });

                _groupNameInputField.value = string.Empty;
                _groupView.RefreshItems();
            };
        }

        private void BuildGroupPart()
        {
            _groupView.makeItem = () =>
            {
                var view = new IdGroupElement();
                return view;
            };

            _groupView.bindItem = (element, i) => BindItem(element as IdGroupElement, i);
        }

        private void BindItem(IdGroupElement element, int index)
        {
            element.UnSubscribe();
            var group = new SerializedIdGroup();

            group.SetOwner(_database);
            group.SetSource(_database.Records[index].Source);

            element.Source = group;

            element.SubscribeOnDeleteGroup(() =>
            {
                if(EditorUtility.DisplayDialog("Group removing", $"Delete group with name {group.Name}?", "Yes", "No") == false)
                    return;
                
                var startId = _database.Records[index].Id;
                
                _database.Records.RemoveAt(index);
                _groupView.RefreshItems();
                
                for (int i = index; i < _database.Records.Count; i++)
                {
                    _database.Records[i].Id = startId;
                    startId++;
                }

                _database.Owner.ApplyModifiedProperties();
            });
        }

        [MenuItem("Qw1nt/SelfId/Open Database")]
        private static void Open()
        {
            var window = GetWindow<IdDatabaseEditorWindow>();
            window.titleContent = new GUIContent("Id Database");

            window.minSize = new Vector2(150f, 350f);
        }
    }
}