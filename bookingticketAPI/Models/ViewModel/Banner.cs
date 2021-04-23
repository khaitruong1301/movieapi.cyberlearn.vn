using System;
using System.Collections.Generic;

namespace bookingticketAPI.Models
{

	public partial class Banner
	{
		public Banner()
		{
		}
		public int MaBanner { get; set; }

		public int MaPhim { get; set; }
		public string HinhAnh { get; set; }
	}
}
