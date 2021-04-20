using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bookingticketAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using bookingticketAPI.Models.ViewModel;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using static bookingticketAPI.Common;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using bookingticketAPI;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using bookingticketAPI.StatusConstants;
using bookingticketAPI.Reponsitory;

namespace bookingticketAPI.Controllers
{
    [Route("api/[controller]")]
    public class QuanLyRapController : ControllerBase
    {
        dbRapChieuPhimContext db = new dbRapChieuPhimContext();
       
        ThongBaoLoi tbl = new ThongBaoLoi();

        private readonly IHostingEnvironment hostingEnv;

        [HttpGet("LayThongTinHeThongRap")]
        public async Task<ResponseEntity> LayThongTinHeThongRap(string maHeThongRap = "")
        {
         
            if (string.IsNullOrEmpty(maHeThongRap))
            {
                return new ResponseEntity(StatusCodeConstants.OK, db.HeThongRap.Select(n => new { n.MaHeThongRap, n.TenHeThongRap, n.BiDanh, Logo = DomainImage + n.Logo }), MessageConstant.MESSAGE_SUCCESS_200);

               
            }
            var lstResult = db.HeThongRap.Where(n => n.MaHeThongRap == maHeThongRap).Select(n => new { n.MaHeThongRap, n.TenHeThongRap, n.BiDanh, Logo = DomainImage + n.Logo });

            return new ResponseEntity(StatusCodeConstants.OK, lstResult, MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok(lstResult);
        }
        [HttpGet("LayThongTinCumRapTheoHeThong")]
        public async Task<ResponseEntity> LayThongTinCumRapTheoHeThong(string maHeThongRap = "")
        {
            if (string.IsNullOrEmpty(maHeThongRap))
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã hệ thống rạp không tồn tại!", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã hệ thống rạp không tồn tại!");
            }
            var lstCumRap = db.CumRap.Where(n => n.MaHeThongRap == maHeThongRap);
            if (lstCumRap.Count() == 0)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã hệ thống rạp không tồn tại!", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi400, "Hệ thống rạp sắp khai trương chưa có rạp!");
            }
            IEnumerable<ThongTinCumRapVM> lstResult = lstCumRap.Select(n => new ThongTinCumRapVM { MaCumRap = n.MaCumRap, TenCumRap = n.TenCumRap, DiaChi = n.ThongTin ,DanhSachRap = db.Rap.Where(e => e.MaCumRap == n.MaCumRap).Select(p => new RapVM { MaRap = p.MaRap, TenRap = p.TenRap }) });
                return new ResponseEntity(StatusCodeConstants.OK, lstResult, MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok(lstResult);
        }
       

        [HttpGet("LayThongTinLichChieuHeThongRap")]
        public async Task<ResponseEntity> LayThongTinLichChieuHeThongRap(string maHeThongRap = "", string maNhom = "GP01")
        {
            var conn = new SqlConnection(Config.connect);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            IEnumerable<HeThongRap> lstHeThongRap = db.HeThongRap;
            if (!string.IsNullOrEmpty(maHeThongRap))
            {
                lstHeThongRap = lstHeThongRap.Where(n => n.MaHeThongRap == maHeThongRap);
            }
            List<HeThongRap_> lstResult = new List<HeThongRap_>();




            string LichChieu = "LichChieu" + maNhom;
            string query = string.Format("select HeThongRap.Logo,CumRap.ThongTin,Phim.TenPhim,Phim.HinhAnh,Phim.Hot,Phim.DangChieu,Phim.SapChieu,CumRap.TenCumRap,Rap.TenRap , {0}.MaLichChieu,{0}.MaRap,{0}.MaPhim,{0}.NgayChieuGioChieu,{0}.GiaVe,{0}.ThoiLuong,{0}.MaNhom,{0}.MaHeThongRap,{0}.MaCumRap,TenHeThongRap,TenCumRap from Phim,HeThongRap,CumRap,{0},Rap where Phim.MaPhim = {0}.MaPhim and HeThongRap.MaHeThongRap = {0}.MaHeThongRap and {0}.MaCumRap = CumRap.MaCumRap  and {0}.MaRap=Rap.MaRap", LichChieu);
            if (!string.IsNullOrEmpty(maHeThongRap))
            {
                query = string.Format("select HeThongRap.Logo,CumRap.ThongTin,Phim.TenPhim,Phim.Hot,Phim.DangChieu,Phim.SapChieu,Phim.HinhAnh,CumRap.TenCumRap,Rap.TenRap , {0}.MaLichChieu,{0}.MaRap,{0}.MaPhim,{0}.NgayChieuGioChieu,{0}.GiaVe,{0}.ThoiLuong,{0}.MaNhom,{0}.MaHeThongRap,{0}.MaCumRap,TenHeThongRap,TenCumRap from Phim,HeThongRap,CumRap,{0},Rap where {0}.MaHeThongRap='{1}' and Phim.MaPhim = {0}.MaPhim and HeThongRap.MaHeThongRap = {0}.MaHeThongRap and {0}.MaCumRap = CumRap.MaCumRap  and {0}.MaRap=Rap.MaRap", LichChieu, maHeThongRap);
            }


            var lstLichChieu = conn.Query<LichChieuMapView>(query);
            foreach (var ghtr in lstLichChieu.GroupBy(n => new { n.MaHeThongRap, n.TenHeThongRap,n.Logo }))
            {
                HeThongRap_ htr = new HeThongRap_();
                htr.MaHeThongRap = ghtr.Key.MaHeThongRap;
                htr.TenHeThongRap = ghtr.Key.TenHeThongRap;
                htr.Logo = DomainImage + ghtr.Key.Logo;
                htr.Mahom = ghtr.First().MaNhom;
               
                foreach (var cumRap in ghtr.GroupBy(n=>new {n.MaCumRap,n.TenCumRap,n.ThongTin }))
                {
                    CumRap_ cum = new CumRap_();
                    cum.MaCumRap = cumRap.Key.MaCumRap;
                    cum.TenCumRap = cumRap.Key.TenCumRap;
                    cum.DiaChi = cumRap.Key.ThongTin;
                        
                    foreach(var phim in cumRap.GroupBy(n=> new { n.MaPhim, n.TenPhim,n.HinhAnh,n.DangChieu,n.SapChieu,n.Hot}))
                    {

                        Phim_ phimModel = new Phim_();
                        phimModel.MaPhim = phim.Key.MaPhim;
                        phimModel.TenPhim = phim.Key.TenPhim;
                        phimModel.DangChieu = phim.Key.DangChieu;
                        phimModel.SapChieu = phim.Key.SapChieu;
                        phimModel.Hot = phim.Key.Hot;
                        phimModel.HinhAnh = DomainImage + phim.Key.HinhAnh;

                        
                        foreach (var lichChieu in phim)
                        {
                            LichChieuTheoPhim_ lich = new LichChieuTheoPhim_();
                            
                            lich.MaRap = lichChieu.MaRap;
                            lich.TenRap = lichChieu.TenRap;
                            lich.NgayChieuGioChieu = lichChieu.NgayChieuGioChieu;
                            lich.GiaVe = lichChieu.GiaVe;
                            lich.MaLichChieu = lichChieu.MaLichChieu;
                            phimModel.lstLichChieuTheoPhim.Add(lich);
                        }
                        cum.DanhSachPhim.Add(phimModel);
                        

                    }

                    htr.lstCumRap.Add(cum);
                }


                lstResult.Add(htr);
            }



            conn.Close();
            return new ResponseEntity(StatusCodeConstants.OK, lstResult, MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok(lstResult);
        }
        [HttpGet("LayThongTinLichChieuPhim")]
        public async Task<ResponseEntity> LayThongTinLichChieuPhim(int MaPhim = 0)
        {
            Phim p = db.Phim.SingleOrDefault(n => n.MaPhim == MaPhim);
            if (p == null)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã phim không tồn tại!", MessageConstant.BAD_REQUEST);

            }
            var conn = new SqlConnection(Config.connect);
            //List<ChiTietPhimTheoHeThongRap> lstResult = new List<ChiTietPhimTheoHeThongRap>();
            LichChieuTheoPhim phim = new LichChieuTheoPhim();
            phim.MaPhim = p.MaPhim;
            phim.TenPhim = p.TenPhim;
            phim.BiDanh = p.BiDanh;
            phim.Trailer = p.Trailer;
            phim.HinhAnh = DomainImage + p.HinhAnh;
            phim.MoTa = p.MoTa;
            phim.MaNhom = p.MaNhom;
            phim.NgayKhoiChieu = p.NgayKhoiChieu;
            phim.DanhGia = p.DanhGia;
            phim.Hot = p.Hot;
            phim.DangChieu = p.DangChieu;
            phim.SapChieu = p.SapChieu;
            foreach (var item in db.HeThongRap)
            {
                string LichChieu = "LichChieu" + p.MaNhom;
                string query = string.Format("select HeThongRap.Logo, CumRap.ThongTin,Phim.TenPhim,CumRap.TenCumRap,Rap.TenRap , {0}.MaLichChieu,{0}.MaRap,{0}.MaPhim,{0}.NgayChieuGioChieu,{0}.GiaVe,{0}.ThoiLuong,{0}.MaNhom,{0}.MaHeThongRap,{0}.MaCumRap,TenHeThongRap,TenCumRap from Phim,HeThongRap,CumRap,{0},Rap where Phim.MaPhim = {0}.MaPhim and HeThongRap.MaHeThongRap = {0}.MaHeThongRap and {0}.MaCumRap = CumRap.MaCumRap and {0}.MaHeThongRap = '{1}' and {0}.MaPhim= {2} and {0}.MaRap=Rap.MaRap", LichChieu, item.MaHeThongRap, p.MaPhim);
                var lstLichChieu = conn.Query<LichChieuMapView>(query);

                foreach (var htr in lstLichChieu.GroupBy(n => new { n.MaHeThongRap, n.TenHeThongRap,n.Logo }))
                {

                    ThongTinLichChieuHeThongRap heThongRap = new ThongTinLichChieuHeThongRap();
                    heThongRap.Logo = DomainImage + htr.Key.Logo;
                    heThongRap.MaHeThongRap = htr.Key.MaHeThongRap;
                    heThongRap.TenHeThongRap = htr.Key.TenHeThongRap;
                    foreach (var cumRap in htr.GroupBy(n => new { n.MaCumRap, n.TenCumRap }))
                    {
                        ThongTinLichChieuCumRap cum = new ThongTinLichChieuCumRap();
                        cum.MaCumRap = cumRap.Key.MaCumRap;
                        cum.TenCumRap = cumRap.Key.TenCumRap;
                        foreach (var lichChieu in cumRap)
                        {
                            LichChieuPhim lcPhim = new LichChieuPhim();
                            lcPhim.MaLichChieu = lichChieu.MaLichChieu.ToString();
                            lcPhim.MaRap = lichChieu.MaRap;
                            lcPhim.TenRap = lichChieu.TenRap;
                            lcPhim.GiaVe = lichChieu.GiaVe;
                            lcPhim.ThoiLuong = lichChieu.ThoiLuong;
                            lcPhim.NgayChieuGioChieu = lichChieu.NgayChieuGioChieu;


                            cum.LichChieuPhim.Add(lcPhim);
                        }
                        heThongRap.CumRapChieu.Add(cum);
                    }


                    phim.HeThongRapChieu.Add(heThongRap);
                }
            }

            conn.Close();

            //return Ok(phim);
            return new ResponseEntity(StatusCodeConstants.OK, phim, MessageConstant.MESSAGE_SUCCESS_200);

        }




        //[HttpGet("UpdateData")]
        //public async Task<ActionResult> UpdateData()
        //{
        //    var lst = db.LichChieu;
        //    foreach (var item in lst.Where(n => n.MaLichChieu > 36603))
        //    {
        //        CumRap cum = item.MaRapNavigation.MaCumRapNavigation;
        //        item.MaCumRap = cum.MaCumRap;
        //        item.MaHeThongRap = cum.MaHeThongRap;
        //        db.SaveChanges();
        //    }

        //    return Ok("");

        //}
        //[HttpGet("UpdateData")]
        //public async Task<ActionResult> UpdateData()
        //{
        //    var lst = db.Phim;
        //    foreach (var item in lst)
        //    {
        //        item.BiDanh = LoaiBoKyTu.bestLower(item.TenPhim);

        //    }
        //    db.SaveChanges();
        //    return Ok("");

        //}
    }
}