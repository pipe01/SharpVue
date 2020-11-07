using System.Collections.Generic;
using System.Diagnostics;

namespace SharpVue.Common.Documentation
{
    [DebuggerDisplay("{Kind} {Name.Name}")]
    public class MemberData
    {
        public MemberName Name { get; }
        public MemberKind Kind => Name.Kind;
        public Content? Summary { get; set; }
        public Content? Remarks { get; set; }
        public Content? Returns { get; set; }
        public IDictionary<string, Content> Parameters { get; } = new Dictionary<string, Content>();
        public IDictionary<string, Content> TypeParameters { get; } = new Dictionary<string, Content>();
        public IDictionary<MemberName, Content> Exceptions { get; } = new Dictionary<MemberName, Content>();
        public IList<MemberName> SeeAlso { get; } = new List<MemberName>();
        public IList<MemberData> Children { get; } = new List<MemberData>();

        public MemberData? Parent { get; set; }

        public MemberData(string fullName)
        {
            this.Name = MemberName.Parse(fullName);
        }
    }
}
