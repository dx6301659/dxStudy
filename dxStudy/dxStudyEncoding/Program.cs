using System;
using System.Text;

namespace dxStudyEncoding
{
    class Program
    {
        static void Main(string[] args)
        {
            var arrTestByteLength = Encoding.Default.GetBytes("123");
            arrTestByteLength = Encoding.Default.GetBytes("12d");
            arrTestByteLength = Encoding.Default.GetBytes("丁");
            arrTestByteLength = Encoding.Default.GetBytes("丁旭");
            arrTestByteLength = Encoding.Default.GetBytes("12d丁");

            arrTestByteLength = Encoding.UTF8.GetBytes("123");
            arrTestByteLength = Encoding.UTF8.GetBytes("12d");
            arrTestByteLength = Encoding.UTF8.GetBytes("丁");
            arrTestByteLength = Encoding.UTF8.GetBytes("丁旭");
            arrTestByteLength = Encoding.UTF8.GetBytes("12d丁");

            arrTestByteLength = Encoding.Unicode.GetBytes("123");
            arrTestByteLength = Encoding.Unicode.GetBytes("12d");
            arrTestByteLength = Encoding.Unicode.GetBytes("丁");
            arrTestByteLength = Encoding.Unicode.GetBytes("丁旭");
            arrTestByteLength = Encoding.Unicode.GetBytes("12d丁");

            arrTestByteLength = Encoding.ASCII.GetBytes("123");
            arrTestByteLength = Encoding.ASCII.GetBytes("12d");
            arrTestByteLength = Encoding.ASCII.GetBytes("丁");
            arrTestByteLength = Encoding.ASCII.GetBytes("丁旭");
            arrTestByteLength = Encoding.ASCII.GetBytes("12d丁");

            Console.WriteLine("Hello World!");
        }
    }
}
