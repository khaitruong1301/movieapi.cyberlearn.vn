using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class ThongTinDatVe
    {
        public int MaVe { get; set; }
        public List<ThongTinGhe> DanhSachGhe = new List<ThongTinGhe>();

        public DateTime NgayDat { get; set; }
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaVe { get; set; }
        public int ThoiLuongPhim { get; set; }

    }

    public class ThongTinGhe {

        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public int? MaRap { get; set; }
        public string TenRap { get; set; }
        public int MaGhe { get; set; }
        public string TenGhe { get; set; }
    }
}
