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
using com.apthai.APTimeStamp.CustomModel;
using com.apthai.CoreApp.Data.Services;
using com.apthai.APTimeStamp.HttpRestModel;
using Microsoft.Extensions.Primitives;

namespace com.apthai.APTimeStamp.Controllers
{
    public class MasterController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMasterRepository _masterRepository;
        //private List<MStorageServer> _QISStorageServer;
        protected AppSettings _appSetting;

        public MasterController(IAuthorizeService authorizeService,IUnitOfWork unitOfWork, IMasterRepository masterRepository)
        {
            _hostingEnvironment  = UtilsProvider.HostingEnvironment;
            _config = UtilsProvider.Config;
            _appSetting = UtilsProvider.AppSetting;
            _unitOfWork = unitOfWork;
            // ------------- แบบนี้จะอ้าง ถึง Repo ตรงๆ ไม่รอรับการทำ AutoMate-Test ได้
            //_masterRepository = new MasterRepository(_hostingEnvironment, _config);
            // ------------- ควรใช้แบบนี้
            _masterRepository = masterRepository;
            _authorizeService = authorizeService;
        }
        [HttpGet]
        [Route("Test")]
        public async Task<object> GetMasterCallType()
        {
                    return new
                    {
                        success = true,
                        data = "Hello World!"
                    };
             
        }
        //-------------- Verify Key and Token From HTTP Header ------------------
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool VerifyHeader(out string ErrorMsg)
        {
            //if (data == null)
            //{
            //    return BadRequest();
            //}

            //string ipaddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string ipaddress = "5555555";
            StringValues api_key;
            StringValues EmpCode;

            var isValidHeader = false;
            //APIITVendor //VendorData = new APIITVendor();
            if (Request.Headers.TryGetValue("api_Accesskey", out api_key) && Request.Headers.TryGetValue("EmpCode", out EmpCode))
            {
                string AccessKey = api_key.First();
                string EmpCodeKey = EmpCode.First();

                if (!string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(EmpCodeKey))
                {
                    //bool CanAccess = _authorizeService.AccessKeyAuthentication(AccessKey, EmpCodeKey);
                    //if (CanAccess == true)
                    //{
                    //    ErrorMsg = "";
                    //    return true;
                    //}
                }
            }
            else
            {
                if (!isValidHeader)
                {
                    //_log.LogDebug(ipaddress + " :: Missing Authorization Header.");
                    ErrorMsg = ipaddress + " :: Missing Authorization Header.";
                    //VendorData = new APIITVendor();
                    return false;
                    //  return BadRequest("Missing Authorization Header.");
                }
            }
            //VendorData = new APIITVendor();
            ErrorMsg = "SomeThing Wrong with Header Contact Developer ASAP";
            return false;
        }
    }


}
