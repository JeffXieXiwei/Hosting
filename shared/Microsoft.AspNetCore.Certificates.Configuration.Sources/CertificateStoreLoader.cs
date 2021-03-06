﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.AspNetCore.Certificates.Configuration
{
    /// <summary>
    /// Loads certificates from certificate stores.
    /// </summary>
    internal class CertificateStoreLoader : ICertificateStoreLoader
    {
        /// <summary>
        /// Load a ceritificate from the given store location.
        /// </summary>
        /// <param name="subject">The certificate subject name to match.</param>
        /// <param name="storeName">The store to open.</param>
        /// <param name="storeLocation">The store location to open.</param>
        /// <param name="validOnly">Only load currently valid certs.</param>
        /// <returns>The closest matching vertificate.</returns>
        public X509Certificate2 Load(string subject, string storeName, StoreLocation storeLocation, bool validOnly)
        {
            using (var store = new X509Store(storeName, storeLocation))
            {
                X509Certificate2Collection storeCertificates = null;
                X509Certificate2Collection foundCertificates = null;
                X509Certificate2 foundCertificate = null;

                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    storeCertificates = store.Certificates;
                    foundCertificates = storeCertificates.Find(X509FindType.FindBySubjectDistinguishedName, subject, validOnly);
                    foundCertificate = foundCertificates
                        .OfType<X509Certificate2>()
                        .OrderByDescending(certificate => certificate.NotAfter)
                        .FirstOrDefault();

                    return foundCertificate;
                }
                finally
                {
                    if (foundCertificate != null)
                    {
                        storeCertificates.Remove(foundCertificate);
                        foundCertificates.Remove(foundCertificate);
                    }

                    DisposeCertificates(storeCertificates);
                    DisposeCertificates(foundCertificates);
                }
            }
        }

        private void DisposeCertificates(X509Certificate2Collection certificates)
        {
            if (certificates != null)
            {
                foreach (var certificate in certificates)
                {
                    certificate.Dispose();
                }
            }
        }
    }
}
