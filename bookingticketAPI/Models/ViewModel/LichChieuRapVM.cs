using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class LichChieuRap
    {
        public ThongTinPhim ThongTinPhim = new ThongTinPhim();
      
        public List<GheVM> DanhSachGhe { get; set; }

    }
    public class ThongTinPhim {
        public int MaLichChieu { get; set; }
        public string TenCumRap { get; set; }
        public string TenRap { get; set; }
        public string DiaChi { get; set; }
        public string TenPhim { get; set; }
        public string hinhAnh { get; set; }
        public string ngayChieu { get; set; }
        public string gioChieu { get; set; }
    }
    public class GheVM
    {

        public int? MaGhe { get; set; }
        public string TenGhe { get; set; }
        public Nullable<int> MaRap { get; set; }
        public string LoaiGhe { get; set; }
        public string STT { get; set; }
        public Nullable<decimal> GiaVe { get; set; }
        public bool DaDat { get; set; }
       
        public string TaiKhoanNguoiDat { get; set; }
    }
}
