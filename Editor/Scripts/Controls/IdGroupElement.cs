using System;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class IdGroupElement : VisualElement
    {
        public const string RootKey = "root";
        public const string NameKey = "group-name";
        public const string DeleteGroupButtonKey = "delete-button";

        private const string VisualTreePath = "IdGroupElementAsset";
        private const string UssStylePath = "Uss/" + nameof(IdGroupElement) + "Style";
        
        private readonly Label _label;
        private readonly Button _deleteGroupButton;
        
        private SerializedIdGroup _source;
        
        private Action _deleteGroupSubscribe;

        [Preserve]
        public new class UxmlFactrory : UxmlFactory<IdGroupElement>
        {
        }

        public IdGroupElement()
        {
            var root = new VisualElement() {name = RootKey};
            Resources.Load<VisualTreeAsset>(VisualTreePath).CloneTree(root);

            _label = root.Q<Label>(NameKey);
            _deleteGroupButton = root.Q<Button>(DeleteGroupButtonKey);

            hierarchy.Add(root);
        }

        public SerializedIdGroup Source
        {
            get => _source;

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _label.text = value.Name;
                _source = value;
            }
        }

        public void SubscribeOnDeleteGroup(Action action)
        {
            _deleteGroupSubscribe = action;
            _deleteGroupButton.clicked += _deleteGroupSubscribe;
        }

        public void UnSubscribe()
        {
            _deleteGroupButton.clicked -= _deleteGroupSubscribe;
        }
    }
}