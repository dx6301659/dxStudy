using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXml.ReadByOpenXml
{
    public class ReadFromTableToDataTable : OpenXmlBase
    {
        public DataTable ReadTableFromExcelSheet(string strExcelFilePath, string strSheetName, string strTableName, string[] arrColumn)
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName) || string.IsNullOrWhiteSpace(strTableName) || arrColumn == null || arrColumn.Length == 0)
                return null;

            using (var document = SpreadsheetDocument.Open(strExcelFilePath, false))
            {
                var workbookPart = document.WorkbookPart;
                var listSheet = workbookPart.Workbook.Descendants<Sheet>();
                var objSheet = listSheet.FirstOrDefault(item => strSheetName.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                if (objSheet == null)
                    return null;

                var worksheetPart = workbookPart.GetPartById(objSheet.Id) as WorksheetPart;
                var listTable = worksheetPart.TableDefinitionParts;
                if (listTable == null || listTable.Count() == 0)
                    return null;

                var tableDefinitionPart = listTable.FirstOrDefault(item => strTableName.Equals(item.Table.DisplayName, StringComparison.OrdinalIgnoreCase));
                if (tableDefinitionPart == null)
                    return null;

                var listRow = worksheetPart.Worksheet.Descendants<Row>();
                if (listRow == null || listRow.Count() == 0)
                    return null;

                return MakeDataTable(tableDefinitionPart.Table, workbookPart, listRow, arrColumn);
            }
        }

        private DataTable MakeDataTable(Table objTable, WorkbookPart workbookPart, IEnumerable<Row> listRow, string[] arrColumn)
        {
            string strPositions = objTable.Reference.Value;
            string[] arrPositions = strPositions.Split(':');
            string strStartPosition = arrPositions[0];
            string strEndPosition = arrPositions[1];
            string strStartRow = Regex.Replace(strStartPosition, "[a-zA-Z]", "");
            string strEndRow = Regex.Replace(strEndPosition, "[a-zA-Z]", "");
            int intStartRow = Convert.ToInt32(strStartRow);
            int intEndRow = Convert.ToInt32(strEndRow);

            //make DataTable structure
            var dataTable = new DataTable(objTable.DisplayName);
            var dcRowIndex = new DataColumn("RowIndex");
            dataTable.Columns.Add(dcRowIndex);
            var columnRow = listRow.First(item => item.RowIndex == intStartRow);
            var listColumnCell = columnRow.Descendants<Cell>();
            var dicColumnPosition = new Dictionary<string, string>();
            var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
            foreach (string item in arrColumn)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var columnCell = listColumnCell.FirstOrDefault(cellItem =>
                {
                    string strCellValue = GetCellValue(sharedStringTable, cellItem);
                    return item.Equals(strCellValue, StringComparison.OrdinalIgnoreCase);
                });

                if (columnCell != null)
                {
                    var dataColumn = new DataColumn(item);
                    dataTable.Columns.Add(dataColumn);

                    string strColumnPosition = Regex.Replace(columnCell.CellReference, "[0-9]", "");
                    dicColumnPosition.Add(item, strColumnPosition);
                }
            }

            //make DataTable data
            var listResultRow = listRow.Where(item =>
            {
                int intCurrentRowIndex = (int)item.RowIndex.Value;
                return intCurrentRowIndex > intStartRow && intCurrentRowIndex <= intEndRow;
            });

            foreach (var row in listResultRow)
            {
                var listCell = row.Descendants<Cell>();
                if (listCell == null || listCell.Count() == 0)
                    continue;

                var dataRow = dataTable.NewRow();
                foreach (var item in dicColumnPosition)
                {
                    string strCellPosition = item.Value;
                    if (string.IsNullOrWhiteSpace(strCellPosition))
                        continue;

                    strCellPosition = item.Value + row.RowIndex.Value;
                    var targetCell = listCell.FirstOrDefault(cell => strCellPosition.Equals(cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        continue;

                    dataRow[item.Key] = GetCellValue(sharedStringTable, targetCell);
                }

                bool blnAnyColumnNotEmpty = false;
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    var dcData = dataRow[dataColumn.ColumnName];
                    blnAnyColumnNotEmpty = blnAnyColumnNotEmpty || (dcData != null && dcData != DBNull.Value);
                }

                if (blnAnyColumnNotEmpty)
                {
                    dataRow["RowIndex"] = row.RowIndex.Value;
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
