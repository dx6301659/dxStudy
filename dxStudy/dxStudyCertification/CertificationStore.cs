using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace dxStudyCertification
{
    public class CertificationStore
    {
        public string RSAEncrypt1(string strContent)
        {
            if (string.IsNullOrWhiteSpace(strContent))
                return null;

            var arrByte = Encoding.Default.GetBytes(strContent);
            byte[] arrResult = null;
            string strCertPath = @"../../../CertFiles/dxStudyCertification.cer";
            using (var publicCert = new X509Certificate2(strCertPath))
            {
                var rsa = publicCert.GetRSAPublicKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    arrResult = rsa.Encrypt(arrByte, RSAEncryptionPadding.OaepSHA256);
                }
            }

            if (arrResult != null)
                return Convert.ToBase64String(arrResult);

            return null;
        }

        public string RSADecrypt1(string strEncryptedString)
        {
            if (string.IsNullOrWhiteSpace(strEncryptedString))
                return null;

            var arrByte = Convert.FromBase64String(strEncryptedString);
            byte[] arrResult = null;
            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                //find the pfx cert dxStudyCertification,!!!!!notice: need to set the pfx could export pfx key when import the pfx cert file
                X509Certificate2Collection resultCollection = certStore.Certificates.Find(X509FindType.FindBySerialNumber, "5cf997299bbe568d4e08d1dfc589ed48", false);
                if (resultCollection != null && resultCollection.Count > 0)
                {
                    using (var privateCert = resultCollection[0])
                    {
                        var rsa = privateCert.GetRSAPrivateKey();
                        if (rsa == null)
                            return null;

                        using (rsa)
                        {
                            arrResult = rsa.Decrypt(arrByte, RSAEncryptionPadding.OaepSHA256);
                        }
                    }
                }
            }

            if (arrResult != null)
                return Encoding.Default.GetString(arrResult);

            return null;
        }
    }
}
