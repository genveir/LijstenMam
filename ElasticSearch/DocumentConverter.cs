using LijstenMam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.ElasticSearch
{
    public class DocumentConverter
    {
        private List<FileElementDTO> elements;

        private DocumentConverter()
        {
            elements = new List<FileElementDTO>();
        }

        public static IEnumerable<FileElementDTO> Convert(Document document)
        {
            return new DocumentConverter()._Convert(document);
        }

        private IEnumerable<FileElementDTO> _Convert(Document document)
        {
            document.Convert(this, new FileElementDTO());

            return elements;
        }

        private void MapNextLevel(FileElement fileElement, FileElementDTO info)
        {
            foreach (var childElement in fileElement.Children)
            {
                childElement.Convert(this, info.Copy());
            }
        }

        public void ConvertElement(Document document, FileElementDTO parentInfo)
        {
            MapNextLevel(document, parentInfo);
        }

        public void ConvertElement(Genre genre, FileElementDTO parentInfo)
        {
            var genreDTO = parentInfo;

            genreDTO.ParagraphNumber = genre.ParagraphNumber;
            genreDTO.Genre = genre.Text;

            this.elements.Add(genreDTO);

            MapNextLevel(genre, genreDTO);
        }

        public void ConvertElement(Book book, FileElementDTO parentInfo)
        {
            var bookDTO = parentInfo;
            
            bookDTO.ParagraphNumber = book.ParagraphNumber;
            bookDTO.Compilers = book.GetAuthors();
            bookDTO.BookTitle = book.GetTitle();

            this.elements.Add(bookDTO);

            MapNextLevel(book, bookDTO);
        }

        public void ConvertElement(Article article, FileElementDTO parentInfo)
        {
            var articleDTO = parentInfo;

            articleDTO.ParagraphNumber = article.ParagraphNumber;
            articleDTO.Authors = article.GetAuthors();
            articleDTO.ArticleTitle = article.GetTitle();

            MapNextLevel(article, articleDTO);
        }
    }
}
