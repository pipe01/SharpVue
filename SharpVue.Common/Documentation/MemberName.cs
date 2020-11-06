using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharpVue.Common.Documentation
{
    [DebuggerDisplay("{Kind} {FullName}")]
    public readonly struct MemberName
    {
        private const int MinimumNameLength = 3;

        public MemberKind Kind { get; }
        public string Root { get; }
        public string Name { get; }
        public string FullName { get; }

        public string Original { get; }

        private MemberName(MemberKind type, string root, string name, string fullName, string original)
        {
            this.Kind = type;
            this.Root = root;
            this.Name = name;
            this.FullName = fullName;
            this.Original = original;
        }

        public static MemberName Parse(string fullName)
        {
            if (fullName.Length < MinimumNameLength)
                throw new FormatException("Member name is too short");

            var kind = fullName[0] switch
            {
                'M' => MemberKind.Method,
                'T' => MemberKind.Type,
                'F' => MemberKind.Field,
                'P' => MemberKind.Property,
                'C' => MemberKind.Constructor,
                'E' => MemberKind.Event,
                _ => throw new FormatException($"Unknown member type '{fullName[0]}'")
            };

            var parts = SplitFullName(fullName.AsSpan(2));
            var root = new List<string>();
            string? name = null;

            for (int i = 0; i < parts.Count; i++)
            {
                var part = parts[i];

                if (i == parts.Count - 1)
                {
                    name = part;
                }
                else
                {
                    root.Add(part);
                }
            }

            if (name == null)
                throw new Exception(); // Should never be reached

            name = NormalizeTypeName(name);

            return new MemberName(kind, string.Join('.', root), name, fullName.Substring(2), fullName);

            static IReadOnlyList<string> SplitFullName(ReadOnlySpan<char> name)
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

            static string NormalizeTypeName(string name)
            {
                var apos = name.IndexOf('`');

                if (apos >= 0)
                {
                    return name.Substring(0, apos);
                }

                return name;
            }
        }
    }
}
