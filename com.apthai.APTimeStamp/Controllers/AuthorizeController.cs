using System;
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
    public class AuthorizeController : BaseController
    {


        private readonly IMasterRepository _masterRepo;
        private readonly IAuthorizeService _authorizeService;
        private readonly IUserRepository _UserRepository;
        public AuthorizeController(IMasterRepository masterRepo , IAuthorizeService authorizeService,IUserRepository userRepository)
        {

            _masterRepo = masterRepo;
            _authorizeService = authorizeService;
            _UserRepository = userRepository;

        }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "Log In เข้าสู้ระบบเพื่อรับ Access Key ",
        Description = "Access Key ใช้ในการเรียหใช้ Function ต่างๆ เพื่อไม่ให้ User Login หลายเครื่องในเวลาเดียวกัน")]
        public async Task<object> PostLogin([FromBody] LoginData data)
        {
            try
            {
                var userName = data.UserName;
                var password = data.Password;
                var appCode = data.AppCode;

                string APApiKey = Environment.GetEnvironmentVariable("API_Key");
                if (APApiKey == null)
                {
                    APApiKey = UtilsProvider.AppSetting.ApiKey;
                }

                var client = new HttpClient();
                var Content = new StringContent(JsonConvert.SerializeObject(data));
                Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Content.Headers.Add("api_key", APApiKey);
                string PostURL = Environment.GetEnvironmentVariable("AuthenticationURL");
                if (PostURL == null)
                {
                    PostURL = UtilsProvider.AppSetting.AuthorizeURL;
                }
                var Respond = await client.PostAsync(PostURL, Content);
                if (Respond.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new
                    {
                        success = false,
                        data = new AutorizeDataJWT(),
                        Message = "ser Authentication Fail"
                    };
                }
                var RespondData = await Respond.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<AutorizeDataJWT>(RespondData);
                if (Result.LoginResult == false)
                {
                    return new
                    {
                        success = false,
                        data = new AutorizeDataJWT(),
                        Message = Result.LoginResultMessage
                    };
                }

                Model.APFamily.EmpProfile empProfile = _UserRepository.GetEmpProfile(Result.EmployeeID);
                if (empProfile == null)
                {
                    Model.APFamily.EmpProfile emp = new Model.APFamily.EmpProfile();
                    emp.EmpCode = Result.EmployeeID;
                    emp.EmpDeviceID = data.DeviceID;
                    emp.EmpName = Result.FirstName;
                    emp.EmpLastName = Result.LastName;
                    emp.PositionName = Result.Division;
                    emp.EMail = Result.Email;
                    emp.EmpLoginToken = generateToken(data.DeviceID);

                    bool InsertEmpData = _UserRepository.InsertEmpProfile(emp);

                    return new
                    {
                        success = true,
                        data = emp,
                        Token = emp.EmpLoginToken,
                        Message = "LogIn Success!"
                    };

                }
                else
                {
                    if (data.DeviceID == empProfile.EmpDeviceID)
                    {
                        DateTime ExtainToken = Convert.ToDateTime(empProfile).AddDays(15);
                        empProfile.EmpTokenExpire = ExtainToken;

                        bool updateProfile = _UserRepository.UpdateEmpProfile(empProfile);
                    }
                    else
                    {
                        return new
                        {
                            success = false,
                            data = empProfile = new Model.APFamily.EmpProfile(),
                            Token = "",
                            Message = "You Have Change you Device! Please Contact IT Admin for further Use!"
                        };
                    }
                }

                return new
                {
                    success = false,
                    data = new AutorizeDataJWT(),
                    Message = Result.LoginResultMessage
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :: " + ex.Message);
            }
        }

       // [HttpPost]
       // [Route("CheckIN")]
       // [SwaggerOperation(Summary = "Register User เพื่อใช่ระบบ ซึ่งจะไป หาข้อมูลจากระบบ CRM",
       //Description = "Access Key ใช้ในการเรียหใช้ Function ต่างๆ เพื่อไม่ให้ User Login หลายเครื่องในเวลาเดียวกัน")]
       // public async Task<object> PostCheckIn([FromBody]CheckPinParam data)
       // {
       //     try
       //     {
       //         StringValues api_key;
       //         StringValues EmpCode;

       //         //Model.CRMWeb.Contact cRMContact = _UserRepository.GetCRMContactByIDCardNO(data.CitizenIdentityNo);
       //         //if (cRMContact == null)
       //         //{
       //         //    return new
       //         //    {
       //         //        success = false,
       //         //        data = new AutorizeDataJWT(),
       //         //        message = "Only AP Customer Can Regist to the System !!"
       //         //    };
       //         //}
       //         //VerifyPINReturnObj cSUserProfile = _UserRepository.GetUserLogin_Mobile(data.PINCode, data.AccessKey);
       //         //if (cSUserProfile == null)
       //         //{
       //         //    return new
       //         //    {
       //         //        success = false,
       //         //        data = new AutorizeDataJWT(),
       //         //        message = "Cannot Find the Matach Data"
       //         //    };
       //         //}
       //         //else
       //         //{
       //         //    //Model.CRMMobile.UserLogin userLogin = _UserRepository.GetUserLoginByID_Mobile(cSUserProfile.UserLoginID);
       //         //    //string GenerateAccessToken = SHAHelper.ComputeHash(data.DeviceID, "SHA512", null);
       //         //    //userLogin.UserToken = GenerateAccessToken;
       //         //    //cSUserProfile.UserToken = GenerateAccessToken;
       //         //    //bool UpdateUserToken = _UserRepository.UpdateCSUserLogin(userLogin);

       //         return new
       //         {
       //             success = true,
       //             data = cSUserProfile,
       //             message = "PIN Correct!"
       //         };
       //         //}
       //     }
       //     catch (Exception ex)
       //     {
       //         return StatusCode(500, "Internal server error :: " + ex.Message);
       //     }
       // }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string generateToken(string DeviceID)
        {
            return string.Format("{0}_{1:N}", DeviceID, Guid.NewGuid());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string generateAccessKey(string EmpCode)
        {
            return string.Format("{0}_{1:N}", EmpCode, Guid.NewGuid());
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
                    bool CorrectACKey = SHAHelper.VerifyHash("APiCRMMobile","SHA512", AccessKey);
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