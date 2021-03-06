﻿using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public class Rds: _BaseClass
    {
        public Rds() { }
        public Rds(IGlobalSettings globalSettings): base(globalSettings) { }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public bool DeletionProtection { get; set; } = true;
        public bool MultiAz { get; set; }
        public int BackupRetentionInDays { get; set; }
        public bool StorageEncrypted { get; set; }
        public bool AutoMinorVersionUpgrade { get; set; }

    }
}
