namespace NamCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            BinhLuans = new HashSet<BinhLuan>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DisplayName("ID máy ảnh")]
        [Required(ErrorMessage = "Bạn phải nhập ID máy ảnh")]
        public int SanPhamID { get; set; }

        [DisplayName("Tên máy ảnh")]
        [Required(ErrorMessage = "Bạn phải nhập tên máy ảnh")]
        [StringLength(30)]
        public string TenSanPham { get; set; }

        [DisplayName("Hãng máy ảnh")]
        [Required(ErrorMessage = "Bạn phải nhập hãng máy ảnh")]
        [StringLength(30)]
        public string Hang { get; set; }

        [DisplayName("Giá nhập")]
        public double? GiaNhap { get; set; }

        [DisplayName("Giá bán")]
        [Required(ErrorMessage = "Bạn phải nhập giá bán")]
        public double GiaBan { get; set; }

        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "Bạn phải nhập số lượng")]
        public int SoLuong { get; set; }

        [Column(TypeName = "text")]
        [DisplayName("Hình ảnh 01")]
        public string Anh01 { get; set; }

        [Column(TypeName = "text")]
        [DisplayName("Hình ảnh 02")]
        public string Anh02 { get; set; }

        [Column(TypeName = "text")]
        [DisplayName("Hình ảnh 03")]
        public string Anh03 { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập thông tin cho máy ảnh")]
        [StringLength(4000)]
        [DisplayName("Thông tin máy ảnh")]
        public string ThongTin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
