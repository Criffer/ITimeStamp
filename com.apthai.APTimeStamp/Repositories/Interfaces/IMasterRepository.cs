using com.apthai.APTimeStamp.CustomModel;
using com.apthai.APTimeStamp.Model;
using com.apthai.APTimeStamp.Model.APFamily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.apthai.APTimeStamp.Repositories.Interfaces
{
    public interface IMasterRepository
    {
        List<ManagementBeacon> GetAllBeaconDatas();
        //List<calltype> GetCallCallType_Sync();
        //List<callarea> GetCallAreaByProductCat_Sync(string ProductTypeCate);
        //callTDefect GetCallTDefect_Sync(int TDefectID);
    }
}
