using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using OpenFiddle.Database.Entities;

namespace OpenFiddle.Database.Repositories
{
    public class LogRepository
    {
        private CloudTable _table;

        public LogRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString;
            var storageAccount = connectionString == "useDevelopmentStorage=true"
                ? CloudStorageAccount.DevelopmentStorageAccount
                : CloudStorageAccount.Parse(connectionString);

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
