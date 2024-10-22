using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    internal struct SavePathSetter
    {
        public bool TryExecute(string title, string defaultName, string extensions, out string result)
        {
            var path = EditorUtility.SaveFilePanelInProject(title, defaultName, extensions, "");
            result = string.Empty;
            
            if (string.IsNullOrEmpty(path) == true)
                return false;

            result = path;
            return true;
        }
    }
}