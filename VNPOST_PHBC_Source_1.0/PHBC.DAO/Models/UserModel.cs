namespace PHBC.DAO.Models
{
    using PHBC.DAO.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class UserModel
    {
        public UserModel()
        {
            LstRole = new List<AspNetRole>();
        }
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage=Enums.ErrorMessage.WrongFormat)]
        public string Email { get; set; }

        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9 ]*$", ErrorMessage=Enums.ErrorMessage.WrongFormat)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage=Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(6)]
        [Display(Name = "Mã đơn vị")]
        public string UnitCode { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Tên đơn vị")]
        public string UnitName { get; set; }

        [ScaffoldColumn(false)]
        public string ProvinCode { get; set; }

        [ScaffoldColumn(false)]
        public string DistrictCode { get; set; }

        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tên người dùng")]
        public string DislayName { get; set; }

        [Display(Name = "Cấp người dùng")]
        public int Level { get; set; }
        public List<AspNetRole> LstRole { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }

    public class UserRoleModel
    {
        public UserRoleModel()
        {
            Role_Current = new List<AspNetRole>();
            Role_NotMap = new List<AspNetRole>();
        }
        public string Id { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Tên người dùng")]
        public string DislayName { get; set; }

        [Display(Name = "Tên đơn vị")]
        public string UnitName { get; set; }
        public string LstRole { get; set; }

        public List<AspNetRole> Role_Current { get; set; }

        public List<AspNetRole> Role_NotMap { get; set; }
    }

    public class UserSearchModel
    {
        public string Id { get; set; }

        [StringLength(6, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Mã đơn vị")]
        public string UnitCode { get; set; }

        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string Email { get; set; }

        [Display(Name = "Cấp người dùng")]
        public int Level { get; set; }

        [Display(Name = "Tên người dùng")]
        public string DislayName { get; set; }
    }
}
