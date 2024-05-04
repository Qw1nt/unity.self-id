using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct IdCalculator
    {
        [FieldOffset(0)] public readonly ushort Group;
        [FieldOffset(2)] public readonly ushort Subgroup;

        [FieldOffset(0)] public readonly int LocationHash;

        [FieldOffset(4)] public readonly int Index;
        [FieldOffset(8)] internal readonly int Hash;

        public IdCalculator(ushort group, ushort subgroup, int index)
        {
            LocationHash = default;
            Group = group;
            Subgroup = subgroup;
            Index = index;
            Hash = LocationHash ^ Index;
        }

        public bool Equals(IdCalculator other)
        {
            return LocationHash == other.LocationHash;
        }

        public override bool Equals(object obj)
        {
            return obj is IdCalculator other && Equals(other);
        }

        public override string ToString()
        {
            return Hash.ToString();
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        public static implicit operator int(IdCalculator calculator)
        {
            return calculator.Hash;
        }

        public static bool operator ==(IdCalculator left, IdCalculator right)
        {
            return left.Hash == right.Hash;
        }

        public static bool operator !=(IdCalculator left, IdCalculator right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public struct Id
    {
        private const uint GroupOffset = 10_000_000u;
        private const uint SubGroupOffset = 10_000u;

        [SerializeField] private ushort _groupId;
        [SerializeField] private ushort _subgroupId;

        [SerializeField] private int _indexInSubgroup;

#if UNITY_EDITOR
        [SerializeField] private string _id;
        [SerializeField] private string _fullName;

        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string EditorStingId => _id;
        
        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string EditorFullName => _fullName;
#endif

        public override string ToString()
        {
            return ((IdCalculator)this).Hash.ToString();
        }

        public bool Equals(Id other)
        {
            return (IdCalculator)this == (IdCalculator)other;
        }

        public override bool Equals(object obj)
        {
            return obj is Id other && Equals(other);
        }

        public static Id Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new();
        }

        public IdCalculator Calculator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(_groupId, _subgroupId, _indexInSubgroup);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Build(ushort group, ushort subGroup, ushort item)
        {
            return group * GroupOffset + subGroup * SubGroupOffset + (uint)Mathf.Clamp(item, 0, 9999);
        }

        public static implicit operator IdCalculator(Id id)
        {
            return new IdCalculator(id._groupId, id._subgroupId, id._indexInSubgroup);
        }

        public static implicit operator int(Id id)
        {
            return new IdCalculator(id._groupId, id._subgroupId, id._indexInSubgroup);
        }

        public static bool operator ==(Id left, Id right)
        {
            var leftCalculator = (IdCalculator)left;
            var rightHash = (IdCalculator)right;

            return leftCalculator == rightHash;
        }

        public static bool operator !=(Id left, Id right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return (IdCalculator)this;
        }
    }
}