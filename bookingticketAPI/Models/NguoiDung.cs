using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            DatVe = new HashSet<DatVe>();
        }

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDt { get; set; }
        public string MaNhom { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string HoTen { get; set; }
        public bool? DaXoa { get; set; }
        public string Ssid { get; set; }
        public string SecretKey { get; set; }
        public string BiDanh { get; set; }

        public virtual LoaiNguoiDung MaLoaiNguoiDungNavigation { get; set; }
        public virtual Nhom MaNhomNavigation { get; set; }
        public virtual ICollection<DatVe> DatVe { get; set; }
    }
}
