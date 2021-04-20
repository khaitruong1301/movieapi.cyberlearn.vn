using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class DatVe
    {
        public int MaVe { get; set; }
        public int MaGhe { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? GiaVe { get; set; }
        public string TaiKhoanNguoiDung { get; set; }
        public int? MaLichChieu { get; set; }
        public int? MaLoaiGhe { get; set; }
        public int? ChietKhau { get; set; }

        public virtual Ghe MaGheNavigation { get; set; }
        public virtual LichChieu MaLichChieuNavigation { get; set; }
        public virtual NguoiDung TaiKhoanNguoiDungNavigation { get; set; }
    }
}
