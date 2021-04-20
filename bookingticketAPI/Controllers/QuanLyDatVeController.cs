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

using ReflectionIT.Mvc.Paging;
using bookingticketAPI.Reponsitory;
using bookingticketAPI.StatusConstants;
using Microsoft.Net.Http.Headers;

namespace bookingticketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyDatVeController : ControllerBase
    {
        dbRapChieuPhimContext db = new dbRapChieuPhimContext();
        ThongBaoLoi tbl = new ThongBaoLoi();
        Common commonService = new Common();

        [Authorize]
        [HttpPost("DatVe")]
        public async Task<ResponseEntity> DatVe(DanhSachVeDat DanhSachVe)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            userToken usToken = commonService.getUserByToken(accessToken).Result;
            if(usToken == null)
            {
                return new ResponseEntity(StatusCodeConstants.AUTHORIZATION, "Token đã hết hạn bạn hãy đăng nhập lại!", MessageConstant.MESSAGE_ERROR_401);

            }

            var nd = db.NguoiDung.Where(n => n.TaiKhoan == usToken.taiKhoan);
           
            if (nd.Count() == 0)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tài khoản người dùng không tồn tại!", MessageConstant.MESSAGE_ERROR_500);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản người dùng không tồn tại!");
            }
            if (DanhSachVe == null)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Danh sách vé rỗng!", MessageConstant.MESSAGE_ERROR_500);

                //return Ok("Danh sách vé rỗng!");
            }

            foreach (var ve in DanhSachVe.DanhSachVe)
            {
                DatVe dv = new DatVe();
                dv.NgayDat = DateTime.Now;
                dv.MaGhe = ve.MaGhe;
                dv.GiaVe = ve.GiaVe;
                dv.TaiKhoanNguoiDung = usToken.taiKhoan;
                dv.MaLichChieu = DanhSachVe.MaLichChieu;
                db.DatVe.Add(dv);
            }
            db.SaveChanges();
            return new ResponseEntity(StatusCodeConstants.OK, "Đặt vé thành công!", MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok("Đặt vé thành công!");
        }
        [HttpGet("LayDanhSachPhongVe")]
        public async Task<ResponseEntity> LayDanhSachPhongVe(int MaLichChieu = 0)
        {
            if (MaLichChieu == 0)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã lịch chiếu không hợp lệ!", MessageConstant.ERROR);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã lịch chiếu không hợp lệ!");
            }

            var LichChieu = db.LichChieu.SingleOrDefault(n => n.MaLichChieu == MaLichChieu);
            if (LichChieu == null)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã lịch chiếu không hợp lệ!", MessageConstant.ERROR);
            }
            decimal giaVe = LichChieu.GiaVe.Value;

            var lstDanhSachGheDuocDat = db.DatVe.Where(n => n.MaLichChieu == MaLichChieu);

            bool flag = false;
            LichChieuRap rap = new LichChieuRap();
            List<GheVM> lstGhe = new List<GheVM>();
            int MaRap = LichChieu.MaRapNavigation.MaRap;

            foreach (var room in db.Ghe.Where(n => n.MaRap == LichChieu.MaRap)) //Lấy ra rạp đang chiếu
            {
                flag = false;
                GheVM ghe = new GheVM();
                ghe.MaRap = room.MaRap;
                ghe.GiaVe = (room.MaLoaiGheNavigation.ChietKhau * giaVe) / 100 + giaVe;
                ghe.LoaiGhe = room.MaLoaiGheNavigation.TenLoaiGhe;
                ghe.MaGhe = room.MaGhe;
                ghe.TenGhe = room.Stt;

                ghe.STT = room.TenGhe;
                ghe.TenGhe = room.TenGhe;
                foreach (var rapDatVe in lstDanhSachGheDuocDat) //Lấy ra danh sách ghế được đặt
                {
                    if (room.MaGhe == rapDatVe.MaGhe)
                    {
                        ghe.TaiKhoanNguoiDat = db.NguoiDung.Single(n => n.TaiKhoan == rapDatVe.TaiKhoanNguoiDung).TaiKhoan;
                        flag = true;
                    }
                }
                if (flag == true)
                {
                    ghe.DaDat = true;
                }
                lstGhe.Add(ghe);
            }
            rap.DanhSachGhe = lstGhe;
            rap.ThongTinPhim.ngayChieu = LichChieu.NgayChieuGioChieu.ToString("dd/MM/yyyy");
            rap.ThongTinPhim.gioChieu = LichChieu.NgayChieuGioChieu.ToString("hh:MM");
            rap.ThongTinPhim.hinhAnh = DomainImage + LichChieu.MaPhimNavigation.HinhAnh;
            rap.ThongTinPhim.MaLichChieu = MaLichChieu;
            rap.ThongTinPhim.TenRap = LichChieu.MaRapNavigation.TenRap;
            rap.ThongTinPhim.TenCumRap = LichChieu.MaRapNavigation.MaCumRapNavigation.TenCumRap;
            rap.ThongTinPhim.DiaChi = LichChieu.MaRapNavigation.MaCumRapNavigation.ThongTin;
            rap.ThongTinPhim.TenPhim = LichChieu.MaPhimNavigation.TenPhim;
            return new ResponseEntity(StatusCodeConstants.OK, rap, MessageConstant.MESSAGE_SUCCESS_200);
        }
        [HttpPost("TaoLichChieu")]
        [Authorize(Roles = "QuanTri")]
        public async Task<ResponseEntity> TaoLichChieu(LichChieuInsert lich)
        {
            Phim p = db.Phim.SingleOrDefault(n => n.MaPhim == lich.MaPhim);
            if (p == null)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "MaPhim không hợp lệ", MessageConstant.ERROR);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "MaPhim không hợp lệ");
            }

            LichChieu lichModel = new LichChieu();
            try
            {
                lichModel.NgayChieuGioChieu = DateTimes.ConvertDateHour(lich.NgayChieuGioChieu);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy hh:mm:ss !", MessageConstant.ERROR);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
            }
            //DateTime temp;
            //try
            //{

            //    if (DateTime.TryParse(lich.NgayChieuGioChieu, out temp))
            //    {
            //        lichModel.NgayChieuGioChieu = DateTimes.ConvertDateHour(lich.NgayChieuGioChieu);
            //    }
            //    else
            //    {
            //        return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu giờ chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy hh:mm:ss !");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu giờ chiếu  không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy hh:mm:ss!");
            //}

            var ckRap = db.Rap.SingleOrDefault(n => n.MaRap == lich.MaRap);
            if (ckRap == null)
            {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Mã rạp không tồn tại !", MessageConstant.MESSAGE_ERROR_404);
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã rạp không tồn tại !");
            }

            var ckCum = db.CumRap.SingleOrDefault(n => n.MaCumRap == ckRap.MaCumRap);
            if (ckCum == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Chọn sai cụm rạp!", MessageConstant.MESSAGE_ERROR_404);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Chọn sai cụm rạp!");
            }
            var ckHeThongRap = db.HeThongRap.SingleOrDefault(n => n.MaHeThongRap == ckCum.MaHeThongRap);
            if (ckHeThongRap == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Chọn sai hệ thống rạp!", MessageConstant.MESSAGE_ERROR_404);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Chọn sai hệ thống rạp!");
            }
            var lichChieu = db.LichChieu.Where(n => n.NgayChieuGioChieu.Date == lichModel.NgayChieuGioChieu.Date && n.MaPhim == p.MaPhim && n.MaCumRap == ckCum.MaCumRap && n.MaHeThongRap == ckHeThongRap.MaHeThongRap && n.MaRap == lich.MaRap);
            if (lichChieu.Count() > 0)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Lịch chiếu đã bị trùng", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Lịch chiếu đã bị trùng");
            }

            if (lich.GiaVe > 200000 || lich.GiaVe < 75000)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Giá từ 75.000 - 200.000", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Giá từ 75.000 - 200.000");
            }



            //Lấy mã rạp ngẫu nhiên không có trong lst đó

            lichModel.MaRap = lich.MaRap;
            lichModel.MaPhim = lich.MaPhim;
            lichModel.ThoiLuong = 120;
            lichModel.MaNhom = p.MaNhom;
            lichModel.MaHeThongRap = ckHeThongRap.MaHeThongRap;
            lichModel.MaCumRap = ckCum.MaCumRap;
            lichModel.GiaVe = lich.GiaVe;
            db.LichChieu.Add(lichModel);
            db.SaveChanges();
            return new ResponseEntity(StatusCodeConstants.OK, "Thêm lịch chiếu thành công!", MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok("Thêm lịch chiếu thành công!");
        }

        //[HttpGet("update")]
        //public ActionResult update() {

        //    IEnumerable<Ghe> lstGhe = db.Ghe;
        //    List<int> lstNumber = new List<int> {
        //        47,48,63,64,79,80,95,96,111,112,127,128
        //    };
        //    foreach (var item in lstGhe) {

        //        if (lstNumber.Any(n => n == (int.Parse(item.TenGhe)))){
        //            item.MaLoaiGhe = 0;
        //        }
        //        if (int.Parse(item.TenGhe) == 78) {
        //            item.MaLoaiGhe = 1;
        //        }


        //    }
        //    db.SaveChanges();


        //    return Ok();
        //}

    }
}
