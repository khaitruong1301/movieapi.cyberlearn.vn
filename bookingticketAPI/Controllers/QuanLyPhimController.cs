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
using 
    System.Linq;
using static bookingticketAPI.Common;
using ReflectionIT.Mvc.Paging;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using bookingticketAPI.StatusConstants;
using bookingticketAPI.Reponsitory;

namespace bookingticketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyPhimController : ControllerBase
    {
        dbRapChieuPhimContext db = new dbRapChieuPhimContext();
        ThongBaoLoi tbl = new ThongBaoLoi();
        //string hostName =  "http://movie0706.cybersoft.edu.vn/hinhanh/";





        [HttpGet("LayDanhSachBanner")]
        public async Task<ResponseEntity> LayDanhSachBanner()
        {
            try
            {
                var lstResult = db.Banner.Select(n => new Banner() { MaPhim = n.MaPhim, HinhAnh = DomainImage + n.HinhAnh, MaBanner = n.MaBanner });

                return new ResponseEntity(StatusCodeConstants.OK, lstResult, MessageConstant.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {

                return new ResponseEntity(StatusCodeConstants.OK, "Không tìm banner !", MessageConstant.MESSAGE_SUCCESS_200);
            }
        }




        [HttpGet("LayDanhSachPhim")]
        public async Task<ResponseEntity> LayDanhSachPhim(string maNhom = "GP01", string tenPhim = "")
        {
            try
            {
                bool ckNhom = db.Nhom.Any(n => n.MaNhom == maNhom);
                if (!ckNhom)
                {
                    var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                    return new ResponseEntity(StatusCodeConstants.OK, response, MessageConstant.MESSAGE_SUCCESS_200);
                }
                tenPhim = LoaiBoKyTu.bestLower(tenPhim);
                IEnumerable<PhimVM> lstResult = db.Phim.Where(n => n.BiDanh.Contains(tenPhim) && n.MaNhom == maNhom && n.DaXoa != true).Select(n => new PhimVM { MaPhim = n.MaPhim, BiDanh = n.BiDanh, DanhGia = n.DanhGia, HinhAnh = DomainImage + n.HinhAnh, MaNhom = n.MaNhom, MoTa = n.MoTa, TenPhim = n.TenPhim, Trailer = n.Trailer, NgayKhoiChieu = n.NgayKhoiChieu, DangChieu = n.DangChieu, Hot = n.Hot, SapChieu = n.SapChieu });

                return new ResponseEntity(StatusCodeConstants.OK, lstResult, MessageConstant.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.OK, "Không tìm thấy phim !", MessageConstant.MESSAGE_SUCCESS_200);
            }
        }





        [HttpGet("LayDanhSachPhimPhanTrang")]
        public async Task<ResponseEntity> LayDanhSachPhimPhanTrang(string maNhom = "GP01", string tenPhim = "", int soTrang = 1, int soPhanTuTrenTrang = 10)
        {
            bool ckNhom = db.Nhom.Any(n => n.MaNhom == maNhom);
            if (!ckNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                return new ResponseEntity(StatusCodeConstants.OK, MessageConstant.MESSAGE_NOTFOUND_GROUP, MessageConstant.MESSAGE_SUCCESS_200);
            }
            tenPhim = LoaiBoKyTu.bestLower(tenPhim);
            IEnumerable<PhimVM> lstResult = db.Phim.Where(n => n.BiDanh.Contains(tenPhim) && n.MaNhom == maNhom && n.DaXoa != true).Select(n => new PhimVM { MaPhim = n.MaPhim, BiDanh = n.BiDanh, DanhGia = n.DanhGia, HinhAnh = DomainImage + n.HinhAnh, MaNhom = n.MaNhom, MoTa = n.MoTa, TenPhim = n.TenPhim, Trailer = n.Trailer, NgayKhoiChieu = n.NgayKhoiChieu, Hot = n.Hot, DangChieu = n.DangChieu, SapChieu = n.SapChieu });

            PaginationSet<PhimVM> result = new PaginationSet<PhimVM>();
            result.CurrentPage = soTrang;
            result.TotalPages = (lstResult.Count() / soPhanTuTrenTrang) + 1;
            result.Items = lstResult.Skip((soTrang - 1) * soPhanTuTrenTrang).Take(soPhanTuTrenTrang);
            result.TotalCount = lstResult.Count();
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstant.MESSAGE_SUCCESS_200);
        }





        [HttpGet("LayDanhSachPhimTheoNgay")]
        public async Task<ResponseEntity> LayDanhSachPhimTheoNgay(string maNhom = "GP01", string tenPhim = "", int soTrang = 1, int soPhanTuTrenTrang = 10, string tuNgay = "", string denNgay = "")
        {
            DateTime dtTuNgay = DateTimes.Now();
            DateTime dtDenNgay = DateTimes.Now();
            if (tuNgay != "")
            {
                try
                {
                    dtTuNgay = DateTimes.ConvertDate(tuNgay);
                }
                catch (Exception ex)
                {

                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày không hợp lệ", MessageConstant.MESSAGE_ERROR_400);
                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày không hợp lệ, Ngày có định dạng dd/MM/yyyy !");
                }

            }
            if (denNgay != "")
            {
                try
                {
                    dtDenNgay = DateTimes.ConvertDate(denNgay);
                }
                catch (Exception ex)
                {
                    //return new ResponseEntity(StatusCodeConstants.OK, "Ngày không hợp lệ", MessageConstant.MESSAGE_SUCCESS_200);

                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày không hợp lệ, Ngày có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_400);
                }

            }
            bool ckNhom = db.Nhom.Any(n => n.MaNhom == maNhom);
            if (!ckNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Nhóm người dùng không hợp lệ", MessageConstant.MESSAGE_ERROR_400); ;
            }
            tenPhim = LoaiBoKyTu.bestLower(tenPhim);
            IEnumerable<PhimVM> lstResult = db.Phim.Where(n => n.BiDanh.Contains(tenPhim) && n.MaNhom == maNhom && n.DaXoa != true && n.NgayKhoiChieu.Value >= dtTuNgay.Date && n.NgayKhoiChieu.Value <= dtDenNgay.Date).Select(n => new PhimVM { MaPhim = n.MaPhim, BiDanh = n.BiDanh, DanhGia = n.DanhGia, HinhAnh = DomainImage + n.HinhAnh, MaNhom = n.MaNhom, MoTa = n.MoTa, TenPhim = n.TenPhim, Trailer = n.Trailer, NgayKhoiChieu = n.NgayKhoiChieu, Hot = n.Hot, DangChieu = n.DangChieu, SapChieu = n.SapChieu });

            var model = PagingList.Create(lstResult, soPhanTuTrenTrang, soTrang);
            return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstant.MESSAGE_SUCCESS_200);
        }



        //[HttpPost("ThemPhim")]
        //[Authorize(Roles = "QuanTri")]
        //public async Task<ResponseEntity> ThemPhim(PhimInsert model)
        //{

        //    model.BiDanh = LoaiBoKyTu.bestLower(model.TenPhim);
        //    try
        //    {
        //        model.MaNhom = model.MaNhom.ToUpper();
        //        bool ckb = db.Nhom.Any(n => n.MaNhom == model.MaNhom);
        //        if (!ckb)
        //        {

        //            return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày không hợp lệ, Ngày có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_400);
        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
        //        }
        //        string tenPhim = LoaiBoKyTu.bestLower(model.TenPhim);
        //        if (string.IsNullOrEmpty(tenPhim))
        //        {

        //            return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Tên phim không hợp lệ !", MessageConstant.MESSAGE_ERROR_400);

        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
        //        }
        //        var p = db.Phim.Where(n => n.BiDanh == model.BiDanh);
        //        if (p.Count() > 1)
        //        {

        //            return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim đã tồn tại !", MessageConstant.MESSAGE_ERROR_500);

        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
        //        }


        //        PhimInsertNew phimNew = Mapper.Map<PhimInsert, PhimInsertNew>(model);
        //        Phim modelInsert = Mapper.Map<PhimInsertNew, Phim>(phimNew);
        //        modelInsert.BiDanh = LoaiBoKyTu.bestLower(modelInsert.TenPhim);
        //        //DateTime temp;
        //        //if (DateTime.TryParse(model.NgayKhoiChieu, out temp))
        //        //{
        //        try
        //        {
        //            modelInsert.NgayKhoiChieu = DateTimes.ConvertDate(model.NgayKhoiChieu);
        //        }
        //        catch (Exception ex)
        //        {

        //            return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy!", MessageConstant.MESSAGE_ERROR_400);

        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
        //        }
        //        //}
        //        //else
        //        //{

        //        //}

        //        if (modelInsert.HinhAnh.Split('.').Count() > 1)
        //        {
        //            modelInsert.HinhAnh = LoaiBoKyTu.bestLower(modelInsert.TenPhim) + "_" + LoaiBoKyTu.bestLower(modelInsert.MaNhom) + "." + modelInsert.HinhAnh.Split('.')[modelInsert.HinhAnh.Split('.').Length - 1];
        //        }
        //        else
        //        {
        //            //var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
        //            return new ResponseEntity(StatusCodeConstants.OK, "Hình ảnh không đúng định dạng!", MessageConstant.MESSAGE_SUCCESS_200); 
        //        }
        //        if (!string.IsNullOrEmpty(modelInsert.Trailer))
        //        {

        //            string newString = modelInsert.Trailer.Replace("https://www.youtube.com/embed/", "♥");
        //            if (newString.Split('♥').Length == 0)
        //            {
        //                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");
        //                return new ResponseEntity(StatusCodeConstants.OK, "Hình ảnh không đúng định dạng!", MessageConstant.MESSAGE_SUCCESS_200); 
        //            }
        //        }
        //        modelInsert.MaPhim = 0;
        //        db.Phim.Add(modelInsert);
        //        db.SaveChanges();

        //        return new ResponseEntity(StatusCodeConstants.OK, modelInsert, MessageConstant.MESSAGE_SUCCESS_200);
        //        //return Ok(modelInsert);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "thuộc tính hinhAnh không đúng định dạng *.jpg, *.png, *.gif!", MessageConstant.MESSAGE_ERROR_500);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "thuộc tính hinhAnh không đúng định dạng *.jpg, *.png, *.gif!");

        //    }
        //}

        //private void SetObjectProperty(string propertyName, string value, ref object objName)
        //{
        //    //some processing on the rest of the code to make sure we actually want to set this value.
        //    objName.GetType().GetProperty("nameOfProperty").SetValue(objName, objValue, null);
        //}
        public class test
        {

            public string maPhim = "1";
            public string tenPhim = "";
            public IFormFile hinhAnh;
        }

        //[Authorize(Roles = "QuanTri")]
        private object Convert([FromForm] IFormCollection form, object model)
        {
            Type type = typeof(PhimUpload);
            FieldInfo[] propertyInfos = type.GetFields();
            foreach (FieldInfo propertyInfo in propertyInfos)
            {
                var mykey = propertyInfo.Name;
                if (!string.IsNullOrEmpty(form[mykey]))
                {
                    try
                    {
                        string value = form[mykey];
                        propertyInfo.SetValue(model, value);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            return model;
        }


        [HttpPost("ThemPhimUploadHinh")]
        //[Authorize(Roles = "QuanTri")]
        public async Task<ResponseEntity> ThemPhimUploadHinh([FromForm] IFormCollection frm)
        {
            try
            {
                PhimUpload model = new PhimUpload();
                model = (PhimUpload)Convert(frm, model);
                model.maNhom = model.maNhom.ToUpper();
                if (string.IsNullOrEmpty(model.maNhom))
                {
                    model.maNhom = "GP01";
                }

                if (Request.Form.Files[0] == null)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Chưa chọn hình ảnh !", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Chưa chọn hình ảnh !");

                }


                model.hinhAnh = Request.Form.Files[0];
                string request = Request.Form["tenPhim"]; ;
                bool ckb = db.Nhom.Any(n => n.MaNhom == model.maNhom);
                if (!ckb)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã nhóm không hợp lệ!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
                }
                string tenPhim = LoaiBoKyTu.bestLower(model.tenPhim);
                if (string.IsNullOrEmpty(tenPhim))
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim không hợp lệ!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
                }
                var p = db.Phim.Where(n => n.BiDanh == model.biDanh);
                if (p.Count() > 1)
                {

                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim đã tồn tại!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
                }


                //PhimInsertNew phimNew = Mapper.Map<PhimInsert, PhimInsertNew>(model)
                Phim modelInsert = new Phim();
                modelInsert.BiDanh = LoaiBoKyTu.bestLower(model.tenPhim);
                modelInsert.DanhGia = model.danhGia;
                modelInsert.DaXoa = false;
                modelInsert.MaPhim = 0;
                modelInsert.HinhAnh = LoaiBoKyTu.bestLower(model.tenPhim) + "_" + LoaiBoKyTu.bestLower(model.maNhom) + "." + model.hinhAnh.FileName.Split('.')[model.hinhAnh.FileName.Split('.').Length - 1];
                //modelInsert.MaNhom = LoaiBoKyTu.bestLower(model.maNhom);
                modelInsert.MoTa = model.moTa;
                modelInsert.TenPhim = model.tenPhim;
                modelInsert.Trailer = model.trailer;
                modelInsert.Hot = model.hot;
                modelInsert.SapChieu = model.sapChieu;
                modelInsert.DangChieu = model.dangChieu;
                DateTime temp;
                try
                {
                    try
                    {
                        modelInsert.NgayKhoiChieu = DateTimes.ConvertDate(model.ngayKhoiChieu);
                    }
                    catch (Exception ex)
                    {
                        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy!", MessageConstant.MESSAGE_ERROR_500);

                        //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy!", MessageConstant.MESSAGE_ERROR_500);
                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                }

                if (!string.IsNullOrEmpty(modelInsert.Trailer))
                {

                    string newString = modelInsert.Trailer.Replace("https://www.youtube.com/embed/", "♥");
                    if (newString.Split('♥').Length == 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]", MessageConstant.MESSAGE_ERROR_500);

                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");
                    }
                }
                db.Phim.Add(modelInsert);
                string kq = UploadHinhAnh(model.hinhAnh, modelInsert.TenPhim, modelInsert.MaNhom);
                if (kq != "")
                {

                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, kq, MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, kq);


                }
                modelInsert.MaNhom = model.maNhom.ToUpper();

                db.SaveChanges();

                return new ResponseEntity(StatusCodeConstants.OK, kq, MessageConstant.MESSAGE_SUCCESS_200);

                //return Ok(modelInsert);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "thuộc tính hinhAnh không đúng định dạng *.jpg, *.png, *.gif!", MessageConstant.MESSAGE_ERROR_400);


                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "thuộc tính hinhAnh không đúng định dạng *.jpg, *.png, *.gif!");
            }
        }

        [Authorize(Roles = "QuanTri")]
        [HttpPost("CapNhatPhimUpload")]
        public async Task<ResponseEntity> CapNhatPhimUpload([FromForm] IFormCollection frm)
        {
            PhimUpload model = new PhimUpload();
            model = (PhimUpload)Convert(frm, model);
            model.maPhim = int.Parse(frm["maPhim"]);
            model.maNhom = model.maNhom.ToUpper();
            if (Request.Form.Files.Count > 0)
            {
                model.hinhAnh = Request.Form.Files[0];
            }

            if (string.IsNullOrEmpty(model.ngayKhoiChieu))
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_400);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
            }
            model.biDanh = LoaiBoKyTu.bestLower(model.tenPhim);
            try
            {

                Phim phimUpdate = db.Phim.SingleOrDefault(n => n.MaPhim == model.maPhim);
                if (phimUpdate == null)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã phim không tồn tại!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã phim không tồn tại!");
                }
                model.maNhom = model.maNhom.ToUpper();
                bool ckb = db.Nhom.Any(n => n.MaNhom == model.maNhom);
                if (!ckb)
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã nhóm không hợp lệ!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
                }
                string tenPhim = LoaiBoKyTu.bestLower(model.tenPhim);
                if (string.IsNullOrEmpty(tenPhim))
                {
                    return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim không hợp lệ!", MessageConstant.MESSAGE_ERROR_500);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
                }
                var p = db.Phim.Where(n => n.BiDanh == model.biDanh);
                int length = p.Count();
                //if (p.Count() > 2)
                //{
                //    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
                //}


                phimUpdate.TenPhim = model.tenPhim;
                phimUpdate.BiDanh = LoaiBoKyTu.bestLower(model.tenPhim);

                phimUpdate.MoTa = model.moTa;
                phimUpdate.Trailer = model.trailer;
                phimUpdate.Hot = model.hot;
                phimUpdate.SapChieu = model.sapChieu;
                phimUpdate.DangChieu = model.dangChieu;
                if (model.hinhAnh != null)
                {

                    //phimUpdate.HinhAnh = model.HinhAnh;
                    phimUpdate.HinhAnh = LoaiBoKyTu.bestLower(model.tenPhim) + "_" + LoaiBoKyTu.bestLower(model.maNhom) + "." + model.hinhAnh.FileName.Split('.')[model.hinhAnh.FileName.Split('.').Length - 1];
                    string kq = UploadHinhAnh(model.hinhAnh, model.tenPhim, model.maNhom);
                    if (kq.Trim() != "")
                    {
                        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, kq, MessageConstant.MESSAGE_ERROR_500);

                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, kq);
                    }
                }
                phimUpdate.DanhGia = model.danhGia;
                DateTime temp;
                try
                {
                    try
                    {
                        phimUpdate.NgayKhoiChieu = DateTimes.ConvertDate(model.ngayKhoiChieu);
                    }
                    catch (Exception ex)
                    {
                        phimUpdate.NgayKhoiChieu = DateTime.Now;
                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_400);

                    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                }

                if (!string.IsNullOrEmpty(model.trailer))
                {

                    string newString = phimUpdate.Trailer.Replace("https://www.youtube.com/embed/", "♥");
                    if (newString.Split('♥').Length == 0)
                    {
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]", MessageConstant.MESSAGE_ERROR_500);

                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");

                    }
                }
                db.SaveChanges();
                return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstant.MESSAGE_SUCCESS_200);

                //return Ok(model);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Dữ liệu không hợp lệ!", MessageConstant.MESSAGE_ERROR_500);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
            }
        }



        //[Authorize(Roles = "QuanTri")]
        //[HttpPost("CapNhatPhim")]
        //public async Task<ResponseEntity>  CapNhatPhim(PhimInsert model)
        //{
        //    if (string.IsNullOrEmpty(model.NgayKhoiChieu))
        //    {
        //        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !", MessageConstant.MESSAGE_ERROR_400);

        //    }
        //    //dbRapChieuPhimContext dbo = new dbRapChieuPhimContext();
        //    ////Fixembed
        //    //var lstPhim = db.Phim;
        //    //foreach (var item in lstPhim)
        //    //{
        //    //    var p = dbo.Phim.Single(n => n.MaPhim == item.MaPhim);
        //    //    string s = item.Trailer.Split('=')[item.Trailer.Split('=').Length - 1];
        //    //    p.Trailer = @"https://www.youtube.com/embed/" + s;

        //    //    dbo.SaveChanges();
        //    //}
        //    model.BiDanh = LoaiBoKyTu.bestLower(model.TenPhim);
        //    try
        //    {

        //        Phim phimUpdate = db.Phim.SingleOrDefault(n => n.MaPhim == model.MaPhim);
        //        if (phimUpdate == null)
        //        {
        //            return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Mã phim không tồn tại!", MessageConstant.MESSAGE_ERROR_404);
        //        }
        //        model.MaNhom = model.MaNhom.ToUpper();
        //        bool ckb = db.Nhom.Any(n => n.MaNhom == model.MaNhom);
        //        if (!ckb)
        //        {
        //            return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã nhóm không hợp lệ!", MessageConstant.MESSAGE_ERROR_400);

        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
        //        }
        //        string tenPhim = LoaiBoKyTu.bestLower(model.TenPhim);
        //        if (string.IsNullOrEmpty(tenPhim))
        //        {
        //            return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim không hợp lệ", MessageConstant.MESSAGE_ERROR_500);

        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
        //        }
        //        var p = db.Phim.Where(n => n.BiDanh == model.BiDanh);
        //        if (p.Count() > 2)
        //        {
        //            return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim đã tồn tại!", MessageConstant.MESSAGE_ERROR_500);

        //        }


        //        phimUpdate.TenPhim = model.TenPhim;
        //        phimUpdate.BiDanh = LoaiBoKyTu.bestLower(model.TenPhim);

        //        phimUpdate.Trailer = model.Trailer;
        //        phimUpdate.MoTa = model.MoTa;
        //        phimUpdate.HinhAnh = model.HinhAnh;
        //        phimUpdate.DanhGia = model.DanhGia;

        //        Phim pCu = db.Phim.Where(n => n.MaPhim == model.MaPhim).FirstOrDefault();

        //        if (string.IsNullOrEmpty(model.HinhAnh))
        //        {
        //            if (pCu != null)
        //            {


        //                phimUpdate.HinhAnh = pCu.HinhAnh;
        //            }
        //        }

        //        DateTime temp;
        //        try
        //        {
        //            try
        //            {
        //                phimUpdate.NgayKhoiChieu = DateTimes.ConvertDate(model.NgayKhoiChieu);
        //            }
        //            catch (Exception ex)
        //            {
        //                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !!", MessageConstant.MESSAGE_ERROR_400);

        //                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !!", MessageConstant.MESSAGE_ERROR_400);
        //            //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
        //        }
        //        if (model.HinhAnh != null)
        //        {
        //            if (model.HinhAnh.Split('.').Count() > 1)
        //            {
        //                phimUpdate.HinhAnh = LoaiBoKyTu.bestLower(phimUpdate.TenPhim) + "_" + LoaiBoKyTu.bestLower(phimUpdate.MaNhom) + "." + phimUpdate.HinhAnh.Split('.')[phimUpdate.HinhAnh.Split('.').Length - 1];
        //            }
        //            else
        //            {
        //                //var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
        //                 return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Hình ảnh không đúng định dạng!", MessageConstant.MESSAGE_ERROR_400); ;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(model.Trailer))
        //        {

        //            string newString = phimUpdate.Trailer.Replace("https://www.youtube.com/embed/", "♥");
        //            if (newString.Split('♥').Length == 0)
        //            {
        //                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]", MessageConstant.MESSAGE_ERROR_400); ;

        //                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");

        //            }
        //        }
        //        db.SaveChanges();
        //        return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstant.INSERT_SUCCESS); 
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Dữ liệu không hợp lệ!", MessageConstant.INSERT_ERROR);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
        //    }
        //}
        [HttpPost]
        public string UploadHinhAnh(IFormFile file, string tenPhim, string maNhom)
        {
            file = Request.Form.Files[0];
            tenPhim = Request.Form["tenPhim"];
            maNhom = Request.Form["maNhom"];
            //maNhom = maNhom.ToUpper();
            tenPhim = LoaiBoKyTu.bestLower(tenPhim);

            if (string.IsNullOrEmpty(tenPhim))
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ");

                //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Tên phim không hợp lệ", MessageConstant.INSERT_ERROR);

                return "Tên phim không hợp lệ";

            }
            if (string.IsNullOrEmpty(maNhom) || !db.Nhom.Any(n => n.MaNhom == maNhom))
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ");
                //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã nhóm không hợp lệ", MessageConstant.INSERT_ERROR);

                return "Mã nhóm không hợp lệ";
            }


            if (file.Length > TenMegaBytes)
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dung lượng file vượt quá 1 MB!");
                //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Dung lượng file vượt quá 1 MB!", MessageConstant.INSERT_ERROR);

                return "Dung lượng file vượt quá 1 MB!";
            }
            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/gif")
            {
                try
                {
                    //tenPhim = LoaiBoKyTu.bestLower(file.FileName);
                    //Check khoa học
                    //var kh = db.Phim.Where(n => n.BiDanh.Contains(tenPhim));
                    //if (kh.Count() == 0)
                    //{
                    //    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Phim không tồn tại không thể upload file");
                    //}

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", tenPhim + "_" + LoaiBoKyTu.bestLower(maNhom) + "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1]);
                    var stream = new FileStream(path, FileMode.Create);
                    file.CopyTo(stream);
                    //return new ResponseEntity(StatusCodeConstants.OK, "Upload file thành công!", MessageConstant.MESSAGE_SUCCESS_200);
                    return "";

                }
                catch
                {
                    //var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Upload file không thành công!");
                    //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Upload file thành công!", MessageConstant.ERROR);
                    return "Upload file không thành công!";

                    //return "Upload file không thành công!";
                }
            }
            else
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Định dạng file không hợp lệ!");
                //return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Định dạng file không hợp lệ!", MessageConstant.ERROR);

                return "Định dạng file không hợp lệ!";
            }
        }

        private const int TenMegaBytes = 1024 * 1024;

        //[HttpPost("UploadHinhAnhPhim")]
        //public async Task<ResponseEntity> UploadHinhAnhPhim()
        //{
        //    IFormFile file = Request.Form.Files[0];
        //    string tenPhim = Request.Form["tenPhim"];
        //    string maNhom = Request.Form["maNhom"];
        //    //maNhom = maNhom.ToUpper();
        //    tenPhim = LoaiBoKyTu.bestLower(tenPhim);

        //    if (string.IsNullOrEmpty(tenPhim))
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Định dạng file không hợp lệ!", MessageConstant.ERROR);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ");
        //    }
        //    if (string.IsNullOrEmpty(maNhom) || !db.Nhom.Any(n => n.MaNhom == maNhom))
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Mã nhóm không hợp lệ", MessageConstant.ERROR);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ");
        //    }


        //    if (file.Length > TenMegaBytes)
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Dung lượng file vượt quá 1 MB!", MessageConstant.ERROR);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dung lượng file vượt quá 1 MB!");

        //    }
        //    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/gif")
        //    {
        //        try
        //        {
        //            //tenPhim = LoaiBoKyTu.bestLower(file.FileName);
        //            //Check khoa học
        //            var kh = db.Phim.Where(n => n.BiDanh.Contains(tenPhim));
        //            if (kh.Count() == 0)
        //            {
        //                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Phim không tồn tại không thể upload file", MessageConstant.ERROR);

        //                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Phim không tồn tại không thể upload file");
        //            }

        //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", tenPhim + "_" + LoaiBoKyTu.bestLower(maNhom) + "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1]);
        //            var stream = new FileStream(path, FileMode.Create);
        //            file.CopyTo(stream);
        //            return new ResponseEntity(StatusCodeConstants.OK, "Upload file thành công", MessageConstant.INSERT_SUCCESS);

        //            //return Ok("Upload file thành công !");
        //        }
        //        catch
        //        {
        //            //var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Upload file không thành công!");
        //            return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Upload file không thành công!", MessageConstant.ERROR);

        //            //return response;
        //        }
        //    }
        //    else
        //    {
        //        return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, "Định dạng file không hợp lệ!", MessageConstant.ERROR);

        //        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Định dạng file không hợp lệ!");
        //    }
        //}

        [Authorize(Roles = "QuanTri")]
        [HttpDelete("XoaPhim")]
        public async Task<ResponseEntity> XoaPhim(int MaPhim)
        {

            bool ckbPhim = db.Phim.Any(n => n.MaPhim == MaPhim);
            if (!ckbPhim)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã phim không hợp lệ!", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã phim không hợp lệ!");
            }
            bool ckbLichChieu = db.LichChieu.Any(n => n.MaPhim == MaPhim);
            if (ckbLichChieu)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Phim đã xếp lịch chiếu không thể xóa!", MessageConstant.MESSAGE_ERROR_404);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Phim đã xếp lịch chiếu không thể xóa");
            }

            Phim p = db.Phim.SingleOrDefault(n => n.MaPhim == MaPhim);
            string hinhAnh = p.HinhAnh;
            db.Phim.Remove(p);
            db.SaveChanges();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", hinhAnh);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return new ResponseEntity(StatusCodeConstants.OK, "Xóa thành công!", MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok();
        }


        [HttpGet("LayThongTinPhim")]
        public async Task<ResponseEntity> LayThongTinPhim(int MaPhim = 0)
        {
            if (MaPhim == 0 || !db.Phim.Any(n => n.MaPhim == MaPhim))
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "Mã phim không hợp lệ!", MessageConstant.BAD_REQUEST);

                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã phim không hợp lệ!");
            }
            Phim phim = db.Phim.Single(n => n.MaPhim == MaPhim);
            ChiTietPhimVM chiTietPhim = new ChiTietPhimVM();
            chiTietPhim.BiDanh = phim.BiDanh;
            chiTietPhim.DanhGia = phim.DanhGia;
            chiTietPhim.HinhAnh = DomainImage + phim.HinhAnh;
            chiTietPhim.MaNhom = phim.MaNhom;
            chiTietPhim.TenPhim = phim.TenPhim;
            chiTietPhim.MaPhim = phim.MaPhim;
            chiTietPhim.MoTa = phim.MoTa;
            chiTietPhim.Trailer = phim.Trailer;
            chiTietPhim.NgayKhoiChieu = phim.NgayKhoiChieu;
            chiTietPhim.Hot = phim.Hot;
            chiTietPhim.DangChieu = phim.DangChieu;
            chiTietPhim.SapChieu = phim.SapChieu;

            var lst = db.LichChieu.Where(n => n.MaPhim == MaPhim);
            if (lst.Count() > 0)
            {
                foreach (var lichChieu in lst)
                {
                    ThongTinLichChieu thongTinLC = new ThongTinLichChieu();
                    thongTinLC.GiaVe = lichChieu.GiaVe;
                    thongTinLC.MaLichChieu = lichChieu.MaLichChieu;
                    thongTinLC.MaPhim = lichChieu.MaPhim;
                    thongTinLC.MaRap = lichChieu.MaRap;
                    thongTinLC.NgayChieuGioChieu = lichChieu.NgayChieuGioChieu;
                    thongTinLC.TenPhim = lichChieu.MaPhimNavigation.TenPhim;
                    thongTinLC.ThoiLuong = lichChieu.ThoiLuong;
                    thongTinLC.ThongTinRap.MaCumRap = lichChieu.MaRapNavigation.MaCumRap;
                    thongTinLC.ThongTinRap.MaHeThongRap = lichChieu.MaRapNavigation.MaCumRapNavigation.MaHeThongRap;
                    thongTinLC.ThongTinRap.MaRap = lichChieu.MaRap;
                    thongTinLC.ThongTinRap.TenRap = lichChieu.MaRapNavigation.TenRap;
                    thongTinLC.ThongTinRap.TenCumRap = lichChieu.MaRapNavigation.MaCumRapNavigation.TenCumRap;
                    thongTinLC.ThongTinRap.TenHeThongRap = lichChieu.MaRapNavigation.MaCumRapNavigation.MaHeThongRapNavigation.TenHeThongRap;

                    chiTietPhim.LichChieu.Add(thongTinLC);
                }
            }
            return new ResponseEntity(StatusCodeConstants.OK, chiTietPhim, MessageConstant.MESSAGE_SUCCESS_200);

            //return Ok(chiTietPhim);
        }


    }





}