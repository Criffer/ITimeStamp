using com.apthai.APTimeStamp.CustomModel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.Repositories
{
    public interface IUserRepository
    {
        Model.APFamilyModel.EmpProfile GetEmpProfile(string EmpCode);



    }
}