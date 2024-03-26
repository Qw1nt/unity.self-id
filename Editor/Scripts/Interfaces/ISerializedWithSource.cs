using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.Interfaces
{
    public interface ISerializedWithSource
    {
        public SerializedProperty Source { get; }
    }
}