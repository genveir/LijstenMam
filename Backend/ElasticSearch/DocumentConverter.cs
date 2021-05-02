using LijstenMam.Backend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.ElasticSearch
{
    public class DocumentConverter
    {
        private DocumentConverter()
        {
            
        }

        public static IEnumerable<FileElementDTO> Convert(Document document)
        {
            return new DocumentConverter()._Convert(document);
        }

        private IEnumerable<FileElementDTO> _Convert(Document document)
        {
            var dtos = new List<FileElementDTO>();

            Convert(document, dtos);

            return dtos;
        }

        private void Convert(FileElement element, List<FileElementDTO> dtos)
        {
            dtos.Add(new FileElementDTO()
            {
                ParagraphNumber = element.ParagraphNumber,
                Genres = element.ElementData.Genres,
                Compilers = element.ElementData.Compilers,
                BookTitle = element.ElementData.BookTitle,
                Authors = element.ElementData.Authors,
                ArticleTitle = element.ElementData.ArticleTitle
            });

            foreach(var child in element.Children)
            {
                Convert(child, dtos);
            }
        }
    }
}
