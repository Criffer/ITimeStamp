using com.apthai.APTimeStamp.CustomModel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.Repositories
{
    public interface IUserRepository
    {
        Model.APFamily.RegisLoginHistory GetEmpProfile(string EmpCode);
        bool InsertEmpProfile(Model.APFamily.RegisLoginHistory data);
        bool UpdateEmpProfile(Model.APFamily.RegisLoginHistory data);
        bool InsertRegisLoginPhoto(Model.APFamily.RegisLoginPhoto data);
        List<Model.APFamily.CheckinHistoty> GetCheckinHistories(string EmpCode, int days);
    }
}