using System;
using System.Runtime.Serialization;

namespace LijstenMam.ElasticSearch
{
    [Serializable]
    internal class InvalidElasticSearchResponseException : Exception
    {
        public InvalidElasticSearchResponseException()
        {
        }

        public InvalidElasticSearchResponseException(string message) : base(message)
        {
        }

        public InvalidElasticSearchResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidElasticSearchResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}