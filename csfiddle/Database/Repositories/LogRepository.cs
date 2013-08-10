using System.Configuration;
using System.Linq;
using csfiddle.Database.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace csfiddle.Database.Repositories
{
    public class LogRepository
    {
        private CloudTable _table;

        public LogRepository()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(typeof(Log).Name);
            _table.CreateIfNotExists();
        }

        public void Insert(Log fiddle)
        {
            var insert = TableOperation.InsertOrReplace(fiddle);
            _table.Execute(insert);
        }
    }
}
