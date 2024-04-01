using System;
using System.Collections.Generic;
using System.Linq;
using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class SubgroupViewControl : VisualElement
    {
        private const string VisualTreePath = "ArrayView";

        private readonly VisualElement _root;
        private readonly Button _addNewElementButton;
        private readonly AddArrayElementControl _addElementView;
        private readonly ListView _itemsContainer;

        private SerializedIdGroup _reference;

        public event Action<SerializedSubgroup> SelectedSubgroup;

        [Preserve]
        public new class UxmlFactrory : UxmlFactory<SubgroupViewControl>
        {
        }

        public SubgroupViewControl()
        {
            _root = new VisualElement();
            Resources.Load<VisualTreeAsset>(VisualTreePath).CloneTree(_root);

            _addNewElementButton = _root.Q<Button>("add-new-element-button");
            _addElementView = new AddArrayElementControl(_root.Q<VisualElement>("add-new-element-view"), AddNewElement,
                CancelAddOperation);

            _itemsContainer = _root.Q<ListView>("elements-container");
            _itemsContainer.fixedItemHeight = 110f;
            _root.style.marginRight = 8f;

            InstallBindings();

            hierarchy.Add(_root);
            _root.Disable();
        }

        private void InstallBindings()
        {
            _addNewElementButton.clicked += OnAddNewElementButton;

            _itemsContainer.makeItem += () => new SubgroupInListViewControl();
            _itemsContainer.bindItem += (element, i) => BindItem(element as SubgroupInListViewControl, i);

            _itemsContainer.selectionChanged += OnItemSelected;
        }

        private void OnItemSelected(IEnumerable<object> obj)
        {
            SelectedSubgroup?.Invoke(obj.FirstOrDefault() as SerializedSubgroup);
        }

        private void BindItem(SubgroupInListViewControl viewControl, int index)
        {
            viewControl.SetReference(_reference.Subgroups[index]);
            viewControl.SubscribeOnDelete(() =>
            {
                var subgroup = _reference.Subgroups[index];
                
                if (EditorUtility.DisplayDialog("Удаление подгруппы", $"Удалить подгруппу с названием {_reference.Subgroups[index].Name}?", "Да",
                        "Нет") == false)
                    return;

                var startId = subgroup.Id;

                _reference.Subgroups.RemoveAt(index);

                if (_itemsContainer.selectedIndex == index)
                    _itemsContainer.selectedIndex = -1;

                for (int i = index; i < _reference.Subgroups.Count; i++)
                {
                    _reference.Subgroups[i].Id = startId;
                    startId++;
                }

                _reference.Owner.ApplyModifiedProperties();
                _itemsContainer.RefreshItems();
            });
        }

        private void OnAddNewElementButton()
        {
            _addNewElementButton.Disable();
            _itemsContainer.Disable();
            _addElementView.Enable();
        }

        private void AddNewElement()
        {
            if (string.IsNullOrEmpty(_addElementView.Name) == true)
            {
                EditorUtility.DisplayDialog("Ошибка", "Название подгруппы не может быть пустым", "Ок");
                return;
            }

            var subgroups = _reference.Subgroups;

            foreach (var subgroup in subgroups)
            {
                if (subgroup.Name != _addElementView.Name)
                    continue;

                EditorUtility.DisplayDialog("Ошибка", $"Подгруппа с названием {subgroup.Name} уже существует", "Ок");
                return;
            }

            var lastId = subgroups.Count > 0
                ? subgroups[^1].Id
                : (ushort) 0;

            subgroups.CreateElement(subgroup =>
            {
                subgroup.GroupId = _reference.Id;
                subgroup.Name = _addElementView.Name;
                subgroup.Id = (ushort) (lastId + 1);
                subgroup.Ids.Clear();
                
                subgroup.ApplyModifiers();
            });

            _itemsContainer.RefreshItems();
            _addElementView.Name = string.Empty;
        }

        private void CancelAddOperation()
        {
            _addNewElementButton.Enable();
            _itemsContainer.Enable();
            _addElementView.Disable();
        }

        public void SetReference(SerializedIdGroup group)
        {
            if (_reference != null && group == null)
                _itemsContainer.selectedIndex = -1;

            if (_reference != null && group != null && _reference.Id != group.Id)
                _itemsContainer.selectedIndex = -1;

            _reference = group;

            if (_reference == null)
            {
                _itemsContainer.itemsSource = null;
                _itemsContainer.Rebuild();
                _root.Disable();
            }
            else
            {
                _reference.Subgroups.BindView(_itemsContainer);
                _itemsContainer.Rebuild();
                _root.Enable();
            }
        }
    }
}