using System;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [Serializable]
    public class IdSubgroup
    {
        [SerializeField] private uint _groupId;
        [SerializeField] private string _name;
        [SerializeField] private uint _id;
        [SerializeField] private Id[] _ids;
    }
}