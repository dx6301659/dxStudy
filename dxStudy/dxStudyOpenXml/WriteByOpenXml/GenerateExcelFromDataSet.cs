using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace dxStudyOpenXml.WriteByOpenXml
{
    public class GenerateExcelFromDataSet
    {
        public void ExportDataSetToFileMultipleSheets(DataSet dataSet, bool blnIsDisplayingDBColumn, string strFileSavedPath)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || string.IsNullOrWhiteSpace(strFileSavedPath))
                return;

            using (var document = SpreadsheetDocument.Create(strFileSavedPath, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Sheets = new Sheets();

                for (int i = 0, j = dataSet.Tables.Count; i < j; i++)
                {
                    var dataTable = dataSet.Tables[i];
                    if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                        continue;

                    var sheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    sheetPart.Worksheet = new Worksheet(sheetData);
                    string strRelationshipId = workbookPart.GetIdOfPart(sheetPart);

                    uint uintMaxSheetId = 1;
                    var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                    var listSheet = sheets.Elements<Sheet>();
                    if (listSheet.Count() > 0)
                        uintMaxSheetId = listSheet.Max(s => s.SheetId.Value) + 1;

                    string strSheetName = string.IsNullOrWhiteSpace(dataTable.TableName) ? i.ToString() : dataTable.TableName;
                    var sheet = new Sheet() { Id = strRelationshipId, SheetId = uintMaxSheetId, Name = strSheetName };
                    sheets.Append(sheet);

                    var listColumnName = GetColumnsForDisplaying(dataTable.Columns, blnIsDisplayingDBColumn, sheetData);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var row = new Row();
                        foreach (string column in listColumnName)
                        {
                            string strValue = dataRow[column].ToString();
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(strValue);
                            row.AppendChild(cell);
                        }

                        sheetData.AppendChild(row);
                    }
                }
            }
        }

        public MemoryStream ExportDataSetToStreamMultipleSheets(DataSet dataSet, bool blnIsDisplayingDBColumn)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
                return null;

            var memoryStream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Sheets = new Sheets();

                for (int i = 0, j = dataSet.Tables.Count; i < j; i++)
                {
                    var dataTable = dataSet.Tables[i];
                    if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                        continue;

                    var sheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    sheetPart.Worksheet = new Worksheet(sheetData);
                    string strRelationshipId = workbookPart.GetIdOfPart(sheetPart);

                    uint uintMaxSheetId = 1;
                    var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                    var listSheet = sheets.Elements<Sheet>();
                    if (listSheet.Count() > 0)
                        uintMaxSheetId = listSheet.Max(s => s.SheetId.Value) + 1;

                    string strSheetName = string.IsNullOrWhiteSpace(dataTable.TableName) ? i.ToString() : dataTable.TableName;
                    var sheet = new Sheet() { Id = strRelationshipId, SheetId = uintMaxSheetId, Name = strSheetName };
                    sheets.Append(sheet);

                    var listColumnName = GetColumnsForDisplaying(dataTable.Columns, blnIsDisplayingDBColumn, sheetData);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var row = new Row();
                        foreach (string column in listColumnName)
                        {
                            string strValue = dataRow[column].ToString();
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(strValue);
                            row.AppendChild(cell);
                        }

                        sheetData.AppendChild(row);
                    }
                }
            }

            return memoryStream;
        }

        public void ExportDataSetToFileOneSheet(DataSet dataSet, bool blnIsDisplayingDBColumn, string strSheetName, string strFileSavedPath)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || string.IsNullOrWhiteSpace(strFileSavedPath))
                return;

            using (var document = SpreadsheetDocument.Create(strFileSavedPath, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Sheets = new Sheets();

                var sheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                string strRelationshipId = workbookPart.GetIdOfPart(sheetPart);

                uint uintMaxSheetId = 1;
                var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                var listSheet = sheets.Elements<Sheet>();
                if (listSheet.Count() > 0)
                    uintMaxSheetId = listSheet.Max(s => s.SheetId.Value) + 1;

                strSheetName = string.IsNullOrWhiteSpace(strSheetName) ? "Sheet1" : strSheetName;
                var sheet = new Sheet() { Id = strRelationshipId, SheetId = uintMaxSheetId, Name = strSheetName };
                sheets.Append(sheet);

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                        continue;

                    var listColumnName = GetColumnsForDisplaying(dataTable.Columns, blnIsDisplayingDBColumn, sheetData);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var row = new Row();
                        foreach (string column in listColumnName)
                        {
                            string strValue = dataRow[column].ToString();
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(strValue);
                            row.AppendChild(cell);
                        }

                        sheetData.AppendChild(row);
                    }

                    var rowSpace = new Row();
                    sheetData.AppendChild(rowSpace);
                }
            }
        }

        public MemoryStream ExportDataSetToStreamOneSheet(DataSet dataSet, bool blnIsDisplayingDBColumn, string strSheetName)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
                return null;

            var memoryStream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                workbookPart.Workbook.Sheets = new Sheets();

                var sheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                string strRelationshipId = workbookPart.GetIdOfPart(sheetPart);

                uint uintMaxSheetId = 1;
                var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                var listSheet = sheets.Elements<Sheet>();
                if (listSheet.Count() > 0)
                    uintMaxSheetId = listSheet.Max(s => s.SheetId.Value) + 1;

                strSheetName = string.IsNullOrWhiteSpace(strSheetName) ? "Sheet1" : strSheetName;
                var sheet = new Sheet() { Id = strRelationshipId, SheetId = uintMaxSheetId, Name = strSheetName };
                sheets.Append(sheet);

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                        continue;

                    var listColumnName = GetColumnsForDisplaying(dataTable.Columns, blnIsDisplayingDBColumn, sheetData);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var row = new Row();
                        foreach (string column in listColumnName)
                        {
                            string strValue = dataRow[column].ToString();
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(strValue);
                            row.AppendChild(cell);
                        }

                        sheetData.AppendChild(row);
                    }

                    var rowSpace = new Row();
                    sheetData.AppendChild(rowSpace);
                }
            }

            return memoryStream;
        }

        private List<string> GetColumnsForDisplaying(DataColumnCollection dataColumnCollection, bool blnIsDisplayingDBColumn, SheetData sheetData)
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
                cell.StyleIndex = 5U;
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            return listColumnName;
        }
    }
}
