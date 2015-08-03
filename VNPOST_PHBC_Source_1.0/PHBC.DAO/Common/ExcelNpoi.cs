using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Reflection;
using System.IO;
using System.Data;

namespace PHBC.DAO.Common
{
    public class ExcelNpoi
    {
        /// <summary>
        /// Tao Style cho o excel
        /// </summary>
        /// <param name="wk"></param>
        /// <param name="Alignment">Can theo chieu doc</param>
        /// <param name="border"></param>
        /// <param name="FillBackgroundColor">Mau nen</param>
        /// <param name="FillForegroundColor">Ma khi chon</param>
        /// <param name="FillPattern">kieu to mau nen</param>
        /// <param name="font">kieu chu</param>
        /// <param name="IsHidden">o an</param>
        /// <param name="IsLocked"> o khoa</param>
        /// <param name="VerticalAlignment">can theo chieu ngang</param>
        /// <param name="WrapText">xuong dong</param>
        /// <param name="Rotation">Do nghieng cua chu - góc</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static HSSFCellStyle createCellType(HSSFWorkbook wk, HorizontalAlignment Alignment, BorderStyle border,
            short FillBackgroundColor, short FillForegroundColor, FillPattern fillPattern, HSSFFont font, bool IsHidden,
            bool IsLocked, VerticalAlignment VerticalAlignment, bool WrapText, short Rotation)
        {
            HSSFCellStyle result = (HSSFCellStyle)wk.CreateCellStyle();
            result.Alignment = Alignment;
            result.BorderBottom = result.BorderLeft = result.BorderRight = result.BorderTop = border;
            result.FillBackgroundColor = FillBackgroundColor;
            result.FillForegroundColor = FillForegroundColor;
            result.FillPattern = fillPattern;
            result.IsHidden = IsHidden;
            result.IsLocked = IsLocked;
            result.SetFont(font);
            result.VerticalAlignment = VerticalAlignment;
            result.WrapText = WrapText;
            result.Rotation = Rotation;
            return result;
        }

        /// <summary>
        /// Tao font
        /// </summary>
        /// <param name="wk">WorkBook</param>
        /// <param name="Boldweight">Độ đậm - FontBoldWeight</param>
        /// <param name="color">Màu chữ - FontColor</param>
        /// <param name="fontsize">cỡ chữ</param>
        /// <param name="FontName">kiểu chữ</param>
        /// <param name="IsItalic">in nghiêng</param>
        /// <param name="Underline">ngạch chân - FontUnderlineType</param>
        /// <param name="TypeOffset">Vi tri - FontSuperScript</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static HSSFFont createFont(HSSFWorkbook wk, short Boldweight, short color, short fontsize, string FontName, bool IsItalic,
            FontUnderlineType Underline, FontSuperScript TypeOffset)
        {
            HSSFFont result = (HSSFFont)wk.CreateFont();
            result.Boldweight = Boldweight;
            result.Color = color;
            result.FontHeightInPoints = fontsize;
            result.FontName = FontName;
            result.IsItalic = IsItalic;
            result.Underline = Underline;
            result.TypeOffset = TypeOffset;
            return result;
        }

        /// <summary>
        /// Tao Merged va tao hoac lay row tuong uong
        /// Sao khi goi ham nay thi chi dung ham HSSFSheet.GetRow hoac  ExcelNpoi.GetCreateRow voi nhung row trong vung
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellType"></param>
        /// <param name="rowFrom"></param>
        /// <param name="colFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="colTo"></param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void AddMergedRegion(HSSFSheet sheet, HSSFCellStyle cellType, int rowFrom, int colFrom, int rowTo, int colTo)
        {
            HSSFRow row = GetCreateRow(sheet, rowFrom);
            sheet.AddMergedRegion(new CellRangeAddress(rowFrom, rowTo, colFrom, colTo));
            HSSFCell hcell = (HSSFCell)row.GetCell(colFrom);
            if (hcell != null)
                cellType = (HSSFCellStyle)hcell.CellStyle;
            int xrow = rowTo - rowFrom;
            switch (xrow)
            {
                case 0:
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                    }
                    break;
                case 1:
                    HSSFRow rowT = GetCreateRow(sheet, rowTo);
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                        rowT.CreateCell(i).CellStyle = cellType;
                    }
                    rowT.CreateCell(colFrom).CellStyle = cellType;
                    break;
                default:
                    HSSFRow rowT2 = GetCreateRow(sheet, rowTo);
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                        rowT2.CreateCell(i).CellStyle = cellType;
                    }
                    rowT2.CreateCell(colFrom).CellStyle = cellType;
                    if (colFrom != colTo)
                    {
                        for (int j = rowFrom + 1; j < rowTo; j++)
                        {
                            row = GetCreateRow(sheet, j);
                            row.CreateCell(colFrom).CellStyle = cellType;
                            row.CreateCell(colTo).CellStyle = cellType;
                        }
                    }
                    else
                    {
                        for (int j = rowFrom + 1; j < rowTo; j++)
                        {
                            row = GetCreateRow(sheet, j);
                            row.CreateCell(colFrom).CellStyle = cellType;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Lay row o vi tri hien tai neu khong co thi tao moi
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static HSSFRow GetCreateRow(HSSFSheet sheet, int rowIndex)
        {
            HSSFRow hsRow = (HSSFRow)sheet.GetRow(rowIndex);
            if (hsRow == null)
                return (HSSFRow)sheet.CreateRow(rowIndex);
            else
                return hsRow;
        }

        /// <summary>
        /// Hàm vẽ một line trên file excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="col1">Cột chứa điểm đầu của line</param>
        /// <param name="row1">Dòng chứa điểm đầu của line</param>
        /// <param name="x1">Tọa độ X điểm đầu của line (Tọa độ x trong 1 cell có giá trị từ 0 - 1023)</param>
        /// <param name="y1">Tọa độ Y điểm đầu của line (Tọa độ Y trong 1 cell có giá trị từ 0 - 255)</param>
        /// <param name="col2">Cột chứa điểm cuối của line</param>
        /// <param name="row2">Dòng chứa điểm cuối của line</param>
        /// <param name="x2">Tọa độ X điểm cuối của line</param>
        /// <param name="y2">Tọa độ Y điểm cuối của line</param>
        /// <modified>
        /// Author      Date        comment
        /// TuanVM      15/07/2012  Tạo mới
        /// </modified>
        public static void DrawLine(HSSFSheet sheet, short col1, int row1, int x1, int y1, short col2, int row2, int x2, int y2)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            HSSFClientAnchor a1 = new HSSFClientAnchor();
            a1.SetAnchor(col1, row1, x1, y1, col2, row2, x2, y2);
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a1);
            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_LINE);
        }

        /// <summary>
        /// Hàm vẽ một line căn vào giữa của 2 cột trên sheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="col1">Cột chứa điểm bắt đầu của line</param>
        /// <param name="row">Dòng chứa điểm bắt đầu của line</param>
        /// <param name="x">Tọa độ x của điểm bắt đầu (Tọa độ x trong 1 cell có giá trị từ 0 - 1023)</param>
        /// <param name="y">Tọa độ y của điểm bắt đầu (Tọa độ Y trong 1 cell có giá trị từ 0 - 255)</param>
        /// <param name="col2"></param>
        /// <modified>
        /// Author      Date        comment
        /// TuanVM      15/07/2012  Tạo mới
        /// </modified>
        public static void DrawLinesToCenter(HSSFSheet sheet, short col1, int row, int x, int y, short col2)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            HSSFClientAnchor a1 = new HSSFClientAnchor();
            if (col2 == col1)
                a1.SetAnchor(col1, row, x, y, col2, row, 1024 - x, y);
            else
            {
                int col1Width = sheet.GetColumnWidth(col1);
                int col2Width = sheet.GetColumnWidth(col2);

                a1.SetAnchor(col1, row, x, y, col2, row, 1024 - x * col1Width / col2Width, y);
            }
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a1);
            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_LINE);
        }

        /// <summary>
        /// Ghi danh mục vào một row trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="valueField">Tên cột dữ liệu</param>
        /// <param name="row">Dòng ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <param name="nameValue">Tên của vùng value</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByRow<T>(HSSFSheet sheet, List<T> lst, string textField, string valueField, int row, string nameText, string nameValue)
        {
            if (lst == null || lst.Count==0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            PropertyInfo pValue = p.First(a => a.Name == valueField);
            if (pText == null || pValue == null)
                throw new Exception("TextField hoạc ValueField không tồn tại");
            HSSFRow rowText = GetCreateRow(sheet, row);
            HSSFRow rowID = GetCreateRow(sheet, row+1);
            for (int i = 0; i < lst.Count; i++)
            {
                rowText.CreateCell(i).SetCellValue(pText.GetValue(lst[i], null).ToString());
                rowID.CreateCell(i).SetCellValue(pValue.GetValue(lst[i], null).ToString());
            }

            HSSFName hsName = (HSSFName)sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(row, row, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
            hsName = (HSSFName)sheet.Workbook.GetName(nameValue);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameValue;
            }
            cellRange = new CellRangeAddress(row, row+1, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Ghi danh mục vào một row trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="row">Dòng ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByRow<T>(HSSFSheet sheet, List<T> lst, string textField, int row, string nameText)
        {
            if (lst == null || lst.Count==0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            if (pText == null )
                throw new Exception("TextField không tồn tại");
            HSSFRow rowText = GetCreateRow(sheet,row);
            
            for (int i = 0; i < lst.Count; i++)
            {
                rowText.CreateCell(i).SetCellValue(pText.GetValue(lst[i], null).ToString());
            }

            HSSFName hsName = (HSSFName)sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(row, row, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }
       
        /// <summary>
        /// Ghi danh mục vào một column trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="valueField">Tên cột dữ liệu</param>
        /// <param name="col">Cột ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <param name="nameValue">Tên của vùng value</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByCol<T>(HSSFSheet sheet, List<T> lst, string textField, string valueField, int col, string nameText, string nameValue)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            PropertyInfo pValue = p.First(a => a.Name == valueField);
            if (pText == null || pValue == null)
                throw new Exception("TextField hoạc ValueField không tồn tại");
            HSSFRow hsrow;
            for (int i = 0; i < lst.Count; i++)
            {
                hsrow = GetCreateRow(sheet, i);
                hsrow.CreateCell(col).SetCellValue(pText.GetValue(lst[i], null).ToString());
                hsrow.CreateCell(col+1).SetCellValue(pValue.GetValue(lst[i], null).ToString());
            }

            HSSFName hsName = (HSSFName)sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(0, lst.Count, col, col);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
            hsName = (HSSFName)sheet.Workbook.GetName(nameValue);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameValue;
            }
            cellRange = new CellRangeAddress(0, lst.Count, col, col+1);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Ghi danh mục vào một column trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="col">Cột ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByCol<T>(HSSFSheet sheet, List<T> lst, string textField, int col, string nameText)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            if (pText == null)
                throw new Exception("TextField không tồn tại");

            for (int i = 0; i < lst.Count; i++)
            {
                GetCreateRow(sheet, i).CreateCell(col).SetCellValue(pText.GetValue(lst[i], null).ToString());
            }

            HSSFName hsName = (HSSFName)sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = (HSSFName)sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(0, lst.Count, col, col);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Đọc excel to HSSFWorkBook
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file excel</param>
        /// <param name="isDelete">Có xóa file gốc không</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static HSSFWorkbook ReadExcelToHSSFWorkBook(string strPath, bool isDelete=false)
        {
            FileInfo fi = new FileInfo(strPath);
            if (fi.Exists)
            {
                FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs);
                fs.Close();
                if (isDelete)
                    fi.Delete();
                return templateWorkbook;
            }
            return null;
        }
        /// <summary>
        /// Move hoạc copy row từ vi trí rowFrom đến rowTo. Không copy margin
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowFrom">Vi trí nguồn. Bắt đầu từ 0</param>
        /// <param name="rowTo">Vị trí đích. Bắt đầu từ 0</param>
        /// <param name="IsMove">True - Move, False - Copy</param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      24/07/2012  Tạo mới
        /// </modified>
        public static void moveOrCopyRow(HSSFSheet sheet, int rowFrom, int rowTo, bool IsMove=true)
        {
            HSSFRow hRowF = (HSSFRow)sheet.GetRow(rowFrom);
            if (hRowF == null)
                return;
            HSSFRow hRowT = (HSSFRow)sheet.CreateRow(rowTo);
            HSSFCell cellF;
            HSSFCell cellT;
            //hRowT = hRowF;
            int bd = (int)hRowF.FirstCellNum;
            int kt = (int)hRowF.LastCellNum;
            foreach (ICell item in hRowF.Cells)
            {
                cellF = (HSSFCell)item;
                cellT = (HSSFCell)hRowT.CreateCell(item.ColumnIndex);
                cellT.CellStyle = cellF.CellStyle;
                switch (cellF.CellType)
                {
                    case CellType.Numeric:
                        cellT.SetCellValue(cellF.NumericCellValue);
                        break;
                    case CellType.String:
                        cellT.SetCellValue(cellF.StringCellValue);
                        break;
                    case CellType.Formula:
                        cellT.SetCellFormula(cellF.CellFormula);
                        break;
                    case CellType.Boolean:
                        cellT.SetCellValue(cellF.BooleanCellValue);
                        break;
                    case CellType.Error:
                        cellT.SetCellValue(cellF.ErrorCellValue);
                        break;
                }
                if (cellF.IsMergedCell)
                    continue;
            }
            if(IsMove)
                sheet.RemoveRow(hRowF);
            
        }

        /// <summary>
        /// Xóa row từ vị trí rowFrom đến rowTo
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowFrom">Vi trí nguồn. Bắt đầu từ 0</param>
        /// <param name="rowTo">Vị trí đích. Bắt đầu từ 0</param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      24/07/2012  Tạo mới
        /// </modified>
        public static void RemoveRow(HSSFSheet sheet, int rowFrom, int rowTo)
        {
            for (int i = rowFrom; i < rowTo; i++)
            {
                HSSFRow hRow = (HSSFRow)sheet.GetRow(i);
                if (hRow != null)
                    sheet.RemoveRow(hRow);
            }
        }
		
        /// <summary>
        /// Đọc excel và ghi dữ liệu HSSFWorkBook Chưa hoàn thành
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file excel</param>
        /// <param name="dtSource"></param>
		/// <param name="rowStart"></param>
		/// <param name="colStart"></param>
		/// <param name="IsCount">True - Có STT, False - Không có STT</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
		public static HSSFWorkbook WriteExcelByTemp(string strPath, DataTable dtSource, int rowStart, int colStart, bool IsCount=false)
		{
			HSSFWorkbook wb = ReadExcelToHSSFWorkBook(strPath);
			if(wb==null)return wb;
			int rowCount = dtSource.Rows.Count;
			if(rowCount==0)
				return wb;
			int colCount = dtSource.Columns.Count;
			int colSTT = 0;
			if(IsCount)
				colSTT++;
			HSSFSheet sheet = (HSSFSheet)wb.GetSheetAt(0);
			HSSFRow Row = (HSSFRow)sheet.GetRow(rowStart);
			if(Row.Cells.Count<colCount + colSTT)
				return wb;
			HSSFCellStyle []lstCellStyle = new HSSFCellStyle[colCount+colSTT];
			HSSFCell Cell;
			for(int i = 0;i<colCount+colSTT;i++)
			{
				Cell = (HSSFCell)Row.GetCell(colStart + colSTT + i);
				lstCellStyle[0+colSTT] = (HSSFCellStyle)Cell.CellStyle;
				//UNDONE Lấy kiểu của dữ liệu
			}
			for(int i=1;i<dtSource.Rows.Count;i++)
			{		
			}
			return wb;
		}
        #region thu
        HSSFWorkbook Workbook;
        public enum cellType
        {
            none = 0,
            style1 = 1,
            style2 = 2,
            style3 = 3
        }
        int Kieu;
        public ExcelNpoi(HSSFWorkbook wk, int _kieu)
        {
            Workbook = wk;
            Kieu = _kieu;
            HSSFFont fontTile1_Bold = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 12, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontTile1_Normal = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 12, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontTile1_Italic = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 12, "Times New Roman", true, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontHead_Bold = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 18, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontHead_Normal = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 18, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontHead_Italic = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 18, "Times New Roman", true, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontTbHead_Bold = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 12, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontTbHead_Normal = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 12, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontTbHead_Italic = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 11, "Times New Roman", true, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontGroup_Bold = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 10, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontGroup_Normal = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 10, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontGroup_Italic = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 10, "Times New Roman", true, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontGroup_Underline = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 10, "Times New Roman", true, FontUnderlineType.Single, FontSuperScript.None);

            HSSFFont fontDetail_Bold = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Bold, 64, 10, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontDetail_Normal = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 10, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);

            HSSFFont fontDetail_Italic = ExcelNpoi.createFont(Workbook, (short)FontBoldWeight.Normal, 64, 10, "Times New Roman", true, FontUnderlineType.None, FontSuperScript.None);
            HSSFCellStyle styleTile1_Normal = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontTile1_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleTile1_Bold = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontTile1_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleTile1_Italic = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontTile1_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleHead_Bold = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontHead_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleHead_Normal = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontHead_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleHead_Italic = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.None,
                                                               64, 64, FillPattern.NoFill, fontHead_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleTbHead_Bold = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontTbHead_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleTbHead_BoldWarpText = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontTbHead_Bold, false, false, VerticalAlignment.Center, true, 0);
            HSSFCellStyle styleTbHead_Normal = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontTbHead_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleTbHead_Italic = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontTbHead_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_BoldCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_NormaCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_ItalicCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_BoldLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_NormalLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_ItalicLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_BoldRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Bold, false, false, VerticalAlignment.Center, false, 0);
            styleGroup_BoldRight.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
            HSSFCellStyle styleGroupNumber_BoldRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Bold, false, false, VerticalAlignment.Center, false, 0);
            styleGroupNumber_BoldRight.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
            HSSFCellStyle styleGroup_NormalRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_ItalicRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleGroup_Underline = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontGroup_Underline, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_BoldCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_NormalCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_ItalicCenter = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Center, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_BoldLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                                           64, 64, FillPattern.NoFill, fontDetail_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_NormalLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Normal, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_ItalicLeft = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Left, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Italic, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetail_BoldRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                                           64, 64, FillPattern.NoFill, fontDetail_Bold, false, false, VerticalAlignment.Center, false, 0);
            HSSFCellStyle styleDetailNumber_BoldRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                                           64, 64, FillPattern.NoFill, fontDetail_Bold, false, false, VerticalAlignment.Center, false, 0);
            styleDetailNumber_BoldRight.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            HSSFCellStyle styleDetail_NormalRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Normal, false, false, VerticalAlignment.Center, false, 0);
            styleDetail_NormalRight.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            HSSFCellStyle styleDetailNumber_NormalRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                               64, 64, FillPattern.NoFill, fontDetail_Normal, false, false, VerticalAlignment.Center, false, 0);
            styleDetailNumber_NormalRight.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
            HSSFCellStyle styleDetail_ItalicRight = ExcelNpoi.createCellType(Workbook, HorizontalAlignment.Right, NPOI.SS.UserModel.BorderStyle.Thin,
                                                            64, 64, FillPattern.NoFill, fontDetail_Italic, false, false, VerticalAlignment.Center, false, 0);
        }

        public HSSFCellStyle Heading1()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Heading2()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Heading3()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Heading4()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Header1()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Header2()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Header3()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Header4()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Group1()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;

        }

        public HSSFCellStyle Group2()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Group3()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }

        public HSSFCellStyle Group4()
        {
            HSSFCellStyle result = (HSSFCellStyle)Workbook.CreateCellStyle();
            return result;
        }
        #endregion
    }
}
