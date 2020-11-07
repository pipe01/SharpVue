using System;
using System.Collections.Generic;

namespace SharpVue.Common.Documentation
{
    public class Content
    {
        public IList<Insertion> Insertions { get; }

        public Content() : this(new List<Insertion>())
        {
        }

        public Content(IList<Insertion> insertions)
        {
            this.Insertions = insertions;
        }

        public void Add(InsertionType type, string text, string? data = null)
            => Insertions.Add(new Insertion(type, text, data));

        public void AddPlainText(string text)
            => Insertions.Add(new Insertion(InsertionType.PlainText, text, null));

        public void AddReferenceType(Type type)
        {
            if (type.IsGenericParameter)
            {
                AddPlainText(type.Name);
            }
            else
            {
                Insertions.Add(new Insertion(InsertionType.ReferenceType, type.FullName!, type.FullName));
            }
        }
    }

    public class Insertion
    {
        public InsertionType Type { get; }
        public string Text { get; }
        public string? Data { get; }

        public Insertion(InsertionType type, string text, string? data)
        {
            this.Type = type;
            this.Data = data;
            this.Text = text;
        }
    }

    public enum InsertionType
    {
        PlainText,
        SiteLink,
        LangKeyword,
        ReferenceType,
    }
}
