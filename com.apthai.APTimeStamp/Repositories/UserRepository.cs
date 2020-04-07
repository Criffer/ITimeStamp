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

namespace com.apthai.APTimeStamp.Repositories
{
    public class UserRepository : BaseRepository , IUserRepository
    {

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserRepository(IHostingEnvironment environment, IConfiguration config) : base(environment, config)
        {
            _config = config;
            _hostingEnvironment = environment;

        }
        //---------------- Log In Module --------------------------------------
        public Model.APFamily.EmpProfile GetEmpProfile(string EmpCode)
        {
            using (IDbConnection conn = WebConnection)
            {
                conn.Open();
                var result = conn.Query<Model.APFamily.EmpProfile>("select * from EmpProfile WITH(NOLOCK) " +
                    "where EmpCode=@EmpCode", new { EmpCode = EmpCode }).FirstOrDefault();

                return result;
            }
        }

        public bool InsertEmpProfile(Model.APFamily.EmpProfile data)
        {
            using (IDbConnection conn = MobileConnection)
            {
                try
                {
                    conn.Open();
                    var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    var result = conn.Insert(data, tran);
                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("MasterRepository.InsertEmpProfile() :: Error ", ex);
                }
            }
        }
        public bool UpdateEmpProfile(Model.APFamily.EmpProfile data)
        {
            using (IDbConnection conn = MobileConnection)
            {
                try
                {
                    conn.Open();
                    var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                    var result = conn.Update(data, tran);
                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("MasterRepository.InsertEmpProfile() :: Error ", ex);
                }
            }
        }
        //---------------------------------------------------------------------
    }

}
