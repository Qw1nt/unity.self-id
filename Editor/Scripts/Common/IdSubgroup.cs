using System;
using System.Collections.Generic;
using Qw1nt.SelfIds.Runtime;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    [Serializable]
    public class IdSubgroup
    {
        [SerializeField] private uint _groupId;
        [SerializeField] private string _name;
        [SerializeField] private uint _id;
        [SerializeField] private Id[] _ids;

        internal string Name => _name;

        internal IReadOnlyList<Id> Ids => _ids;
    }
}