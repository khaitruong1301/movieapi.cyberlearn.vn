using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class PhimVM
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaNhom { get; set; }
        public DateTime? NgayKhoiChieu { get; set; }
        public int? DanhGia { get; set; }
        public bool? Hot { get; set; }
        public bool? DangChieu { get; set; }
        public bool? SapChieu { get; set; }

    }
    public class PhimInsert
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaNhom { get; set; }

        public string NgayKhoiChieu { get; set; }
        public int? DanhGia { get; set; }

    }

    public class PhimUpload
    {
        public int maPhim = 0;
        public string tenPhim = "";
        public string biDanh = "";
        public string trailer = "";
        public string moTa = "";
        public string maNhom = "GP01";
        public string ngayKhoiChieu = "10/10/2020";
        public bool DaXoa = false;

        public string sapChieu = "true";
        public string dangChieu = "true";
        public string hot = "true";
        public string danhGia = "10";
        public IFormFile hinhAnh { get; set; }

    }
    public class PhimUploadResult
    {
        public int maPhim { get; set; }
        public string tenPhim { get; set; }
        public string trailer { get; set; }
        public string moTa { get; set; }
        public string maNhom { get; set; }
        public DateTime ngayKhoiChieu  { get; set; }
    public bool DaXoa { get; set; }

        public bool sapChieu { get; set; }
        public bool dangChieu { get; set; }
        public bool hot { get; set; }
        public int danhGia { get; set; }
        public string hinhAnh { get; set; }

    }
    public class PhimInsertNew
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaNhom { get; set; }
        public int? DanhGia { get; set; }
    }
}
