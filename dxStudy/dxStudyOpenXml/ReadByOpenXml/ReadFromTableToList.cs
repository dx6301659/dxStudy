using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace dxStudyOpenXml.ReadByOpenXml
{
    public class ReadFromTableToList : OpenXmlBase
    {
        public List<T> ReadListFromExcelSheet<T>(string strExcelFilePath, string strSheetName, string strTableName, Dictionary<string, string> dicPropertyNameColumnNameMapping) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(strExcelFilePath) || string.IsNullOrWhiteSpace(strSheetName) || string.IsNullOrWhiteSpace(strTableName))
                return null;

            if (dicPropertyNameColumnNameMapping == null || dicPropertyNameColumnNameMapping.Count == 0)
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

                return MakeList<T>(tableDefinitionPart.Table, workbookPart, listRow, dicPropertyNameColumnNameMapping);
            }
        }

        private List<T> MakeList<T>(Table objTable, WorkbookPart workbookPart, IEnumerable<Row> listRow, Dictionary<string, string> dicPropertyNameColumnNameMapping) where T : class, new()
        {
            string strPositions = objTable.Reference.Value;
            string[] arrPositions = strPositions.Split(':');
            string strStartPosition = arrPositions[0];
            string strEndPosition = arrPositions[1];
            string strStartRow = Regex.Replace(strStartPosition, "[a-zA-Z]", "");
            string strEndRow = Regex.Replace(strEndPosition, "[a-zA-Z]", "");
            int intStartRow = Convert.ToInt32(strStartRow);
            int intEndRow = Convert.ToInt32(strEndRow);

            var columnRow = listRow.First(item => item.RowIndex == intStartRow);
            var listColumnCell = columnRow.Descendants<Cell>();
            var dicPropertyPosition = new Dictionary<string, string>();
            var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
            foreach (var item in dicPropertyNameColumnNameMapping)
            {
                if (string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.Value))
                    continue;

                string strPropertyName = item.Key.Trim();
                string strColumnName = item.Value.Trim();

                var columnCell = listColumnCell.FirstOrDefault(cellItem =>
                {
                    string strCellValue = GetCellValue(sharedStringTable, cellItem);
                    return strColumnName.Equals(strCellValue, StringComparison.OrdinalIgnoreCase);
                });

                if (columnCell != null)
                {
                    string strColumnPosition = Regex.Replace(columnCell.CellReference, "[0-9]", "");
                    dicPropertyPosition.Add(strPropertyName, strColumnPosition);
                }
            }

            var listResultRow = listRow.Where(item =>
            {
                int intCurrentRowIndex = (int)item.RowIndex.Value;
                return intCurrentRowIndex > intStartRow && intCurrentRowIndex <= intEndRow;
            });

            var listResult = new List<T>();
            var type = typeof(T);

            foreach (var row in listResultRow)
            {
                var listCell = row.Descendants<Cell>();
                if (listCell == null || listCell.Count() == 0)
                    continue;

                var tResult = new T();
                foreach (var item in dicPropertyPosition)
                {
                    string strCellPosition = item.Value;
                    if (string.IsNullOrWhiteSpace(strCellPosition))
                        continue;

                    strCellPosition = item.Value + row.RowIndex.Value;
                    var targetCell = listCell.FirstOrDefault(cell => strCellPosition.Equals(cell.CellReference, StringComparison.OrdinalIgnoreCase));
                    if (targetCell == null)
                        continue;

                    var property = type.GetProperty(item.Key);
                    if (property == null)
                        continue;

                    string strCellValue = GetCellValue(sharedStringTable, targetCell);
                    property.SetValue(tResult, strCellValue);
                }

                var rowIndexpPoperty = type.GetProperty("RowIndex");
                if (rowIndexpPoperty != null)
                    rowIndexpPoperty.SetValue(tResult, row.RowIndex.Value);

                listResult.Add(tResult);
            }

            return listResult;
        }
    }
}
