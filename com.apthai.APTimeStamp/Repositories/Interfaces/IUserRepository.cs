using com.apthai.APTimeStamp.CustomModel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.Repositories
{
    public interface IUserRepository
    {
        Model.APFamily.EmpProfile GetEmpProfile(string EmpCode);
        bool InsertEmpProfile(Model.APFamily.EmpProfile data);
        bool UpdateEmpProfile(Model.APFamily.EmpProfile data);

    }
}