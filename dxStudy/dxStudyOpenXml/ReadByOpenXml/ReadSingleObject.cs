using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXml.ReadByOpenXml
{
    public class ReadSingleObject : OpenXmlBase
    {
        public T ReadParticularCellsFromExcelSheet<T>(string strExcelFilePath, string strSheetName, Dictionary<string, string> dicFieldNameValuePositionMapping) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return null;

            if (dicFieldNameValuePositionMapping == null || dicFieldNameValuePositionMapping.Count == 0)
                return null;

            T tResult = default(T);
            var type = typeof(T);

            using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return null;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return null;

                tResult = new T();
                var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                var dicResult = new Dictionary<string, string>();
                foreach (var item in dicFieldNameValuePositionMapping)
                {
                    var property = type.GetProperty(item.Key);
                    if (property == null)
                        return null;

                    string strCellPosition = item.Value;
                    if (string.IsNullOrWhiteSpace(strCellPosition))
                        return null;

                    string strRowIndex = Regex.Replace(strCellPosition, "[a-zA-Z]", "");
                    var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
                    if (targetRow == null)
                        return null;

                    var listCell = targetRow.Descendants<Cell>();
                    if (listCell == null || listCell.Count() == 0)
                        return null;

                    var targetCell = listCell.FirstOrDefault(cell => string.Equals(strCellPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        return null;

                    string strCurrentCellValue = GetCellValue(sharedStringTable, targetCell);
                    property.SetValue(tResult, strCurrentCellValue);
                }

                return tResult;
            }
        }

        public Dictionary<string, string> ReadParticularCellsFromExcelSheet(string strExcelFilePath, string strSheetName, Dictionary<string, string> dicValuePositionMapping)
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return null;

            using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return null;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return null;

                var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                var dicResult = new Dictionary<string, string>();
                foreach (var item in dicValuePositionMapping)
                {
                    string strCellPosition = item.Value;
                    if (string.IsNullOrWhiteSpace(strCellPosition))
                        return null;

                    string strRowIndex = Regex.Replace(strCellPosition, "[a-zA-Z]", "");
                    var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
                    if (targetRow == null)
                        return null;

                    var listCell = targetRow.Descendants<Cell>();
                    if (listCell == null || listCell.Count() == 0)
                        return null;

                    var targetCell = listCell.FirstOrDefault(cell => string.Equals(strCellPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        return null;

                    string strCurrentCellValue = GetCellValue(sharedStringTable, targetCell);
                    dicResult.Add(item.Key, strCurrentCellValue);
                }

                return dicResult;
            }
        }
    }
}
