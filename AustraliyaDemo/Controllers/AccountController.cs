using AustraliyaDemo.Context;
using AustraliyaDemo.Entity;
using AustraliyaDemo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AustraliyaDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbMyContext _dbContext;


        public AccountController(DbMyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.msg = "";
                    using (var context = _dbContext)
                    {

                        var us = context.Users.Where(x => x.EmailId == user.EmailId).FirstOrDefault();
                        if (us == null)
                        {
                            user.TypeId = 3;
                            user.CreatedOn = DateTime.Now;
                            user.LastModified = DateTime.Now;
                            context.Users.Add(user);
                            context.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ModelState.AddModelError("", "This Email is Already Register");
                            ViewBag.msg = "This Email is Already Register";
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex){ }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(Usermodel useremail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = _dbContext)
                    {
                        var status = context.Users.Where(x => x.EmailId == useremail.EmailId && x.Password == useremail.UserPassword).Any();
                        var user1 = context.Users.FirstOrDefault(x => x.EmailId == useremail.EmailId && x.Password == useremail.UserPassword);

                        if (status == true)
                        {
                            var routeValues = new RouteValueDictionary
                        {
                                { "id", user1.Id }

                        };
                            var type = context.UserTypes.Where(x => x.Id == user1.TypeId).FirstOrDefault();
                            if (type.UserName == "Administrator")
                            {

                                return RedirectToAction("Dashboard", "Admin", routeValues);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", routeValues);
                            }


                        }
                        else
                        {
                            ViewBag.msg = "Userid & Password doesn't match";
                            return View();
                        }
                    }

                }
            }
            catch (Exception ex) { }
            return View();
        }
      
        public IActionResult Logout()
        {
           
            return RedirectToAction("Login","Account");
        }
        
    }
}
