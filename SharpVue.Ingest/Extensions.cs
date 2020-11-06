using System.Xml;

namespace SharpVue.Ingest
{
    internal static class Extensions
    {
        public static string ReadAsString(this XmlReader reader)
        {
            //TODO Parse <see> and stuff
            return reader.ReadInnerXml().Trim();
        }
    }
}
