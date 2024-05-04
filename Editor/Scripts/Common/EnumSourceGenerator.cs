using System.IO;
using System.Text;
using Qw1nt.SelfIds.Editor.Scripts.Interfaces;
using Qw1nt.SelfIds.Editor.Scripts.SerialziedTypes;
using Qw1nt.SelfIds.Runtime;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.SelfIds.Editor.Scripts.Common
{
    // Group Name
    public struct Items
    {
        // Subgroup Name
        public enum Swords : uint
        {
            // | Items
            WoodSword = 1
        }
    }
    
    internal struct EnumSourceGenerator : ISourceGenerator
    {
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
            
            builder.Append($"\t\tpublic enum {_subgroup.Name} : uint \n");
            builder.Append("\t\t {\n");

            var ids = _subgroup.Ids;
            
            foreach (var id in ids)
                builder.Append($"\t\t\t{id.Name} = {(int)new IdCalculator(id.GroupId, id.SubgroupId, id.IndexInSubgroup)},\n");

            builder.Append("\t\t}\n");
            builder.Append("\t}\n");
            builder.Append("}");

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), savePath);
            
            File.WriteAllText(fullPath, builder.ToString());
            AssetDatabase.Refresh();
        }
    }
}