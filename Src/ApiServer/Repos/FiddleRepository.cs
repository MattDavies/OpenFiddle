using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using OpenFiddle.Repos.Data;

namespace OpenFiddle.Repos
{
    public interface IFiddleRepository
    {
        Fiddle Get(string id);
        void Insert(Fiddle fiddle);
    }

    public class FiddleRepository : IFiddleRepository
    {
        private CloudTable _table;

        public FiddleRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString;
            var storageAccount = connectionString == "useDevelopmentStorage=true"
                ? CloudStorageAccount.DevelopmentStorageAccount
                : CloudStorageAccount.Parse(connectionString);

            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(typeof(Fiddle).Name);
            _table.CreateIfNotExists();
        }

        public Fiddle Get(string id)
        {
            return _table.CreateQuery<Fiddle>().AsQueryable().Where(f => f.PartitionKey == id && f.RowKey == id).Take(1).FirstOrDefault();
        }

        public void Insert(Fiddle fiddle)
        {
            var insert = TableOperation.InsertOrReplace(fiddle);
            _table.Execute(insert);
        }
    }
}
