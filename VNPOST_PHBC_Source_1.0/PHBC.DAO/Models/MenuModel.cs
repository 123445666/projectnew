
namespace PHBC.DAO.Models
{
    using PHBC.DAO.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class MenuModel
    {
        public MenuModel()
        { }
        public MenuModel(SysMenu SysMenu)
        {
            this.Id = SysMenu.Id;
            this.Name = SysMenu.Name;
            this.ActionCode = SysMenu.ActionCode;
            this.Area = SysMenu.Area;
            this.Controller = SysMenu.Controller;
            this.Action = SysMenu.Action;
            this.Params = SysMenu.Params;
            this.Pram1 = SysMenu.Pram1;
            this.Pram2 = SysMenu.Pram2;
            this.Pram3 = SysMenu.Pram3;
            this.QuerryString = SysMenu.QuerryString;
            this.ParentId = SysMenu.ParentId;
            this.Order = SysMenu.Order;
            this.Description = SysMenu.Description;
            this.MenuType = SysMenu.MenuType;
            this.SysAction = SysMenu.SysAction;
            this.SysMenu1 = SysMenu.SysMenu1;
            this.SysMenu2 = SysMenu.SysMenu2;
            this.Icon = SysMenu.Icon;
            this.CreateBy = SysMenu.CreateBy;
            this.CreateDate = SysMenu.CreateDate;
            this.ModifyBy = SysMenu.ModifyBy;
            this.ModifyDate = SysMenu.ModifyDate;
        }
        [ScaffoldColumn(false)]
        [Display(Name = "ID Menu")]
        [Key]
        public string Id { get; set; }
        [Display(Name = "Tên Menu")]
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256)]
        public string Name { get; set; }

        [Display(Name = "Action Code")]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string ActionCode { get; set; }

        [Display(Name = "Phạm vi")]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Editable(false,AllowInitialValue = false)]
        public string Area { get; set; }

        [Display(Name = "Controller")]
        [StringLength(50)]
        [Editable(false, AllowInitialValue = false)]
        public string Controller { get; set; }

        [Display(Name = "Action")]
        [StringLength(50)]
        [Editable(false, AllowInitialValue = false)]
        public string Action { get; set; }

        [StringLength(500)]
        [Editable(false, AllowInitialValue = false)]
        public string Params { get; set; }

        [StringLength(50, ErrorMessage=Enums.ErrorMessage.StringLengthMax)]
        public string Pram1 { get; set; }

        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string Pram2 { get; set; }

        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string Pram3 { get; set; }

        [StringLength(200, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string QuerryString { get; set; }

        [StringLength(500, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Editable(false, AllowInitialValue = false)]
        public string Url { get; set; }

        [Display(Name = "Menu cha")]
        [StringLength(128, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Editable(false, AllowInitialValue = false)]
        public string ParentId { get; set; }

        [Display(Name = "Order")]
        [RegularExpression("^[0-9]+$", ErrorMessage = Enums.ErrorMessage.OnlyNumber)]
        public int? Order { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(500, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string Description { get; set; }
        [ScaffoldColumn(false)]
        public int? MenuType { get; set; }

        public virtual SysAction SysAction { get; set; }
        public virtual ICollection<SysMenu> SysMenu1 { get; set; }
        public virtual SysMenu SysMenu2 { get; set; }

        public string Icon { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }

        public SysMenu toSysMenu()
        {
            SysMenu result = new SysMenu();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.Name = this.Name;
            result.ActionCode = this.ActionCode;
            result.Area = this.Area;
            result.Controller = this.Controller;
            result.Action = this.Action;
            result.Params = this.Params;
            result.Pram1 = this.Pram1;
            result.Pram2 = this.Pram2;
            result.Pram3 = this.Pram3;
            result.QuerryString = this.QuerryString;
            result.ParentId = this.ParentId;
            result.Order = this.Order;
            result.Description = this.Description;
            result.MenuType = this.MenuType;
            result.SysAction = this.SysAction;
            result.SysMenu1 = this.SysMenu1;
            result.SysMenu2 = this.SysMenu2;
            result.Icon = this.Icon;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            return result;
        }
    }


    public class MenuTree
    {
        public MenuTree()
        {
            ChildMenu = new List<MenuTree>();
        }
        public string Id { get; set; }
        public string MenuName { get; set; }
        public List<MenuTree> ChildMenu { get; set; }
        
    }
    public class MenuView
    {

        public MenuView()
        {
            ChildMenu = new List<MenuView>();
        }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Id { get; set; }
        public string MenuName { get; set; }
        public List<MenuView> ChildMenu { get; set; }
    }
}
