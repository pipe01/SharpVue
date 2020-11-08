using Markdig;
using SharpVue.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpVue.Loading
{
    public class ArticleLoader
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
                .UseSyntaxHighlighting()
                .Build();

            LoadAll();
        }

        private void LoadAll()
        {
            foreach (var folder in Config.Articles)
            {
                var fullPath = Path.Combine(BaseFolder, folder);

                LoadFolder(new DirectoryInfo(fullPath), Articles);
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
            var html = Markdown.ToHtml(md, MarkdownPipeline);

            return new Article(file.Name, html);
        }

        private static string GetFolderArticleName(DirectoryInfo dir)
        {
            var nameFile = new FileInfo(Path.Combine(dir.FullName, FolderNameFile));

            if (nameFile.Exists)
                return File.ReadAllText(nameFile.FullName);

            return dir.Name;
        }
    }
}
