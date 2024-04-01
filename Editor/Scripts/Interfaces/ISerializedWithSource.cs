using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.Interfaces
{
    internal interface ISerializedWithSource
    {
        public SerializedProperty Source { get; }
    }
}