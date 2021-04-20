using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class Ghe
    {
        public Ghe()
        {
            DatVe = new HashSet<DatVe>();
        }

        public int MaGhe { get; set; }
        public string TenGhe { get; set; }
        public int? MaRap { get; set; }
        public string Stt { get; set; }
        public int? MaLoaiGhe { get; set; }
        public bool? KichHoat { get; set; }

        public virtual LoaiGhe MaLoaiGheNavigation { get; set; }
        public virtual Rap MaRapNavigation { get; set; }
        public virtual ICollection<DatVe> DatVe { get; set; }
    }
}
