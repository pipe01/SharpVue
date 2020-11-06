using SharpVue.Common.Documentation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SharpVue.Ingest
{
    public class CSharpXmlIngestion : Ingestion
    {

        protected override IEnumerable<MemberData> Load(XmlNodeReader reader)
        {
            MemberData? currentMember = null;

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                if (reader.Name == "member")
                {
                    currentMember = new MemberData(reader.GetAttribute("name"));
                    yield return currentMember;
                }
                else if (currentMember != null)
                {
                    switch (reader.Name)
                    {
                        case "summary":
                            currentMember.Summary = reader.ReadAsString();
                            break;

                        case "remarks":
                            currentMember.Remarks = reader.ReadAsString();
                            break;

                        case "seealso":
                            currentMember.SeeAlso.Add(MemberName.Parse(reader.GetAttribute("cref")));
                            break;

                        case "exception":
                            currentMember.Exceptions[MemberName.Parse(reader.GetAttribute("cref"))] = reader.ReadAsString();
                            break;

                        case "param":
                            currentMember.Parameters[reader.GetAttribute("name")] = reader.ReadAsString();
                            break;

                        case "typeparam":
                            currentMember.TypeParameters[reader.GetAttribute("name")] = reader.ReadAsString();
                            break;

                        case "returns":
                        case "value":
                            currentMember.Returns = reader.ReadAsString();
                            break;
                    }
                }
            }
        }
    }
}
