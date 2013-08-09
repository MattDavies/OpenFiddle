using Microsoft.WindowsAzure.Storage.Table;

namespace csfiddle.Database.Entities
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
    }
}