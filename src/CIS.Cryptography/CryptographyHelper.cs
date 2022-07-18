using System;
using System.Collections.Generic;
using System.Linq;
using CAdESCOM;
using CAPICOM;

namespace CIS.Cryptography
{
    /// <summary>
    /// Crypto data methods.
    /// </summary>
    public static class CryptographyHelper
    {
        private const string STORE_NAME_MY = "My";

        /// <summary>
        /// Method for signing data Base64 format
        /// </summary>
        /// <param name="base64stringToSign">Base64 string data to sign</param>
        /// <param name="detached">Wheither signature is detached</param>
        /// <param name="thumbprint">The thumbprint string of certificate. Don't specify to let user choose. </param>
        /// <returns></returns>
        public static string SignBase64Data(string base64stringToSign, bool detached = false, string thumbprint = null)
        {
            var store = new CAdESCOM.CPStoreClass();
            store.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, STORE_NAME_MY, CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_EXISTING_ONLY);

            ICertificates certificates = store.Certificates;

            store.Close();
            if (certificates.Count == 0)
                throw new Exception("Не найдено ни одного сертификата.");

            CPSignerClass oSigner = null;

            if (thumbprint != null)
            {
                oSigner = new CPSignerClass();

                foreach (ICertificate cert in certificates)
                    if (cert.Thumbprint.ToUpper() == (thumbprint).ToUpper())
                        oSigner.Certificate = cert;
            }

            var oSignedData = new CAdESCOM.CadesSignedData
            {
                ContentEncoding = CADESCOM_CONTENT_ENCODING_TYPE.CADESCOM_BASE64_TO_BINARY,
                Content = base64stringToSign
            };

            var signedData = oSignedData.SignCades(oSigner, CADESCOM_CADES_TYPE.CADESCOM_CADES_BES, detached, CAdESCOM.CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);

            signedData = signedData.Replace("\n", String.Empty).Replace("\r", String.Empty);

            #region VerifySignature
            //try
            //{
            //    oSignedData.VerifyCades(signedData, CADESCOM_CADES_TYPE.CADESCOM_CADES_BES);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion

            return signedData;
        }

        /// <summary>
        /// Method to get certificate boxed into [object] type
        /// </summary>
        /// <param name="onlyActive"></param>
        /// <param name="ordered"></param>
        /// <returns></returns>
        public static List<CISCertificate> GetCertificates(bool onlyActive = false, bool ordered = false)
        {
            return GetICertificates(onlyActive, ordered).Select(c => new CISCertificate(c)).ToList();
        }

        /// <summary>
        /// Return certificate collection
        /// </summary>
        /// <param name="onlyActive">get only active certificates</param>
        /// <param name="ordered">get ordered certificate list by date</param>
        /// <returns></returns>
        private static List<ICertificate> GetICertificates(bool onlyActive = false, bool ordered = false)
        {
            try
            {
                var store = new CAdESCOM.CPStoreClass();
                store.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, STORE_NAME_MY,
                    CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_EXISTING_ONLY);

                ICertificates certificates = store.Certificates;
                List<ICertificate> resultList = new List<ICertificate>();

                store.Close();

                foreach (Certificate cert in certificates)
                {
                    if (onlyActive && cert.IsValid().Result)
                    {
                        resultList.Add(cert);
                    }
                }

                return ordered ? resultList.OrderByDescending(c => c.ValidToDate).ToList() : resultList;

            }
            catch (Exception ex)
            {
                throw new Exception("При выборе сертификата произошла ошибка", innerException: ex);
            }
        }
    }
}
