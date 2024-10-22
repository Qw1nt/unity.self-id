using System.Text.RegularExpressions;
using UnityEditor;

namespace Qw1nt.SelfIds.Editor.Scripts.Extensions
{
    public static class StringValidationExtensions
    {
        public static bool IsValidName(this string name)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                EditorUtility.DisplayDialog("Ошибка", "Название не может быть пустым", "Ок");
                return false;
            }

            var firstChar = name[0];

            if (Regex.IsMatch(firstChar.ToString(), "^[a-zA-Z]+$") == false)
            {
                EditorUtility.DisplayDialog("Ошибка", "Название начинается с некорректного символа", "Ок");
                return false;
            }

            return true;
        }
    }
}