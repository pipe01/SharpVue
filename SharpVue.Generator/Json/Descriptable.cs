using SharpVue.Common.Documentation;
using System.Text.Json.Serialization;

namespace SharpVue.Generator.Json
{
    public abstract class Descriptable
    {
        public string Name { get; }
        public Content? Summary { get; }
        public Content? Remarks { get; }

        [JsonIgnore]
        public MemberData? Data { get; }

        public Descriptable(MemberData? data, string name)
        {
            this.Name = name;
            this.Data = data;
            this.Summary = data?.Summary;
            this.Remarks = data?.Remarks;
        }
    }
}
