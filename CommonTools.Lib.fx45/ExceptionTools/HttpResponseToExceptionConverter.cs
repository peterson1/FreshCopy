using CommonTools.Lib.ns11.ExceptionTools;
using System.Net;

namespace CommonTools.Lib.fx45.ExceptionTools
{
    public static class HttpResponseToExceptionConverter
    {
        public static UnexpectedResponseException ToUnexpectedException(this HttpWebResponse resp, HttpStatusCode expected)
            => new UnexpectedResponseException(resp.StatusDescription, 
                                               expected.ToString(), 
                                               resp.StatusCode.ToString());
    }
}
