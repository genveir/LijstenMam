﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class Genre : FileElement
    {
        public Genre(long paragraphNumber, string text) : base( paragraphNumber, text) 
        {
            
        }

        internal override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        internal override void Add(Genre genre)
        {
            if (this.Parent == null) throw new NotImplementedException("genre has no parent");

            this.Parent.Add(genre);
        }

        internal override void Add(Book book)
        {
            this._Children.Add(book);
            book.Parent = this;
        }

        internal override void Add(Article article)
        {
            this._Children.Add(article);
            article.Parent = this;
        }

        internal override void SetElementData()
        {
            this.ElementData = this.Parent.ElementData.Copy();
            this.ElementData.Genres = this.ElementData.Genres.Append(this.RawText);
        }
    }
}