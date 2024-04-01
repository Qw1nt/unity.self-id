using System;
using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class SubgroupInListViewControl : VisualElement, IDisposable
    {
        private const string VisualTreePath = "SubgroupListViewCard";

        private readonly TextField _nameField;
        private readonly Label _idsCountLabel;
        private readonly Button _deleteButton;

        private SerializedSubgroup _subgroup;

        private Action _deleteCallback;
        
        [Preserve]
        public new class UxmlFactory : UxmlFactory<SubgroupInListViewControl>
        {
        }

        public SubgroupInListViewControl()
        {
            var root = new VisualElement();
            Resources.Load<VisualTreeAsset>(VisualTreePath).CloneTree(root);

            _nameField = root.Q<TextField>("subgroup-name");
            _nameField.RegisterCallback<FocusOutEvent>(OnNameChanged);

            _idsCountLabel = root.Q<Label>("ids-count-label");
            _deleteButton = root.Q<Button>("delete-button");
            _deleteButton.clicked += () => _deleteCallback?.Invoke();
            
            hierarchy.Add(root);
        }

        private void OnNameChanged(FocusOutEvent evt)
        {
            if (_nameField.value.IsValidName() == false)
            {
                _nameField.value = _subgroup.Name;
                return;
            }

            _subgroup.Name = _nameField.value;
            _subgroup.ApplyModifiers();
        }

        public void SetReference(SerializedSubgroup subgroup)
        {
            if (_subgroup != null)
            {
                _subgroup.Ids.Changed -= OnIdsChanged;
                _nameField.UnregisterCallback<FocusOutEvent>(OnNameChanged);
            }

            _subgroup = subgroup;
            _nameField.value = _subgroup.Name;
            _idsCountLabel.text = _subgroup.Ids.Count.ToString();

            subgroup.Ids.Changed += OnIdsChanged;
        }

        public void SubscribeOnDelete(Action action)
        {
            _deleteCallback = action;
        }

        private void OnIdsChanged()
        {
            _idsCountLabel.text = _subgroup.Ids.Count.ToString();
        }

        public void Dispose()
        {
            _nameField.UnregisterCallback<FocusOutEvent>(OnNameChanged);
            _deleteButton.clicked -= _deleteCallback;

            if (_subgroup != null)
                _subgroup.Ids.Changed -= OnIdsChanged;
        }
    }
}