using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXml
{
    public class OpenXmlBase
    {
        public string GetCellValue(SharedStringTable sharedStringTable, Cell cell)
        {
            string strValue = cell.CellValue == null ? null : cell.CellValue.InnerXml ?? null;
            if (string.IsNullOrWhiteSpace(strValue))
                return null;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                string strResult = sharedStringTable.ChildElements[int.Parse(strValue)].InnerText;
                return !string.IsNullOrWhiteSpace(strResult) ? strResult.Trim() : strResult;
            }

            double d = 0;
            if (double.TryParse(cell.CellValue?.Text, out d))
                return d.ToString();

            return !string.IsNullOrWhiteSpace(strValue) ? strValue.Trim() : strValue;
        }

        public List<string> GetColumnsForDisplaying(DataColumnCollection dataColumnCollection, bool blnIsDisplayingDBColumn, SheetData sheetData)
        {
            var listColumnName = new List<string>();
            if (!blnIsDisplayingDBColumn)
            {
                foreach (DataColumn column in dataColumnCollection)
                    listColumnName.Add(column.ColumnName);

                return listColumnName;
            }

            var headerRow = new Row();
            foreach (DataColumn column in dataColumnCollection)
            {
                string strColumnName = column.ColumnName;
                listColumnName.Add(strColumnName);

                var cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(strColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            return listColumnName;
        }

        public bool RemoveParticularRow(string strFilePath, string strSheetName, int intInputDataStartRowIndex)
        {
            if (string.IsNullOrWhiteSpace(strFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return false;

            if (intInputDataStartRowIndex < 0)
                return true;

            using (var document = SpreadsheetDocument.Open(strFilePath, true))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                {
                    document.Close();
                    return false;
                }

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                {
                    document.Close();
                    return false;
                }

                var targetRow = listRow.FirstOrDefault(item => item.RowIndex == intInputDataStartRowIndex);
                if (targetRow == null)
                {
                    document.Close();
                    return false;
                }

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                sheetData.RemoveChild(targetRow);
                document.Close();
                return true;
            }
        }

        /// <summary>
        /// Check if the position and value of the cell is same as the items in the dictionary
        /// dictionary key : cell position
        /// dictionary value : cell value
        /// </summary>
        public bool CheckCellsPositionValueMapping(string strExcelFilePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping)
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName) || dicCellPositionValueMapping == null || dicCellPositionValueMapping.Count == 0)
                return false;

            using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return false;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return false;

                var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                foreach (var item in dicCellPositionValueMapping)
                {
                    string strFieldPosition = item.Key;
                    string strFieldName = item.Value;

                    string strRowIndex = Regex.Replace(item.Key, "[a-zA-Z]", "");
                    var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
                    if (targetRow == null)
                        return false;

                    var listCell = targetRow.Descendants<Cell>();
                    if (listCell == null || listCell.Count() == 0)
                        return false;

                    var targetCell = listCell.FirstOrDefault(cell => string.Equals(strFieldPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        return false;

                    string strCurrentCellValue = GetCellValue(sharedStringTable, targetCell);
                    if (!string.Equals(strFieldName, strCurrentCellValue, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                return true;
            }
        }        
    }
}
