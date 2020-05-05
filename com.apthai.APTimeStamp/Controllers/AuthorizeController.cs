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
using com.apthai.APTimeStamp.Model.APFamily;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace com.apthai.APTimeStamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : BaseController
    {


        private readonly IMasterRepository _masterRepo;
        private readonly IAuthorizeService _authorizeService;
        private readonly IUserRepository _UserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AuthorizeController(IMasterRepository masterRepo, IAuthorizeService authorizeService, IUserRepository userRepository)
        {
            _hostingEnvironment = UtilsProvider.HostingEnvironment;
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

                if (data.UserLoginImage != null) // ถ่ายรูป
                {

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
                            Message = "Authentication Fail"
                        };
                    }
                    var RespondData = await Respond.Content.ReadAsStringAsync();
                    AutorizeDataJWT Result = JsonConvert.DeserializeObject<AutorizeDataJWT>(RespondData);
                    if (Result.LoginResult == false)
                    {
                        return new
                        {
                            success = false,
                            data = new AutorizeDataJWT(),
                            Message = Result.LoginResultMessage
                        };
                    }

                    AutorizeDataJWTReturnObject Return = new AutorizeDataJWTReturnObject();
                    Return.AccountExpirationDate = Result.AccountExpirationDate;
                    Return.AppUserRole = Result.AppUserRole;
                    Return.AuthenticationProvider = Result.AuthenticationProvider;
                    Return.CostCenterCode = Result.CostCenterCode;
                    Return.CostCenterName = Result.CostCenterName;
                    Return.DisplayName = Result.DisplayName;
                    Return.Division = Result.Division;
                    Return.DomainUserName = Result.DomainUserName;
                    Return.Email = Result.Email;
                    Return.EmployeeID = Result.EmployeeID;
                    Return.FirstName = Result.FirstName;
                    Return.LastLogon = Result.LastLogon;
                    Return.LastName = Result.LastName;
                    Return.LoginResult = Result.LoginResult;
                    Return.LoginResultMessage = Result.LoginResultMessage;
                    Return.SysAppCode = Result.SysAppCode;
                    Return.SysUserData = JsonConvert.DeserializeObject<UserModel>(Result.SysUserData);
                    Return.SysUserId = Result.SysUserId;
                    Return.SysUserRoles = JsonConvert.DeserializeObject<vwUserRole>(Result.SysUserRoles);
                    Return.Token = Result.Token;
                    Return.UserApp = JsonConvert.DeserializeObject<List<vwUserApp>>(Result.UserApp);
                    Return.UserPrincipalName = Result.UserPrincipalName;

                    Model.APFamily.RegisLoginHistory empProfile = _UserRepository.GetEmpProfile(Result.EmployeeID);
                    if (empProfile == null)
                    {
                        Model.APFamily.RegisLoginHistory emp = new Model.APFamily.RegisLoginHistory();
                        emp.EmpCode = Result.EmployeeID;
                        emp.EmpDeviceID = data.DeviceID;
                        emp.EmpName = Result.FirstName;
                        emp.EmpLastName = Result.LastName;
                        emp.PositionName = Result.Division;
                        emp.Email = Result.Email;
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
                            return new
                            {
                                success = true,
                                data = empProfile,
                                Token = empProfile.EmpLoginToken,
                                Message = "LogIn Success!"
                            };
                        }
                        else
                        {
                            return new
                            {
                                success = false,
                                data = empProfile = new Model.APFamily.RegisLoginHistory(),
                                Token = "",
                                Message = "You Have Change you Device! Please Contact IT Admin for further Use!"
                            };
                        }
                    }

                }
                else
                {
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
                            Message = "Authentication Fail"
                        };
                    }
                    var RespondData = await Respond.Content.ReadAsStringAsync();
                    AutorizeDataJWT Result = JsonConvert.DeserializeObject<AutorizeDataJWT>(RespondData);
                    if (Result.LoginResult == false)
                    {
                        return new
                        {
                            success = false,
                            data = new AutorizeDataJWT(),
                            Message = Result.LoginResultMessage
                        };
                    }

                    AutorizeDataJWTReturnObject Return = new AutorizeDataJWTReturnObject();
                    Return.AccountExpirationDate = Result.AccountExpirationDate;
                    Return.AppUserRole = Result.AppUserRole;
                    Return.AuthenticationProvider = Result.AuthenticationProvider;
                    Return.CostCenterCode = Result.CostCenterCode;
                    Return.CostCenterName = Result.CostCenterName;
                    Return.DisplayName = Result.DisplayName;
                    Return.Division = Result.Division;
                    Return.DomainUserName = Result.DomainUserName;
                    Return.Email = Result.Email;
                    Return.EmployeeID = Result.EmployeeID;
                    Return.FirstName = Result.FirstName;
                    Return.LastLogon = Result.LastLogon;
                    Return.LastName = Result.LastName;
                    Return.LoginResult = Result.LoginResult;
                    Return.LoginResultMessage = Result.LoginResultMessage;
                    Return.SysAppCode = Result.SysAppCode;
                    Return.SysUserData = JsonConvert.DeserializeObject<UserModel>(Result.SysUserData);
                    Return.SysUserId = Result.SysUserId;
                    Return.SysUserRoles = JsonConvert.DeserializeObject<vwUserRole>(Result.SysUserRoles);
                    Return.Token = Result.Token;
                    Return.UserApp = JsonConvert.DeserializeObject<List<vwUserApp>>(Result.UserApp);
                    Return.UserPrincipalName = Result.UserPrincipalName;

                    Model.APFamily.RegisLoginHistory empProfile = _UserRepository.GetEmpProfile(Result.EmployeeID);
                    if (empProfile == null)
                    {
                        Model.APFamily.RegisLoginHistory emp = new Model.APFamily.RegisLoginHistory();
                        emp.EmpCode = Result.EmployeeID;
                        emp.EmpDeviceID = data.DeviceID;
                        emp.EmpName = Result.FirstName;
                        emp.EmpLastName = Result.LastName;
                        emp.PositionName = Result.Division;
                        emp.Email = Result.Email;
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
                            return new
                            {
                                success = true,
                                data = empProfile,
                                Token = empProfile.EmpLoginToken,
                                Message = "LogIn Success!"
                            };
                        }
                        else
                        {
                            return new
                            {
                                success = false,
                                data = empProfile = new Model.APFamily.RegisLoginHistory(),
                                Token = "",
                                Message = "You Have Change you Device! Please Contact IT Admin for further Use!"
                            };
                        }
                    }
                } //ไม่ถ่ายรูป
                return new
                {
                    success = false,
                    data = new AutorizeDataJWT(),
                    Message = "Authentication Fail"
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error :: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckIn")]
        [SwaggerOperation(Summary = "Log In เข้าสู้ระบบเพื่อรับ Access Key ",
        Description = "Access Key ใช้ในการเรียหใช้ Function ต่างๆ เพื่อไม่ให้ User Login หลายเครื่องในเวลาเดียวกัน")]
        public async Task<object> CheckIn([FromBody] LoginData data)
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

                if (data.UserLoginImage != null) // ถ่ายรูป
                {

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
                            Message = "Authentication Fail"
                        };
                    }
                    var RespondData = await Respond.Content.ReadAsStringAsync();
                    AutorizeDataJWT Result = JsonConvert.DeserializeObject<AutorizeDataJWT>(RespondData);
                    if (Result.LoginResult == false)
                    {
                        return new
                        {
                            success = false,
                            data = new AutorizeDataJWT(),
                            Message = Result.LoginResultMessage
                        };
                    }

                    AutorizeDataJWTReturnObject Return = new AutorizeDataJWTReturnObject();
                    Return.AccountExpirationDate = Result.AccountExpirationDate;
                    Return.AppUserRole = Result.AppUserRole;
                    Return.AuthenticationProvider = Result.AuthenticationProvider;
                    Return.CostCenterCode = Result.CostCenterCode;
                    Return.CostCenterName = Result.CostCenterName;
                    Return.DisplayName = Result.DisplayName;
                    Return.Division = Result.Division;
                    Return.DomainUserName = Result.DomainUserName;
                    Return.Email = Result.Email;
                    Return.EmployeeID = Result.EmployeeID;
                    Return.FirstName = Result.FirstName;
                    Return.LastLogon = Result.LastLogon;
                    Return.LastName = Result.LastName;
                    Return.LoginResult = Result.LoginResult;
                    Return.LoginResultMessage = Result.LoginResultMessage;
                    Return.SysAppCode = Result.SysAppCode;
                    Return.SysUserData = JsonConvert.DeserializeObject<UserModel>(Result.SysUserData);
                    Return.SysUserId = Result.SysUserId;
                    Return.SysUserRoles = JsonConvert.DeserializeObject<vwUserRole>(Result.SysUserRoles);
                    Return.Token = Result.Token;
                    Return.UserApp = JsonConvert.DeserializeObject<List<vwUserApp>>(Result.UserApp);
                    Return.UserPrincipalName = Result.UserPrincipalName;

                    Model.APFamily.RegisLoginHistory empProfile = _UserRepository.GetEmpProfile(Result.EmployeeID);
                    if (empProfile == null)
                    {
                        Model.APFamily.RegisLoginHistory emp = new Model.APFamily.RegisLoginHistory();
                        emp.EmpCode = Result.EmployeeID;
                        emp.EmpDeviceID = data.DeviceID;
                        emp.EmpName = Result.FirstName;
                        emp.EmpLastName = Result.LastName;
                        emp.PositionName = Result.Division;
                        emp.Email = Result.Email;
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
                            return new
                            {
                                success = true,
                                data = empProfile,
                                Token = empProfile.EmpLoginToken,
                                Message = "LogIn Success!"
                            };
                        }
                        else
                        {
                            return new
                            {
                                success = false,
                                data = empProfile = new Model.APFamily.RegisLoginHistory(),
                                Token = "",
                                Message = "You Have Change you Device! Please Contact IT Admin for further Use!"
                            };
                        }
                    }

                }
                else
                {
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
                            Message = "Authentication Fail"
                        };
                    }
                    var RespondData = await Respond.Content.ReadAsStringAsync();
                    AutorizeDataJWT Result = JsonConvert.DeserializeObject<AutorizeDataJWT>(RespondData);
                    if (Result.LoginResult == false)
                    {
                        return new
                        {
                            success = false,
                            data = new AutorizeDataJWT(),
                            Message = Result.LoginResultMessage
                        };
                    }

                    AutorizeDataJWTReturnObject Return = new AutorizeDataJWTReturnObject();
                    Return.AccountExpirationDate = Result.AccountExpirationDate;
                    Return.AppUserRole = Result.AppUserRole;
                    Return.AuthenticationProvider = Result.AuthenticationProvider;
                    Return.CostCenterCode = Result.CostCenterCode;
                    Return.CostCenterName = Result.CostCenterName;
                    Return.DisplayName = Result.DisplayName;
                    Return.Division = Result.Division;
                    Return.DomainUserName = Result.DomainUserName;
                    Return.Email = Result.Email;
                    Return.EmployeeID = Result.EmployeeID;
                    Return.FirstName = Result.FirstName;
                    Return.LastLogon = Result.LastLogon;
                    Return.LastName = Result.LastName;
                    Return.LoginResult = Result.LoginResult;
                    Return.LoginResultMessage = Result.LoginResultMessage;
                    Return.SysAppCode = Result.SysAppCode;
                    Return.SysUserData = JsonConvert.DeserializeObject<UserModel>(Result.SysUserData);
                    Return.SysUserId = Result.SysUserId;
                    Return.SysUserRoles = JsonConvert.DeserializeObject<vwUserRole>(Result.SysUserRoles);
                    Return.Token = Result.Token;
                    Return.UserApp = JsonConvert.DeserializeObject<List<vwUserApp>>(Result.UserApp);
                    Return.UserPrincipalName = Result.UserPrincipalName;

                    Model.APFamily.RegisLoginHistory empProfile = _UserRepository.GetEmpProfile(Result.EmployeeID);
                    if (empProfile == null)
                    {
                        Model.APFamily.RegisLoginHistory emp = new Model.APFamily.RegisLoginHistory();
                        emp.EmpCode = Result.EmployeeID;
                        emp.EmpDeviceID = data.DeviceID;
                        emp.EmpName = Result.FirstName;
                        emp.EmpLastName = Result.LastName;
                        emp.PositionName = Result.Division;
                        emp.Email = Result.Email;
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
                            return new
                            {
                                success = true,
                                data = empProfile,
                                Token = empProfile.EmpLoginToken,
                                Message = "LogIn Success!"
                            };
                        }
                        else
                        {
                            return new
                            {
                                success = false,
                                data = empProfile = new Model.APFamily.RegisLoginHistory(),
                                Token = "",
                                Message = "You Have Change you Device! Please Contact IT Admin for further Use!"
                            };
                        }
                    }
                } //ไม่ถ่ายรูป
                return new
                {
                    success = false,
                    data = new AutorizeDataJWT(),
                    Message = "Authentication Fail"
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> UploadPicture(List<IFormFile> PicTure)
        {

            string ipaddress = "5555555";
            StringValues api_key;
            StringValues EmpCode;

            int SuccessUploadCount = 0;
            int count = 0;
            for (int i = 0; i < PicTure.Count(); i++)
            {
                RegisLoginPhoto callResourceDate = new RegisLoginPhoto();

                // -- New ---- for Docker
                var yearPath = DateTime.Now.Year;
                var MonthPath = DateTime.Now.Month;
                var dirPath1 = $"{yearPath}/{MonthPath}";
                int dataPath = 0;
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "data", "uploads", dirPath1);

                string FileBinary;


                long size = PicTure.Sum(f => f.Length);
                string FileExtension = Path.GetExtension(PicTure[i].FileName);  // -------------- > Get File Extention
                var fileName = PicTure[i].FileName;// string.Format("{0}{1}" , DateTime.Now.ToString("DDMMyy") , Path.GetExtension(formFile.FileName)); //Path.GetFileName(Path.GetTempFileName());

                var dirPath = $"{yearPath}\\M{dataPath}";

                // --- New Docker ----
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (PicTure[i].Length > 0)
                {
                    var filePath = Path.Combine(uploads, PicTure[i].FileName);
                    var message = "";
                    if (System.IO.File.Exists(filePath))
                    {
                        string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                        string extension = Path.GetExtension(filePath);
                        string path = Path.GetDirectoryName(filePath);
                        string newFullPath = filePath;
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, Guid.NewGuid().ToString());
                        newFullPath = Path.Combine(path, tempFileName + extension);
                        using (var fileStream = new FileStream(newFullPath, FileMode.Create))
                        {
                            message = PicTure[i].Length.ToString();
                            await PicTure[i].CopyToAsync(fileStream);
                            fileName = tempFileName + extension;

                        }
                    }
                    else
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            message = PicTure[i].Length.ToString();
                            await PicTure[i].CopyToAsync(fileStream);
                        }
                    }

                    // -- -End New -----
                    //if (System.IO.File.Exists(savePath.FullName))
                    if (System.IO.File.Exists(filePath))
                    {
                        string FileExtention = Path.GetExtension(filePath);
                        // ----- Old -----
                        //TresourceData[i].FilePath = "data\\uploads\\" + dirPath + "\\" + fileName;
                        // ----- New Docker -----
                        callResourceDate.VerifyID = 0;
                        callResourceDate.PhotoURL = "";
                        callResourceDate.CreateBy = "System-Register";
                        callResourceDate.CreateDate = DateTime.Now;
                        callResourceDate.UpdateBy = null;
                        callResourceDate.UpdateDate = null;
                        callResourceDate.FileName = fileName;
                        callResourceDate.FilePath = "data/uploads/" + yearPath + "/" + MonthPath + "/" + fileName;
                        callResourceDate.RegisLoginPhotoID = 1;
                        callResourceDate.FileLength = size;
                        bool InsertResult = _UserRepository.InsertRegisLoginPhoto(callResourceDate);
                        return true;  
                    }
                }
            }
            return false;
        }
    }
}