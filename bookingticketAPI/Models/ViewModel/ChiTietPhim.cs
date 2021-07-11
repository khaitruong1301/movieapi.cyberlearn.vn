using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class ChiTietPhimVM
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
        public List<ThongTinLichChieu> LichChieu = new List<ThongTinLichChieu>();
        
    }


    public class ChiTietPhimViewModel
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

    }


    public class ThongTinLichChieu {
        public int MaLichChieu { get; set; }
        public int? MaRap { get; set; }

        public int? MaPhim { get; set; }
        public string TenPhim { get; set; }
        public DateTime? NgayChieuGioChieu { get; set; }
        public decimal? GiaVe { get; set; }
        public int? ThoiLuong { get; set; }

        public ThongTinRap ThongTinRap = new ThongTinRap();
    }

    public class ThongTinRap {
        public int ?MaRap{get;set;}
        public string TenRap { get; set; }

        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }

        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
    }
}
