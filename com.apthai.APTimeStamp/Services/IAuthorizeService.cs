using System.Threading.Tasks;
using com.apthai.APTimeStamp.Data.CustomModels;

namespace com.apthai.CoreApp.Data.Services
{
    public interface IAuthorizeService
    {
        Task<WSAuthorizeModel> UserLoginAsync(string userName, string password, string AppCode = "QIS");
        //bool AccessKeyAuthentication(string AC, string EmpCode);
       
    }
}