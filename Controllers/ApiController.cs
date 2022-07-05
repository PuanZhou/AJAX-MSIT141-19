using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjAJAXDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjAJAXDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;

        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _host = hostEnvironment;
        }
        public IActionResult Index(CUser user)
        {
            //System.Threading.Thread.Sleep(5000); //停止5秒鐘

            //return Content("Ajax, 你好!!","text/plain", System.Text.Encoding.UTF8);
            if (String.IsNullOrEmpty(user.name))
            {
                user.name = "Ajax";
            }
            return Content($"{user.name}你好,你的年紀是{user.age},你的email是{user.email}!!", "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult CheckName(string name)
        {
            var exists = _context.Members.Any(m => m.Name == name);
            return Content(exists.ToString(), "text/plain");
        }

        public IActionResult Register(Member member,IFormFile file)
        {

            //C:\Users\Student\Documents\Ajax\MSIT141Site\wwwroot\uploads
            //string path = _host.ContentRootPath; //會取得專案資料夾的實際路徑

            string path = Path.Combine(_host.WebRootPath, "uploads", file.FileName); //會取得專案資料夾下wwwroot的實際路徑

            using(var fileStream=new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);//儲存檔案到uploads資料夾中
            }

            byte[] imgByte = null;
            using (var memorystream = new MemoryStream())
            {
                file.CopyTo(memorystream);
                imgByte = memorystream.ToArray();
            }
            member.FileName = file.FileName;
            member.FileData = imgByte;
            
            
            _context.Members.Add(member);
            _context.SaveChanges();

            string info = $"{file.FileName} - {file.ContentType} - {file.Length}";

            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }

        //讀取所有城市的資料
        public IActionResult City()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        //根據城市名稱讀出鄉鎮區的資料
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(a=>a.City==city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }

        //根據鄉鎮區的資料讀出路名
        public IActionResult Roads(string district)
        {
            var roads = _context.Addresses.Where(a => a.SiteId == district).Select(a => a.Road);
            return Json(roads);
        }


        public IActionResult GetImageBytes(int id = 1)
        {
            Member member = _context.Members.Find(id);

            byte[] img = member.FileData;

            return File(img, "imge/jpeg");
        }

        public IActionResult Members()
        {
            return Json(_context.Members);
        }

    }
}
