using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace dxStudyOpenXmlExportExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            string strOutputFilePath = @"MockFile\134.xlsx";
            var dataSet = CreateDataSet();
            //ExportDataSetToFileMultipleSheets(dataSet, true, strOutputFilePath);

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //var memoryStream = ExportDataSetToStreamMultipleSheets(dataSet, true);
            //var fileStream = new FileStream(strOutputFilePath, FileMode.Create);
            //fileStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            //fileStream.Close();
            //fileStream.Dispose();
            //memoryStream.Close();
            //memoryStream.Dispose();

            //ZipSingleFile(memoryStream, "123.xlsx", "123", @"C:\Users\dingxu\Desktop\Result.zip");

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            ExportDataSetToFileOneSheet(dataSet, true, null, strOutputFilePath);
            ////ExportDataSetToFileOneSheet(dataSet, true, "dxTest", strOutputFilePath);

            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //var memoryStream = ExportDataSetToStreamOneSheet(dataSet, true, null);
            ////var memoryStream = ExportDataSetToStreamOneSheet(dataSet, true, "dxTest");
            ////var fileStream = new FileStream(strOutputFilePath, FileMode.Create);
            ////fileStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            ////fileStream.Close();
            ////fileStream.Dispose();
            ////memoryStream.Close();
            ////memoryStream.Dispose();

            //ZipSingleFile(memoryStream, "123.xlsx", "123", @"C:\Users\dingxu\Desktop\Result.zip");
            Console.WriteLine("Hello World!");
        }

        static void ExportDataSetToFileMultipleSheets(DataSet dataSet, bool blnIsDisplayingDBColumn, string strFileSavedPath)
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

        static MemoryStream ExportDataSetToStreamMultipleSheets(DataSet dataSet, bool blnIsDisplayingDBColumn)
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

        static void ExportDataSetToFileOneSheet(DataSet dataSet, bool blnIsDisplayingDBColumn, string strSheetName, string strFileSavedPath)
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

        static void ZipSingleFile(Stream stream, string strFileName, string strPassword, string strTargetZipFile)
        {
            stream.Position = 0;
            int intLength = (int)stream.Length;
            byte[] buffer = new byte[intLength];
            using (stream)
            {
                stream.Read(buffer, 0, intLength);
                stream.Close();
            }

            var entry = new ZipEntry(strFileName);
            entry.DateTime = DateTime.Now;
            entry.Size = intLength;

            var zipFileStream = File.Create(strTargetZipFile);
            using (var zipOut = new ZipOutputStream(zipFileStream))
            {
                if (!string.IsNullOrWhiteSpace(strPassword))
                    zipOut.Password = strPassword;

                zipOut.PutNextEntry(entry);
                zipOut.Write(buffer, 0, intLength);
                zipOut.CloseEntry();
                zipOut.Finish();
                zipOut.Close();
            }
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
            var dataSet = new DataSet();

            //var dataTable1 = new DataTable("Header");
            var dataTable1 = new DataTable();
            var dataColumn1_1 = new DataColumn("Scheme Scope");
            var dataColumn1_2 = new DataColumn("Org Scope");
            dataTable1.Columns.Add(dataColumn1_1);
            dataTable1.Columns.Add(dataColumn1_2);

            //var dataTable2 = new DataTable("Record");
            var dataTable2 = new DataTable();
            var dataColumn2_1 = new DataColumn("Nric");
            var dataColumn2_2 = new DataColumn("Age");
            var dataColumn2_3 = new DataColumn("Address");
            dataTable2.Columns.Add(dataColumn2_1);
            dataTable2.Columns.Add(dataColumn2_2);
            dataTable2.Columns.Add(dataColumn2_3);

            var dataRow1 = dataTable1.NewRow();
            dataRow1["Scheme Scope"] = "Test scheme";
            dataRow1["Org Scope"] = "Test org";
            dataTable1.Rows.Add(dataRow1);

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

            dataSet.Tables.Add(dataTable1);
            dataSet.Tables.Add(dataTable2);

            return dataSet;
        }
    }
}
