using System;
using System.Collections.Generic;
using System.IO;

namespace dxStudyOpenXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string strFilePath = @"../../../MockFile\134.xlsx";
            string strSheetName = "BQ1-IT";
            string strTableName = "Table2";
            string[] arrColumn = { "UIN", "All Schemes" };
            //strFilePath = Path.GetFullPath(strFilePath);

            var readTableToDataTable = new ReadFromTableToDataTable();
            var dtResult1 = readTableToDataTable.ReadTableFromExcelSheet(strFilePath, strSheetName, strTableName, arrColumn);

            Console.WriteLine("*********************************************************");

            var dicPropertyNameColumnNameMapping = new Dictionary<string, string>();
            dicPropertyNameColumnNameMapping.Add("UIN", "UIN");
            var readFromTableToList = new ReadFromTableToList();
            var listResult = readFromTableToList.ReadListFromExcelSheet<TableClass>(strFilePath, strSheetName, strTableName, dicPropertyNameColumnNameMapping);

            Console.WriteLine("*********************************************************");

            var dicFieldNamePositionMapping = new Dictionary<string, string>();
            dicFieldNamePositionMapping.Add("SchemeScope", "B2");
            dicFieldNamePositionMapping.Add("OrganisationScope", "B3");
            dicFieldNamePositionMapping.Add("MTCType", "B4");
            dicFieldNamePositionMapping.Add("MTDateFrom", "B5");
            dicFieldNamePositionMapping.Add("MTDateTo", "B6");
            dicFieldNamePositionMapping.Add("MeansTestedAs", "B6");
            var readSingleObject = new ReadSingleObject();
            var dicResult = readSingleObject.ReadParticularCellsFromExcelSheet(strFilePath, strSheetName, dicFieldNamePositionMapping);
            var entityResult = readSingleObject.ReadParticularCellsFromExcelSheet<HeaderClass>(strFilePath, strSheetName, dicFieldNamePositionMapping);

            Console.WriteLine("*********************************************************");

            Console.WriteLine("Hello World!");
        }
    }
}
