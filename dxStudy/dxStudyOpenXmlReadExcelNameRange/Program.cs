using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXmlReadExcelNameRange
{
    class Program
    {
        static void Main(string[] args)
        {
            string strFilePath = @"MockFile\134.xlsx";
            string strSheetName = "BQ1-IT";
            string strNameRangeName = "VariableName";
            // to do : use ReadNameRangeFromExcelSheet

            Console.WriteLine("Hello World!");
        }

        static DataTable ReadNameRangeFromExcelSheet(string strExcelFilePath, string strSheetName, string strNameRangeName)
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return null;

            using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var definedNames = workbook.DefinedNames;
                if (definedNames == null || definedNames.Count() == 0)
                    return null;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return null;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return null;

                // to do : get the defined name value                
                foreach (DefinedName dn in definedNames)
                {
                    string strRangeName = dn.Name.Value;
                    string strPositionTag = dn.Text;
                    int intIndex = strPositionTag.IndexOf("!");
                    if (intIndex >= 0)
                    {
                        string strPositions = strPositionTag.Substring(intIndex).Replace("!", "").Replace("$", "");

                        // to do :
                        string[] arrPositions = strPositions.Split(':');
                        string strStartPosition = arrPositions[0];
                        string strEndPosition = arrPositions[1];
                        string strStartRow = Regex.Replace(strStartPosition, "[a-zA-Z]", "");
                        string strEndRow = Regex.Replace(strEndPosition, "[a-zA-Z]", "");
                        int intStartRow = Convert.ToInt32(strStartRow);
                        int intEndRow = Convert.ToInt32(strEndRow);
                    }
                }

                return null;
            }
        }
    }
}
