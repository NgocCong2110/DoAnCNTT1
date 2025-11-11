using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module
{
    public class Module_Ngan_Hang
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        // Tạo đơn hàng và trả về URL Sepay
        public static string taoDonHang(tao_don_hang tdh)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            string payment_Url = "";

            try
            {
                Console.WriteLine($"[Debug] Bắt đầu taoDonHang cho ma_dich_vu={tdh.ma_dich_vu}, ma_cong_ty={tdh.ma_cong_ty}");

                // Lấy giá dịch vụ
                decimal gia_dich_vu = 0;
                string lay_gia_dich_vu = "SELECT gia FROM dich_vu WHERE ma_dich_vu = @ma_Dich_Vu";
                using (var cmd = new MySqlCommand(lay_gia_dich_vu, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    var kq = cmd.ExecuteScalar();
                    if (kq == null) throw new Exception("Dịch vụ không tồn tại");
                    gia_dich_vu = Convert.ToDecimal(kq);
                    Console.WriteLine($"[Debug] Gia dich vu: {gia_dich_vu}");
                }

                // Insert đơn hàng
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
                    Console.WriteLine($"[Debug] Ma don hang: {ma_don_hang}");
                }

                // Insert chi tiết đơn hàng
                string tao_chi_tiet_don_hang = @"
                    INSERT INTO chi_tiet_don_hang (ma_don_hang, ma_dich_vu, so_luong, don_gia)
                    VALUES (@ma_Don_Hang, @ma_Dich_Vu, 1, @don_Gia)";

                using (var cmd = new MySqlCommand(tao_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    cmd.Parameters.AddWithValue("@don_Gia", gia_dich_vu);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("[Debug] Insert chi tiet don hang thanh cong");
                }

                trans.Commit();
                Console.WriteLine("[Debug] Commit transaction thanh cong");

                // Tạo URL Sepay
                payment_Url = GenerateSepayUrl(ma_don_hang, gia_dich_vu);
                Console.WriteLine($"[Debug] Payment URL: {payment_Url}");
            }
            catch (Exception ex)
            {
                try { trans.Rollback(); Console.WriteLine("[Debug] Transaction rollback do loi"); } catch { }
                Console.WriteLine($"[Debug] Exception: {ex.Message}");
                throw;
            }

            return payment_Url;
        }

        // Tạo URL thanh toán Sepay
        private static string GenerateSepayUrl(int ma_don_hang, decimal tongTien)
        {
            string merchantId = "SP-TEST-NNB9A3A5";
            string secretKey = "spsk_test_XUJgAqig8s7Wn2ibK2XTQhKsFFPoDkD4";
            string returnUrl = "https://d5145c868967.ngrok-free.app/api/API_Ngan_Hang/SepayCallback";

            var sepay_Params = new Dictionary<string, string>
            {
                { "merchant_id", merchantId },
                { "order_id", ma_don_hang.ToString() },
                { "amount", ((long)(tongTien * 100)).ToString() }, // nhân 100 đơn vị nhỏ nhất
                { "currency", "VND" },
                { "order_info", $"Thanh toán đơn hàng {ma_don_hang}" },
                { "return_url", returnUrl },
                { "created_at", DateTime.UtcNow.ToString("yyyyMMddHHmmss") } // dùng UTC
            };

            Console.WriteLine("---- Sepay Params ----");
            foreach (var kv in sepay_Params)
                Console.WriteLine($"{kv.Key} = {kv.Value}");

            var sortedParams = sepay_Params.OrderBy(k => k.Key);

            var hashData = new StringBuilder();
            foreach (var kv in sortedParams)
            {
                if (hashData.Length > 0) hashData.Append("&");
                hashData.Append($"{kv.Key}={kv.Value}");
            }
            Console.WriteLine($"[Debug] Chuoi hashData: {hashData}");

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData.ToString()));
            string secureHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            Console.WriteLine($"[Debug] SecureHash: {secureHash}");

            var queryString = new StringBuilder();
            foreach (var kv in sortedParams)
            {
                if (queryString.Length > 0) queryString.Append("&");
                queryString.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
            }

            var finalUrl = $"https://sandbox.sepay.vn/payment?{queryString}&signature={secureHash}";
            Console.WriteLine($"[Debug] Final Payment URL: {finalUrl}");
            Console.WriteLine("-------------------------");

            return finalUrl;
        }
        
        public static bool capNhatTrangThaiDonHang(int ma_don_hang, decimal so_tien, string sepay_res, string transactionNo, string bankCode)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                Console.WriteLine($"[Debug] Cap nhat don hang ma_don_hang={ma_don_hang}, so_tien={so_tien}, res={sepay_res}, transNo={transactionNo}, bankCode={bankCode}");

                string cap_nhat_don_hang = "UPDATE don_hang SET trang_thai_don_hang = 'da_Thanh_Toan' WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"[Debug] Cap nhat don hang rows affected: {rows}");
                }

                string cap_nhat_chi_tiet_don_hang = "UPDATE chi_tiet_don_hang SET don_gia=@so_Tien WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@so_Tien", so_tien);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"[Debug] Cap nhat chi tiet don hang rows affected: {rows}");
                }

                string them_thanh_toan_moi = @"
                    INSERT INTO thanh_toan (ma_don_hang, so_tien, response_code, transaction_no, bank_code, ngay_thanh_toan, trang_thai_thanh_toan, ngay_tao)
                    VALUES (@ma_Don_Hang, @so_Tien, @res, @transNo, @bankCode, NOW(), 'thanh_Cong', NOW())";
                using (var cmd = new MySqlCommand(them_thanh_toan_moi, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@so_Tien", so_tien);
                    cmd.Parameters.AddWithValue("@res", sepay_res);
                    cmd.Parameters.AddWithValue("@transNo", transactionNo);
                    cmd.Parameters.AddWithValue("@bankCode", bankCode);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"[Debug] Insert thanh toan rows affected: {rows}");
                }

                trans.Commit();
                Console.WriteLine("[Debug] Commit transaction thanh cong");
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine($"[Debug] Transaction rollback do loi: {ex.Message}");
                return false;
            }
        }
    }
}
