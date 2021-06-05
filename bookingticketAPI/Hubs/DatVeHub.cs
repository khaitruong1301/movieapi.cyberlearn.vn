using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Hubs
{
    public class DatVeHub : Hub
    {
        private readonly string _connectionString;

        public DatVeHub(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        //private readonly string connectionString;

        //public DatVeHub(string _connectionString)
        //{
        //    this.connectionString = _connectionString;
        //}
        public override Task OnConnectedAsync()
        {
            //Here check the list if this is going to be our first client and if so call your method, next time because our list is filled your method won't be invoked.

            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendListGhe(string taiKhoan, string danhSachGheDangDat, int maLichChieu)
        {
            IEnumerable<DanhSachVeDangDatVM> dsGheDangDatReturn;
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TENTAIKHOAN", taiKhoan);
                param.Add("@DANHSACHGHE", danhSachGheDangDat);
                param.Add("@MALICHCHIEU", maLichChieu);
                await connection.ExecuteAsync("PUT_DS_GHE_DANG_DAT", param, commandType: CommandType.StoredProcedure);
                dsGheDangDatReturn = await connection.QueryAsync<DanhSachVeDangDatVM>("SELECT * FROM [dbo].[DANHSACHDATVE]", commandType: CommandType.Text);
            }
            await Clients.All.SendAsync("ReceiveListGheDangDat", dsGheDangDatReturn);
        }

        public async Task loadListSeat()
        {
            IEnumerable<DanhSachVeDangDatVM> dsGheDangDatReturn;
            using (var connection = new SqlConnection(_connectionString))
            {
                dsGheDangDatReturn = await connection.QueryAsync<DanhSachVeDangDatVM>("SELECT * FROM [dbo].[DANHSACHDATVE]");
            }
            await Clients.All.SendAsync("ReceiveListGheDangDat", dsGheDangDatReturn);
        }


        public async Task SendRequestData(string taiKhoan, int maLichChieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TENTAIKHOAN", taiKhoan);
                param.Add("@MALICHCHIEU", maLichChieu);
                await connection.ExecuteAsync("DS_GHE_DANG_DAT_DELETE", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
