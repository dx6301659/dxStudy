using System;

namespace dxStudyCertification
{
    class Program
    {
        static void Main(string[] args)
        {
            var localCert = new LocalCertificationFile();
            string strEncryptedContent1 = localCert.RSAEncrypt1("丁旭测试数据");
            string strSignedAndEncryptedContent1 = localCert.RSASignData1(strEncryptedContent1);
            bool blnVerifySignResult1 = localCert.RSAVerifyData1(strEncryptedContent1, strSignedAndEncryptedContent1);
            string strDecryptedContent1 = localCert.RSADecrypt1(strEncryptedContent1);

            var arrEncryptedContent2 = localCert.RSAEncrypt2("丁旭测试数据");
            var arrSignedAndEncryptedContent2 = localCert.RSASignData2(arrEncryptedContent2);
            bool blnVerifySignResult2 = localCert.RSAVerifyData2(arrEncryptedContent2, arrSignedAndEncryptedContent2);
            string strDecryptedContent2 = localCert.RSADecrypt2(arrEncryptedContent2);

            string strEncryptedString = localCert.RSAEncryptByJWT("丁旭测试数据");
            string strDecryptedString = localCert.RSADecryptByJWT(strEncryptedString);

            var storeCert = new CertificationStore();
            string strEncryptedContent4 = storeCert.RSAEncrypt1("丁旭测试数据");
            string strDecryptedContent4 = storeCert.RSADecrypt1(strEncryptedContent4);

            Console.WriteLine("Hello World!");
        }
    }
}
