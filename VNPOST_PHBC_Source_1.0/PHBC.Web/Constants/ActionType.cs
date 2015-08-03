namespace PHBC.Web.Constants
{
    public static class ActionType
    {
        public const string Index = "Index";
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string Details = "Details";
        public const string Delete = "Delete";
        public const string Search = "Search";
        public const string Export = "Export";
        public const string ExportExcel = "ExportExcel";
        public const string ExportDoc = "ExportDoc";
        public const string ExportPdf = "ExportPdf";
        public const string Import = "Import";
        public const string Print = "Print";
        public const string GroupBase = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search;
        public const string GroupAdvance = Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
        public const string GroupAll = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search + "|" + Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
        public const string GiaMua = "GiaMua";
    }

    public static class ActionTypeDesc
    {
        public const string Index = "Danh sách";
        public const string Create = "Tạo mới";
        public const string Edit = "Sửa";
        public const string Details = "Chi tiết";
        public const string Delete = "Xóa";
        public const string Search = "Tìm kiếm";
        public const string Export = "Xuất";
        public const string ExportExcel = "Xuất exel";
        public const string ExportDoc = "Xuất doc";
        public const string ExportPdf = "Xuất pdf";
        public const string Import = "Import";
        public const string Print = "Print";
        public const string GroupBase = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search;
        public const string GroupAdvance = Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
        public const string GroupAll = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search + "|" + Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
    }
    public static class ActionTypeMenu
    {
        public const string Index = "1";
        public const string Create = "0";
        public const string Edit = "0";
        public const string Details = "0";
        public const string Delete = "0";
        public const string Search = "0";
        public const string Export = "0";
        public const string ExportExcel = "0";
        public const string ExportDoc = "0";
        public const string ExportPdf = "0";
        public const string Import = "0";
        public const string Print = "0";
        public const string GroupBase = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search;
        public const string GroupAdvance = Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
        public const string GroupAll = Index + "|" + Create + "|" + Edit + "|" + Details + "|" + Delete + "|" + Search + "|" + Export + "|" + ExportExcel + "|" + ExportDoc + "|" + ExportPdf + "|" + Import + "|" + Print;
    }
}