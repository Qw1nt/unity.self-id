using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Qw1nt.SelfIds.Editor.Scripts.Interfaces;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    internal struct EnumSourceGenerator : ISourceGenerator
    {
        private static readonly Regex NameCheckRegex = new(@"\s*(\([^()]*\))");
        
        private readonly SerializedIdGroup _group;
        private readonly SerializedSubgroup _subgroup;
        private readonly string _namespace;
        
        public EnumSourceGenerator(SerializedIdGroup group, SerializedSubgroup subgroup, string codeNamespace)
        {
            _group = group;
            _subgroup = subgroup;
            _namespace = codeNamespace;
        }
        
        public void Execute(StringBuilder builder, string savePath)
        {
            builder.Append("// -> Auto generated <- // \n\n");

            builder.Append($"namespace {_namespace}\n");
            builder.Append("{\n");
            
            builder.Append($"\tpublic partial struct {_group.Name}\n");
            builder.Append("\t{\n");
            
            builder.Append($"\t\tpublic enum {_subgroup.Name.Replace(" ", "")} : ulong \n");
            builder.Append("\t\t {\n");

            var ids = _subgroup.Ids;
            
            foreach (var id in ids)
                builder.Append($"\t\t\t{NameCheckRegex.Replace(id.Name, "").Replace(" ", "")} = {id.Hash}UL,\n");

            builder.Append("\t\t}\n");
            builder.Append("\t}\n");
            builder.Append("}");

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), savePath);
            
            File.WriteAllText(fullPath, builder.ToString());
            AssetDatabase.Refresh();
        }
    }
}