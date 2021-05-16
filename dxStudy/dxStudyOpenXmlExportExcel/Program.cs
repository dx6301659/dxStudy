using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace dxStudyOpenXmlExportExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            string strOutputFilePath = @"MockFile\134.xlsx";
            var dataSet = CreateDataSet();
            //ExportDataSetToFileMultipleSheets(dataSet, true, strOutputFilePath);

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //var memoryStream = ExportDataSetToStreamMultipleSheets(dataSet, true);
            //var fileStream = new FileStream(strOutputFilePath, FileMode.Create);
            //fileStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            //fileStream.Close();
            //fileStream.Dispose();
            //memoryStream.Close();
            //memoryStream.Dispose();

            //ZipSingleFile(memoryStream, "123.xlsx", "123", @"C:\Users\dingxu\Desktop\Result.zip");

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //ExportDataSetToFileOneSheet(dataSet, true, null, strOutputFilePath);
            ////ExportDataSetToFileOneSheet(dataSet, true, "dxTest", strOutputFilePath);

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //var memoryStream = ExportDataSetToStreamOneSheet(dataSet, true, null);
            ////var memoryStream = ExportDataSetToStreamOneSheet(dataSet, true, "dxTest");
            ////var fileStream = new FileStream(strOutputFilePath, FileMode.Create);
            ////fileStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            ////fileStream.Close();
            ////fileStream.Dispose();
            ////memoryStream.Close();
            ////memoryStream.Dispose();

            //ZipSingleFile(memoryStream, "123.xlsx", "123", @"C:\Users\dingxu\Desktop\Result.zip");
            Console.WriteLine("Hello World!");
        }

        static DataSet CreateDataSet()
        {
            var dataSet = new DataSet();

            //var dataTable1 = new DataTable("Header");
            var dataTable1 = new DataTable();
            var dataColumn1_1 = new DataColumn("Scheme Scope");
            var dataColumn1_2 = new DataColumn("Org Scope");
            dataTable1.Columns.Add(dataColumn1_1);
            dataTable1.Columns.Add(dataColumn1_2);

            //var dataTable2 = new DataTable("Record");
            var dataTable2 = new DataTable();
            var dataColumn2_1 = new DataColumn("Nric");
            var dataColumn2_2 = new DataColumn("Age");
            var dataColumn2_3 = new DataColumn("Address");
            dataTable2.Columns.Add(dataColumn2_1);
            dataTable2.Columns.Add(dataColumn2_2);
            dataTable2.Columns.Add(dataColumn2_3);

            var dataRow1 = dataTable1.NewRow();
            dataRow1["Scheme Scope"] = "Test scheme";
            dataRow1["Org Scope"] = "Test org";
            dataTable1.Rows.Add(dataRow1);

            var dataRow2_1 = dataTable2.NewRow();
            dataRow2_1["Nric"] = "dxTestNric1";
            dataRow2_1["Age"] = "dxTestAge1";
            dataRow2_1["Address"] = "dxTestAddress1";
            dataTable2.Rows.Add(dataRow2_1);

            var dataRow2_2 = dataTable2.NewRow();
            dataRow2_2["Nric"] = "dxTestNric2";
            dataRow2_2["Age"] = "dxTestAge2";
            dataRow2_2["Address"] = "dxTestAddress2";
            dataTable2.Rows.Add(dataRow2_2);

            var dataRow2_3 = dataTable2.NewRow();
            dataRow2_3["Nric"] = "dxTestNric3";
            dataRow2_3["Age"] = "dxTestAge3";
            dataRow2_3["Address"] = "dxTestAddress3";
            dataTable2.Rows.Add(dataRow2_3);

            dataSet.Tables.Add(dataTable1);
            dataSet.Tables.Add(dataTable2);

            return dataSet;
        }

    }
}
