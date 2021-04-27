﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.ElasticSearch
{
    [ElasticsearchType(IdProperty = "ParagraphNumber")]
    public class FileElementDTO
    {
        public FileElementDTO()
        {
            this.Genres = new List<string>();
            this.Compilers = new List<string>();
            this.Authors = new List<string>();
        }

        public long ParagraphNumber { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Compilers { get; set; }

        public string BookTitle { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string ArticleTitle { get; set; }
    }
}
