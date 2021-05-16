using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dxStudyOpenXml.WriteByOpenXml
{
    public class WriteListToExcelDirectly
    {
        public bool WriteListToExcel<T>(string strFilePath, string strSheetName, List<T> listSoureData, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, bool blnIsDeleteTempRow = true) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(strFilePath) || string.IsNullOrWhiteSpace(strSheetName))
                return false;

            if (listSoureData == null || listSoureData.Count == 0)
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

                bool blnResult = ImportListData(worksheetPart, listRow, listSoureData, intInputDataStartRowIndex, intInputDataStartColumnIndex, blnIsDeleteTempRow);
                document.Close();
                return blnResult;
            }
        }

        private bool ImportListData<T>(WorksheetPart worksheetPart, IEnumerable<Row> listRow, List<T> listSourceData, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, bool blnIsDeleteTempRow = true) where T : class, new()
        {
            if (listSourceData == null || listSourceData.Count == 0)
                return true;

            var rowObj = listRow.FirstOrDefault(item => item.RowIndex == intInputDataStartRowIndex);
            if (rowObj == null)
                return false;

            var arrProperties = typeof(T).GetProperties();
            if (arrProperties == null || arrProperties.Length == 0)
                return false;

            var cellObj = rowObj.GetFirstChild<Cell>();
            var cellStyleIndex = cellObj.StyleIndex;
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            foreach (var item in listSourceData)
            {
                if (item == null)
                    continue;

                var rowAdded = new Row();
                for (int i = 1; i < intInputDataStartColumnIndex; i++)
                    rowAdded.AppendChild(new Cell());

                foreach (var subItem in arrProperties)
                {
                    string strValue = subItem.GetValue(item) as string;
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
