using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ICSharpCode.SharpZipLib.Zip;
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
            //string strTemplatePath = @"../../../MockFile/123.xlsx";
            //string strSheetName = "BQ1-OT";
            //string strSavePath = @"C:\Users\ding_\Desktop\dxTestOpenXml.xlsx";
            //var dataTable = CreateDataTable();
            //var dicCellPositionValueMapping = new Dictionary<string, string>();
            //dicCellPositionValueMapping.Add("C3", "Dingxu, Test Scheme");
            //dicCellPositionValueMapping.Add("C8", "Test Scheme");
            //dicCellPositionValueMapping.Add("C9", "Test Organisation");
            //bool blnResult = ExportDataTableToFileByTemplate(strTemplatePath, strSheetName, dataTable, 27, 2, dicCellPositionValueMapping, strSavePath);
            //ExportToFileByTemplate(strTemplatePath, strSheetName, dicCellPositionValueMapping, dataTable, 27, 2, strSavePath);

            //if (blnResult)
            //{
            //    var dicCellPositionValueMapping = new Dictionary<string, string>();
            //    dicCellPositionValueMapping.Add("C3", "Dingxu, Test Scheme");
            //    dicCellPositionValueMapping.Add("C8", "Test Scheme");
            //    dicCellPositionValueMapping.Add("C9", "Test Organisation");
            //    blnResult = ChangeCellValue(strSavePath, strSheetName, dicCellPositionValueMapping);
            //}

            Console.WriteLine("***************************************************************");


            string strTemplatePath = @"../../../MockFile/TEMPLATE-BQ2-OT.xlsx";
            string strSheetName = "BQ2-OT";
            string strSavePath = @"C:\Users\ding_\Desktop\dxTestOpenXml.zip";
            var dataTable = CreateDataTable();
            var dicCellPositionValueMapping = new Dictionary<string, string>();
            dicCellPositionValueMapping.Add("C3", "Dingxu, Test Scheme");
            dicCellPositionValueMapping.Add("C4", "Dingxu, Test Scheme");
            dicCellPositionValueMapping.Add("C5", DateTime.Now.ToString("yyyy-MM-dd"));
            var testClass = new TestClass();
            var dtResult = testClass.GetOutputRecordData();
            TestGenerateFileByStreamAndZipIt(strTemplatePath, strSheetName, dicCellPositionValueMapping, dtResult, 20, 2, strSavePath);

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

        static MemoryStream ExportToFileByTemplate(string strTemplatePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex)
        {
            if (string.IsNullOrWhiteSpace(strTemplatePath) || string.IsNullOrWhiteSpace(strSheetName))
                return null;

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(strTemplatePath, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);

            using (var document = SpreadsheetDocument.Open(memoryStream, true))
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

                bool blnResult = ChangeParticularCellValue(listRow, dicCellPositionValueMapping);
                if (!blnResult)
                    return null;

                blnResult = ImportDataTable(worksheetPart, listRow, sourceDataTable, intInputDataStartRowIndex, intInputDataStartColumnIndex);
                if (!blnResult)
                    return null;

                document.Close();
            }

            memoryStream.Position = 0;
            return memoryStream;
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

                string strItemValue = string.IsNullOrWhiteSpace(item.Value) ? "" : item.Value;
                targetCell.DataType = CellValues.String;
                targetCell.CellValue = new CellValue(strItemValue);
            }

            return true;
        }

        static bool ImportDataTable(WorksheetPart worksheetPart, IEnumerable<Row> listRow, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, bool blnIsDeleteTempRow = true)
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

        static void TestGenerateFileByStreamAndZipIt(string strTemplatePath, string strSheetName, Dictionary<string, string> dicCellPositionValueMapping, DataTable sourceDataTable, int intInputDataStartRowIndex, int intInputDataStartColumnIndex, string strFileSavedPath)
        {
            var memoryStream = ExportToFileByTemplate(strTemplatePath, strSheetName, dicCellPositionValueMapping, sourceDataTable, intInputDataStartRowIndex, intInputDataStartColumnIndex);
            if (memoryStream != null)
            {
                ZipSingleFile(memoryStream, "dxTestFile.xlsx", "123", strFileSavedPath);
            }
        }
    }

    public class QMCOutputColumnName
    {
        public const string ColumnName1 = "UIN";
        public const string ColumnName2 = "Name";
        public const string ColumnName3 = "ResidentialStatus";
        public const string ColumnName4 = "Race";
        public const string ColumnName5 = "DOB";
        public const string ColumnName6 = "Gender";
        public const string ColumnName7 = "LivingStatus";
        public const string ColumnName8 = "DemisedDate";
        public const string ColumnName9 = "InactiveDate";
        public const string ColumnName10 = "PassType";
        public const string ColumnName11 = "PassStartDate";
        public const string ColumnName12 = "PassEndDate";
        public const string ColumnName13 = "RHHAddressPostalCode";
        public const string ColumnName14 = "RHHAddressBlockNo";
        public const string ColumnName15 = "RHHAddressFloor";
        public const string ColumnName16 = "RHHAddressUnitNo";
        public const string ColumnName17 = "RHHAddressStreetName";
        public const string ColumnName18 = "RHHAddressBuildingName";
        public const string ColumnName19 = "RHHAddressHDBFlatType";
        public const string ColumnName20 = "GenerationIndicator";
        public const string ColumnName21 = "PreQSchemeName1";
        public const string ColumnName22 = "PreQSchemeStartDate1";
        public const string ColumnName23 = "PreQSchemeEndDate1";
        public const string ColumnName24 = "PreQSchemeName2";
        public const string ColumnName25 = "PreQSchemeStartDate2";
        public const string ColumnName26 = "PreQSchemeEndDate2";
        public const string ColumnName27 = "PreQSchemeName3";
        public const string ColumnName28 = "PreQSchemeStartDate3";
        public const string ColumnName29 = "PreQSchemeEndDate3";
        public const string ColumnName30 = "PreQSchemeName4";
        public const string ColumnName31 = "PreQSchemeStartDate4";
        public const string ColumnName32 = "PreQSchemeEndDate4";
        public const string ColumnName33 = "PreQSchemeName5";
        public const string ColumnName34 = "PreQSchemeStartDate5";
        public const string ColumnName35 = "PreQSchemeEndDate5";
        public const string ColumnName36 = "PreQSchemeName6";
        public const string ColumnName37 = "PreQSchemeStartDate6";
        public const string ColumnName38 = "PreQSchemeEndDate6";
        public const string ColumnName39 = "PreQSchemeName7";
        public const string ColumnName40 = "PreQSchemeStartDate7";
        public const string ColumnName41 = "PreQSchemeEndDate7";
        public const string ColumnName42 = "PreQSchemeName8";
        public const string ColumnName43 = "PreQSchemeStartDate8";
        public const string ColumnName44 = "PreQSchemeEndDate8";
        public const string ColumnName45 = "PreQSchemeName9";
        public const string ColumnName46 = "PreQSchemeStartDate9";
        public const string ColumnName47 = "PreQSchemeEndDate9";
        public const string ColumnName48 = "PreQSchemeName10";
        public const string ColumnName49 = "PreQSchemeStartDate10";
        public const string ColumnName50 = "PreQSchemeEndDate10";
        public const string ColumnName51 = "MTCLastUpdatedDate";
        public const string ColumnName52 = "MTCType";
        public const string ColumnName53 = "HasOnGoingMTCRecon";
        public const string ColumnName54 = "MTCMemberUIN1";
        public const string ColumnName55 = "MTCMemberName1";
        public const string ColumnName56 = "MTCMemberResidentStatus1";
        public const string ColumnName57 = "MTCMemberDOB1";
        public const string ColumnName58 = "MTCMemberGender1";
        public const string ColumnName59 = "MTCMemberRelationship1";
        public const string ColumnName60 = "MTCMemberStatusTag1";
        public const string ColumnName61 = "MTCMemberLatestUsableConsentID1";
        public const string ColumnName62 = "MTCMemberLatestReusableConsentScope1";
        public const string ColumnName63 = "MTCMemberUIN2";
        public const string ColumnName64 = "MTCMemberName2";
        public const string ColumnName65 = "MTCMemberResidentialStatus2";
        public const string ColumnName66 = "MTCMemberDOB2";
        public const string ColumnName67 = "MTCMemberGender2";
        public const string ColumnName68 = "MTCMemberRelationship2";
        public const string ColumnName69 = "MTCMemberStatusTags2";
        public const string ColumnName70 = "MTCMemberLatestReusableConsentID2";
        public const string ColumnName71 = "MTCMemberLatestReusableConsentScope2";
        public const string ColumnName72 = "MTCMemberUIN3";
        public const string ColumnName73 = "MTCMemberName3";
        public const string ColumnName74 = "MTCMemberResidentialStatus3";
        public const string ColumnName75 = "MTCMemberDOB3";
        public const string ColumnName76 = "MTCMemberGender3";
        public const string ColumnName77 = "MTCMemberRelationship3";
        public const string ColumnName78 = "MTCMemberStatusTags3";
        public const string ColumnName79 = "MTCMemberLatestReusableConsentID3";
        public const string ColumnName80 = "MTCMemberLatestReusableConsentScope3";
        public const string ColumnName81 = "MTCMemberUIN4";
        public const string ColumnName82 = "MTCMemberName4";
        public const string ColumnName83 = "MTCMemberResidentialStatus4";
        public const string ColumnName84 = "MTCMemberDOB4";
        public const string ColumnName85 = "MTCMemberGender4";
        public const string ColumnName86 = "MTCMemberRelationship4";
        public const string ColumnName87 = "MTCMemberStatusTags4";
        public const string ColumnName88 = "MTCMemberLatestReusableConsentID4";
        public const string ColumnName89 = "MTCMemberLatestReusableConsentScope4";
        public const string ColumnName90 = "MTCMemberUIN5";
        public const string ColumnName91 = "MTCMemberName5";
        public const string ColumnName92 = "MTCMemberResidentialStatus5";
        public const string ColumnName93 = "MTCMemberDOB5";
        public const string ColumnName94 = "MTCMemberGender5";
        public const string ColumnName95 = "MTCMemberRelationship5";
        public const string ColumnName96 = "MTCMemberStatusTags5";
        public const string ColumnName97 = "MTCMemberLatestReusableConsentID5";
        public const string ColumnName98 = "MTCMemberLatestReusableConsentScope5";
        public const string ColumnName99 = "MTCMemberUIN6";
        public const string ColumnName100 = "MTCMemberName6";
        public const string ColumnName101 = "MTCMemberResidentialStatus6";
        public const string ColumnName102 = "MTCMemberDOB6";
        public const string ColumnName103 = "MTCMemberGender6";
        public const string ColumnName104 = "MTCMemberRelationship6";
        public const string ColumnName105 = "MTCMemberStatusTags6";
        public const string ColumnName106 = "MTCMemberLatestReusableConsentID6";
        public const string ColumnName107 = "MTCMemberLatestReusableConsentScope6";
        public const string ColumnName108 = "MTCMemberUIN7";
        public const string ColumnName109 = "MTCMemberName7";
        public const string ColumnName110 = "MTCMemberResidentialStatus7";
        public const string ColumnName111 = "MTCMemberDOB7";
        public const string ColumnName112 = "MTCMemberGender7";
        public const string ColumnName113 = "MTCMemberRelationship7";
        public const string ColumnName114 = "MTCMemberStatusTags7";
        public const string ColumnName115 = "MTCMemberLatestReusableConsentID7";
        public const string ColumnName116 = "MTCMemberLatestReusableConsentScope7";
        public const string ColumnName117 = "MTCMemberUIN8";
        public const string ColumnName118 = "MTCMemberName8";
        public const string ColumnName119 = "MTCMemberResidentialStatus8";
        public const string ColumnName120 = "MTCMemberDOB8";
        public const string ColumnName121 = "MTCMemberGender8";
        public const string ColumnName122 = "MTCMemberRelationship8";
        public const string ColumnName123 = "MTCMemberStatusTags8";
        public const string ColumnName124 = "MTCMemberLatestReusableConsentID8";
        public const string ColumnName125 = "MTCMemberLatestReusableConsentScope8";
        public const string ColumnName126 = "MTCMemberUIN9";
        public const string ColumnName127 = "MTCMemberName9";
        public const string ColumnName128 = "MTCMemberResidentialStatus9";
        public const string ColumnName129 = "MTCMemberDOB9";
        public const string ColumnName130 = "MTCMemberGender9";
        public const string ColumnName131 = "MTCMemberRelationship9";
        public const string ColumnName132 = "MTCMemberStatusTags9";
        public const string ColumnName133 = "MTCMemberLatestReusableConsentID9";
        public const string ColumnName134 = "MTCMemberLatestReusableConsentScope9";
        public const string ColumnName135 = "MTCMemberUIN10";
        public const string ColumnName136 = "MTCMemberName10";
        public const string ColumnName137 = "MTCMemberResidentialStatus10";
        public const string ColumnName138 = "MTCMemberDOB10";
        public const string ColumnName139 = "MTCMemberGender10";
        public const string ColumnName140 = "MTCMemberRelationship10";
        public const string ColumnName141 = "MTCMemberStatusTags10";
        public const string ColumnName142 = "MTCMemberLatestReusableConsentID10";
        public const string ColumnName143 = "MTCMemberLatestReusableConsentScope10";
        public const string ColumnName144 = "MTCMemberUIN11";
        public const string ColumnName145 = "MTCMemberName11";
        public const string ColumnName146 = "MTCMemberResidentialStatus11";
        public const string ColumnName147 = "MTCMemberDOB11";
        public const string ColumnName148 = "MTCMemberGender11";
        public const string ColumnName149 = "MTCMemberRelationship11";
        public const string ColumnName150 = "MTCMemberStatusTags11";
        public const string ColumnName151 = "MTCMemberLatestReusableConsentID11";
        public const string ColumnName152 = "MTCMemberLatestReusableConsentScope11";
        public const string ColumnName153 = "MTCMemberUIN12";
        public const string ColumnName154 = "MTCMemberName12";
        public const string ColumnName155 = "MTCMemberResidentialStatus12";
        public const string ColumnName156 = "MTCMemberDOB12";
        public const string ColumnName157 = "MTCMemberGender12";
        public const string ColumnName158 = "MTCMemberRelationship12";
        public const string ColumnName159 = "MTCMemberStatusTags12";
        public const string ColumnName160 = "MTCMemberLatestReusableConsentID12";
        public const string ColumnName161 = "MTCMemberLatestReusableConsentScope12";
        public const string ColumnName162 = "MTCMemberUIN13";
        public const string ColumnName163 = "MTCMemberName13";
        public const string ColumnName164 = "MTCMemberResidentialStatus13";
        public const string ColumnName165 = "MTCMemberDOB13";
        public const string ColumnName166 = "MTCMemberGender13";
        public const string ColumnName167 = "MTCMemberRelationship13";
        public const string ColumnName168 = "MTCMemberStatusTags13";
        public const string ColumnName169 = "MTCMemberLatestReusableConsentID13";
        public const string ColumnName170 = "MTCMemberLatestReusableConsentScope13";
        public const string ColumnName171 = "MTCMemberUIN14";
        public const string ColumnName172 = "MTCMemberName14";
        public const string ColumnName173 = "MTCMemberResidentialStatus14";
        public const string ColumnName174 = "MTCMemberDOB14";
        public const string ColumnName175 = "MTCMemberGender14";
        public const string ColumnName176 = "MTCMemberRelationship14";
        public const string ColumnName177 = "MTCMemberStatusTags14";
        public const string ColumnName178 = "MTCMemberLatestReusableConsentID14";
        public const string ColumnName179 = "MTCMemberLatestReusableConsentScope14";
        public const string ColumnName180 = "MTCMemberUIN15";
        public const string ColumnName181 = "MTCMemberName15";
        public const string ColumnName182 = "MTCMemberResidentialStatus15";
        public const string ColumnName183 = "MTCMemberDOB15";
        public const string ColumnName184 = "MTCMemberGender15";
        public const string ColumnName185 = "MTCMemberRelationship15";
        public const string ColumnName186 = "MTCMemberStatusTags15";
        public const string ColumnName187 = "MTCMemberLatestReusableConsentID15";
        public const string ColumnName188 = "MTCMemberLatestReusableConsentScope15";
        public const string ColumnName189 = "MTCMemberUIN16";
        public const string ColumnName190 = "MTCMemberName16";
        public const string ColumnName191 = "MTCMemberResidentialStatus16";
        public const string ColumnName192 = "MTCMemberDOB16";
        public const string ColumnName193 = "MTCMemberGender16";
        public const string ColumnName194 = "MTCMemberRelationship16";
        public const string ColumnName195 = "MTCMemberStatusTags16";
        public const string ColumnName196 = "MTCMemberLatestReusableConsentID16";
        public const string ColumnName197 = "MTCMemberLatestReusableConsentScope16";
        public const string ColumnName198 = "MTCMemberUIN17";
        public const string ColumnName199 = "MTCMemberName17";
        public const string ColumnName200 = "MTCMemberResidentialStatus17";
        public const string ColumnName201 = "MTCMemberDOB17";
        public const string ColumnName202 = "MTCMemberGender17";
        public const string ColumnName203 = "MTCMemberRelationship17";
        public const string ColumnName204 = "MTCMemberStatusTags17";
        public const string ColumnName205 = "MTCMemberLatestReusableConsentID17";
        public const string ColumnName206 = "MTCMemberLatestReusableConsentScope17";
        public const string ColumnName207 = "MTCMemberUIN18";
        public const string ColumnName208 = "MTCMemberName18";
        public const string ColumnName209 = "MTCMemberResidentialStatus18";
        public const string ColumnName210 = "MTCMemberDOB18";
        public const string ColumnName211 = "MTCMemberGender18";
        public const string ColumnName212 = "MTCMemberRelationship18";
        public const string ColumnName213 = "MTCMemberStatusTags18";
        public const string ColumnName214 = "MTCMemberLatestReusableConsentID18";
        public const string ColumnName215 = "MTCMemberLatestReusableConsentScope18";
        public const string ColumnName216 = "MTCMemberUIN19";
        public const string ColumnName217 = "MTCMemberName19";
        public const string ColumnName218 = "MTCMemberResidentialStatus19";
        public const string ColumnName219 = "MTCMemberDOB19";
        public const string ColumnName220 = "MTCMemberGender19";
        public const string ColumnName221 = "MTCMemberRelationship19";
        public const string ColumnName222 = "MTCMemberStatusTags19";
        public const string ColumnName223 = "MTCMemberLatestReusableConsentID19";
        public const string ColumnName224 = "MTCMemberLatestReusableConsentScope19";
        public const string ColumnName225 = "MTCMemberUIN20";
        public const string ColumnName226 = "MTCMemberName20";
        public const string ColumnName227 = "MTCMemberResidentialStatus20";
        public const string ColumnName228 = "MTCMemberDOB20";
        public const string ColumnName229 = "MTCMemberGender20";
        public const string ColumnName230 = "MTCMemberRelationship20";
        public const string ColumnName231 = "MTCMemberStatusTags20";
        public const string ColumnName232 = "MTCMemberLatestReusableConsentID20";
        public const string ColumnName233 = "MTCMemberLatestReusableConsentScope20";
    }

    public class TestClass
    {
        private DataTable CreateDataTableStructure()
        {
            var arrColumns = new DataColumn[]
            {
                new DataColumn(QMCOutputColumnName.ColumnName1),
                new DataColumn(QMCOutputColumnName.ColumnName2),
                new DataColumn(QMCOutputColumnName.ColumnName3),
                new DataColumn(QMCOutputColumnName.ColumnName4),
                new DataColumn(QMCOutputColumnName.ColumnName5),
                new DataColumn(QMCOutputColumnName.ColumnName6),
                new DataColumn(QMCOutputColumnName.ColumnName7),
                new DataColumn(QMCOutputColumnName.ColumnName8),
                new DataColumn(QMCOutputColumnName.ColumnName9),
                new DataColumn(QMCOutputColumnName.ColumnName10),
                new DataColumn(QMCOutputColumnName.ColumnName11),
                new DataColumn(QMCOutputColumnName.ColumnName12),
                new DataColumn(QMCOutputColumnName.ColumnName13),
                new DataColumn(QMCOutputColumnName.ColumnName14),
                new DataColumn(QMCOutputColumnName.ColumnName15),
                new DataColumn(QMCOutputColumnName.ColumnName16),
                new DataColumn(QMCOutputColumnName.ColumnName17),
                new DataColumn(QMCOutputColumnName.ColumnName18),
                new DataColumn(QMCOutputColumnName.ColumnName19),
                new DataColumn(QMCOutputColumnName.ColumnName20),
                new DataColumn(QMCOutputColumnName.ColumnName21),
                new DataColumn(QMCOutputColumnName.ColumnName22),
                new DataColumn(QMCOutputColumnName.ColumnName23),
                new DataColumn(QMCOutputColumnName.ColumnName24),
                new DataColumn(QMCOutputColumnName.ColumnName25),
                new DataColumn(QMCOutputColumnName.ColumnName26),
                new DataColumn(QMCOutputColumnName.ColumnName27),
                new DataColumn(QMCOutputColumnName.ColumnName28),
                new DataColumn(QMCOutputColumnName.ColumnName29),
                new DataColumn(QMCOutputColumnName.ColumnName30),
                new DataColumn(QMCOutputColumnName.ColumnName31),
                new DataColumn(QMCOutputColumnName.ColumnName32),
                new DataColumn(QMCOutputColumnName.ColumnName33),
                new DataColumn(QMCOutputColumnName.ColumnName34),
                new DataColumn(QMCOutputColumnName.ColumnName35),
                new DataColumn(QMCOutputColumnName.ColumnName36),
                new DataColumn(QMCOutputColumnName.ColumnName37),
                new DataColumn(QMCOutputColumnName.ColumnName38),
                new DataColumn(QMCOutputColumnName.ColumnName39),
                new DataColumn(QMCOutputColumnName.ColumnName40),
                new DataColumn(QMCOutputColumnName.ColumnName41),
                new DataColumn(QMCOutputColumnName.ColumnName42),
                new DataColumn(QMCOutputColumnName.ColumnName43),
                new DataColumn(QMCOutputColumnName.ColumnName44),
                new DataColumn(QMCOutputColumnName.ColumnName45),
                new DataColumn(QMCOutputColumnName.ColumnName46),
                new DataColumn(QMCOutputColumnName.ColumnName47),
                new DataColumn(QMCOutputColumnName.ColumnName48),
                new DataColumn(QMCOutputColumnName.ColumnName49),
                new DataColumn(QMCOutputColumnName.ColumnName50),
                new DataColumn(QMCOutputColumnName.ColumnName51),
                new DataColumn(QMCOutputColumnName.ColumnName52),
                new DataColumn(QMCOutputColumnName.ColumnName53),
                new DataColumn(QMCOutputColumnName.ColumnName54),
                new DataColumn(QMCOutputColumnName.ColumnName55),
                new DataColumn(QMCOutputColumnName.ColumnName56),
                new DataColumn(QMCOutputColumnName.ColumnName57),
                new DataColumn(QMCOutputColumnName.ColumnName58),
                new DataColumn(QMCOutputColumnName.ColumnName59),
                new DataColumn(QMCOutputColumnName.ColumnName60),
                new DataColumn(QMCOutputColumnName.ColumnName61),
                new DataColumn(QMCOutputColumnName.ColumnName62),
                new DataColumn(QMCOutputColumnName.ColumnName63),
                new DataColumn(QMCOutputColumnName.ColumnName64),
                new DataColumn(QMCOutputColumnName.ColumnName65),
                new DataColumn(QMCOutputColumnName.ColumnName66),
                new DataColumn(QMCOutputColumnName.ColumnName67),
                new DataColumn(QMCOutputColumnName.ColumnName68),
                new DataColumn(QMCOutputColumnName.ColumnName69),
                new DataColumn(QMCOutputColumnName.ColumnName70),
                new DataColumn(QMCOutputColumnName.ColumnName71),
                new DataColumn(QMCOutputColumnName.ColumnName72),
                new DataColumn(QMCOutputColumnName.ColumnName73),
                new DataColumn(QMCOutputColumnName.ColumnName74),
                new DataColumn(QMCOutputColumnName.ColumnName75),
                new DataColumn(QMCOutputColumnName.ColumnName76),
                new DataColumn(QMCOutputColumnName.ColumnName77),
                new DataColumn(QMCOutputColumnName.ColumnName78),
                new DataColumn(QMCOutputColumnName.ColumnName79),
                new DataColumn(QMCOutputColumnName.ColumnName80),
                new DataColumn(QMCOutputColumnName.ColumnName81),
                new DataColumn(QMCOutputColumnName.ColumnName82),
                new DataColumn(QMCOutputColumnName.ColumnName83),
                new DataColumn(QMCOutputColumnName.ColumnName84),
                new DataColumn(QMCOutputColumnName.ColumnName85),
                new DataColumn(QMCOutputColumnName.ColumnName86),
                new DataColumn(QMCOutputColumnName.ColumnName87),
                new DataColumn(QMCOutputColumnName.ColumnName88),
                new DataColumn(QMCOutputColumnName.ColumnName89),
                new DataColumn(QMCOutputColumnName.ColumnName90),
                new DataColumn(QMCOutputColumnName.ColumnName91),
                new DataColumn(QMCOutputColumnName.ColumnName92),
                new DataColumn(QMCOutputColumnName.ColumnName93),
                new DataColumn(QMCOutputColumnName.ColumnName94),
                new DataColumn(QMCOutputColumnName.ColumnName95),
                new DataColumn(QMCOutputColumnName.ColumnName96),
                new DataColumn(QMCOutputColumnName.ColumnName97),
                new DataColumn(QMCOutputColumnName.ColumnName98),
                new DataColumn(QMCOutputColumnName.ColumnName99),
                new DataColumn(QMCOutputColumnName.ColumnName100),
                new DataColumn(QMCOutputColumnName.ColumnName101),
                new DataColumn(QMCOutputColumnName.ColumnName102),
                new DataColumn(QMCOutputColumnName.ColumnName103),
                new DataColumn(QMCOutputColumnName.ColumnName104),
                new DataColumn(QMCOutputColumnName.ColumnName105),
                new DataColumn(QMCOutputColumnName.ColumnName106),
                new DataColumn(QMCOutputColumnName.ColumnName107),
                new DataColumn(QMCOutputColumnName.ColumnName108),
                new DataColumn(QMCOutputColumnName.ColumnName109),
                new DataColumn(QMCOutputColumnName.ColumnName110),
                new DataColumn(QMCOutputColumnName.ColumnName111),
                new DataColumn(QMCOutputColumnName.ColumnName112),
                new DataColumn(QMCOutputColumnName.ColumnName113),
                new DataColumn(QMCOutputColumnName.ColumnName114),
                new DataColumn(QMCOutputColumnName.ColumnName115),
                new DataColumn(QMCOutputColumnName.ColumnName116),
                new DataColumn(QMCOutputColumnName.ColumnName117),
                new DataColumn(QMCOutputColumnName.ColumnName118),
                new DataColumn(QMCOutputColumnName.ColumnName119),
                new DataColumn(QMCOutputColumnName.ColumnName120),
                new DataColumn(QMCOutputColumnName.ColumnName121),
                new DataColumn(QMCOutputColumnName.ColumnName122),
                new DataColumn(QMCOutputColumnName.ColumnName123),
                new DataColumn(QMCOutputColumnName.ColumnName124),
                new DataColumn(QMCOutputColumnName.ColumnName125),
                new DataColumn(QMCOutputColumnName.ColumnName126),
                new DataColumn(QMCOutputColumnName.ColumnName127),
                new DataColumn(QMCOutputColumnName.ColumnName128),
                new DataColumn(QMCOutputColumnName.ColumnName129),
                new DataColumn(QMCOutputColumnName.ColumnName130),
                new DataColumn(QMCOutputColumnName.ColumnName131),
                new DataColumn(QMCOutputColumnName.ColumnName132),
                new DataColumn(QMCOutputColumnName.ColumnName133),
                new DataColumn(QMCOutputColumnName.ColumnName134),
                new DataColumn(QMCOutputColumnName.ColumnName135),
                new DataColumn(QMCOutputColumnName.ColumnName136),
                new DataColumn(QMCOutputColumnName.ColumnName137),
                new DataColumn(QMCOutputColumnName.ColumnName138),
                new DataColumn(QMCOutputColumnName.ColumnName139),
                new DataColumn(QMCOutputColumnName.ColumnName140),
                new DataColumn(QMCOutputColumnName.ColumnName141),
                new DataColumn(QMCOutputColumnName.ColumnName142),
                new DataColumn(QMCOutputColumnName.ColumnName143),
                new DataColumn(QMCOutputColumnName.ColumnName144),
                new DataColumn(QMCOutputColumnName.ColumnName145),
                new DataColumn(QMCOutputColumnName.ColumnName146),
                new DataColumn(QMCOutputColumnName.ColumnName147),
                new DataColumn(QMCOutputColumnName.ColumnName148),
                new DataColumn(QMCOutputColumnName.ColumnName149),
                new DataColumn(QMCOutputColumnName.ColumnName150),
                new DataColumn(QMCOutputColumnName.ColumnName151),
                new DataColumn(QMCOutputColumnName.ColumnName152),
                new DataColumn(QMCOutputColumnName.ColumnName153),
                new DataColumn(QMCOutputColumnName.ColumnName154),
                new DataColumn(QMCOutputColumnName.ColumnName155),
                new DataColumn(QMCOutputColumnName.ColumnName156),
                new DataColumn(QMCOutputColumnName.ColumnName157),
                new DataColumn(QMCOutputColumnName.ColumnName158),
                new DataColumn(QMCOutputColumnName.ColumnName159),
                new DataColumn(QMCOutputColumnName.ColumnName160),
                new DataColumn(QMCOutputColumnName.ColumnName161),
                new DataColumn(QMCOutputColumnName.ColumnName162),
                new DataColumn(QMCOutputColumnName.ColumnName163),
                new DataColumn(QMCOutputColumnName.ColumnName164),
                new DataColumn(QMCOutputColumnName.ColumnName165),
                new DataColumn(QMCOutputColumnName.ColumnName166),
                new DataColumn(QMCOutputColumnName.ColumnName167),
                new DataColumn(QMCOutputColumnName.ColumnName168),
                new DataColumn(QMCOutputColumnName.ColumnName169),
                new DataColumn(QMCOutputColumnName.ColumnName170),
                new DataColumn(QMCOutputColumnName.ColumnName171),
                new DataColumn(QMCOutputColumnName.ColumnName172),
                new DataColumn(QMCOutputColumnName.ColumnName173),
                new DataColumn(QMCOutputColumnName.ColumnName174),
                new DataColumn(QMCOutputColumnName.ColumnName175),
                new DataColumn(QMCOutputColumnName.ColumnName176),
                new DataColumn(QMCOutputColumnName.ColumnName177),
                new DataColumn(QMCOutputColumnName.ColumnName178),
                new DataColumn(QMCOutputColumnName.ColumnName179),
                new DataColumn(QMCOutputColumnName.ColumnName180),
                new DataColumn(QMCOutputColumnName.ColumnName181),
                new DataColumn(QMCOutputColumnName.ColumnName182),
                new DataColumn(QMCOutputColumnName.ColumnName183),
                new DataColumn(QMCOutputColumnName.ColumnName184),
                new DataColumn(QMCOutputColumnName.ColumnName185),
                new DataColumn(QMCOutputColumnName.ColumnName186),
                new DataColumn(QMCOutputColumnName.ColumnName187),
                new DataColumn(QMCOutputColumnName.ColumnName188),
                new DataColumn(QMCOutputColumnName.ColumnName189),
                new DataColumn(QMCOutputColumnName.ColumnName190),
                new DataColumn(QMCOutputColumnName.ColumnName191),
                new DataColumn(QMCOutputColumnName.ColumnName192),
                new DataColumn(QMCOutputColumnName.ColumnName193),
                new DataColumn(QMCOutputColumnName.ColumnName194),
                new DataColumn(QMCOutputColumnName.ColumnName195),
                new DataColumn(QMCOutputColumnName.ColumnName196),
                new DataColumn(QMCOutputColumnName.ColumnName197),
                new DataColumn(QMCOutputColumnName.ColumnName198),
                new DataColumn(QMCOutputColumnName.ColumnName199),
                new DataColumn(QMCOutputColumnName.ColumnName200),
                new DataColumn(QMCOutputColumnName.ColumnName201),
                new DataColumn(QMCOutputColumnName.ColumnName202),
                new DataColumn(QMCOutputColumnName.ColumnName203),
                new DataColumn(QMCOutputColumnName.ColumnName204),
                new DataColumn(QMCOutputColumnName.ColumnName205),
                new DataColumn(QMCOutputColumnName.ColumnName206),
                new DataColumn(QMCOutputColumnName.ColumnName207),
                new DataColumn(QMCOutputColumnName.ColumnName208),
                new DataColumn(QMCOutputColumnName.ColumnName209),
                new DataColumn(QMCOutputColumnName.ColumnName210),
                new DataColumn(QMCOutputColumnName.ColumnName211),
                new DataColumn(QMCOutputColumnName.ColumnName212),
                new DataColumn(QMCOutputColumnName.ColumnName213),
                new DataColumn(QMCOutputColumnName.ColumnName214),
                new DataColumn(QMCOutputColumnName.ColumnName215),
                new DataColumn(QMCOutputColumnName.ColumnName216),
                new DataColumn(QMCOutputColumnName.ColumnName217),
                new DataColumn(QMCOutputColumnName.ColumnName218),
                new DataColumn(QMCOutputColumnName.ColumnName219),
                new DataColumn(QMCOutputColumnName.ColumnName220),
                new DataColumn(QMCOutputColumnName.ColumnName221),
                new DataColumn(QMCOutputColumnName.ColumnName222),
                new DataColumn(QMCOutputColumnName.ColumnName223),
                new DataColumn(QMCOutputColumnName.ColumnName224),
                new DataColumn(QMCOutputColumnName.ColumnName225),
                new DataColumn(QMCOutputColumnName.ColumnName226),
                new DataColumn(QMCOutputColumnName.ColumnName227),
                new DataColumn(QMCOutputColumnName.ColumnName228),
                new DataColumn(QMCOutputColumnName.ColumnName229),
                new DataColumn(QMCOutputColumnName.ColumnName230),
                new DataColumn(QMCOutputColumnName.ColumnName231),
                new DataColumn(QMCOutputColumnName.ColumnName232),
                new DataColumn(QMCOutputColumnName.ColumnName233)
            };

            var dataTableResult = new DataTable();
            dataTableResult.Columns.AddRange(arrColumns);
            return dataTableResult;
        }

        private void PrepareDataRow(DataRow dataRow)
        {
            dataRow[QMCOutputColumnName.ColumnName1] = "S1002126Z";
            dataRow[QMCOutputColumnName.ColumnName2] = "Member of S1002126Z";
            dataRow[QMCOutputColumnName.ColumnName3] = "searchSubject.ResidentialStatus";
            dataRow[QMCOutputColumnName.ColumnName4] = "searchSubject.Race";
            dataRow[QMCOutputColumnName.ColumnName5] = "searchSubject.DOB";
            dataRow[QMCOutputColumnName.ColumnName6] = "searchSubject.Gender";
            dataRow[QMCOutputColumnName.ColumnName7] = "searchSubject.LivingStatus";
            dataRow[QMCOutputColumnName.ColumnName8] = "searchSubject.DemisedDate";
            dataRow[QMCOutputColumnName.ColumnName9] = "searchSubject.InactiveDate";
            dataRow[QMCOutputColumnName.ColumnName10] = "searchSubject.PassType";
            dataRow[QMCOutputColumnName.ColumnName11] = "searchSubject.PassStartDate";
            dataRow[QMCOutputColumnName.ColumnName12] = "searchSubject.PassEndDate";
            dataRow[QMCOutputColumnName.ColumnName13] = "addressEntity.PostalCode";
            dataRow[QMCOutputColumnName.ColumnName14] = "addressEntity.BlockNo";
            dataRow[QMCOutputColumnName.ColumnName15] = "addressEntity.Floor";
            dataRow[QMCOutputColumnName.ColumnName16] = "addressEntity.UnitNo";
            dataRow[QMCOutputColumnName.ColumnName17] = "addressEntity.StreetName";
            dataRow[QMCOutputColumnName.ColumnName18] = "addressEntity.BuildingName";
            dataRow[QMCOutputColumnName.ColumnName19] = "addressEntity.HDBFlatType";
            dataRow[QMCOutputColumnName.ColumnName20] = "searchSubject.GenerationIndicator";
        }

        public DataTable GetOutputRecordData()
        {
            var dataTableResult = CreateDataTableStructure();
            for (int i = 0; i < 20; i++)
            {
                var dataRow = dataTableResult.NewRow();
                PrepareDataRow(dataRow);
                dataTableResult.Rows.Add(dataRow);
            }

            return dataTableResult;
        }
    }
}
