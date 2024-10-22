using UnityEngine.UIElements;

namespace Qw1nt.SelfIds.Editor.Scripts.Extensions
{
    internal static class VisualElementsExtensions
    {
        public static void Enable(this VisualElement visualElement)
        {
            visualElement.style.display = DisplayStyle.Flex;
        }
        
        public static void Disable(this VisualElement visualElement)
        {
            visualElement.style.display = DisplayStyle.None;
        }
    }
}