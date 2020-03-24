using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace com.apthai.CoreApp.Data.Services
{
    public interface IDataCrawlerServices
    {
        //(MProject, List<SyncModelUnitData>, List<com.apthai.APTimeStamp.Model.DefectAPISync.TResource>) UnitDataCrawler(Guid sessionId, IEnumerable<JToken> jsonListObjects, DateTime syncDate, string sessionUserId, string userRole, Dictionary<string, string> fileTemp, ref List<int> returnIds, ref Dictionary<string, string> files_copy);
    }
}