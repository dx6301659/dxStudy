using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXmlReadParticularCell
{
    class Program
    {
        static void Main(string[] args)
        {
            string strFilePath = @"MockFile\134.xlsx";
            string strSheetName = "BQ1-IT";

            //string strHeaderFieldPositionNameMapping = "A2:Scheme Scope,A3:Organisation Scope,A4:MTC Type,A5:MT Date From,A6:MT Date To,A7:Means Tested As";
            string strHeaderFieldPositionNameMapping = "A5:MT Date From";
            string[] arrFieldPositionMapping = strHeaderFieldPositionNameMapping.Split(',');
            var dicFieldPositionMapping = new Dictionary<string, string>();
            foreach (string item in arrFieldPositionMapping)
            {
                string[] arrMapping = item.Split(':');
                dicFieldPositionMapping.Add(arrMapping[0], arrMapping[1]);
            }

            //bool blnResult = CheckCellsPositionValueMapping(strFilePath, strSheetName, dicFieldPositionMapping);

            //string strHeaderFieldNameValuePositionMapping = "Scheme Scope:B2,Organisation Scope:B3,MTC Type:B4,MT Date From:B5,MT Date To:B6,Means Tested As:B7";
            string strHeaderFieldNameValuePositionMapping = "MT Date From:B5";
            string[] arrFieldValuePositionMapping = strHeaderFieldNameValuePositionMapping.Split(',');
            var dicFieldValuePositionMapping = new Dictionary<string, string>();
            foreach (var item in arrFieldValuePositionMapping)
            {
                string[] arrMapping = item.Split(':');
                dicFieldValuePositionMapping.Add(arrMapping[0], arrMapping[1]);
            }

            //var dicResult = ReadParticularCellsFromExcelSheet(strFilePath, strSheetName, dicFieldValuePositionMapping);


            Console.WriteLine("Hello World!");
        }

        ///// <summary>
        ///// Check if the position and value of the cell is same as the items in the dictionary
        ///// dictionary key : cell position
        ///// dictionary value : cell value
        ///// </summary>
        //static bool CheckCellsPositionValueMapping(string strExcelFilePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping)
        //{
        //    if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName) || dicCellPositionValueMapping == null || dicCellPositionValueMapping.Count == 0)
        //        return false;

        //    using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
        //    {
        //        var workbookPart = document.WorkbookPart;
        //        var workbook = workbookPart.Workbook;

        //        var listSheet = workbook.Descendants<Sheet>();
        //        var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
        //        if (objSheet == null)
        //            return false;

        //        var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
        //        var listRow = worksheetPart.Worksheet.Descendants<Row>();
        //        if (listRow == null || listRow.Count() == 0)
        //            return false;

        //        var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
        //        foreach (var item in dicCellPositionValueMapping)
        //        {
        //            string strFieldPosition = item.Key;
        //            string strFieldName = item.Value;

        //            string strRowIndex = Regex.Replace(item.Key, "[a-zA-Z]", "");
        //            var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
        //            if (targetRow == null)
        //                return false;

        //            var listCell = targetRow.Descendants<Cell>();
        //            if (listCell == null || listCell.Count() == 0)
        //                return false;

        //            var targetCell = listCell.FirstOrDefault(cell => string.Equals(strFieldPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
        //            if (targetCell == null)
        //                return false;

        //            string strCurrentCellValue = GetCellValue(sharedStringTable, targetCell);
        //            if (!string.Equals(strFieldName, strCurrentCellValue, StringComparison.OrdinalIgnoreCase))
        //                return false;
        //        }

        //        return true;
        //    }
        //}

        ///// <summary>
        ///// Get the particular cell value by position in the dictionary
        ///// dictionary key : related value, use it to be key for the returned dictionary
        ///// dictionary value : cell position
        ///// </summary>
        //static Dictionary<string, string> ReadParticularCellsFromExcelSheet(string strExcelFilePath, string strSheetName, Dictionary<string, string> dicValuePositionMapping)
        //{
        //    if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName))
        //        return null;

        //    using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
        //    {
        //        var workbookPart = document.WorkbookPart;
        //        var workbook = workbookPart.Workbook;

        //        var listSheet = workbook.Descendants<Sheet>();
        //        var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
        //        if (objSheet == null)
        //            return null;

        //        var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
        //        var listRow = worksheetPart.Worksheet.Descendants<Row>();
        //        if (listRow == null || listRow.Count() == 0)
        //            return null;

        //        var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
        //        var dicResult = new Dictionary<string, string>();
        //        foreach (var item in dicValuePositionMapping)
        //        {
        //            string strCellPosition = item.Value;
        //            if (string.IsNullOrWhiteSpace(strCellPosition))
        //                return null;

        //            string strRowIndex = Regex.Replace(strCellPosition, "[a-zA-Z]", "");
        //            var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
        //            if (targetRow == null)
        //                return null;

        //            var listCell = targetRow.Descendants<Cell>();
        //            if (listCell == null || listCell.Count() == 0)
        //                return null;

        //            var targetCell = listCell.FirstOrDefault(cell => string.Equals(strCellPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
        //            if (targetCell == null)
        //                return null;

        //            string strCurrentCellValue = GetCellValue(sharedStringTable, targetCell);
        //            dicResult.Add(item.Key, strCurrentCellValue);
        //        }

        //        return dicResult;
        //    }
        //}

        //static string GetCellValue(SharedStringTable sharedStringTable, Cell cell)
        //{
        //    string strValue = cell.CellValue == null ? null : cell.CellValue.InnerXml ?? null;
        //    if (string.IsNullOrWhiteSpace(strValue))
        //        return null;

        //    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        //    {
        //        string strResult = sharedStringTable.ChildElements[int.Parse(strValue)].InnerText;
        //        return !string.IsNullOrWhiteSpace(strResult) ? strResult.Trim() : strResult;
        //    }

        //    double d = 0;
        //    if (double.TryParse(cell.CellValue?.Text, out d))
        //        return d.ToString();

        //    return !string.IsNullOrWhiteSpace(strValue) ? strValue.Trim() : strValue;
        //}
    }
}
