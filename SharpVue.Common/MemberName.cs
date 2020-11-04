using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpVue.Common
{
    public readonly struct MemberName
    {
        private const int MinimumNameLength = 3;

        public MemberType Type { get; }
        public string Namespace { get; }
        public string Name { get; }
        public int TypeParamCount { get; }

        public MemberName(MemberType type, string @namespace, string name, int typeParamCount)
        {
            this.Type = type;
            this.Namespace = @namespace;
            this.Name = name;
            this.TypeParamCount = typeParamCount;
        }

        public static MemberName Parse(string fullName)
        {
            if (fullName.Length < MinimumNameLength)
                throw new FormatException("Member name is too short");

            var type = fullName[0] switch
            {
                'M' => MemberType.Method,
                'T' => MemberType.Type,
                'F' => MemberType.Field,
                'P' => MemberType.Property,
                'C' => MemberType.Constructor,
                'E' => MemberType.Event,
                _ => throw new FormatException($"Unknown member type '{fullName[0]}'")
            };

            var parts = SplitFullName(fullName);
            var ns = new List<string>();
            string name = null;

            for (int i = 0; i < parts.Count; i++)
            {
                var part = parts[i];

                if (i == parts.Count - 1)
                {
                    name = part;
                }
                else
                {
                    ns.Add(part);
                }
            }

            return default;

            static IReadOnlyList<string> SplitFullName(string name)
            {
                var parts = new List<string>();
                var str = new StringBuilder();

                for (int i = 0; i < name.Length; i++)
                {
                    char c = name[i];

                    if (c == '.')
                    {
                        parts.Add(str.ToString());
                        str.Clear();
                    }
                    else if (c == '(')
                    {
                        break;
                    }
                    else
                    {
                        str.Append(c);
                    }
                }

                if (str.Length > 0)
                    parts.Add(str.ToString());

                return parts;
            }
        }
    }
}
