﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.Jobs.Host.Protocols;
using Microsoft.Azure.Jobs.Host.Timers;
using Microsoft.WindowsAzure.Storage;

namespace Microsoft.Azure.Jobs.Host.Executors
{
    internal class UpdateHostHeartbeatCommand : ICanFailCommand
    {
        private readonly IHeartbeatCommand _heartbeatCommand;

        public UpdateHostHeartbeatCommand(IHeartbeatCommand heartbeatCommand)
        {
            if (heartbeatCommand == null)
            {
                throw new ArgumentNullException("heartbeatCommand");
            }

            _heartbeatCommand = heartbeatCommand;
        }

        public bool TryExecute()
        {
            try
            {
                _heartbeatCommand.Beat();
                return true;
            }
            catch (StorageException exception)
            {
                if (exception.IsServerSideError())
                {
                    return false;
                }

                throw;
            }
        }
    }
}
