using System.Web.Mvc;

namespace PHBC.Web.Areas.Bao
{
    public class BaoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Bao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Bao_default",
                "Bao/{controller}/{action}/{id}",
                new {controller= "ThongTinBao", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                   "Bao_kehoachxuatban_edit",
                   "Bao/KeHoachXuatBan/Edit/{id}/{idkehoach}",
                   new { controller = "KeHoachXuatBan", action = "Add", id = UrlParameter.Optional }
               );
            context.MapRoute(
                   "Bao_kehoachxuatban_new",
                   "Bao/KeHoachXuatBan/{action}/{id}/{idkehoach}",
                   new { controller = "KeHoachXuatBan", action = "Index", id = UrlParameter.Optional }
               );
            context.MapRoute(
                   "Bao_kehoachxuatban_create_dieuchinh",
                   "Bao/KeHoachXuatBan/DieuChinh/TaoMoi/{type}/{id}",
                   new { controller = "DieuChinhKHXB", action = "CreateDieuChinh", id = UrlParameter.Optional }
               );
            context.MapRoute(
                   "Bao_DieuChinhKHXB_Create",
                   "Bao/DieuChinhKHXB/TaoMoi/{type}/{id}",
                   new { controller = "DieuChinhKHXB", action = "Create"}
               );
            //context.MapRoute(
            //       "Bao_kehoachxuatban_dieuchinhquy",
            //       "Bao/KeHoachXuatBan/DieuChinh/{type}/{id}/{idkehoach}",
            //       new { controller = "KeHoachXuatBan", action = "DieuChinhQuyKHXB", id = UrlParameter.Optional }
            //   );
   
        }
    }
}