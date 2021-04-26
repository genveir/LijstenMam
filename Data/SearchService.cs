using LijstenMam.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class SearchService : ISearchService
    {
        public bool UsingElasticSearch { get; set; }

        private IFileService fileService;
        private bool filled = false;

        public SearchService(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public async Task<IEnumerable<FileElement>> Search(string term, SearchOptions options)
        {
            await CheckOnline();

            var file = fileService.File;

            IEnumerable<long> paragraphIds;
            if (UsingElasticSearch) paragraphIds = await SearchES(file, term, options);
            else paragraphIds = await SearchManual(file, term, options);

            var elements = file.GetElementsByParagraphId(paragraphIds);

            return elements;
        }

        private async Task<IEnumerable<long>> SearchES(File file, string term, SearchOptions options)
        {
            if (!filled)
            {
                await new ESClient().Fill(file.FileRoot);
            }

            return await new ESClient().Search(term, options);
        }

        private async Task<IEnumerable<long>> SearchManual(File file, string term, SearchOptions options)
        {
            var root = file.FileRoot;

            HashSet<long> result = new HashSet<long>();
            await Task.Run(() => SearchRecursive(root, term, options, ref result));

            return result;
        }

        private void SearchRecursive(FileElement element, string term, SearchOptions options, ref HashSet<long> result)
        {
            var tokens = term
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim());

            foreach(var token in tokens)
            {
                if (element.Text.Contains(token, StringComparison.InvariantCultureIgnoreCase)) result.Add(element.ParagraphNumber);
            }

            foreach(var child in element.Children)
            {
                SearchRecursive(child, term, options, ref result);
            }
        }

        private async Task<bool> CheckOnline()
        {
            return await new ESClient().Check();
        }
    }
}
