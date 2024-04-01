using System.Collections.Generic;
using Qw1nt.SelfIds.Runtime;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    [CreateAssetMenu]
    public class IdsDatabase : ScriptableObject
    {
        [SerializeField] private List<IdGroup> _records;

        internal IReadOnlyList<IdGroup> Records => _records;
    }
}