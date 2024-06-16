using Qw1nt.SelfIds.Editor.Scripts.Common;
using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class IdsExplorerControl : VisualElement
    {
        private const string VisualTreePath = "ArrayView";

        private readonly VisualElement _root;
        private readonly Button _addNewElementButton;
        private readonly AddArrayElementControl _addElementView;
        private readonly ListView _itemsContainer;

        private SerializedIdGroup _group;
        private SerializedSubgroup _subgroup;

        [Preserve]
        public new class UxmlFactory : UxmlFactory<IdsExplorerControl>
        {
        }

        public IdsExplorerControl()
        {
            _root = Resources.Load<VisualTreeAsset>(VisualTreePath).CloneTree();

            _addNewElementButton = _root.Q<Button>("add-new-element-button");
            _addElementView = new AddArrayElementControl(_root.Q<VisualElement>("add-new-element-view"), AddNewElement,
                CancelAddOperation);

            _itemsContainer = _root.Q<ListView>("elements-container");
            _itemsContainer.selectionType = SelectionType.None;
            _itemsContainer.fixedItemHeight = 90f;

            InstallBindings();
            
            hierarchy.Add(_root);
            
            _root.Disable();
        }

        private void InstallBindings()
        {
            _addNewElementButton.clicked += OnAddNewElementButton;

            _itemsContainer.makeItem += () => new IdCardControl();
            _itemsContainer.bindItem += (element, i) => BindItem(element as IdCardControl, i);
        }

        private void OnAddNewElementButton()
        {
            _addNewElementButton.Disable();
            _itemsContainer.Disable();
            _addElementView.Enable();
        }
        
        private void BindItem(IdCardControl element, int index)
        {
            element.UnSubscribe();
            
            element.SetReference(_group, _subgroup, _subgroup.Ids[index]);
            element.SubscribeOnDelete(() =>
            {
                var id = _subgroup.Ids[index];
                
                if (EditorUtility.DisplayDialog("Удаление ID", $"Удалить ID с названием {id.Name}?", "Да",
                        "Нет") == false)
                    return;
                
                var startIndex = (ushort) id.IndexInSubgroup;

                _subgroup.Ids.RemoveAt(index);
                _itemsContainer.RefreshItems();
                    
                for (int i = index; i < _subgroup.Ids.Count; i++)
                {
                    _subgroup.Ids[i].IndexInSubgroup = startIndex;
                    startIndex++;
                }

                _itemsContainer.RefreshItems();
                
                _subgroup.ApplyModifiers();
            });
        }

        private void AddNewElement()
        {
            if (string.IsNullOrEmpty(_addElementView.Name) == true)
            {
                EditorUtility.DisplayDialog("Ошибка", "Название ID не может быть пустым", "Ок");
                return;
            }

            if (int.TryParse(_addElementView.Name[0].ToString(), out _) == true)
            {
                EditorUtility.DisplayDialog("Ошибка", "Название ID не может начинаться с цифры", "Ок");
                return;
            }
            
            var ids = _subgroup.Ids;

            foreach (var id in ids)
            {
                if (id.Name != _addElementView.Name)
                    continue;

                EditorUtility.DisplayDialog("Ошибка", $"ID с названием {id.Name} уже существует", "Ок");
                return;
            }

            var lastId = ids.Count > 0
                ? ids[^1].IndexInSubgroup
                : 0;

            ids.CreateElement(id =>
            {
                id.Name = _addElementView.Name;
                id.EditorFullName = SerializedId.GenerateEditorFullName(_group, _subgroup, id);

                id.Group = _group.Id;
                id.Subgroup = _subgroup.Id;
                id.Item = Id.GenerateFromGuid();
                
                id.Hash = Id.Build(_group.Id, _subgroup.Id, id.Item);

                id.IndexInSubgroup = lastId + 1;
                
                id.ApplyModifiers();
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

        public void SetReference(SerializedIdGroup group, SerializedSubgroup subgroup)
        {
            _group = group;
            _subgroup = subgroup;
            
            if (_subgroup == null)
            {
                _itemsContainer.itemsSource = null;
                _itemsContainer.Rebuild();
                _root.Disable();
            }
            else
            {
                _subgroup.Ids.BindView(_itemsContainer);
                _itemsContainer.Rebuild();
                _root.Enable();
            }
        }
    }
}