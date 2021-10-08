using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using WebAppBackupMysql.Models;

namespace WebAppBackupMysql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Backup([FromServices] MySqlConnection connection, [FromServices] IWebHostEnvironment web)
        {
            using MySqlCommand cmd = new();
            using MySqlBackup mb = new(cmd);
            cmd.Connection = connection;
            connection.Open();

            Names names = new();
            string name = names.Name;
            string nameSql = names.GetNameSql();
            string nameZip = names.GetNameZip();

            mb.ExportToFile(web.WebRootPath + $"/backup/{nameSql}");
            mb.Dispose();
            cmd.Dispose();

            using FileStream streamFileSql = new(web.WebRootPath + $"/backup/{nameZip}", FileMode.Create);
            using ZipArchive zipArchive = new(streamFileSql, ZipArchiveMode.Create);
            zipArchive.CreateEntryFromFile(web.WebRootPath + $"/backup/{nameSql}", name + ".sql");
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
