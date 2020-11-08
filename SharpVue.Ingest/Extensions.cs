using SharpVue.Common.Documentation;
using System.Collections.Generic;
using System.Xml;

namespace SharpVue.Ingest
{
    internal static class Extensions
    {
        public static string ReadAsString(this XmlReader reader)
        {
            return reader.ReadInnerXml().Trim();
        }

        public static Content ReadContent(this XmlReader reader)
        {
            var insertions = new List<Insertion>();

            void AddInsertion(InsertionType type, string text, string? data = null) => insertions!.Add(new Insertion(type, text, data));

            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    AddInsertion(InsertionType.PlainText, reader.Value);
                }
                else
                {
                    var name = reader.Name;
                    reader.MoveToFirstAttribute();

                    switch (name)
                    {
                        case "see" when reader.Name == "cref":
                            var cref = MemberName.Parse(reader.Value);
                            string href;

                            switch (cref.Kind)
                            {
                                case MemberKind.Type:
                                    href = $"/ref/{cref.FullName}";
                                    break;

                                case MemberKind.Method:
                                case MemberKind.Field:
                                case MemberKind.Property:
                                case MemberKind.Constructor:
                                case MemberKind.Event:
                                    href = $"/ref/{cref.Root}/{cref.Name}";
                                    break;

                                case MemberKind.Namespace:
                                    continue;

                                default:
                                    throw new System.Exception($"Unknown cref \"{reader.Value}\"");
                            }

                            AddInsertion(InsertionType.SiteLink, cref.Name, href);
                            break;

                        case "see" when reader.Name == "langword":
                        case "typeparamref":
                        case "paramref":
                            AddInsertion(InsertionType.LangKeyword, reader.Value);
                            break;

                        default:
                            break;
                    }
                }
            }

            return new Content(insertions);
        }
    }
}
