using System;
using Qw1nt.SelfIds.Editor.Scripts.Extensions;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class AddArrayElementControl : IDisposable
    {
        private readonly VisualElement _root;
        private readonly TextField _nameField;
        private readonly Button _createButton;
        private readonly Button _cancelButton;

        private readonly Action _onCreate;
        private readonly Action _onCancel;

        public AddArrayElementControl(VisualElement rootView, Action onCreate, Action onCancel)
        {
            _root = rootView;
            _nameField = _root.Q<TextField>("element-name-field");
            _createButton = _root.Q<Button>("create-element-button");
            _cancelButton = _root.Q<Button>("cancel-create-button");

            _onCreate = onCreate;

            _onCancel = onCancel;

            _createButton.clicked += _onCreate;
            _cancelButton.clicked += _onCancel;

            Disable();
        }

        public string Name
        {
            get => _nameField.value;
            set => _nameField.value = value;
        }

        public void Enable()
        {
            _root.Enable();
        }

        public void Disable()
        {
            _root.Disable();
        }

        public void Dispose()
        {
            _createButton.clicked -= _onCreate;
            _cancelButton.clicked -= _onCancel;
        }
    }
}