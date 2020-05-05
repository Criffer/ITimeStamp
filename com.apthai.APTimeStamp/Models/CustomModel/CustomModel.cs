using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.CustomModel
{
    public class GetUserCRMPhoneNumber
    {
        //public List<Model.CRMWeb.ContactPhone> contactPhones { get; set; }
        public string FirstNameTH { get; set; }
        public string LastNameTH { get; set; }
        public string CitizenIdentityNo { get; set; }
        public bool IsVIP { get; set; }
    }
    public class ThaiBulkOTPRequest
    {
        public string key { get; set; }
        public string secret { get; set; }
        public string msisdn { get; set; }
    }
    public class ThaiBulkVerifyOTP
    {
        public string token { get; set; }
        public string pin { get; set; }
    }
    public class thaiBulkOTPRequestReturnObj
    {
        public ThaiBulkOTPRequestReturn data { get; set; }
    }
    public class ThaiBulkOTPRequestReturn
    {
        public string status { get; set; }
        public string token { get; set; }
    }
    public class thaiBulkOTPVerifyReturnObj
    {
        public ThaiBulkOTPRequestReturn data { get; set; }
    }
    public class thaiBulkOTPVerifyReturn
    {
        public string status { get; set; }
        public string token { get; set; }
    }
    public class UserModel
    {
        public vwUser User { get; set; }
        public string TitleMsg { get; set; }
        public string RedirectMsg { get; set; }
        public string TypeMsg { get; set; }

    }
    public class vwUser
    {
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public string Email { get; set; }
        public string FullCodeName { get; set; }
        public string UserNameLogin { get; set; }
        public string RGUID { get; set; }
    }
    public partial class vwUserRole
    {
        public int? ID { get; set; }
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public int RoleID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string AssignType { get; set; }
        public string SourceType { get; set; }
        public string Remark { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public partial class UserProjectType
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string AssignType { get; set; }
        public string SourceType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Remark { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string producttypecate { get; set; }
    }
    public partial class vwUserApp
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string UserGUID { get; set; }
        public string EmpCode { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int AppID { get; set; }
        public string AppCode { get; set; }
        public string AppName { get; set; }
        public string AssignType { get; set; }
        public string SourceType { get; set; }
        public string PositionName { get; set; }
        public string Remark { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
