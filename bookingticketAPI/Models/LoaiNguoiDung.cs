using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class LoaiNguoiDung
    {
        public LoaiNguoiDung()
        {
            NguoiDung = new HashSet<NguoiDung>();
        }

        public string MaLoaiNguoiDung { get; set; }
        public string TenLoai { get; set; }

        public virtual ICollection<NguoiDung> NguoiDung { get; set; }
    }
}
