﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using com.apthai.CoreApp.Data.Services;
using com.apthai.APTimeStamp.CustomModel;
using com.apthai.APTimeStamp.HttpRestModel;
using com.apthai.APTimeStamp.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using com.apthai.APTimeStamp.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Primitives;

namespace com.apthai.APTimeStamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControlle : BaseController
    {


        private readonly IMasterRepository _masterRepo;
        private readonly IAuthorizeService _authorizeService;
        private readonly IUserRepository _UserRepository;
        public UserControlle(IMasterRepository masterRepo , IAuthorizeService authorizeService,IUserRepository userRepository)
        {

            _masterRepo = masterRepo;
            _authorizeService = authorizeService;
            _UserRepository = userRepository;
        }

       // [HttpPost]
       // [Route("CheckIN")]
       // [SwaggerOperation(Summary = "เรียกดูเบอร์โทรศัพท์ของลูกค้าจากระบบ CRM ทั้งหมด",
       //Description = "Access Key ใช้ในการเรียหใช้ Function ถึงจะเรียกใช้ Function ได้")]
       // public async Task<object> CheckIN([FromBody]GetUserPhoneParam data)
       // {
       //     try
       //     {
       //         StringValues api_key;
       //         StringValues EmpCode;
                
       //     }
       //     catch
       //     {

       //     }
       // }

      

        [ApiExplorerSettings(IgnoreApi = true)]
        public string generateToken(string PhoneNumber)
        {
            return string.Format("{0}_{1:N}", PhoneNumber, Guid.NewGuid());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool VerifyHeader(out string ErrorMsg)
        {

            string ipaddress = "5555555";
            StringValues api_key;
            StringValues EmpCode;

            var isValidHeader = false;
            //APIITVendor //VendorData = new APIITVendor();
            if (Request.Headers.TryGetValue("api_Accesskey", out api_key))
            {
                string AccessKey = api_key.First();
                string EmpCodeKey = EmpCode.First();

                if (!string.IsNullOrEmpty(AccessKey))
                {
                    bool CorrectACKey = SHAHelper.VerifyHash("APiCRMMobile", "SHA512", AccessKey);
                    if (CorrectACKey)
                    {
                        ErrorMsg = "";
                        return true;
                    }
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