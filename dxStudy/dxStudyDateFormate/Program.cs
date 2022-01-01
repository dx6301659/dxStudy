// See https://aka.ms/new-console-template for more information

string strDateFormat1 = "yyyy-MM-dd";
string strDateFormat2 = "YYYY-MM-dd";  // wrong format
string strDateFormat3 = "yyyy-mm-dd";  // wrong format
DateTime dtTest = new DateTime(2019, 12, 31);

string strResult1 = dtTest.ToString(strDateFormat1);
string strResult2 = dtTest.ToString(strDateFormat2);
string strResult3 = dtTest.ToString(strDateFormat3);

Console.WriteLine($"{strResult1}\t{strResult2}\t{strResult3}");

Console.WriteLine("Hello, World!");

Console.ReadKey();
