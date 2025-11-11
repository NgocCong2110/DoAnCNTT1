using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using DotNet_WEB.Module;
using DotNet_WEB.Class;
using System.Net;

namespace DotNet_WEB
{
    [ApiController]
    [Route("api/[controller]")]
    public class API_Ngan_Hang : ControllerBase
    {
        [HttpPost("thanhToanSepay")]
        public IActionResult thanhToanSepay([FromBody] tao_don_hang don_hang_thong_tin)
        {
            try
            {
                var urlThanhToan = Module_Ngan_Hang.taoDonHang(don_hang_thong_tin);
                if (!string.IsNullOrEmpty(urlThanhToan))
                {
                    return Ok(new { success = true, urlThanhToan });
                }
                return Ok(new { success = false, message = "Không tạo được url Sepay" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("SepayCallback")]
        public IActionResult SepayCallback([FromQuery] SepayCallbackModel callbackGet, [FromForm] SepayCallbackModel callbackPost)
        {
            SepayCallbackModel callback;

            if (Request.Method == "POST")
                callback = callbackPost;
            else
                callback = callbackGet;

            Console.WriteLine("[Debug] Sepay callback received via " + Request.Method);
            Console.WriteLine($"OrderId = {callback.OrderId}, Amount = {callback.Amount}, ResponseCode = {callback.ResponseCode}");

            _ = Task.Run(() =>
            {
                try
                {
                    if (callback.ResponseCode == "00")
                    {
                        bool res = Module_Ngan_Hang.capNhatTrangThaiDonHang(
                            callback.OrderId,
                            callback.Amount,
                            callback.ResponseCode,
                            callback.TransactionNo,
                            callback.BankCode
                        );
                        Console.WriteLine($"[Debug] DB update result: {res}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Debug] Exception khi update DB: {ex.Message}");
                }
            });

            // Trả về ngay 200 OK
            return Ok(new { success = true, message = "Callback received" });
        }




    }

    public class SepayCallbackModel
    {
        public string? TransactionNo { get; set; }
        public string? ResponseCode { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? BankCode { get; set; }
    }
}
