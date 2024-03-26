using System;
using JetBrains.Annotations;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using Qw1nt.SelfIds.Runtime;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Controls
{
    internal class IdGroupElement : VisualElement
    {
        public const string RootKey = "root";
        public const string NameKey = "group-name";

        private const string UssStylePath = "Uss/" + nameof(IdGroupElement) + "Style";

        private readonly Label _label;

        private SerializedIdGroup _source;

        [Preserve]
        public new class UxmlFactrory : UxmlFactory<IdGroupElement>
        {
        }

        public IdGroupElement()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(UssStylePath));

            var root = new VisualElement() {name = RootKey};
            _label = new Label {name = NameKey};

            root.Add(_label);

            hierarchy.Add(root);
        }

        public SerializedIdGroup Source
        {
            get => _source;

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _label.text = value.Name.stringValue;
                _source = value;
            }
        }
        
    }
}