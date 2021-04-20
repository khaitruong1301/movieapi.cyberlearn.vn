using System;
using System.Collections.Generic;

namespace bookingticketAPI.Controllers
{
    public class DanhSachVeDat
    {
        public int MaLichChieu { get; set; }
        public ICollection<VeVM> DanhSachVe { get; set; }
     

    }

    public class VeVM
    {
        public int MaGhe { get; set; }
        public decimal GiaVe { get; set; }
    }
}