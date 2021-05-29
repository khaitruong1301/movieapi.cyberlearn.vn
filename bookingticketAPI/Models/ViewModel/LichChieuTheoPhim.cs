using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class LichChieuTheoPhim
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaNhom { get; set; }
        public bool? Hot { get; set; }
        public bool? DangChieu { get; set; }
        public bool? SapChieu { get; set; }
        public DateTime? NgayKhoiChieu { get; set; }
        public int? DanhGia { get; set; }
        public List<ThongTinLichChieuHeThongRap> HeThongRapChieu = new List<ThongTinLichChieuHeThongRap>();
    }
    public class ThongTinLichChieuHeThongRap
    {
        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public string Logo { get; set; }
        public List<ThongTinLichChieuCumRap> CumRapChieu = new List<ThongTinLichChieuCumRap>();
    }
    public class ThongTinLichChieuCumRap {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string HinhAnh { get; set; }
        public string DiaChi { get; set; }

        public List<LichChieuPhim> LichChieuPhim = new List<LichChieuPhim>();
        
    }
    public class LichChieuPhim {
        public string MaLichChieu { get; set; }
        public string MaRap { get; set; }
        public string TenRap { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public decimal? GiaVe { get; set; }
        public int? ThoiLuong { get; set; }

    }
}
