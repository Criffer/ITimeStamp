using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using com.apthai.APTimeStamp.Model;
using com.apthai.APTimeStamp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Ionic.Zip;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using com.apthai.APTimeStamp.Repositories;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Microsoft.Extensions.Configuration;
using com.apthai.APTimeStamp.Configuration;
using Microsoft.AspNetCore.StaticFiles;

namespace com.apthai.APTimeStamp.Controllers
{
    public class BaseController : ControllerBase
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        //private List<MStorageServer> _QISStorageServer;
        protected AppSettings _appSetting;

        public BaseController()
        {


            _hostingEnvironment = UtilsProvider.HostingEnvironment;
            _config = UtilsProvider.Config;
            _appSetting = UtilsProvider.AppSetting;
            _unitOfWork = new UnitOfWork(_hostingEnvironment, _config);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {

                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
