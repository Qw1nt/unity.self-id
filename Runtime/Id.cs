using System;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
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
        [SerializeField] private int _hash;

        public override string ToString()
        {
            return _id;
        }

        public bool Equals(Id other)
        {
            return _hash == other._hash;
        }

        public override bool Equals(object obj)
        {
            return obj is Id other && Equals(other);
        }
        
        public static Id Null => new();

        public static implicit operator string(Id id)
        {
            return id._id;
        }
        
        public static implicit operator int(Id id)
        {
            return id._hash;
        }

        public static implicit operator Id(string id)
        {
            return new Id
            {
                _id = id
            };
        }

        public static bool operator ==(Id left, Id right)
        {
            return left._id == right._id;
        }

        public static bool operator !=(Id left, Id right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return _hash;
        }
    }
}