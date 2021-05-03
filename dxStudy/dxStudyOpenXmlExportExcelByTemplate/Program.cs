using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace dxStudyOpenXmlExportExcelByTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            string strTemplatePath = @"../../../MockFile/123.xlsx";
            string strSheetName = "dxTest";
            string strSavePath = @"C:\Users\ding_\Desktop\dxTestOpenXml.xlsx";
            var dataSet = CreateDataSet();
            ExportDataSetToFileOneSheet(strTemplatePath, dataSet, true, strSheetName, strSavePath);

            Console.WriteLine("Hello World!");
        }

        static void ExportDataSetToFileOneSheet(string strTemplatePath, DataSet dataSet, bool blnIsDisplayingDBColumn, string strSheetName, string strFileSavedPath)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || string.IsNullOrWhiteSpace(strFileSavedPath))
                return;

            string strPath = Path.GetFullPath(strTemplatePath);

            using (var document = SpreadsheetDocument.CreateFromTemplate(strTemplatePath))
            {
                var workbookPart = document.WorkbookPart;
                var workbook = workbookPart.Workbook;
                //workbookPart.Workbook.Sheets = new Sheets();

                var listSheet = workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return;



                //foreach (DataTable dataTable in dataSet.Tables)
                //{
                //    if (dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
                //        continue;

                //    var listColumnName = GetColumnsForDisplaying(dataTable.Columns, blnIsDisplayingDBColumn, sheetData);
                //    foreach (DataRow dataRow in dataTable.Rows)
                //    {
                //        var row = new Row();
                //        foreach (string column in listColumnName)
                //        {
                //            string strValue = dataRow[column].ToString();
                //            var cell = new Cell();
                //            cell.DataType = CellValues.String;
                //            cell.CellValue = new CellValue(strValue);
                //            row.AppendChild(cell);
                //        }

                //        sheetData.AppendChild(row);
                //    }

                //    var rowSpace = new Row();
                //    sheetData.AppendChild(rowSpace);
                //}

                document.SaveAs(strFileSavedPath);
            }
        }

        static MemoryStream ExportDataSetToStreamOneSheet(DataSet dataSet, bool blnIsDisplayingDBColumn, string strSheetName)
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

        static List<string> GetColumnsForDisplaying(DataColumnCollection dataColumnCollection, bool blnIsDisplayingDBColumn, SheetData sheetData)
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

        static DataSet CreateDataSet()
        {
            var dataTable = new DataTable();
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
    }
}
