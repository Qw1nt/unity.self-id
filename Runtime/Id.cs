using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    /*
     * Группа          Подгруппа      Уникальное значение,
     * |----16 бит----|----16 бит----|------------32 бита------------|
     */
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Id
    {
        private const int GroupBitsOffset = 48;
        private const int SubgroupBitsOffset = 32;

#if UNITY_EDITOR
        [SerializeField] private string _id;
        [SerializeField] private string _editorFullName;
        [SerializeField] private uint _indexInSubgroup;

        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string EditorFullName => _editorFullName;
#endif

        [SerializeField] private ushort _group;
        [SerializeField] private ushort _subgroup;
        [SerializeField] private uint _item;

        [SerializeField] private ulong _hash;

        public ushort Group
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _group;
        }

        public ushort Subgroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _subgroup;
        }

        public uint Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _item;
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            return _id;
        }
#else
        public override string ToString()
        {
            return _hash.ToString();
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Id other)
        {
            return _hash == other._hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Id other && Equals(other);
        }

        public static Id Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Build(ushort group, ushort subGroup, uint item)
        {
            return ((ulong)group << GroupBitsOffset) | ((ulong)subGroup << SubgroupBitsOffset) | item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetGroup(ulong id)
        {
            return (ushort)((id >> GroupBitsOffset) & 0xFFFF);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetSubgroup(ulong id)
        {
            return (ushort)((id >> SubgroupBitsOffset) & 0xFFFF);
        }        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetUniqueNumber(ulong id)
        {
            return (ushort)(id & 0xFFFFFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(Id id)
        {
            return unchecked((int)id._hash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(Id id)
        {
            return id._hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Id left, Id right)
        {
            return left._hash == right._hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Id left, Id right)
        {
            return !(left == right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return unchecked((int)_hash);
        }
    }
}