using System;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [Serializable]
    public class IdGroup
    {
        [SerializeField] private string _name;
        [SerializeField] private uint _id;

        [SerializeField] private IdSubgroup[] _subgroups;
        
        public IdGroup(string name, uint id)
        {
            _name = name;
            _id = id;
        }

        public string Name => _name;

        public uint Id => _id;
    }
}