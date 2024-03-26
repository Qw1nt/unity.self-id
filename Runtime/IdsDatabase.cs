using System.Collections.Generic;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [CreateAssetMenu]
    public class IdsDatabase : ScriptableObject
    {
        [SerializeField] private List<IdGroup> _records;
    }
}