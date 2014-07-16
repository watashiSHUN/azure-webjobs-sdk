﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Dashboard.Data.Logs
{
    public interface IIndexerLogReader
    {
        IndexerLogEntry ReadWithDetails(string logEntryId);

        IResultSegment<IndexerLogEntry> ReadWithoutDetails(int maximumResults, string continuationToken);
    }
}
