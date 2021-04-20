using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class Rap
    {
        public Rap()
        {
            Ghe = new HashSet<Ghe>();
            LichChieu = new HashSet<LichChieu>();
        }

        public int MaRap { get; set; }
        public string TenRap { get; set; }
        public int? SoGhe { get; set; }
        public string MaCumRap { get; set; }

        public virtual CumRap MaCumRapNavigation { get; set; }
        public virtual ICollection<Ghe> Ghe { get; set; }
        public virtual ICollection<LichChieu> LichChieu { get; set; }
    }
}
