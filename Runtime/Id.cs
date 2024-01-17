using System;
using UnityEngine;

namespace Runtime.Shared.SelfIds
{
    [Serializable]
    public struct Id
    {
#if UNITY_EDITOR
        [SerializeField] private bool _usePrefix;
        [SerializeField] private string _prefix;
        [SerializeField] private string _content;
#endif

        [SerializeField] private string _id;

        public override string ToString()
        {
            return _id;
        }

        public static implicit operator string(Id id)
        {
            return id._id;
        }

        public static implicit operator Id(string id)
        {
            return new Id
            {
                _id = id
            };
        }
    }
}