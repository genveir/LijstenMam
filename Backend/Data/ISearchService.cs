using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public interface ISearchService
    {
        Task<IEnumerable<FileElement>> Search(string term, SearchOptions options);

        Task Fill();

        bool UsingElasticSearch { get; }
    }
}
