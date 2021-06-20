using LijstenMam.Backend.Data.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public interface IESClient
    {
        Task<bool> Check();

        Task Fill(Document document);

        Task<IEnumerable<long>> Search(string term, SearchOptions options);
    }
}
