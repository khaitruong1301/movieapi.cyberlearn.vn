using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class CumRap
    {
        public CumRap()
        {
            Rap = new HashSet<Rap>();
        }

        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string ThongTin { get; set; }
        public string MaHeThongRap { get; set; }

        public virtual HeThongRap MaHeThongRapNavigation { get; set; }
        public virtual ICollection<Rap> Rap { get; set; }
        public virtual ICollection<LichChieu> LichChieu { get; set; }
    }
}
