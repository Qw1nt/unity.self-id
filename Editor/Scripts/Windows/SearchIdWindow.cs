using System;
using System.Collections.Generic;
using System.Linq;
using Qw1nt.SelfIds.Editor.Scripts.Common;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Windows
{
    public class SearchIdWindow : ScriptableObject, ISearchWindowProvider
    {
        private Action<Id> _selectCallback;

        public void SetSelectCallback(Action<Id> selectedCallback)
        {
            _selectCallback = selectedCallback;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (TryLoadDatabase(out var database) == false)
                return null;

            var result = new List<SearchTreeEntry>();
            result.Add(new SearchTreeGroupEntry(new GUIContent("Ids"), 0));

            foreach (var record in database.Records)
            {
                result.Add(new SearchTreeGroupEntry(new GUIContent(record.Name), 1));

                foreach (var subgroup in record.Subgroups)
                {
                    result.Add(new SearchTreeGroupEntry(new GUIContent(subgroup.Name), 2));

                    foreach (var id in subgroup.Ids)
                    {
                        result.Add(new SearchTreeEntry(new GUIContent(id.ToString()))
                        {
                            level = 3,
                            userData = id
                        });
                    }
                }
            }

            return result;
        }

        private bool TryLoadDatabase(out IdsDatabase database)
        {
            database = null;
            var guid = AssetDatabase.FindAssets("l:" + IdDatabaseEditorWindow.DatabaseAssetLabel).FirstOrDefault();

            if (string.IsNullOrEmpty(guid) == true)
                return false;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            database = AssetDatabase.LoadAssetAtPath<IdsDatabase>(path);

            return true;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            _selectCallback?.Invoke((Id) entry.userData);
            return true;
        }
    }
}