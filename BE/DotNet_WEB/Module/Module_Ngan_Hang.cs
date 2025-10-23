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

namespace DotNet_WEB.Module
{
    public class Module_Ngan_Hang
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static string taoDonHang(tao_don_hang tdh)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            string payment_Url = "";
            try
            {
                decimal gia_dich_vu = 0;
                string lay_gia_dich_vu = "SELECT gia FROM dich_vu WHERE ma_dich_vu = @ma_Dich_Vu";
                using (var cmd = new MySqlCommand(lay_gia_dich_vu, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    var kq = cmd.ExecuteScalar();
                    if (kq == null) throw new Exception("Dịch vụ không tồn tại");
                    gia_dich_vu = Convert.ToDecimal(kq);
                }

                int ma_don_hang;
                string them_don_hang = @"
            INSERT INTO don_hang (ma_cong_ty, tong_tien, trang_thai_don_hang, ngay_tao)
            VALUES (@ma_Cong_Ty, @tong_tien, 'cho_Thanh_Toan', NOW());
            SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(them_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Cong_Ty", tdh.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@tong_tien", gia_dich_vu);
                    var res = cmd.ExecuteScalar();
                    if (res == null) throw new Exception("Không lấy được ID đơn hàng");
                    ma_don_hang = Convert.ToInt32(res);
                }

                string tao_chi_tiet_don_hang = @"
            INSERT INTO chi_tiet_don_hang (ma_don_hang, ma_dich_vu, so_luong, don_gia, trang_thai_don_hang)
            VALUES (@ma_Don_Hang, @ma_Dich_Vu, 1, @don_Gia, 'cho_Thanh_Toan')";

                using (var cmd = new MySqlCommand(tao_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    cmd.Parameters.AddWithValue("@don_Gia", gia_dich_vu);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();

                payment_Url = GenerateVNPayUrl(ma_don_hang, gia_dich_vu);
            }
            catch
            {
                try { trans.Rollback(); } catch { }
                throw;
            }
            return payment_Url;
        }
        private static string GenerateVNPayUrl(int ma_don_hang, decimal tongTien)
        {
            var vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", "BROPIXNH" },
                { "vnp_Amount", ((long)(tongTien * 100)).ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", ma_don_hang.ToString() },
                { "vnp_OrderInfo", $"Thanh toán đơn hàng {ma_don_hang}" },
                { "vnp_OrderType", "other" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", "https://1e3b4214677f.ngrok-free.app/api/API_WEB/VNPayReturn" },
                { "vnp_IpAddr", "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_SecureHashType", "SHA256" }
            };

            var sortedParams = vnp_Params.OrderBy(k => k.Key);
            var queryString = new StringBuilder();
            var hashData = new StringBuilder();

            foreach (var kv in sortedParams)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&");
                    hashData.Append("&");
                }
                queryString.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
                hashData.Append($"{kv.Key}={kv.Value}");
            }

            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("I7LL3FX1ZJQZ6OCXQ9EGY9DVT0W0Q3EE"));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData.ToString()));
            var vnp_SecureHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

            return $"{"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"}?{queryString}&vnp_SecureHash={vnp_SecureHash}";
        }

        public static bool capNhatTrangThaiDonHang(int ma_don_hang, decimal so_tien, string vnpay_res)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string cap_nhat_don_hang = "UPDATE don_hang SET trang_thai_don_hang = 'da_Thanh_Toan' WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.ExecuteNonQuery();
                }

                string cap_nhat_chi_tiet_don_hang = "UPDATE chi_tiet_don_hang SET trang_thai_don_hang = 'da_Thanh_Toan' WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.ExecuteNonQuery();
                }

                string them_thanh_toan_moi = @"INSERT INTO thanh_toan (ma_don_hang, so_tien, response_code, ngay_thanh_toan, trang_thai_thanh_toan) " +
                        "VALUES (@ma_Don_Hang, @so_Tien, @res, now(), 'da_Thanh_Toan')";
                using (var cmd = new MySqlCommand(them_thanh_toan_moi, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@so_Tien", so_tien);
                    cmd.Parameters.AddWithValue("@res", vnpay_res);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }
    }
}