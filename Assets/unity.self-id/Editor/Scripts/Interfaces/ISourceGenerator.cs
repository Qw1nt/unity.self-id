using System.Text;

namespace Qw1nt.SelfIds.Editor.Scripts.Interfaces
{
    internal interface ISourceGenerator
    {
        public void Execute(StringBuilder buffer, string savePath);
    }
}