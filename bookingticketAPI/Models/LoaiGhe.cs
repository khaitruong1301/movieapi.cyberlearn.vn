using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{
    public partial class LoaiGhe
    {
        public LoaiGhe()
        {
            Ghe = new HashSet<Ghe>();
        }

        public int MaLoaiGhe { get; set; }
        public string TenLoaiGhe { get; set; }
        public string MoTa { get; set; }
        public int? ChietKhau { get; set; }

        public virtual ICollection<Ghe> Ghe { get; set; }
    }
}
