using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [Serializable]
    public struct Id
    {
        private const int GroupOffset = 10_000_000;
        private const int SubGroupOffset = 10_000;

#if UNITY_EDITOR
        [SerializeField] private string _id;
        [SerializeField] private string _fullName;
        [SerializeField] private uint _indexInSubgroup;

        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string FullName => _fullName;
#endif

        [SerializeField] private int _hash;

#if UNITY_EDITOR
        public override string ToString()
        {
            return _id;
        }
#endif

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
        public static int Build(ushort group, ushort subGroup, ushort item)
        {
            return group * GroupOffset + subGroup * SubGroupOffset + Mathf.Clamp(item, 0, 9999);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetGroupId(ushort group)
        {
            return group * GroupOffset;
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