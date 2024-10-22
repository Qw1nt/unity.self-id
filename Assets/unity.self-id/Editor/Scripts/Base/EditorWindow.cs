using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.Base
{
    internal abstract class EditorWindow<TData> : EditorWindow
    {
        public abstract void SetData(TData data);
    }
}