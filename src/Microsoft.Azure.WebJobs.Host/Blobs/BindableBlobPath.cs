﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;

namespace Microsoft.Azure.WebJobs.Host.Blobs
{
    internal static class BindableBlobPath
    {
        public static IBindableBlobPath Create(string pattern, bool isContainerBinding = false)
        {
            var containerBlob = BlobPath.TryConvertAbsUrlToContainerBlob(pattern);
            BlobPath parsedPattern = null;
            if (!string.IsNullOrEmpty(containerBlob))
            {
                parsedPattern = BlobPath.Parse(containerBlob, isContainerBinding);
            }
            else
            {
                parsedPattern = BlobPath.Parse(pattern, isContainerBinding);
            }

            if (parsedPattern == null)
            {
                BindingTemplate urlTemplate = BindingTemplate.FromString(pattern);
                if (urlTemplate.ParameterNames.Count() == 1)
                {
                    return new ParameterizedBlobPath(urlTemplate);
                }
                throw new FormatException($"Invalid blob path '{pattern}'. Paths must be in the format 'container/blob' or 'blob Url'.");
            }

            BindingTemplate containerNameTemplate = BindingTemplate.FromString(parsedPattern.ContainerName);
            BindingTemplate blobNameTemplate = BindingTemplate.FromString(parsedPattern.BlobName);

            if (containerNameTemplate.HasParameters || blobNameTemplate.HasParameters)
            {
                return new ParameterizedBlobPath(containerNameTemplate, blobNameTemplate);
            }

            BlobClient.ValidateContainerName(parsedPattern.ContainerName);
            if (!string.IsNullOrEmpty(parsedPattern.BlobName))
            {
                BlobClient.ValidateBlobName(parsedPattern.BlobName);
            }
            return new BoundBlobPath(parsedPattern);
        }
    }
}
