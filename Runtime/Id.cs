using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [Serializable]
    public struct Id
    {
        private const uint GroupOffset = 10_000_000u;
        private const uint SubGroupOffset = 10_000u;
        
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Build(ushort group, ushort subGroup, ushort item)
        {
            return group * GroupOffset + subGroup * SubGroupOffset + (uint) Mathf.Clamp(item, 0, 9999);
        }

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