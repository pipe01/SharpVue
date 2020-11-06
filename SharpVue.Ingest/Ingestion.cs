using SharpVue.Common.Documentation;
using System.Collections.Generic;
using System.Xml;

namespace SharpVue.Ingest
{
    public abstract class Ingestion
    {
        public abstract IEnumerable<MemberData> Load(XmlNodeReader reader);
    }
}
