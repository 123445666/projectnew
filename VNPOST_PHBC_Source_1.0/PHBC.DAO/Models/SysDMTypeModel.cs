namespace PHBC.DAO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SysDMTypeModel
    {
         public SysDMTypeModel()
        {

        }
         public SysDMTypeModel(SysDMType SysDMType)
        {
            this.Id = SysDMType.Id;
            this.Name = SysDMType.Name;
            this.Description = SysDMType.Description;
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; }

        public SysDMType toSysDMType()
        {
            SysDMType result = new SysDMType();
            //result.Id = string.IsNullOrEmpty(this.Id.ToString()) ? Int32.Parse(Guid.NewGuid().ToString()) : this.Id;
            result.Id = this.Id;
            result.Name = this.Name;
            result.Description = this.Description;
            return result;
        }
    }

    public partial class SysDMTypeSearchModel
    {
        public SysDMTypeSearchModel()
        {

        }
        [StringLength(10)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }
    }
}
