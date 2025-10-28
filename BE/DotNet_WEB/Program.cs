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
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCV")),
    RequestPath = "/cv-files"
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
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDien")),
    RequestPath = "/LuuTruAnhDaiDien"
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCVOnline")),
    RequestPath = "/LuuTruCVOnline"
});

app.MapControllers();

app.Run();
