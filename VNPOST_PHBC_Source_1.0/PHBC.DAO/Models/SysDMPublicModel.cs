namespace PHBC.DAO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SysDMPublicModel
    {
         public SysDMPublicModel()
        {

        }
         public SysDMPublicModel(SysDMPublic SysDMPublic)
        {
            this.TypeId = SysDMPublic.TypeId;
            this.Code = SysDMPublic.Code;
            this.Name = SysDMPublic.Name;
            if (SysDMPublic.IsLock == 1)
            {
                this.bLock = true;
            }
            else this.bLock = false;
            this.IsLock = SysDMPublic.IsLock;
            this.Description = SysDMPublic.Description;
        }
        [Required]
        [Display(Name = "Mã danh mục chính")]
        public int TypeId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Mã danh mục con")]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Tên danh mục con")]
        public string Name { get; set; }
        [Display(Name = "Khóa")]
        public Nullable<int> IsLock { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public bool bLock { get; set; }
        public SysDMPublic toSysDMPublic()
        {
            SysDMPublic result = new SysDMPublic();
            result.TypeId = string.IsNullOrEmpty(this.TypeId.ToString()) ? Int32.Parse(Guid.NewGuid().ToString()) : this.TypeId;
            result.Code = this.Code;
            result.Name = this.Name;
            if (this.bLock == true) {
                result.IsLock = 1;
            }
            else
            {
                result.IsLock = 0;
            }
            
            result.Description = this.Description;
            return result;
        }
    }

    public partial class SysDMPublicSearchModel
    {
        public SysDMPublicSearchModel()
        {

        }

        [Display(Name = "Mã danh mục chính")]
        public int TypeId { get; set; }

        [StringLength(100)]
        [Display(Name = "Tên danh mục chính")]
        public string Name { get; set; }
    }
}
