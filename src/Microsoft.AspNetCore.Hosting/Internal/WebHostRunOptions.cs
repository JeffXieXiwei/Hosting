﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Hosting.Internal
{
    public class WebHostRunOptions
    {
        /// <summary>
        /// Indicates if host startup status messages should be written to the console.
        /// The default is true.
        /// </summary>
        public bool WriteStatusMessages { get; set; } = true;
    }
}
