using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class LichChieu
    {
        public LichChieu()
        {
            DatVe = new HashSet<DatVe>();
        }

        public int MaLichChieu { get; set; }
        public int? MaRap { get; set; }
        public int? MaPhim { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public decimal? GiaVe { get; set; }
        public int? ThoiLuong { get; set; }
        public string MaNhom { get; set; }
        public string MaCumRap { get; set; }
        public string MaHeThongRap { get; set; }
        public virtual Nhom MaNhomNavigation { get; set; }
        public virtual Phim MaPhimNavigation { get; set; }
        public virtual Rap MaRapNavigation { get; set; }
        public virtual CumRap MaCumRapNavigation { get; set; }
        public virtual HeThongRap MaHeThongRapNavigation { get; set; }
        public virtual ICollection<DatVe> DatVe { get; set; }
    }
}
