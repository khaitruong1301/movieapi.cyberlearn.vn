using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class HeThongRap
    {
        public HeThongRap()
        {
            CumRap = new HashSet<CumRap>();
        }

        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public string BiDanh { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<CumRap> CumRap { get; set; }
        public virtual ICollection<LichChieu> LichChieu { get; set; }
    }
}
