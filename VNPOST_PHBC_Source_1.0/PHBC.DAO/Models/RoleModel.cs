namespace PHBC.DAO.Models
{
    using PHBC.DAO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
    public class AspNetRoleModel
    {
        public AspNetRoleModel()
        {

        }
        public AspNetRoleModel(AspNetRole aspNetRole)
        {
            this.Id = aspNetRole.Id;
            this.Name = aspNetRole.Name;
            this.Level = aspNetRole.Level;
            this.Discriminator = aspNetRole.Discriminator;
        }

        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Required(ErrorMessage=Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage=Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Chi tiết")]
        public string Discriminator { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Cấp")]
        public int Level { get; set; }

        public string userId { get; set; }

        public AspNetRole toAspNetRole()
        {
            AspNetRole result = new AspNetRole();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.Name = this.Name;
            result.Level = this.Level;
            result.Discriminator = this.Discriminator;
            return result;
        }
    }

    public class RoleSearchModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        [Display(Name = "Chi tiết")]
        public string Discriminator { get; set; }

        [Display(Name = "Cấp")]
        public Nullable<int> Level { get; set; }
    }
    public class RoleActionModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        public string lstAction { get; set;}

        public List<SysAction> lstActions_Curent { get; set; }

        public List<SysAction> lstActions_noMap{get;set;}
    }


    public class RoleUserModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên vai trò")]
        public string Name { get; set; }

        public string lstUser { get; set; }

        public List<UserModel> lstUser_Curent { get; set; }

        public List<UserModel> lstUser_noMap { get; set; }
    }
}
