
using com.apthai.APTimeStamp.CustomModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.HttpRestModel
{
    public partial class LoginData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AppCode { get; set; }
        public string DeviceID { get; set; }
        public IFormFile UserLoginImage { get; set; }
    }
    public partial class Register
    {
        public string PhoneNumber { get; set; }
        public int PINCode { get; set; }
        public string CitizenIdentityNo { get; set; }
    }
    public partial class CheckPinParam
    {
        public int PINCode { get; set; }
        public string AccessKey { get; set; }
        public string DeviceID { get; set; }
    }
    public partial class CheckInHistoryParam
    {
        public string EmpCode { get; set; }
        public int Days { get; set; }
    }
    public partial class GetBeaconDataParam
    {
        public string BeaconID { get; set; }
    }
    public class AutorizeDataJWT
    {
        public bool LoginResult { get; set; }
        public string LoginResultMessage { get; set; }
        public string UserPrincipalName { get; set; }
        public string DomainUserName { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public string Division { get; set; }

        public string Token { get; set; }

        public DateTime? AccountExpirationDate { get; set; }
        public DateTime? LastLogon { get; set; }

        public string AuthenticationProvider { get; set; }
        public string SysUserId { get; set; }
        public string SysUserData { get; set; }
        public string SysUserRoles { get; set; }
        public string SysAppCode { get; set; }
        public string AppUserRole { get; set; }
        public string UserProject { get; set; }
        public string UserApp { get; set; }
        
    }
    public class AutorizeDataJWTReturnObject
    {
        public bool LoginResult { get; set; }
        public string LoginResultMessage { get; set; }
        public string UserPrincipalName { get; set; }
        public string DomainUserName { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public string Division { get; set; }

        public string Token { get; set; }

        public DateTime? AccountExpirationDate { get; set; }
        public DateTime? LastLogon { get; set; }

        public string AuthenticationProvider { get; set; }
        public string SysUserId { get; set; }
        public UserModel SysUserData { get; set; }
        public vwUserRole SysUserRoles { get; set; }
        public string SysAppCode { get; set; }
        public string AppUserRole { get; set; }
        public List<UserProjectType> UserProject { get; set; }
        public List<vwUserApp> UserApp { get; set; }

    }
    public class GetCAllArea
    {
        public string ProductTypeCate { get; set; }
        public string AccessKey { get; set; }
        public string EmpCode { get; set; }
    }
    public class callTDefectObj
    {
        public int TDefectID { get; set; }
        public string AccessKey { get; set; }
        public string EmpCode { get; set; }
    }
    public class CreateDefectTransactionParam
    {
        public string DefectType { get; set; }
        public string ProductID { get; set; }
        public string ItemID { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public string AccessKey { get; set; }
        public string EmpCode { get; set; }
    }
    public class GetCAllType
    {
        public string AccessKey { get; set; }
        public string EmpCode { get; set; }
    }
    public class RegisterData
    {
        public string CitizenIdentityNo { get; set; }
        public string PhoneNumber { get; set; }
        public string DeviceID { get; set; }
        public string DeviceType { get; set; }
        public int PINCode { get; set; }
    }
    public class RequestOTPParam
    {
        public string PhoneNumber { get; set; }
    }
}
