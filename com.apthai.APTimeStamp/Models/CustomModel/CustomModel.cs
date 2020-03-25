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
   

}
