using Dapper;
using com.apthai.APTimeStamp.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Hosting;
using com.apthai.APTimeStamp.CustomModel;
using com.apthai.APTimeStamp.Model.APFamily;

namespace com.apthai.APTimeStamp.Repositories
{
    public class MasterRepository : BaseRepository, IMasterRepository
    {


        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        string TAG = "MasterRepository";

        public MasterRepository(IHostingEnvironment environment, IConfiguration config) : base(environment, config)
        {
            _config = config;
            _hostingEnvironment = environment;

        }
        public async Task<int> TanonchaiJobSample()
        {
            int a = 20;
            int b = 60;
            int c = a + b;
            return c;
        }

        public List<ManagementBeacon> GetAllBeaconDatas()
        {
            using (IDbConnection conn = WebConnection)
            {
                try
                {
                        string sQuery = "Select * From ManagementBeacons where IsActiveBeacons = 1 ";
                        var result = conn.Query<ManagementBeacon>(sQuery).ToList();
                        return result;
                   
                }
                catch (Exception ex)
                {
                    throw new Exception("MasterRepository.GetAllBeaconDatas() :: Error ", ex);
                }
            }
        }
        //public List<calltype> GetCallCallType_Sync()
        //{
        //    using (IDbConnection conn = WebConnection)
        //    {
        //        try
        //        {
        //            string sQuery = "Select * From callType where Active = 1 ";
        //            var result = conn.Query<calltype>(sQuery).ToList();
        //            return result;

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("MasterRepository.GetCallAreaByProductCat_Sync() :: Error ", ex);
        //        }
        //    }
        //}
        //public callTDefect GetCallTDefect_Sync(int TDefectID)
        //{
        //    using (IDbConnection conn = WebConnection)
        //    {
        //        try
        //        {
        //            string sQuery = "Select * From callTDefect " +
        //                "where TDefectID = @TDefectID And DocIsActive = 1 ";
        //            var result = conn.Query<callTDefect>(sQuery, new { TDefectID = TDefectID }).FirstOrDefault();
        //            return result;

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("MasterRepository.GetCallAreaByProductCat_Sync() :: Error ", ex);
        //        }
        //    }
        //}
    }
}
