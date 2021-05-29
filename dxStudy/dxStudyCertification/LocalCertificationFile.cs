using Jose;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace dxStudyCertification
{
    public class LocalCertificationFile
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
            string strCertPath = @"../../../CertFiles/dxStudyCertification.pfx";
            string strCertPassword = "dx6301659";
            using (var privateCert = new X509Certificate2(strCertPath, strCertPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet))
            {
                var rsa = privateCert.GetRSAPrivateKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    arrResult = rsa.Decrypt(arrByte, RSAEncryptionPadding.OaepSHA256);
                }
            }

            if (arrResult != null)
                return Encoding.Default.GetString(arrResult);

            return null;
        }

        public byte[] RSAEncrypt2(string strContent)
        {
            if (string.IsNullOrWhiteSpace(strContent))
                return null;

            var arrByte = Encoding.Default.GetBytes(strContent);
            string strCertPath = @"../../../CertFiles/dxStudyCertification.cer";
            using (var publicCert = new X509Certificate2(strCertPath))
            {
                var rsa = publicCert.GetRSAPublicKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    return rsa.Encrypt(arrByte, RSAEncryptionPadding.OaepSHA256);
                }
            }
        }

        public string RSADecrypt2(byte[] arrByte)
        {
            if (arrByte == null)
                return null;

            byte[] arrResult = null;
            string strCertPath = @"../../../CertFiles/dxStudyCertification.pfx";
            string strCertPassword = "dx6301659";
            using (var privateCert = new X509Certificate2(strCertPath, strCertPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet))
            {
                var rsa = privateCert.GetRSAPrivateKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    arrResult = rsa.Decrypt(arrByte, RSAEncryptionPadding.OaepSHA256);
                }
            }

            if (arrResult != null)
                return Encoding.Default.GetString(arrResult);

            return null;
        }

        public string RSASignData1(string strRSAEncryptedContent)
        {
            if (string.IsNullOrWhiteSpace(strRSAEncryptedContent))
                return null;

            var arrEncryptedByte = Convert.FromBase64String(strRSAEncryptedContent);
            byte[] arrResult = null;
            string strPrivateCertPath = @"../../../CertFiles/dxStudyCertification2.pfx";
            string strCertPassword = "dx6301659";
            using (var privateCert = new X509Certificate2(strPrivateCertPath, strCertPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet))
            {
                var rsa = privateCert.GetRSAPrivateKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    arrResult = rsa.SignData(arrEncryptedByte, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }

            if (arrResult != null)
                return Convert.ToBase64String(arrResult);

            return null;
        }

        public bool RSAVerifyData1(string strRSAEncryptedContent, string strRSAEncryptedAndSignedContent)
        {
            if (string.IsNullOrWhiteSpace(strRSAEncryptedContent) || string.IsNullOrWhiteSpace(strRSAEncryptedAndSignedContent))
                return false;

            var arrRSAEncryptedContent = Convert.FromBase64String(strRSAEncryptedContent);
            var arrRSAEncryptedAndSignedContent = Convert.FromBase64String(strRSAEncryptedAndSignedContent);
            string strCertPath = @"../../../CertFiles/dxStudyCertification2.cer";
            using (var publicCert = new X509Certificate2(strCertPath))
            {
                var rsa = publicCert.GetRSAPublicKey();
                if (rsa == null)
                    return false;

                using (rsa)
                {
                    return rsa.VerifyData(arrRSAEncryptedContent, arrRSAEncryptedAndSignedContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }
        }

        public byte[] RSASignData2(byte[] arrRSAEncryptedContent)
        {
            if (arrRSAEncryptedContent == null)
                return null;

            string strPrivateCertPath = @"../../../CertFiles/dxStudyCertification2.pfx";
            string strCertPassword = "dx6301659";
            using (var privateCert = new X509Certificate2(strPrivateCertPath, strCertPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet))
            {
                var rsa = privateCert.GetRSAPrivateKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    return rsa.SignData(arrRSAEncryptedContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }
        }

        public bool RSAVerifyData2(byte[] arrRSAEncryptedContent, byte[] arrRSAEncryptedAndSignedContent)
        {
            if (arrRSAEncryptedContent == null || arrRSAEncryptedAndSignedContent == null)
                return false;

            string strCertPath = @"../../../CertFiles/dxStudyCertification2.cer";
            using (var publicCert = new X509Certificate2(strCertPath))
            {
                var rsa = publicCert.GetRSAPublicKey();
                if (rsa == null)
                    return false;

                using (rsa)
                {
                    return rsa.VerifyData(arrRSAEncryptedContent, arrRSAEncryptedAndSignedContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
            }
        }

        public string RSAEncryptByJWT(string strContent)
        {
            if (string.IsNullOrWhiteSpace(strContent))
                return null;

            string strCertPath = @"../../../CertFiles/dxStudyCertification.cer";
            using (var publicCert = new X509Certificate2(strCertPath))
            {
                var rsa = publicCert.GetRSAPublicKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    return JWT.Encode(strContent, rsa, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
                }
            }
        }

        public string RSADecryptByJWT(string strEncryptedString)
        {
            if (string.IsNullOrWhiteSpace(strEncryptedString))
                return null;

            string strCertPath = @"../../../CertFiles/dxStudyCertification.pfx";
            string strCertPassword = "dx6301659";
            using (var privateCert = new X509Certificate2(strCertPath, strCertPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet))
            {
                var rsa = privateCert.GetRSAPrivateKey();
                if (rsa == null)
                    return null;

                using (rsa)
                {
                    return JWT.Decode(strEncryptedString, rsa, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
                }
            }
        }
    }
}
