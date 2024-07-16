using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace Qw1nt.SelfIds.Runtime
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Id
    {
        internal const ulong Offset = 7_777_777_777_777_777UL;

#if UNITY_EDITOR
        [SerializeField] private string _id;
        [SerializeField] private string _editorFullName;
        [SerializeField] private uint _indexInSubgroup;

        /// <summary>
        /// ONLY EDITOR
        /// </summary>
        public string EditorFullName => _editorFullName;
#endif

        [SerializeField] private ulong _group;
        [SerializeField] private ulong _subgroup;
        [SerializeField] private ulong _item;

        [SerializeField] private ulong _hash;

        public ulong Group
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _group;
        }

        public ulong Subgroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _subgroup;
        }

        public ulong Item
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

        public static Id Null => new();

        /// <summary>
        /// WARN: Allocated array
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GenerateFromGuid()
        {
            var guid = Guid.NewGuid();
            var parts = guid.ToString().Split('-');

            var firstPart = (ulong)HashCode.Combine(parts[0], parts[1]);
            var secondPart = (ulong)HashCode.Combine(parts[2], parts[3]);
            
            return firstPart ^ secondPart * Offset;
        }       
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GenerateFromWords(string first, string second, string third, string fourth)
        {
            var firstPart = (ulong)HashCode.Combine(first, third);
            var secondPart = (ulong)HashCode.Combine(second, fourth);
            
            return firstPart ^ secondPart * Offset;
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Build(ulong group, ulong subGroup, ulong item)
        {
            return unchecked(group ^ subGroup * item * Offset);
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