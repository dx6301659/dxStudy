using DocumentFormat.OpenXml;
using XL14 = DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Linq;

namespace dxStudyOpenXmlChangeCellStyle
{
    class Program
    {
        static void Main(string[] args)
        {
            string sFile = @"C:\Users\dingxu\Desktop\123.xlsx";
            if (File.Exists(sFile))
            {
                File.Delete(sFile);
            }
            BuildWorkbook(sFile);
            //ModifyStyle(sFile);
            Console.WriteLine("Hello World!");
        }

        private static void ModifyStyle(string sFile)
        {
            using (SpreadsheetDocument xl = SpreadsheetDocument.Open(sFile, true))
            {
                Stylesheet styleSheet = xl.WorkbookPart.WorkbookStylesPart.Stylesheet;
                var desFills = styleSheet.Descendants<Fill>();

                // find red element and change to be black
                var res = from t in desFills
                          where t.Descendants<ForegroundColor>().FirstOrDefault() != null
                          && t.Descendants<ForegroundColor>().FirstOrDefault().Rgb.Value == "FFFF0000"
                          select t;
                if (res.Count() != 0)
                {
                    res.FirstOrDefault().Descendants<ForegroundColor>().FirstOrDefault().Rgb = "00000000";
                }
            }
        }

        private static void BuildWorkbook(string sFile)
        {
            try
            {
                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(sFile, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart wbp = xl.AddWorkbookPart();
                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                    Workbook wb = new Workbook();
                    FileVersion fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";


                    Worksheet ws = new Worksheet();
                    WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                    // add styles to sheet
                    wbsp.Stylesheet = CreateStylesheet();
                    wbsp.Stylesheet.Save();

                    // generate rows
                    SheetData sd = CreateSheetData();
                    ws.Append(sd);
                    wsp.Worksheet = ws;
                    wsp.Worksheet.Save();
                    Sheets sheets = new Sheets();
                    Sheet sheet = new Sheet();
                    sheet.Name = "Sheet1";
                    sheet.SheetId = 1;
                    sheet.Id = wbp.GetIdOfPart(wsp);
                    sheets.Append(sheet);
                    wb.Append(fv);
                    wb.Append(sheets);


                    xl.WorkbookPart.Workbook = wb;
                    xl.WorkbookPart.Workbook.Save();
                    xl.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        private static SheetData CreateSheetData()
        {
            SheetData sheetData1 = new SheetData();
            Row row1 = new Row() { RowIndex = (UInt32Value)1U, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D };
            Cell cell1 = new Cell() { CellReference = "A1", StyleIndex = (UInt32Value)1U };


            row1.Append(cell1);


            Row row2 = new Row() { RowIndex = (UInt32Value)2U, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D };
            Cell cell2 = new Cell() { CellReference = "B2", StyleIndex = (UInt32Value)2U };


            row2.Append(cell2);


            Row row3 = new Row() { RowIndex = (UInt32Value)3U, Spans = new ListValue<StringValue>() { InnerText = "1:3" }, DyDescent = 0.25D };
            Cell cell3 = new Cell() { CellReference = "C3", StyleIndex = (UInt32Value)3U };


            row3.Append(cell3);


            sheetData1.Append(row1);
            sheetData1.Append(row2);
            sheetData1.Append(row3);

            return sheetData1;
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");


            Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };


            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };


            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);


            fonts1.Append(font1);


            Fills fills1 = new Fills() { Count = (UInt32Value)5U };


            // FillId = 0
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.Append(patternFill1);


            // FillId = 1
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };
            fill2.Append(patternFill2);


            // FillId = 2,RED
            Fill fill3 = new Fill();
            PatternFill patternFill3 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = "FFFF0000" };
            BackgroundColor backgroundColor1 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill3.Append(foregroundColor1);
            patternFill3.Append(backgroundColor1);
            fill3.Append(patternFill3);


            // FillId = 3,BLUE
            Fill fill4 = new Fill();
            PatternFill patternFill4 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor2 = new ForegroundColor() { Rgb = "FF0070C0" };
            BackgroundColor backgroundColor2 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill4.Append(foregroundColor2);
            patternFill4.Append(backgroundColor2);
            fill4.Append(patternFill4);


            // FillId = 4,YELLO
            Fill fill5 = new Fill();
            PatternFill patternFill5 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor3 = new ForegroundColor() { Rgb = "FFFFFF00" };
            BackgroundColor backgroundColor3 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill5.Append(foregroundColor3);
            patternFill5.Append(backgroundColor3);
            fill5.Append(patternFill5);


            fills1.Append(fill1);
            fills1.Append(fill2);
            fills1.Append(fill3);
            fills1.Append(fill4);
            fills1.Append(fill5);


            Borders borders1 = new Borders() { Count = (UInt32Value)1U };


            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();


            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);


            borders1.Append(border1);


            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };


            cellStyleFormats1.Append(cellFormat1);


            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)4U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)3U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat5 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)4U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };


            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);
            cellFormats1.Append(cellFormat5);


            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };


            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleMedium9" };


            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();


            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            XL14.SlicerStyles slicerStyles1 = new XL14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };


            stylesheetExtension1.Append(slicerStyles1);


            stylesheetExtensionList1.Append(stylesheetExtension1);


            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);
            return stylesheet1;
        }

        static void CreateFonts(Stylesheet stylesheet1)
        {
            Font font1 = new Font()
            {
                FontSize = new FontSize() { Val = 11D },
                Color = new Color() { Theme = (UInt32Value)1U },
                FontName = new FontName() { Val = "Calibri" },
                FontFamilyNumbering = new FontFamilyNumbering() { Val = 2 },
                FontScheme = new FontScheme() { Val = FontSchemeValues.Minor }
            };

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };
            fonts1.Append(font1);

            stylesheet1.Append(fonts1);
        }

        static void CreateFills(Stylesheet stylesheet1)
        {
            Fills fills1 = new Fills();

            // FillId = 0
            Fill fill0 = new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.None }
            };
            fills1.Append(fill0);

            // FillId = 1
            Fill fill1 = new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 }
            };
            fills1.Append(fill1);

            // FillId = 2,RED
            Fill fill2 = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor() { Rgb = "FFFF0000" },
                    BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U }
                }
            };
            fills1.Append(fill2);

            // FillId = 3,BLUE
            Fill fill3 = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor() { Rgb = "FF0070C0" },
                    BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U }
                }
            };
            fills1.Append(fill3);

            // FillId = 4,YELLO
            Fill fill4 = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor() { Rgb = "FFFFFF00" },
                    BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U }
                }
            };
            fills1.Append(fill4);

            // FillId = 5, LIGHTGRAY
            Fill fill5 = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor() { Rgb = "D3D3D3" },
                    BackgroundColor = new BackgroundColor() { Indexed = (UInt32Value)64U }
                }
            };
            fills1.Append(fill5);

            stylesheet1.Append(fills1);
        }

        static void CreateBorders(Stylesheet stylesheet1)
        {
            Border border1 = new Border()
            {
                LeftBorder = new LeftBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Auto = true } },//{ Rgb = "FFFFFF00", Indexed = (UInt32Value)64U } };
                TopBorder = new TopBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Auto = true } }, //{ Rgb = "FFFFFF00", Indexed = (UInt32Value)64U } };
                RightBorder = new RightBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Auto = true } },
                //BottomBorder = 
            };



            BottomBorder bottomBorder1 = new BottomBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Auto = true } };//{ Rgb = "FFFFFF00", Indexed = (UInt32Value)64U } };
            border1.BottomBorder = bottomBorder1;

            Borders borders1 = new Borders();
            borders1.Append(border1);

            stylesheet1.Append(borders1);
        }

        static void CreateCellFormats(Stylesheet stylesheet1)
        {
            CellFormats cellFormats1 = new CellFormats();
            CellFormat cellFormat0 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)1U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)3U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)4U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat5 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)5U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };

            cellFormats1.Append(cellFormat0);
            cellFormats1.Append(cellFormat1);
            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);
            cellFormats1.Append(cellFormat5);

            stylesheet1.Append(cellFormats1);
        }

        static Stylesheet CreateStylesheet1()
        {
            Stylesheet stylesheet1 = new Stylesheet();
            CreateFonts(stylesheet1);
            CreateFills(stylesheet1);
            CreateBorders(stylesheet1);
            CreateCellFormats(stylesheet1);

            return stylesheet1;
        }
    }
}
