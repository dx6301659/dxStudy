using System;
using System.IO;
using System.Net;

namespace dxStudyDownloadFileByURL
{
    class Program
    {
        static void Main(string[] args)
        {
            string strURLFilePath = @"C:\Users\ding_\Desktop\URLPath.txt";
            string strSavedPath = @"D:\迅雷下载\123\东北潘某\";

            //read path from the file
            using (var fs = new FileStream(strURLFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    string str = sr.ReadLine();
                    while (!string.IsNullOrWhiteSpace(str))
                    {
                        //DownloadFileAsync(str, strSavedPath);
                        DownloadFileSync(str, strSavedPath);
                        str = sr.ReadLine();
                    }
                }
            }

            Console.WriteLine("Download completed!");
        }

        static async void DownloadFileAsync(string strFileURL, string strSavePath)
        {
            string strExtensionName = Path.GetExtension(strFileURL);
            string strSaveFilePath = $"{strSavePath}{Guid.NewGuid().ToString()}.{strExtensionName}";

            using (var webClient = new WebClient())
            {
                await webClient.DownloadFileTaskAsync(strFileURL, strSaveFilePath);
                Console.WriteLine($"Download complete :{strFileURL}");
            }
        }

        static void DownloadFileSync(string strFileURL, string strSavePath)
        {
            string strExtensionName = Path.GetExtension(strFileURL);
            string strSaveFilePath = $"{strSavePath}{Guid.NewGuid().ToString()}.{strExtensionName}";

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(strFileURL, strSaveFilePath);
            }

            Console.WriteLine($"Download complete :{strFileURL}");
        }
    }
}
