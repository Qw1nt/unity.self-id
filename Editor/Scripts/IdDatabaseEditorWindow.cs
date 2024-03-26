using System;
using System.Collections.Generic;
using System.Linq;
using Qw1nt.SelfIds.Editor.Scripts.Controls;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts
{
    public class IdDatabaseEditorWindow : EditorWindow
    {
        public const string DatabaseAssetLabel = "IdDatabase";
        private const string WindowTreeAssetPath = "IdDatabaseWindow";

        private readonly List<SerializedProperty> _groups = new(32);
        private SerializedIdDatabase _database;

        private ListView _groupView;
        private VisualElement _contentDisplay;

        private void CreateGUI()
        {
            LoadDatabase();
            BuildView();
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

            foreach (var record in _database.Records)
                _groups.Add(record.Source);
        }

        private void BuildView()
        {
            var tree = Resources.Load<VisualTreeAsset>(WindowTreeAssetPath);
            tree.CloneTree(rootVisualElement);

            var panel = new TwoPaneSplitView(0, 250f, TwoPaneSplitViewOrientation.Horizontal);
            _groupView = new ListView
            {
                fixedItemHeight = 48
            };

            _contentDisplay = new VisualElement();

            BuildGroupPart();

            panel.Add(_groupView);
            panel.Add(_contentDisplay);

            _groupView.itemsSource = _groups;

            rootVisualElement.Q<VisualElement>("content").Add(panel);
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

        private void BindItem(IdGroupElement element, int i)
        {
            var group = new SerializedIdGroup();

            group.SetOwner(_database);
            group.SetSource(_groups[i]);

            element.Source = group;
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