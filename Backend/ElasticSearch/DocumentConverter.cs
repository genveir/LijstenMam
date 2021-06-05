using LijstenMam.Backend.Data;
using LijstenMam.Backend.Data.DocumentModel;
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
            var dto = ConvertElement(element);

            dtos.Add(dto);

            foreach(var child in element.Children)
            {
                Convert(child, dtos);
            }
        }

        private FileElementDTO ConvertElement(FileElement element)
        {
            var dto = new FileElementDTO()
            {
                ParagraphNumber = element.ParagraphNumber,
                Genres = element.ElementData.Genres,
                BookTitle = element.ElementData.BookTitle,
                Compilers = element.ElementData.Compilers,
                ArticleTitle = element.ElementData.ArticleTitle,
                Authors = element.ElementData.Authors
            };

            return dto;
        }
    }
}
