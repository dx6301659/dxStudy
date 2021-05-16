using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace dxStudyZipFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static void ZipSingleFile(Stream stream, string strFileName, string strPassword, string strTargetZipFile)
        {
            stream.Position = 0;
            int intLength = (int)stream.Length;
            byte[] buffer = new byte[intLength];
            using (stream)
            {
                stream.Read(buffer, 0, intLength);
                stream.Close();
            }

            var entry = new ZipEntry(strFileName);
            entry.DateTime = DateTime.Now;
            entry.Size = intLength;

            var zipFileStream = File.Create(strTargetZipFile);
            using (var zipOut = new ZipOutputStream(zipFileStream))
            {
                if (!string.IsNullOrWhiteSpace(strPassword))
                    zipOut.Password = strPassword;

                zipOut.PutNextEntry(entry);
                zipOut.Write(buffer, 0, intLength);
                zipOut.CloseEntry();
                zipOut.Finish();
                zipOut.Close();
            }
        }
    }
}
