using System;
using System.Collections.Generic;
using Qw1nt.SelfIds.Runtime;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    [Serializable]
    public class IdGroup
    {
        [SerializeField] private string _name;
        [SerializeField] private ushort _id;

        [SerializeField] private IdSubgroup[] _subgroups;
        
        public IdGroup(string name, ushort id)
        {
            _name = name;
            _id = id;
        }

        public string Name => _name;

        public ushort Id => _id;

        internal IReadOnlyList<IdSubgroup> Subgroups => _subgroups;
    }
}