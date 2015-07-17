using Microsoft.WindowsAzure.Storage.Table;
using OpenFiddle.Models.Shared;

namespace OpenFiddle.Repos.Data
{
    public class Fiddle : TableEntity
    {
        public string InputCode { get; set; }

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            { 
                _id = value;
                PartitionKey = value;
                RowKey = value;
            }
        }

        public string Result { get; set; }

        public Language Language { get; set; }
    }
}