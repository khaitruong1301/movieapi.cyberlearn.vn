using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class ThongTinTaiKhoanVM
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDT { get; set; }
        public string MaNhom { get; set; }
        public string LoaiNguoiDung { get; set; }
        public IEnumerable<ThongTinDatVe> ThongTinDatVe { get; set; }
    }
}
