using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ApplicationServices.RequestManager.Models
{
    public class ResponseModel
    {
        public HttpResponseMessage httpResponseMessage { get; set; }
        public string httpResponseString { get; set; }
    }
}
