using System;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

namespace Qw1nt.SelfIds.Runtime
{
    public struct RuntimeIdGenerator
    {
        private static readonly object Locker = new();
        private static ulong _previousResult = 1UL;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong Execute()
        {
            lock (Locker)
            {
                var sourceOffset = (ulong) Random.Range(0, int.MaxValue) * Id.Offset;
    
                var firstPart = (ulong) HashCode.Combine(sourceOffset << 2, sourceOffset << 6);
                var second = (ulong) HashCode.Combine(sourceOffset << 4, sourceOffset << 8);

                var result = _previousResult * firstPart ^ second * Id.Offset;
                _previousResult = result;
                
                return _previousResult;
            }
        }
    }
}