using System.Collections.Generic;
using System.Diagnostics;

namespace SharpVue.Common.Documentation
{
    [DebuggerDisplay("{Kind} {Name.Name}")]
    public class MemberData
    {
        public MemberName Name { get; }
        public MemberKind Kind => Name.Kind;
        public string? Summary { get; set; }
        public string? Remarks { get; set; }
        public string? Returns { get; set; }
        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> TypeParameters { get; } = new Dictionary<string, string>();
        public IDictionary<MemberName, string> Exceptions { get; } = new Dictionary<MemberName, string>();
        public IList<MemberName> SeeAlso { get; } = new List<MemberName>();
        public IList<MemberData> Children { get; } = new List<MemberData>();

        public MemberData? Parent { get; set; }

        public MemberData(string fullName)
        {
            this.Name = MemberName.Parse(fullName);
        }
    }
}
