using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using ZstdSharp.Unsafe;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography.X509Certificates;
using System.Net.Quic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_thong_bao
{
    public class chuc_nang_thong_bao_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<thong_bao> layDanhSachThongBao(thong_bao_kieu_nguoi_dung tb_knd)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            if (tb_knd.kieu_nguoi_dung == "nguoi_Tim_Viec")
            {
                sql = @"select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, qt.ho_ten,
                                tb.ma_quan_tri, tb.ma_cong_ty, ct.ten_cong_ty, cttm.dia_diem, cttm.thoi_gian, tb.ngay_tao, tb.ma_nguoi_tim_viec
                            from thong_bao tb 
                            LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                            LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                            LEFT JOIN chi_tiet_thu_moi cttm on tb.ma_thong_bao = cttm.ma_thong_bao
                            WHERE tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                                OR (tb.loai_thong_bao = 'thu_Moi_Phong_Van' AND tb.ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec)
                            ORDER BY tb.ma_thong_bao ASC";
            }
            if (tb_knd.kieu_nguoi_dung != "nguoi_Tim_Viec")
            {
                sql = @"select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, qt.ho_ten,
                                tb.ma_quan_tri, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao
                            from thong_bao tb 
                            LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                            LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                            WHERE tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                            ORDER BY tb.ma_thong_bao ASC";
            }
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("ma_Nguoi_Tim_Viec", tb_knd.ma_nguoi_tim_viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao = new List<thong_bao>();
            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    loai_thong_bao = reader.IsDBNull(reader.GetOrdinal("loai_thong_bao")) ? LoaiThongBao.None : (LoaiThongBao)Enum.Parse(typeof(LoaiThongBao), reader.GetString("loai_thong_bao")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                if (danh_sach.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {

                    int ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec");

                    danh_sach.chi_tiet_thu_moi = layChiTietThuMoi(ma_nguoi_tim_viec);

                }

                if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten")
                    };
                }
                if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    };
                }
                danh_sach_thong_bao.Add(danh_sach);
            }
            return danh_sach_thong_bao;
        }

        public static List<thong_bao> chonThongBaoCoDinh(thong_bao thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            if (thong_Bao.ma_cong_ty != 0 || thong_Bao.ma_quan_tri != 0)
            {
                sql = @"SELECT tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, 
                    tb.ma_quan_tri, qt.ho_ten, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao, tb.ma_nguoi_tim_viec
                    FROM thong_bao tb 
                LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                WHERE tb.loai_thong_bao = @loai_Thong_Bao
                ORDER BY tb.ma_thong_bao ASC";
            }

            else
            {
                sql = @"SELECT tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, 
                    tb.ma_quan_tri, qt.ho_ten, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao, tb.ma_nguoi_tim_viec
                    FROM thong_bao tb 
                LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                WHERE tb.loai_thong_bao = @loai_Thong_Bao and (tb.loai_thong_bao != 'thu_Moi_Phong_Van' or tb.ma_nguoi_tim_viec = @ma_nguoi_tim_viec)
                ORDER BY tb.ma_thong_bao ASC";
            }

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@loai_Thong_Bao", thong_Bao.loai_thong_bao);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", thong_Bao.ma_nguoi_tim_viec);

            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao_co_dinh = new List<thong_bao>();

            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    loai_thong_bao = reader.IsDBNull(reader.GetOrdinal("loai_thong_bao"))
                                     ? LoaiThongBao.None
                                     : (LoaiThongBao)Enum.Parse(typeof(LoaiThongBao), reader.GetString("loai_thong_bao")),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };

                if (danh_sach.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {
                    int ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec");
                    danh_sach.chi_tiet_thu_moi = layChiTietThuMoi(ma_nguoi_tim_viec);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten")
                    };
                }

                // Cong ty
                if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    };
                }

                danh_sach_thong_bao_co_dinh.Add(danh_sach);
            }

            return danh_sach_thong_bao_co_dinh;
        }
        
        private static List<chi_tiet_thu_moi> layChiTietThuMoi(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select thoi_gian, dia_diem from chi_tiet_thu_moi where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_Nguoi_Tim_Viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<chi_tiet_thu_moi>();
            while (reader.Read())
            {
                var chi_tiet = new chi_tiet_thu_moi
                {
                    thoi_gian = reader.IsDBNull(reader.GetOrdinal("thoi_gian")) ? DateTime.MinValue : reader.GetDateTime("thoi_gian"),

                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem")
                };

                danh_sach.Add(chi_tiet);
            }
            return danh_sach;
        }
    }
}