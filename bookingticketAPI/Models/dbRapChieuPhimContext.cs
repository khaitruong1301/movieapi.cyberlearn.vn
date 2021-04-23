using System;
using bookingticketAPI.StatusConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bookingticketAPI.Models
{
    public partial class dbRapChieuPhimContext : DbContext
    {
        public dbRapChieuPhimContext()
        {
        }

        public dbRapChieuPhimContext(DbContextOptions<dbRapChieuPhimContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CumRap> CumRap { get; set; }
        public virtual DbSet<DatVe> DatVe { get; set; }
        public virtual DbSet<Ghe> Ghe { get; set; }
        public virtual DbSet<HeThongRap> HeThongRap { get; set; }
        public virtual DbSet<LichChieu> LichChieu { get; set; }
        public virtual DbSet<LoaiGhe> LoaiGhe { get; set; }
        public virtual DbSet<LoaiNguoiDung> LoaiNguoiDung { get; set; }
        public virtual DbSet<NguoiDung> NguoiDung { get; set; }
        public virtual DbSet<Nhom> Nhom { get; set; }
        public virtual DbSet<Phim> Phim { get; set; }
        public virtual DbSet<Rap> Rap { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.


                //optionsBuilder.UseSqlServer("Server=61.14.235.39,1433;database=dbRapChieuPhim;user id=khai;password=Khai123456@;Trusted_Connection=True;Integrated Security=False ");
                //Bật tham chiếu =>khóa ngoại cài thêm using Microsoft.EntityFrameworkCore.Proxies;
            
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(Config.connect);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CumRap>(entity =>
            {
                entity.HasKey(e => e.MaCumRap);

                entity.Property(e => e.MaCumRap)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.MaHeThongRap).HasMaxLength(50);

                entity.HasOne(d => d.MaHeThongRapNavigation)
                    .WithMany(p => p.CumRap)
                    .HasForeignKey(d => d.MaHeThongRap)
                    .HasConstraintName("FK_CumRap_HeThongRap");
            });

            modelBuilder.Entity<DatVe>(entity =>
            {
                entity.HasKey(e => e.MaVe);

                entity.Property(e => e.GiaVe).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NgayDat).HasColumnType("datetime");

                entity.Property(e => e.TaiKhoanNguoiDung).HasMaxLength(150);

                entity.HasOne(d => d.MaGheNavigation)
                    .WithMany(p => p.DatVe)
                    .HasForeignKey(d => d.MaGhe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatVe_Ghe");

                entity.HasOne(d => d.MaLichChieuNavigation)
                    .WithMany(p => p.DatVe)
                    .HasForeignKey(d => d.MaLichChieu)
                    .HasConstraintName("FK_DatVe_LichChieu");

                entity.HasOne(d => d.TaiKhoanNguoiDungNavigation)
                    .WithMany(p => p.DatVe)
                    .HasForeignKey(d => d.TaiKhoanNguoiDung)
                    .HasConstraintName("FK_DatVe_NguoiDung");
            });

            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.HasKey(e => e.MaGhe);

                entity.Property(e => e.Stt)
                    .HasColumnName("STT")
                    .HasMaxLength(50);

                entity.Property(e => e.TenGhe).HasMaxLength(50);

                entity.HasOne(d => d.MaLoaiGheNavigation)
                    .WithMany(p => p.Ghe)
                    .HasForeignKey(d => d.MaLoaiGhe)
                    .HasConstraintName("FK_Ghe_LoaiGhe");

                entity.HasOne(d => d.MaRapNavigation)
                    .WithMany(p => p.Ghe)
                    .HasForeignKey(d => d.MaRap)
                    .HasConstraintName("FK_Ghe_Rap");
            });

            modelBuilder.Entity<HeThongRap>(entity =>
            {
                entity.HasKey(e => e.MaHeThongRap);

                entity.Property(e => e.MaHeThongRap)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BiDanh).HasMaxLength(50);

                entity.Property(e => e.Logo).HasMaxLength(50);

                entity.Property(e => e.TenHeThongRap).HasMaxLength(50);
            });

            modelBuilder.Entity<LichChieu>(entity =>
            {
                entity.HasKey(e => e.MaLichChieu);

                entity.Property(e => e.GiaVe).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MaNhom).HasMaxLength(50);

                entity.Property(e => e.NgayChieuGioChieu).HasColumnType("datetime");

                entity.HasOne(d => d.MaNhomNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.MaNhom)
                    .HasConstraintName("FK_LichChieu_Nhom");

                entity.HasOne(d => d.MaPhimNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.MaPhim)
                    .HasConstraintName("FK_LichChieu_Phim");

                entity.HasOne(d => d.MaRapNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.MaRap)
                    .HasConstraintName("FK_LichChieu_Rap");
            });

            modelBuilder.Entity<LoaiGhe>(entity =>
            {
                entity.HasKey(e => e.MaLoaiGhe);

                entity.Property(e => e.MaLoaiGhe).ValueGeneratedNever();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TenLoaiGhe).HasMaxLength(50);
            });

            modelBuilder.Entity<LoaiNguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaLoaiNguoiDung);

                entity.Property(e => e.MaLoaiNguoiDung)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.TenLoai).HasMaxLength(50);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan);

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(150)
                    .ValueGeneratedNever();

                entity.Property(e => e.BiDanh).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.HoTen).HasMaxLength(200);

                entity.Property(e => e.MaLoaiNguoiDung).HasMaxLength(50);

                entity.Property(e => e.MaNhom).HasMaxLength(50);

                entity.Property(e => e.MatKhau).HasMaxLength(50);

                entity.Property(e => e.SecretKey).HasMaxLength(50);

                entity.Property(e => e.SoDt)
                    .HasColumnName("SoDT")
                    .HasMaxLength(50);

                entity.Property(e => e.Ssid)
                    .HasColumnName("SSID")
                    .HasMaxLength(50);

                entity.HasOne(d => d.MaLoaiNguoiDungNavigation)
                    .WithMany(p => p.NguoiDung)
                    .HasForeignKey(d => d.MaLoaiNguoiDung)
                    .HasConstraintName("FK_NguoiDung_LoaiNguoiDung");

                entity.HasOne(d => d.MaNhomNavigation)
                    .WithMany(p => p.NguoiDung)
                    .HasForeignKey(d => d.MaNhom)
                    .HasConstraintName("FK_NguoiDung_Nhom");
            });

            modelBuilder.Entity<Nhom>(entity =>
            {
                entity.HasKey(e => e.MaNhom);

                entity.Property(e => e.MaNhom)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.TenNhom).HasMaxLength(50);
            });

            modelBuilder.Entity<Phim>(entity =>
            {
                entity.HasKey(e => e.MaPhim);

                entity.Property(e => e.MaNhom).HasMaxLength(50);

                entity.Property(e => e.NgayKhoiChieu).HasColumnType("datetime");

                entity.HasOne(d => d.MaNhomNavigation)
                    .WithMany(p => p.Phim)
                    .HasForeignKey(d => d.MaNhom)
                    .HasConstraintName("FK_Phim_Nhom1");
            });


            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.MaBanner);
                entity.Property(e => e.MaPhim).HasColumnType("int");

                entity.Property(e => e.HinhAnh).HasMaxLength(255);


              
            });

            modelBuilder.Entity<Rap>(entity =>
            {
                entity.HasKey(e => e.MaRap);

                entity.Property(e => e.MaCumRap).HasMaxLength(50);

                entity.Property(e => e.TenRap).HasMaxLength(50);

                entity.HasOne(d => d.MaCumRapNavigation)
                    .WithMany(p => p.Rap)
                    .HasForeignKey(d => d.MaCumRap)
                    .HasConstraintName("FK_Rap_CumRap");
            });
        }
    }
}
