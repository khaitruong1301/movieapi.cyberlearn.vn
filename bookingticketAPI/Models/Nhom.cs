using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class Nhom
    {
        public Nhom()
        {
            LichChieu = new HashSet<LichChieu>();
            NguoiDung = new HashSet<NguoiDung>();
            Phim = new HashSet<Phim>();
        }

        public string MaNhom { get; set; }
        public string TenNhom { get; set; }

        public virtual ICollection<LichChieu> LichChieu { get; set; }
        public virtual ICollection<NguoiDung> NguoiDung { get; set; }
        public virtual ICollection<Phim> Phim { get; set; }
    }
}
