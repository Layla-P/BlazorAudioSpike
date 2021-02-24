using System.Net;

namespace AssemblyAi.Common.Helpers
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Content { get; set; } // the result (e.g. a list of records), or an error message (when in error state)
    }

    /// <summary>
    /// Project intended to provide a Generalised container for the response from a service class.
    /// Typical intended use is a container to relay either:
    /// a) valid data (e.g. a dataset returned from a database query)
    /// b) error information (e.g. why a database call failed)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        
        public string ErrorMessage { get; set; }
        public string Message { get; set; } //  optional supplementary message (e.g. "total records found")

        public ServiceResponse()
        {
            HttpStatusCode = HttpStatusCode.Ambiguous;
            Message = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}