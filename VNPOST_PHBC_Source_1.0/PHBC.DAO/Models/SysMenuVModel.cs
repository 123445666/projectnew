namespace PHBC.DAO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysMenuV")]
    public partial class SysMenuVModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ActionCode { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Pram1 { get; set; }

        [StringLength(50)]
        public string Pram2 { get; set; }

        [StringLength(50)]
        public string Pram3 { get; set; }

        [StringLength(200)]
        public string QuerryString { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(128)]
        public string ParentId { get; set; }

        public int? Order { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(128)]
        public string sys_MenuH_Id { get; set; }
    }
}
