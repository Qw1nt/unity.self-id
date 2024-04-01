using System;
using System.Collections.Generic;
using System.Linq;
using Qw1nt.SelfIds.Editor.Scripts.Controls;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Qw1nt.SelfIds.Editor.Scripts.Windows
{
    internal sealed class IdDatabaseEditorWindow : EditorWindow
    {
        public const string DatabaseAssetLabel = "IdDatabase";
        private const string WindowTreeAssetPath = "IdDatabaseWindow";

        private bool _hasListChanges;

        private SerializedIdDatabase _database;
        private TextField _groupNameInputField;

        private ListView _groupView;
        private VisualElement _contentDisplay;

        private SubgroupViewControl _subgroupContainer;
        private IdsExplorerControl _idsExplorerControl;

        private void CreateGUI()
        {
            if (TryLoadDatabase() == false)
                return;

            BuildView();
            InstallBindings();
        }

        private bool TryLoadDatabase()
        {
            var guid = AssetDatabase.FindAssets("l:" + DatabaseAssetLabel).FirstOrDefault();

            if (string.IsNullOrEmpty(guid) == true)
            {
                Close();
                return false;
            }

            var path = AssetDatabase.GUIDToAssetPath(guid);
            var database = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            var serializedObject = new SerializedObject(database);

            _database = new SerializedIdDatabase();
            _database.SetOwner(serializedObject)
                .SetSource(serializedObject.FindProperty("_records"));

            return true;
        }

        private void BuildView()
        {
            var tree = Resources.Load<VisualTreeAsset>(WindowTreeAssetPath);
            tree.CloneTree(rootVisualElement);

            _groupNameInputField = rootVisualElement.Q<TextField>("group-name-input");

            var panel = new TwoPaneSplitView(0, 250f, TwoPaneSplitViewOrientation.Horizontal);

            _groupView = new ListView
            {
                virtualizationMethod = CollectionVirtualizationMethod.FixedHeight,
                fixedItemHeight = 48
            };

            _contentDisplay = new TwoPaneSplitView(0, 250f, TwoPaneSplitViewOrientation.Horizontal);
            _subgroupContainer = new SubgroupViewControl();
            _idsExplorerControl = new IdsExplorerControl();

            _contentDisplay.Add(_subgroupContainer);
            _contentDisplay.Add(_idsExplorerControl);

            BuildGroupPart();

            panel.Add(_groupView);
            panel.Add(_contentDisplay);

            _database.Records.BindView(_groupView);
            _groupView.selectionChanged += OnSelectionChanges;

            rootVisualElement.Q<VisualElement>("content").Add(panel);
        }

        private void OnSelectionChanges(IEnumerable<object> obj)
        {
            if (obj == null || obj.Count() == 0)
            {
                _subgroupContainer.SetReference(null);
                _idsExplorerControl.SetReference(null);

                return;
            }

            var selected = obj.First();
            var group = (SerializedIdGroup) selected;

            _subgroupContainer.SetReference(group);
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

                foreach (var group in _database.Records)
                {
                    if (group.Name != _groupNameInputField.value)
                        continue;

                    EditorUtility.DisplayDialog("Ошибка", $"Группа с названием {group.Name} уже существует", "Ок");
                    return;
                }

                var lastId = _database.Records.Count == 0
                    ? (ushort) 0u
                    : _database.Records[^1].Id;

                _database.Records.CreateElement(item =>
                {
                    item.Id = (ushort) (lastId + 1);
                    item.Name = _groupNameInputField.value;
                    item.Subgroups.Clear();

                    item.ApplyModifiers();
                });

                _groupNameInputField.value = string.Empty;
                _groupView.RefreshItems();
            };

            _subgroupContainer.SelectedSubgroup += OnSelectedSubgroup;
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
                if (EditorUtility.DisplayDialog("Удаление группы", $"Удалить группу с названием {group.Name}?", "Да",
                        "Нет") == false)
                    return;

                var startId = _database.Records[index].Id;

                _database.Records.RemoveAt(index);
                _groupView.RefreshItems();

                if (_groupView.selectedIndex == index)
                    _groupView.selectedIndex = -1;

                for (int i = index; i < _database.Records.Count; i++)
                {
                    _database.Records[i].Id = startId;
                    startId++;
                }

                _database.Owner.ApplyModifiedProperties();
            });
        }

        private void OnSelectedSubgroup(SerializedSubgroup subgroup)
        {
            _idsExplorerControl.SetReference(subgroup);
        }

        [MenuItem("Qw1nt/SelfId/Open Database")]
        private static void Open()
        {
            var window = GetWindow<IdDatabaseEditorWindow>();
            window.titleContent = new GUIContent("Id Database");

            window.minSize = new Vector2(150f, 350f);
        }

        [MenuItem("Qw1nt/SelfId/Create Database")]
        public static void Create()
        {
            var path = "Assets/New Ids Database1.asset";
            AssetDatabase.CreateAsset(CreateInstance<IdsDatabase>(), path);
            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath<IdsDatabase>(path);
            
            AssetDatabase.SetLabels(asset, new []{DatabaseAssetLabel});
        }
    }
}