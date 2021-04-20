using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Models.ViewModel
{
    public class ThongTinCumRapVM
    {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string DiaChi { get; set; }
        public IEnumerable<RapVM> DanhSachRap { get; set; }
    }
    public class RapVM {
       public int? MaRap { get; set; }
        public string TenRap { get; set; }
    }
}
