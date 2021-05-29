using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{

    public class HeThongRap_ {
        public string MaHeThongRap{get;set;}
        public string TenHeThongRap { get; set; }
        public string Logo { get; set; }
        public string Mahom { get; set; }
        public List<CumRap_> lstCumRap = new List<CumRap_>();
    }

    public class CumRap_ {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string HinhAnh { get; set; }
        public string DiaChi { get; set; }
        public List<Phim_> DanhSachPhim = new List<Phim_>();
        
    }
    public class LichChieuTheoPhim_
    {

        public int MaLichChieu { get; set; }
        public string MaRap { get; set; }
        public string TenRap { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public decimal? GiaVe { get; set; }

    }

    public class Phim_ {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public bool? Hot { get; set; }
        public bool? DangChieu { get; set; }
        public bool? SapChieu { get; set; }

        public List<LichChieuTheoPhim_> lstLichChieuTheoPhim = new List<LichChieuTheoPhim_>();

    }

    public class LichChieuMapView{
        public string Logo { get; set; }
        public string HinhAnh { get; set; }
        public int MaLichChieu { get; set; }
        public string MaRap { get; set; }
        public string TenRap { get; set; }
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public decimal? GiaVe { get; set; }
        public int ThoiLuong { get; set; }
        public string MaNhom { get; set; }
        public string MaHeThongRap { get; set; }
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string TenHeThongRap { get; set; }
        public bool? Hot { get; set; }
        public bool? DangChieu { get; set; }
        public bool? SapChieu { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public string ThongTin { get; set; }
    }
}
