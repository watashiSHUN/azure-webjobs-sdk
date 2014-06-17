﻿using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Azure.Jobs
{
    class StorageConverter : StringConverter<CloudStorageAccount>
    {
        public override string GetAsString(CloudStorageAccount value)
        {
            return value.ToString(exportSecrets: true);
        }
        public override object ReadFromString(string value)
        {
            return CloudStorageAccount.Parse(value);
        }
    }
}
