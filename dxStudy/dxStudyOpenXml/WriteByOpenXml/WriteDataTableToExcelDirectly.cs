using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace dxStudyOpenXml.WriteByOpenXml
{
    public class WriteDataTableToExcelDirectly
    {
        public bool WriteDataTableToExcel(string strFilePath, string strSheetName, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, bool blnIsDeleteTempRow = true)
        {
            if (string.IsNullOrWhiteSpace(strFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return false;

            if (sourceDataTable == null || sourceDataTable.Rows == null || sourceDataTable.Rows.Count == 0)
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

                bool blnResult = ImportDataTable(worksheetPart, listRow, sourceDataTable, intInputDataStartRowIndex, intInputDataStartColumnIndex, blnIsDeleteTempRow);
                document.Close();
                return blnResult;
            }
        }

        private bool ImportDataTable(WorksheetPart worksheetPart, IEnumerable<Row> listRow, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, bool blnIsDeleteTempRow = true)
        {
            if (sourceDataTable == null || sourceDataTable.Rows.Count == 0)
                return true;

            var rowObj = listRow.FirstOrDefault(item => item.RowIndex == intInputDataStartRowIndex);
            if (rowObj == null)
                return false;

            var cellObj = rowObj.GetFirstChild<Cell>();
            var cellStyleIndex = cellObj.StyleIndex;
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            foreach (DataRow dataRow in sourceDataTable.Rows)
            {
                var rowAdded = new Row();
                for (int i = 1; i < intInputDataStartColumnIndex; i++)
                    rowAdded.AppendChild(new Cell());

                foreach (DataColumn dataColumn in sourceDataTable.Columns)
                {
                    string strValue = dataRow[dataColumn.ColumnName] as string;
                    strValue = string.IsNullOrWhiteSpace(strValue) ? "" : strValue;
                    var cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(strValue);
                    cell.StyleIndex = cellStyleIndex;
                    rowAdded.AppendChild(cell);
                }

                sheetData.AppendChild(rowAdded);
            }

            if (blnIsDeleteTempRow)
                sheetData.RemoveChild(rowObj);

            return true;
        }
    }
}
