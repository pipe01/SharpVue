using ColorCode.Styling;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using SharpVue.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpVue.Loading
{
    public class ArticleLoader : ILoader
    {
        private const string FolderNameFile = ".name";

        public IList<Article> Articles { get; } = new List<Article>();

        private readonly MarkdownPipeline MarkdownPipeline;

        private readonly Config Config;
        private readonly string BaseFolder;

        public ArticleLoader(Config config, string baseFolder)
        {
            this.Config = config;
            this.BaseFolder = baseFolder;

            this.MarkdownPipeline = new MarkdownPipelineBuilder()
                .UseSyntaxHighlighting(StyleDictionary.DefaultDark)
                .UseUrlRewriter(RewriteUrl)
                .UseYamlFrontMatter()
                .Build();

            LoadAll();
        }

        public void Reload()
        {
            //TODO Only remove changed files
            Articles.Clear();

            LoadAll();
        }

        private void LoadAll()
        {
            foreach (var folder in Config.Articles)
            {
                var fullPath = Path.Combine(BaseFolder, folder);

                LoadFolder(new DirectoryInfo(fullPath), Articles);
            }

            if (Articles.Count == 1)
            {
                var root = Articles[0];
                Articles.Clear();

                foreach (var child in root.Children)
                {
                    Articles.Add(child);
                }
            }
        }

        private void LoadFolder(DirectoryInfo dir, IList<Article> parent)
        {
            if (!dir.Exists)
                throw new FileNotFoundException("Cannot find article folder at " + dir.FullName);

            var folderName = GetFolderArticleName(dir);
            var container = new Article(folderName, null);

            parent.Add(container);

            foreach (var item in dir.GetDirectories())
            {
                LoadFolder(item, container.Children);
            }

            foreach (var file in dir.GetFiles("*.md"))
            {
                var article = LoadArticle(file);

                container.Children.Add(article);
            }
        }

        private Article LoadArticle(FileInfo file)
        {
            var md = File.ReadAllText(file.FullName);

            using var writer = new StringWriter();

            var renderer = new HtmlRenderer(writer);
            MarkdownPipeline.Setup(renderer);

            var doc = Markdown.Parse(md, MarkdownPipeline);
            renderer.Render(doc);

            string? name = null, id = null;

            if (doc.Count > 0 && doc[0] is YamlFrontMatterBlock yaml)
            {
                (name, id) = ReadYamlMatter(yaml);
            }

            name ??= Path.GetFileNameWithoutExtension(file.FullName);
            id ??= Guid.NewGuid().ToString().ToLower();

            return new Article(name, writer.ToString())
            {
                ID = id
            };
        }

        private static (string? Name, string? Id) ReadYamlMatter(YamlFrontMatterBlock yaml)
        {
            string? name = null, id = null;

            foreach (var line in yaml.Lines)
            {
                if (line is StringLine str && str.Slice.Text != null)
                {
                    int colon = str.Slice.IndexOf(':');
                    var key = str.Slice.Text[str.Slice.Start..colon];
                    var value = str.Slice.Text[(colon + 1)..(str.Slice.End + 1)].Trim();

                    if (key == "id")
                        id = value;
                    else if (key == "name")
                        name = value;
                    else
                        throw new Exception($"Unknown YAML key '{key}' at line {str.Line}");
                }
            }

            return (name, id);
        }

        private static string GetFolderArticleName(DirectoryInfo dir)
        {
            var nameFile = new FileInfo(Path.Combine(dir.FullName, FolderNameFile));

            if (nameFile.Exists)
                return File.ReadAllText(nameFile.FullName);

            return dir.Name;
        }

        private static string RewriteUrl(LinkInline link)
        {
            if (link.Url.Length == 0)
                return link.Url;

            if (link.Url[0] == '@')
            {
                var member = link.Url.Substring(1);

                // [](@Type) syntax
                if (link.FirstChild == null)
                {
                    link.AppendChild(new LiteralInline(member));
                }
                // [<](@Type) syntax
                else if (link.FirstChild is LiteralInline lit && lit.Content.Length == 1 && lit.Content.CurrentChar == '<')
                {
                    var parts = member.Split('.');

                    link.Clear();
                    link.AppendChild(new LiteralInline(parts[parts.Length - 1]));
                }

                return $"#/ref/{member}";
            }
            else if (link.Url[0] == '?')
            {
                return link.Url;
            }

            return link.Url;
        }
    }
}
