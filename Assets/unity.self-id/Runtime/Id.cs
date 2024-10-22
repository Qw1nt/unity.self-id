using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Qw1nt.SelfIds.Runtime
{
    /*
     * Группа          Подгруппа      Уникальное значение,
     * |----8 бит----|----8 бит----|------------16 бита------------|
     */
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Id
    {
        private const byte ByteMask = 0xFF;
        private const ushort ShortMask = 0xFFFF;
        
        private const int GroupBitsOffset = 24;
        private const int SubgroupBitsOffset = 16;

#if UNITY_EDITOR
        [SerializeField] private string _id;
        [SerializeField] private string _editorFullName;
        [SerializeField] private uint _indexInSubgroup;
        
        [SerializeField] private byte _group;
        [SerializeField] private byte _subgroup;
        [SerializeField] private ushort _item;

        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string EditorFullName => _editorFullName;
        
        public byte Group
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _group;
        }

        public byte Subgroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _subgroup;
        }

        public ushort Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _item;
        }
#endif

        [SerializeField] private int _hash;

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
        public override int GetHashCode()
        {
            return _hash;
        }

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
        public static int Build(byte group, byte subGroup, ushort item)
        {
            return (group << GroupBitsOffset) | (subGroup << SubgroupBitsOffset) | item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetGroup(int id)
        {
            return (byte)((id >> GroupBitsOffset) & ByteMask);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetSubgroup(int id)
        {
            return (byte)((id >> SubgroupBitsOffset) & ByteMask);
        }        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort GetUniqueNumber(int id)
        {
            return (ushort)(id & ShortMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(Id id)
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
    }
}