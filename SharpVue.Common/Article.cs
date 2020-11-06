using System.Collections.Generic;

namespace SharpVue.Common
{
    public sealed class Article
    {
        public string? Folder { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }

        public IList<Article> Children { get; } = new List<Article>();

        public Article(string title, string contents)
        {
            this.Title = title;
            this.Contents = contents;
        }
    }
}
