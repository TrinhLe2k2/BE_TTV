using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTV.Models;

namespace TTV.Controllers
{
    public class AccountController : Controller
    {

        [HttpPost]
        public ActionResult Login(string InputUsername, string inputPassword)
        {
            TangThuVienEntities db = new TangThuVienEntities();
            var user = db.Users.SingleOrDefault(m => m.username.ToLower() == InputUsername.ToLower() && m.password == inputPassword);

            if (user != null)
            {
                Session["user"] = user;
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect("~/Home/Error");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("~/Home/Index");
        }
    }
}