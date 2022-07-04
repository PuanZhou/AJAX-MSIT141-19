using Microsoft.AspNetCore.Mvc;
using prjAJAXDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjAJAXDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;

        public ApiController(DemoContext context)
        {
            _context = context;
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

        public IActionResult Regiser(CUser user)
        {
            bool datas=false;
            if (string.IsNullOrEmpty(user.name))
            {
                return Content("<span style=\"color: red\">請填寫姓名</span>", "text/Html", System.Text.Encoding.UTF8);
            }
            datas = _context.Members.Any(m => m.Name.ToLower().Contains(user.name.ToLower()));
            if (datas == true)
            {
                return Content("<span style=\"color: red\">該名稱已被註冊</span>", "text/Html", System.Text.Encoding.UTF8);
            }
            return Content("該名稱可以被註冊", "text/plain", System.Text.Encoding.UTF8);
        }
    }
}
