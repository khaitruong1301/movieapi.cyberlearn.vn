using bookingticketAPI.Models;
using bookingticketAPI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI
{
    public class AutoMapperConfig : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a => a.AddProfile<AutoMapperConfig>());
        }
        public AutoMapperConfig()
        {
            CreateMap<NguoiDungVM, NguoiDung>();
            CreateMap<ThongTinTaiKhoanVM, NguoiDung>();
            CreateMap<NguoiDung, ThongTinTaiKhoanVM>();
            CreateMap<PhimVM, Phim>();
            CreateMap<PhimInsert, Phim>();
            CreateMap<PhimInsert, PhimInsertNew>();
            CreateMap<PhimInsertNew, Phim>();
            //CreateMap<NguoiDungVMM, NguoiDung>();
            //CreateMap<NguoiDung, ThongTinTaiKhoan>();
            //CreateMap<KhoaHoc, KhoaHocVM>();
        }
    }
}
