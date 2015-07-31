﻿using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace OpenFiddle.Repos.Data
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