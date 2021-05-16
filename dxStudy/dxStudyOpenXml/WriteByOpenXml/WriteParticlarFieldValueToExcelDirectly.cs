using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXml.WriteByOpenXml
{
    public class WriteParticlarFieldValueToExcelDirectly
    {
        public bool ChangeParticularCellValue(string strFilePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping)
        {
            if (string.IsNullOrWhiteSpace(strFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return false;

            if (dicCellPositionValueMapping == null || dicCellPositionValueMapping.Count == 0)
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

                bool blnResult = ChangeParticularCellValue(listRow, dicCellPositionValueMapping);
                document.Close();
                return blnResult;
            }
        }

        private bool ChangeParticularCellValue(IEnumerable<Row> listRow, Dictionary<string, string> dicCellPositionValueMapping)
        {
            if (dicCellPositionValueMapping == null || dicCellPositionValueMapping.Count == 0)
                return true;

            foreach (var item in dicCellPositionValueMapping)
            {
                string strCellPosition = item.Key;
                if (string.IsNullOrWhiteSpace(strCellPosition))
                    return false;

                string strRowIndex = Regex.Replace(strCellPosition, "[a-zA-Z]", "");
                var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
                if (targetRow == null)
                    continue;

                var listCell = targetRow.Descendants<Cell>();
                if (listCell == null || listCell.Count() == 0)
                    return false;

                var targetCell = listCell.FirstOrDefault(cell => string.Equals(strCellPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
                if (targetCell == null)
                    continue;

                string strItemValue = string.IsNullOrWhiteSpace(item.Value) ? "" : item.Value;
                targetCell.DataType = CellValues.String;
                targetCell.CellValue = new CellValue(strItemValue);
            }

            return true;
        }
    }
}
