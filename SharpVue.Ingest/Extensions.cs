using System.Xml;

namespace SharpVue.Ingest
{
    internal static class Extensions
    {
        public static string ReadAsString(this XmlReader reader)
        {
            return reader.ReadElementContentAsString().Trim();
        }
    }
}
