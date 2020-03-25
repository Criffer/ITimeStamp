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
       // [Route("GetUserPhoneNumberByIDCardNo")]
       // [SwaggerOperation(Summary = "เรียกดูเบอร์โทรศัพท์ของลูกค้าจากระบบ CRM ทั้งหมด",
       //Description = "Access Key ใช้ในการเรียหใช้ Function ถึงจะเรียกใช้ Function ได้")]
       // public async Task<object> GetUserPhoneNumberByIDCardNo([FromBody]GetUserPhoneParam data)
       // {
       //     try
       //     {
       //         StringValues api_key;
       //         StringValues EmpCode;
       //         //if (Request.Headers.TryGetValue("api_Accesskey", out api_key) && Request.Headers.TryGetValue("EmpCode", out EmpCode))
       //         //{
       //         //    string AccessKey = api_key.First();
       //         //    string EmpCodeKey = EmpCode.First();

       //         //    if (!string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(EmpCodeKey))
       //         //    {
       //         //        return new
       //         //        {
       //         //            success = false,
       //         //            data = new AutorizeDataJWT(),
       //         //            message = "Require Key to Access the Function"
       //         //        };
       //         //    }
       //         //    else
       //         //    {
       //         //        string APApiKey = Environment.GetEnvironmentVariable("API_Key");
       //         //        if (APApiKey == null)
       //         //        {
       //         //            APApiKey = UtilsProvider.AppSetting.ApiKey;
       //         //        }
       //         //        if (api_key != APApiKey)
       //         //        {
       //         //            return new
       //         //            {
       //         //                success = false,
       //         //                data = new AutorizeDataJWT(),
       //         //                message = "Incorrect API KEY !!"
       //         //            };
       //         //        }
       //         //    }
       //         //}
                
       //         Model.CRMWeb.Contact contact = _UserRepository.GetCRMContactByIDCardNO(data.CitizenIdentityNo);
       //         if (contact == null)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = new AutorizeDataJWT(),
       //                 message = "Only AP Customer Can Regist to the System !!"
       //             };
       //         }
       //         List<Model.CRMWeb.ContactPhone> contactPhone = _UserRepository.GetContactPhoneNumberByContactID_Web(contact.ID.ToString());
       //         if (contactPhone == null)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = new AutorizeDataJWT(),
       //                 message = "There is no Assosiate Phone Number with this IDCard Number!!"
       //             };
       //         }
       //         GetUserCRMPhoneNumber cRMContact = new GetUserCRMPhoneNumber();
       //         cRMContact.contactPhones = contactPhone;
       //         cRMContact.FirstNameTH = contact.FirstNameTH;
       //         cRMContact.LastNameTH = contact.LastNameTH;
       //         cRMContact.IsVIP = contact.IsVIP;
       //         cRMContact.CitizenIdentityNo = contact.CitizenIdentityNo;

       //         return new
       //         {
       //             success = true,
       //             data = cRMContact,
       //             message = "Get User Phone Success !"
       //         };

       //     }
       //     catch (Exception ex)
       //     {
       //         return StatusCode(500, "Internal server error :: " + ex.Message);
       //     }
       // }

       // [HttpPost]
       // [Route("Register")]
       // [SwaggerOperation(Summary = "Log In เข้าสู้ระบบเพื่อรับ Access Key ",
       // Description = "Access Key ใช้ในการเรียหใช้ Function ต่างๆ เพื่อไม่ให้ User Login หลายเครื่องในเวลาเดียวกัน")]
       // public async Task<object> Register([FromBody]RegisterData data)
       // {
       //     try
       //     {
       //         //#region VerifyHeader
       //         //string ErrorHeader = "";
       //         //if (!VerifyHeader(out ErrorHeader))
       //         //{
       //         //    return new
       //         //    {
       //         //        success = false,
       //         //        data = "Invalid AccessKey!!. ",
       //         //        valid = false
       //         //    };
       //         //}
       //         //#endregion
                
       //         //bool asd = SHAHelper.VerifyHash("verify", "SHA512", GenerateAccessToken);
       //         Model.CRMWeb.Contact contact = _UserRepository.GetCRMContactByIDCardNO(data.CitizenIdentityNo);
       //         if (contact == null)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = "Invalid ID Card No. Or there is no user with this IDCard on CRM System ",
       //                 valid = false
       //             };
       //         }
       //         Model.CRMWeb.ContactPhone contactPhone = _UserRepository.GetSingleContactPhoneNumberByContactID_Web(contact.ID.ToString(),data.PhoneNumber);
       //         if (contactPhone == null)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = "Invalid Phone Number that assosiate with the IDCard !! ",
       //                 valid = false
       //             };
       //         }

       //         Model.CRMMobile.UserProfile cSUserProfile = new Model.CRMMobile.UserProfile();
       //         cSUserProfile.CRMContactID = contact.ID.ToString();
       //         cSUserProfile.TitleExtEN = contact.TitleExtTH;
       //         cSUserProfile.FirstNameTH = contact.FirstNameTH;
       //         cSUserProfile.LastNameTH = contact.LastNameTH;
       //         cSUserProfile.Nickname = contact.Nickname;
       //         cSUserProfile.TitleExtEN = contact.TitleExtEN;
       //         cSUserProfile.FirstNameEN = contact.FirstNameEN;
       //         cSUserProfile.MiddleNameEN = contact.MiddleNameEN;
       //         cSUserProfile.LastNameEN = contact.LastNameEN;
       //         cSUserProfile.CitizenIdentityNo = contact.CitizenIdentityNo;
       //         cSUserProfile.Created = DateTime.Now.ToShortDateString();
       //         cSUserProfile.CreatedBy = "System-Register";
       //         cSUserProfile.Updated = null;
       //         cSUserProfile.UpdatedBy = null;
       //         cSUserProfile.IsActive = true;
       //         cSUserProfile.PINCode = data.PINCode;
       //         long ProfileID = 0;
       //         bool insert = _UserRepository.InsertCSUserProfile(cSUserProfile,out ProfileID);

       //         string GenerateAccessToken = SHAHelper.ComputeHash(data.DeviceID, "SHA512", null);
       //         Model.CRMMobile.UserLogin cSUserLogin = new Model.CRMMobile.UserLogin();
       //         cSUserLogin.UserPhoneNumber = data.PhoneNumber;
       //         cSUserLogin.LoginDate = DateTime.Now.ToShortDateString();
       //         cSUserLogin.DeviceID = data.DeviceID;
       //         cSUserLogin.DeviceType = data.DeviceType;
       //         cSUserLogin.UserToken = GenerateAccessToken ;
       //         cSUserLogin.UserProfileID = Convert.ToInt32(ProfileID);
       //         bool insertUserLogin = _UserRepository.InsertCSUserLogin(cSUserLogin);

       //         return new
       //         {
       //             success = true,
       //             data = cSUserLogin,
       //             Message = "Register Complete!"
       //         };
                
       //     }
       //     catch (Exception ex)
       //     {
       //         return StatusCode(500, "Internal server error :: " + ex.Message);
       //     }
       // }

       // [HttpPost]
       // [Route("RequestOTP")]
       // [SwaggerOperation(Summary = "Request OTP From ThaiBulk เพื่อมสเพื่อยืนยันตัวตน เพื่อใช่งานระบบ ",
       // Description = "เพื่อทำการ ยืนยันตัวตนของผู้ใช้งาน ซึ่ง 1 คนสามารถมีมากกว่า 1 UserLogin ได้")]
       // public async Task<object> RequestOTP([FromBody]RequestOTPParam data)
       // {
       //     try
       //     {
       //         //#region VerifyHeader
       //         //string ErrorHeader = "";
       //         //if (!VerifyHeader(out ErrorHeader))
       //         //{
       //         //    return new
       //         //    {
       //         //        success = false,
       //         //        data = "Invalid AccessKey!!. ",
       //         //        valid = false
       //         //    };
       //         //}
       //         //#endregion
               
       //         var client = new HttpClient();
       //         ThaiBulkOTPRequest thaiBulkOTPRequest = new ThaiBulkOTPRequest();
       //         thaiBulkOTPRequest.key = UtilsProvider.AppSetting.ThaiBulkApiKey;
       //         thaiBulkOTPRequest.secret = UtilsProvider.AppSetting.ThaiBulkSecret;
       //         thaiBulkOTPRequest.msisdn = data.PhoneNumber;

       //         var Content = new StringContent(JsonConvert.SerializeObject(thaiBulkOTPRequest));
       //         Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
       //         string PostURL = Environment.GetEnvironmentVariable("ThaiBulkRequestOTPURL");
       //         if (PostURL == null)
       //         {
       //             PostURL = UtilsProvider.AppSetting.ThaiBulkRequestOTPURL;
       //         }
       //         string RequestParam = "?key=" + thaiBulkOTPRequest.key +"&secret=" + thaiBulkOTPRequest.secret + "&msisdn=" + thaiBulkOTPRequest.msisdn;
       //         PostURL = PostURL + RequestParam;
       //         var Respond = await client.PostAsync(PostURL + RequestParam, Content);
       //         if (Respond.StatusCode != System.Net.HttpStatusCode.OK)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = new AutorizeDataJWT(),
       //                 valid = false
       //             };
       //         }
       //         var RespondData = await Respond.Content.ReadAsStringAsync();
       //         RespondData = RespondData.Replace(@"\", "");
       //         thaiBulkOTPRequestReturnObj returnObj = new thaiBulkOTPRequestReturnObj();
       //         returnObj = JsonConvert.DeserializeObject<thaiBulkOTPRequestReturnObj>(RespondData);


       //             return new
       //             {
       //                 success = true,
       //                 data = returnObj,
       //                 Message = "Request OTP Successfully!. Please Check OTP Number sended to your mobile !"
       //             };
                

       //     }
       //     catch (Exception ex)
       //     {
       //         return StatusCode(500, "Internal server error :: " + ex.Message);
       //     }
       // }

       // [HttpPost]
       // [Route("VerifyOTP")]
       // [SwaggerOperation(Summary = "Request OTP From ThaiBulk เพื่อมสเพื่อยืนยันตัวตน เพื่อใช่งานระบบ ",
       //     Description = "เพื่อทำการ ยืนยันตัวตนของผู้ใช้งาน ซึ่ง 1 คนสามารถมีมากกว่า 1 UserLogin ได้")]
       // public async Task<object> VerifyOTP([FromBody]ThaiBulkVerifyOTP data)
       // {
       //     try
       //     {

       //         //#region VerifyHeader
       //         //string ErrorHeader = "";
       //         //if (!VerifyHeader(out ErrorHeader))
       //         //{
       //         //    return new
       //         //    {
       //         //        success = false,
       //         //        data = "Invalid AccessKey!!. ",
       //         //        valid = false
       //         //    };
       //         //}
       //         //#endregion
       //         string key = Environment.GetEnvironmentVariable("ThaiBulkVerifyOTPURL");
       //         if (key == null)
       //         {
       //             key = UtilsProvider.AppSetting.ThaiBulkApiKey;
       //         }
       //         string secret = Environment.GetEnvironmentVariable("ThaiBulkVerifyOTPURL");
       //         if (secret == null)
       //         {
       //             secret = UtilsProvider.AppSetting.ThaiBulkSecret;
       //         }
       //         var client = new HttpClient();
       //         ThaiBulkOTPRequest thaiBulkOTPRequest = new ThaiBulkOTPRequest();
       //         var Content = new StringContent(JsonConvert.SerializeObject(thaiBulkOTPRequest));
       //         string PostURL = Environment.GetEnvironmentVariable("ThaiBulkVerifyOTPURL");
       //         if (PostURL == null)
       //         {
       //             PostURL = UtilsProvider.AppSetting.ThaiBulkVerifyOTPURL;
       //         }
       //         string RequestParam = "?key=" + key + "&secret=" + secret + "&token=" + data.token + "&pin=" + data.pin;
       //         PostURL = PostURL + RequestParam;
       //         var Respond = await client.PostAsync(PostURL, Content);
       //         if (Respond.StatusCode != System.Net.HttpStatusCode.OK)
       //         {
       //             return new
       //             {
       //                 success = false,
       //                 data = new AutorizeDataJWT(),
       //                 valid = false
       //             };
       //         }
       //         var RespondData = await Respond.Content.ReadAsStringAsync();
       //         RespondData = RespondData.Replace(@"\", "");
       //         thaiBulkOTPVerifyReturnObj returnObj = new thaiBulkOTPVerifyReturnObj();
       //         returnObj = JsonConvert.DeserializeObject<thaiBulkOTPVerifyReturnObj>(RespondData);


       //         return new
       //         {
       //             success = true,
       //             data = returnObj,
       //             Message = "Verify OTP Successfully!. The Pin is Match"
       //         };


       //     }
       //     catch (Exception ex)
       //     {
       //         return StatusCode(500, "Internal server error :: " + ex.Message);
       //     }
       // }

       // [ApiExplorerSettings(IgnoreApi = true)]
       // public string generateToken(string PhoneNumber)
       // {
       //     return string.Format("{0}_{1:N}", PhoneNumber, Guid.NewGuid());
       // }

       // [ApiExplorerSettings(IgnoreApi = true)]
       // public bool VerifyHeader(out string ErrorMsg)
       // {

       //     string ipaddress = "5555555";
       //     StringValues api_key;
       //     StringValues EmpCode;

       //     var isValidHeader = false;
       //     //APIITVendor //VendorData = new APIITVendor();
       //     if (Request.Headers.TryGetValue("api_Accesskey", out api_key))
       //     {
       //         string AccessKey = api_key.First();
       //         string EmpCodeKey = EmpCode.First();

       //         if (!string.IsNullOrEmpty(AccessKey))
       //         {
       //             bool CorrectACKey = SHAHelper.VerifyHash("APiCRMMobile", "SHA512", AccessKey);
       //             if (CorrectACKey)
       //             {
       //                 ErrorMsg = "";
       //                 return true;
       //             }
       //         }
       //     }
       //     else
       //     {
       //         if (!isValidHeader)
       //         {
       //             //_log.LogDebug(ipaddress + " :: Missing Authorization Header.");
       //             ErrorMsg = ipaddress + " :: Missing Authorization Header.";
       //             //VendorData = new APIITVendor();
       //             return false;
       //             //  return BadRequest("Missing Authorization Header.");
       //         }
       //     }
       //     //VendorData = new APIITVendor();
       //     ErrorMsg = "SomeThing Wrong with Header Contact Developer ASAP";
       //     return false;
       // }
    }
}