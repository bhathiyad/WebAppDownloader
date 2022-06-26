using ApplicationServices.RequestManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.RequestManager
{
    public interface IRequestManagerService
    {
        Task<ResponseModel> GetWebPageContent(string baseUrl, string urlPath);
        ResponseModel GetWebPageContentNotAsync(string baseUrl, string urlPath);
    }
}
