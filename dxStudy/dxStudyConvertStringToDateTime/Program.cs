using System;

namespace dxStudyConvertStringToDateTime
{
    class Program
    {
        static void Main(string[] args)
        {
            string strTest = "123,";
            var arrString = strTest.Split(',');
            //string strRowIndex = Regex.Replace(null, "[a-zA-Z]", "");  // exception
            bool blnResult = string.Equals(null, null);

            var dtResult1 = ConvertStringToNullableDateTime(null);
            var dtResult2 = ConvertStringToNullableDateTime("");
            var dtResult3 = ConvertStringToNullableDateTime("ABC");
            var dtResult4 = ConvertStringToNullableDateTime("01");
            var dtResult5 = ConvertStringToNullableDateTime("10");
            var dtResult6 = ConvertStringToNullableDateTime("44315");
            var dtResult7 = ConvertStringToNullableDateTime("2021-4-23");
            var dtResult8 = ConvertStringToNullableDateTime("2021-23-4");
            var dtResult9 = ConvertStringToNullableDateTime("2021/23/4");
            var dtResult10 = ConvertStringToNullableDateTime("23/4/2021");
            var dtResult11 = ConvertStringToNullableDateTime("4/23/2021");

            Console.WriteLine("Hello World!");
        }

        static DateTime? ConvertStringToNullableDateTime(string strDateTime)
        {
            if (string.IsNullOrWhiteSpace(strDateTime))
                return null;

            DateTime dtResult = DateTime.Today;
            bool blnIsParseCorrect = DateTime.TryParse(strDateTime, out dtResult);
            if (blnIsParseCorrect)
                return dtResult;

            double dblOADate = -1;
            blnIsParseCorrect = double.TryParse(strDateTime, out dblOADate);
            if (!blnIsParseCorrect)
                return null;

            return DateTime.FromOADate(dblOADate);
        }
    }
}
