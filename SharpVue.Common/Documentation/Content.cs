using System.Collections.Generic;

namespace SharpVue.Common.Documentation
{
    public class Content
    {
        public IList<Insertion> Insertions { get; }

        public Content(IList<Insertion> insertions)
        {
            this.Insertions = insertions;
        }
    }

    public readonly struct Insertion
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
    }
}
