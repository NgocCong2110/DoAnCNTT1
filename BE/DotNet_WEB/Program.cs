using System;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "libs", "libwkhtmltox.dll");
if (!File.Exists(dllPath))
{
    throw new FileNotFoundException($"Không tìm thấy libwkhtmltox.dll tại {dllPath}");
}

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(dllPath);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCVUngTuyen")),
    RequestPath = "/LuuTruCVUngTuyen"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruLogoCongTy")),
    RequestPath = "/LuuTruLogoCongTy"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDienCV")),
    RequestPath = "/LuuTruAnhDaiDienCV"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDienNguoiTimViec")),
    RequestPath = "/LuuTruAnhDaiDienNguoiTimViec"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruChungChiNguoiTimViec")),
    RequestPath = "/LuuTruChungChiNguoiTimViec"
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCVOnline")),
    RequestPath = "/LuuTruCVOnline",
    ServeUnknownFileTypes = true, 
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
    }
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhBiaCongTy")),
    RequestPath = "/LuuTruAnhBiaCongTy"
});

app.MapControllers();

app.Run();
