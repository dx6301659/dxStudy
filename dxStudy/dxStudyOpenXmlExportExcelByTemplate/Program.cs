using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXmlExportExcelByTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            string strTemplatePath = @"../../../MockFile/123.xlsx";
            string strSheetName = "BQ1-OT";
            string strSavePath = @"C:\Users\ding_\Desktop\dxTestOpenXml.xlsx";
            var dataTable = CreateDataTable();
            var dicCellPositionValueMapping = new Dictionary<string, string>();
            dicCellPositionValueMapping.Add("C3", "Dingxu, Test Scheme");
            dicCellPositionValueMapping.Add("C8", "Test Scheme");
            dicCellPositionValueMapping.Add("C9", "Test Organisation");
            //bool blnResult = ExportDataTableToFileByTemplate(strTemplatePath, strSheetName, dataTable, 27, 1, dicCellPositionValueMapping, strSavePath);
            ExportToFileByTemplate(strTemplatePath, strSheetName, dicCellPositionValueMapping, dataTable, 27, 1, strSavePath);

            //if (blnResult)
            //{
            //    var dicCellPositionValueMapping = new Dictionary<string, string>();
            //    dicCellPositionValueMapping.Add("C3", "Dingxu, Test Scheme");
            //    dicCellPositionValueMapping.Add("C8", "Test Scheme");
            //    dicCellPositionValueMapping.Add("C9", "Test Organisation");
            //    blnResult = ChangeCellValue(strSavePath, strSheetName, dicCellPositionValueMapping);
            //}

            Console.WriteLine("Hello World!");
        }

        static bool ExportDataTableToFileByTemplate(string strTemplatePath, string strSheetName, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, Dictionary<string, string> dicCellPositionValueMapping, string strFileSavedPath)
        {
            if (string.IsNullOrWhiteSpace(strTemplatePath) || sourceDataTable == null || sourceDataTable.Rows.Count == 0 || string.IsNullOrWhiteSpace(strSheetName) || string.IsNullOrWhiteSpace(strFileSavedPath))
                return false;

            using (var document = SpreadsheetDocument.CreateFromTemplate(strTemplatePath))
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

                foreach (var item in dicCellPositionValueMapping)
                {
                    string strCellPosition = item.Key;
                    if (string.IsNullOrWhiteSpace(strCellPosition))
                        return false;

                    string strRowIndex = Regex.Replace(strCellPosition, "[a-zA-Z]", "");
                    var targetRow = listRow.FirstOrDefault(row => row.RowIndex.Value.ToString() == strRowIndex);
                    if (targetRow == null)
                        return false;

                    var listCell = targetRow.Descendants<Cell>();
                    if (listCell == null || listCell.Count() == 0)
                        return false;

                    var targetCell = listCell.FirstOrDefault(cell => string.Equals(strCellPosition, cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        return false;

                    targetCell.CellValue.Remove();
                    targetCell.DataType = CellValues.String;
                    targetCell.CellValue = new CellValue(item.Value);
                }

                var rowObj = listRow.FirstOrDefault(item => item.RowIndex == intInputDataStartRowIndex);
                var cellObj = rowObj.GetFirstChild<Cell>();
                var cellStyleIndex = cellObj.StyleIndex;
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                foreach (DataRow dataRow in sourceDataTable.Rows)
                {
                    var rowAdded = new Row();
                    for (int i = 0; i < intInputDataStartColumnIndex; i++)
                        rowAdded.AppendChild(new Cell());

                    foreach (DataColumn dataColumn in sourceDataTable.Columns)
                    {
                        string strValue = dataRow[dataColumn.ColumnName] as string;
                        var cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(strValue);
                        cell.StyleIndex = cellStyleIndex;
                        rowAdded.AppendChild(cell);
                    }

                    sheetData.InsertBefore(rowAdded, rowObj);
                }

                document.SaveAs(strFileSavedPath);
                document.Close();
            }

            return true;
        }

        static void ExportToFileByTemplate(string strTemplatePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, string strFileSavedPath)
        {
            if (string.IsNullOrWhiteSpace(strTemplatePath) || string.IsNullOrWhiteSpace(strSheetName) || string.IsNullOrWhiteSpace(strFileSavedPath))
                return;

            using (var document = SpreadsheetDocument.CreateFromTemplate(strTemplatePath))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return;

                bool blnResult = ChangeParticularCellValue(listRow, dicCellPositionValueMapping);
                if (!blnResult)
                    return;

                blnResult = ImportDataTable(worksheetPart, listRow, sourceDataTable, intInputDataStartRowIndex, intInputDataStartColumnIndex);
                if (!blnResult)
                    return;

                document.SaveAs(strFileSavedPath);
                document.Close();
            }
        }

        static bool ChangeParticularCellValue(IEnumerable<Row> listRow, Dictionary<string, string> dicCellPositionValueMapping)
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

                targetCell.CellValue.Remove();
                targetCell.DataType = CellValues.String;
                targetCell.CellValue = new CellValue(item.Value);
            }

            return true;
        }

        static bool ImportDataTable(WorksheetPart worksheetPart, IEnumerable<Row> listRow, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex)
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
                for (int i = 0; i < intInputDataStartColumnIndex; i++)
                    rowAdded.AppendChild(new Cell());

                foreach (DataColumn dataColumn in sourceDataTable.Columns)
                {
                    string strValue = dataRow[dataColumn.ColumnName] as string;
                    var cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(strValue);
                    cell.StyleIndex = cellStyleIndex;
                    rowAdded.AppendChild(cell);
                }

                sheetData.InsertBefore(rowAdded, rowObj);
            }

            return true;
        }

        static DataTable CreateDataTable()
        {
            var dataTable2 = new DataTable();
            var dataColumn2_1 = new DataColumn("Nric");
            var dataColumn2_2 = new DataColumn("Age");
            var dataColumn2_3 = new DataColumn("Address");
            dataTable2.Columns.Add(dataColumn2_1);
            dataTable2.Columns.Add(dataColumn2_2);
            dataTable2.Columns.Add(dataColumn2_3);

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

            var dataRow2_4 = dataTable2.NewRow();
            dataRow2_4["Nric"] = "dxTestNric4";
            dataRow2_4["Age"] = "dxTestAge4";
            dataRow2_4["Address"] = "dxTestAddress4";
            dataTable2.Rows.Add(dataRow2_4);

            return dataTable2;
        }
    }
}
