﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace peroline.FaultTolerance
{
    public class Db : DbContext
    {
        public Db()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{Path.DirectorySeparatorChar}retryState.db";
        }

        public DbSet<ErrorMemo> ErrorMemo { get; set; }
        public string DbPath { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class ErrorMemo
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }

        public string ErrorType { get; set; }

        public string RetriedByBotId { get; set; }
        public bool HasBeenRetried { get; set; }
        public int NumOfRetry { get; set; }

        // 1. in-flight app recovery
        // 2. submit recovery 
        public string RecoveryFor { get; set; }

        public string DataForRetry { get; set; }
    }
}