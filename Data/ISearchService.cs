using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public interface ISearchService
    {
        Task<IEnumerable<FileElement>> Search(string term, SearchOptions options);

        bool UsingElasticSearch { get; }
    }
}
