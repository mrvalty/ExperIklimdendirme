using ExperIklimdendirmeApp.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExperIklimdendirmeApp.Controllers
{
    public class LoginController : Controller
    {
        ProjectContext _context = new ProjectContext();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            //ProjectContext _context = new ProjectContext();
            //User _user = new User();

            //foreach (var item in _context.Users)
            //{
            //    if (item.UserName == UserName && item.Password == Password)
            //    {
            //        _user.UserName = UserName;
            //        _user.Password = Password;
            //        _user.Message = "Giriş Başarılı..";
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        TempData["Message"] = "Kullanıcı adı veya şifre hatalı";
            //        return RedirectToAction("Login", "Login");
            //    }

            //}
            //return View();

            var _user = _context.Users.FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
            if (_user != null)
            {
                return RedirectToAction("Index","Home");

            }
            else
            {
                TempData["Message"] = "Kullanıcı adı veya şifre hatalı";
                return RedirectToAction("Login","Login");
            }

        }
    }
}