using System;
using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class IdCardControl : VisualElement
    {
        private const string TreeAssetPath = "IdCard";

        private readonly VisualElement _root;
        
        private readonly TextField _name;
        private readonly Label _hashLabel;
        private readonly Button _deleteButton;

        private SerializedId _reference;
        private Action _deleteCallback;
        
        [Preserve]
        public new class UxmlFactory : UxmlFactory<IdCardControl>
        {
            
        }

        public IdCardControl()
        {
            _root = Resources.Load<VisualTreeAsset>(TreeAssetPath).CloneTree();

            _name = _root.Q<TextField>("name");
            _hashLabel = _root.Q<Label>("numeric-id");
            _deleteButton = _root.Q<Button>("delete-button");

            _name.RegisterCallback<FocusOutEvent>(OnNameChanged);
            
            hierarchy.Add(_root);
        }

        private void OnNameChanged(FocusOutEvent evt)
        {
            if (_name.value.IsValidName() == false)
            {
                _name.value = _reference.Name;
                return;
            }

            _reference.Name = _name.value;
            _reference.ApplyModifiers();
        }

        public void SubscribeOnDelete(Action action)
        {
            _deleteCallback = action;
            _deleteButton.clicked += _deleteCallback;
        }
        
        public void UnSubscribe()
        {
            _deleteButton.clicked -= _deleteCallback;
        }
        
        public void SetReference(SerializedId serializedId)
        {
            if (_reference != null)
                _name.UnregisterCallback<FocusOutEvent>(OnNameChanged);
            
            _reference = serializedId;
            _name.value = _reference.Name;
            _hashLabel.text = _reference.Hash.ToString();
        }
    }
}