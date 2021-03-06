﻿using System.Linq;
using System.Web.Mvc;
using CryptSharp;
using Gameoflife.Models;

namespace Gameoflife.Controllers
{
    public class UsersController : Controller
    {
        private GameOfLifeDataEntities1 db = new GameOfLifeDataEntities1();


        // GET: Users/Create
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,Password,FirstName,LastName")] User user)
        {
            if (ModelState.IsValid)
            {
                user.IsAdmin = false;
                user.Password = Crypter.Blowfish.Crypt(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {
            using (var database = new GameOfLifeDataEntities1())

            {
                User login = database.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

                if (login != null)// && Crypter.CheckPassword(user.Password,login.Password))
                {
                    Session["User"] = login;
                    return RedirectToAction("Index", "Home");
                    
                }



            }
            ViewBag.LoginErrorMessage = "Fail To log in";

            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
