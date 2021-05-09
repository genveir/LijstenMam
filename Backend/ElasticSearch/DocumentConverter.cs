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
            FileElementDTO dto;
            var canConvert = TryConvertGenre(element, out dto) ||
                TryConvertBook(element, out dto) ||
                TryConvertArticle(element, out dto) ||
                TryConvertElement(element, out dto);

            if (canConvert) dtos.Add(dto);

            foreach(var child in element.Children)
            {
                Convert(child, dtos);
            }
        }

        private bool TryConvertGenre(FileElement element, out FileElementDTO dto)
        {
            dto = null;

            if (element is Genre)
            {
                dto = new FileElementDTO()
                {
                    ParagraphNumber = element.ParagraphNumber,
                    Genres = element.ElementData.Genres
                };

                return true;
            }
            return false;
        }

        private bool TryConvertBook(FileElement element, out FileElementDTO dto)
        {
            dto = null;

            if (element is Book)
            {
                dto = new FileElementDTO()
                {
                    ParagraphNumber = element.ParagraphNumber,
                    BookTitle = element.ElementData.BookTitle,
                    Compilers = element.ElementData.Compilers
                };
                return true;
            }
            return false;
        }

        private bool TryConvertArticle(FileElement element, out FileElementDTO dto)
        {
            dto = null;

            if (element is Article)
            {
                dto = new FileElementDTO()
                {
                    ParagraphNumber = element.ParagraphNumber,
                    ArticleTitle = element.ElementData.ArticleTitle,
                    Authors = element.ElementData.Authors
                };
                return true;
            }
            return false;
        }

        private bool TryConvertElement(FileElement element, out FileElementDTO dto)
        {
            dto = new FileElementDTO()
            {
                ParagraphNumber = element.ParagraphNumber,
                Genres = element.ElementData.Genres,
                BookTitle = element.ElementData.BookTitle,
                Compilers = element.ElementData.Compilers,
                ArticleTitle = element.ElementData.ArticleTitle,
                Authors = element.ElementData.Authors
            };
            return true;
        }
    }
}
