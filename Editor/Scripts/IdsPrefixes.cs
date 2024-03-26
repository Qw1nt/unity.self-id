using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts
{
    [CreateAssetMenu]
    internal class IdsPrefixes : ScriptableObject
    {
        [SerializeField] private List<string> _prefixes;

        private static string[] _prefixesArray;
        private static IdsPrefixes _instance;
        
        internal static IdsPrefixes Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                return _instance = Load();
            }
        }

        internal static string[] PrefixesArray
        {
            get
            {
                if (_prefixesArray != null && _prefixesArray.Length == Instance.Prefixes.Count)
                    return _prefixesArray;

                return _prefixesArray = Instance.Prefixes.ToArray();
            }
        }
        
        public IReadOnlyList<string> Prefixes => _prefixes;
        
        public static IdsPrefixes Load()
        {
            return Resources.Load<IdsPrefixes>("IdsPrefixes");
        }
    }
}