using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class Phim
    {
        public Phim()
        {
            LichChieu = new HashSet<LichChieu>();
        }

        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaNhom { get; set; }
        public DateTime? NgayKhoiChieu { get; set; }
        public int? DanhGia { get; set; }
        public bool? DaXoa { get; set; }

        public bool? SapChieu { get; set; }
        public bool? DangChieu { get; set; }
        public bool? Hot { get; set; }
        public virtual Nhom MaNhomNavigation { get; set; }
        public virtual ICollection<LichChieu> LichChieu { get; set; }
    }
}
