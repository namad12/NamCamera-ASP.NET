namespace NamCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TinTuc")]
    public partial class TinTuc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DisplayName("ID tin tức")]
        public int TinTucID { get; set; }

        [StringLength(1000)]
        [DisplayName("Tiêu đề")]
        public string TieuDe { get; set; }

        [StringLength(4000)]
        [DisplayName("Nội dung")]
        public string NoiDung { get; set; }
    }
}
