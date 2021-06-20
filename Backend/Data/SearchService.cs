using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class SearchService : ISearchService
    {
        public bool UsingElasticSearch { get; set; }

        private ILogger<SearchService> logger;
        private IFileService fileService;
        private IESClient client;
        private static File activeFile;

        public SearchService(IFileService fileService, ILogger<SearchService> logger, IESClient client)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.client = client;
        }

        public async Task<IEnumerable<FileElement>> Search(string term, SearchOptions options)
        {
            logger.LogInformation("searching for {term}", term);

            if (term == null) return new List<FileElement>();

            UsingElasticSearch = await CheckOnline();
            logger.LogInformation("using elastic search: " + UsingElasticSearch);

            var file = fileService.File;

            IEnumerable<long> paragraphIds;
            if (UsingElasticSearch) paragraphIds = await SearchES(file, term, options);
            else paragraphIds = await SearchManual(file, term, options);

            logger.LogInformation("found " + paragraphIds.Count() + " results");

            var elements = file.GetElementsByParagraphId(paragraphIds);

            return elements;
        }

        public async Task Fill()
        {
            logger.LogInformation("filling index");

            UsingElasticSearch = await CheckOnline();
            logger.LogInformation("using elastic search: " + UsingElasticSearch);

            var file = fileService.File;

            var sameFile = file == activeFile;
            logger.LogInformation($"uploaded file is {(sameFile ? "" : " not ")} the same as the currently used file");

            if (UsingElasticSearch && !sameFile)
            {
                logger.LogInformation("filling index");
                await client.Fill(file.FileRoot);
                activeFile = file;
            }
        }

        private async Task<IEnumerable<long>> SearchES(File file, string term, SearchOptions options)
        {
            return await client.Search(term, options);
        }

        private async Task<IEnumerable<long>> SearchManual(File file, string term, SearchOptions options)
        {
            var root = file.FileRoot;

            HashSet<long> result = new HashSet<long>();
            await Task.Run(() => SearchRecursive(root, term, options, ref result, false));

            return result;
        }

        private void SearchRecursive(FileElement element, string term, SearchOptions options, ref HashSet<long> result, bool autoAdd)
        {
            if (term == null) return;

            bool shouldAdd = autoAdd;

            if (!shouldAdd)
            {
                var tokens = term
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim());

                shouldAdd = true;
                foreach (var token in tokens)
                {
                    shouldAdd = shouldAdd && element.RawText.Contains(token, StringComparison.InvariantCultureIgnoreCase);
                }
            }
            if (shouldAdd) result.Add(element.ParagraphNumber);

            foreach (var child in element.Children)
            {
                SearchRecursive(child, term, options, ref result, shouldAdd);
            }
        }

        private async Task<bool> CheckOnline()
        {
            return await client.Check();
        }
    }
}
