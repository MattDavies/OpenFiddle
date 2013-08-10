using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace csfiddle.Database.Entities
{
    public class Log : TableEntity
    {
        public Log()
        {
            PartitionKey = DateTime.UtcNow.Ticks.ToString();
            RowKey = DateTime.UtcNow.Ticks.ToString();
        }

        public string InputCode { get; set; }
        public string IpAddress { get; set; }
    }
}