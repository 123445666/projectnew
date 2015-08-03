namespace PHBC.Web.Constants
{
    public class Application
    {
        #region Button
        public struct ButtonLabel
        {
            public const string Save = "Lưu";
            public const string ToEdit = "Sửa";
            public const string Back = "Quay lại";
            public const string Create = "Thêm";
            public const string Delete = "Xóa";
            public const string Details = "Chi tiết";
            public const string List = "Danh sách";
            public const string Search = "Tìm kiếm";
            public const string Print = "In";
            public const string PrintAll = "In toàn bộ";
            public const string ExportPDF = "Xuất ra file Pdf";
            public const string ExportXLS = "Xuất ra file Excel";
            public const string ExportWord = "Xuất ra file Word";
            public const string AddAction = "Thêm chức năng";
            public const string AddUser = "Thêm người dùng";
            public const string UpdateDiemIn = "Cấp Nhật Điểm In";
            public const string KyXuatBan = "Kỳ Xuất Bản";
            public const string GiaMua = "Giá Mua";
        }

        public struct ButtonToltip
        {
            public const string Edit = "Sửa";
            public const string Create = "Thêm";
            public const string Delete = "Xóa";
            public const string Details = "Chi tiết";
            public const string Index = "Danh sách";
            public const string Search = "Tìm kiếm";
            public const string Print = "In";
            public const string PrintAll = "In toàn bộ";
            public const string ExportPDF = "Xuất ra file Pdf";
            public const string ExportXLS = "Xuất ra file Excel";
            public const string ExportWord = "Xuất ra file Word";
            public const string AddRole = "Thêm vai trò";
            public const string SetPassword = "Đặt mật khẩu";
        }
        public struct ButtonIcon
        {
            public const string Save = "<span class=\"fa fa-abc\"></span>";
            public const string ToEdit = "<span class=\"fa fa-pencil\"></span>";
            public const string Back = "<span class=\"fa fa-abc\"></span>";
            public const string Create = "<i class='fa fa-plus'></i>";
            public const string Delete = "<span class=\"fa fa-trash\"></span>";
            public const string Details = "<span class=\"fa fa-file-text\"></span>";
            public const string Choose = "<i class='glyphicon glyphicon-share'></i>";
            public const string TreeOpen = "<i class='fa fa-chevron-circle-down'></i>";
            public const string TreeClose = "<i class='fa fa-chevron-circle-right'></i>";
            public const string Search = "<span class=\"glyphicon glyphicon-search\" aria-hidden=\"true\"></span>";
            public const string UpdateDiemIn = "<span class=\"fa fa-users\"></span>";
            public const string KyXuatBan = "<span class=\"fa fa-calendar\"></span>";
            public const string AddAction = "<span class=\"fa fa-cogs\"></span>";
            public const string AddUser = "<span class=\"fa fa-users\"></span>";
            public const string AddRole = "<span class=\"fa fa-users\"></span>";
            public const string SetPassword = "<i class=\"fa fa-key\"></i>";
            public const string GiaBao = "<span class =\"glyphicon glyphicon-shopping-cart\"></span>";
        }
        public struct ButtonClass
        {
            public const string Default = "btn btn-info";
            public const string Submit = "btn btn-default";
        }
        #endregion

        #region form
        public struct FormLabel
        {
            public const string Edit = "Sửa";
            public const string Back = "Quay lại";
            public const string Create = "Thêm";
            public const string Delete = "Xóa";
            public const string Details = "Chi tiết";
            public const string List = "Danh sách";
            public const string Search = "Tìm kiếm";
            public const string Print = "In";
            public const string PrintAll = "In toàn bộ";
            public const string ExportPDF = "Xuất ra file Pdf";
            public const string ExportXLS = "Xuất ra file Excel";
            public const string ExportWord = "Xuất ra file Word";
        }
        public struct FormMessage
        {
            public const string Delete_Confirm = "Bạn có chắc chắn muốn xóa ?";
            public const string Delete = "Bạn muốn xóa";
            public const string CreateSuccess = "Bạn đã thêm mới thành công.";
            public const string CreateUnSuccess = "Bạn đã thêm mới thất bại!";
            public const string EditSuccess = "Bạn đã sửa thành công.";
            public const string EditUnSuccess = "Bạn đã sửa thất bại!";
        }
        public struct FormColorClass
        {
            public const string active = "active";
            public const string success = "success";
            public const string info = "info";
            public const string warning = "warning";
            public const string danger = "danger";
        }
        #endregion

        //Session
        public struct Session
        {
            public const string ModelSearch = "Search"; // tên của session luu model search
            public const string Permisson = "Permisson";//session permisson name
            public const string User = "User";
            public const string Message = "Message";//Lưu thông báo để gửi về index
            public const string UnitModel = "UnitModel";
            public const string UnitModelDieuChinh = "UnitModelDieuChinh";
        }

        //const name site
        public const string Name = "VNPOST PHBC";
        public const string AppendColon = "{0} :";
        public const string DateTimeFormat = "hh:mm - dd/MM/yyyy";
    }
}