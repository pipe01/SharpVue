using System;
using System.Collections.Generic;

namespace SharpVue.Common
{
    public sealed class Article
    {
        public string ID { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// HTML rendered from Markdown or otherwise. If null, this article will be treated as a folder.
        /// </summary>
        public string? Content { get; set; }

        public IList<Article> Children { get; } = new List<Article>();

        public Article(string title, string? content)
        {
            this.Title = title;
            this.Content = content;
        }
    }
}
