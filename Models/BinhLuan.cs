namespace NamCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BinhLuan")]
    public partial class BinhLuan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BinhLuanID { get; set; }

        public int TenID { get; set; }

        public int SanPhamID { get; set; }

        [StringLength(4000)]
        public string NoiDung { get; set; }

        public BinhLuan()
        {

        }

        public BinhLuan(int a, int b, int c, string d)
        {
            BinhLuanID = a;
            TenID = b;
            SanPhamID = c;
            NoiDung = d;
        }

        public virtual SanPham SanPham { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
