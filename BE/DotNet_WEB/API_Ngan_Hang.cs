using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using DotNet_WEB.Module;
using DotNet_WEB.Class;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;


namespace DotNet_WEB
{
    [ApiController]
    [Route("api/[controller]")]
    public class API_Ngan_Hang : ControllerBase
    {
       

        [HttpPost("thanhToanVNPAY")]
        public IActionResult thanhToanVNPAY([FromBody] tao_don_hang don_hang_thong_tin)
        {
            var urlThanhToan = Module_Ngan_Hang.taoDonHang(don_hang_thong_tin);
            if (!string.IsNullOrEmpty(urlThanhToan))
            {
                return Ok(new { success = true, urlThanhToan });
            }
            return Ok(new { success = false, message = "Không tạo được url" });
        }
        [HttpGet("VNPayReturn")]
        public IActionResult VNPayReturn()
        {
            var query = Request.Query;
            string? vnp_SecureHash = query["vnp_SecureHash"];
            string? vnp_TxnRef = query["vnp_TxnRef"];
            string? vnp_Amount = query["vnp_Amount"];
            string? vnp_ResponseCode = query["vnp_ResponseCode"];

            var fields = query.Where(kv => kv.Key.StartsWith("vnp_") && kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
            .OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
            var hashData = string.Join("&", fields.Select(kv => $"{kv.Key}={kv.Value}"));
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("I7LL3FX1ZJQZ6OCXQ9EGY9DVT0W0Q3EE"));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData));
            var calculatedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            if (calculatedHash != vnp_SecureHash)
                return BadRequest("Sai hash, không hợp lệ");
            if (vnp_ResponseCode != "00")
                return Redirect($"http://localhost:4200/trang-ket-qua-thanh-toan?success=false&ma_don_hang={vnp_TxnRef}");
            int ma_don_hang = int.Parse(vnp_TxnRef);
            decimal so_tien = Convert.ToDecimal(vnp_Amount) / 100m;
            bool ket_qua = Module_Ngan_Hang.capNhatTrangThaiDonHang(ma_don_hang, so_tien, vnp_ResponseCode);
            if (ket_qua)
            {
                return Redirect($"http://localhost:4200/trang-ket-qua-thanh-toan?success=true&ma_don_hang={ma_don_hang}");
            }
            return Redirect($"http://localhost:4200/trang-ket-qua-thanh-toan?success=false&ma_don_hang={ma_don_hang}");
        }
    }
}