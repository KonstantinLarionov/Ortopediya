﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ortopediya.Models.Entitys;

namespace Ortopediya.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private BackContext db { get; set; }

        public AdminController(ILogger<AdminController> logger)
        {
            db = new BackContext(new DbContextOptions<BackContext>());
            _logger = logger;
        }
        private bool Auth(string login = "", string password = "")
        {
            if (login == "" && password == "")
            {
                login = HttpContext.Session.GetString("login");
                password = HttpContext.Session.GetString("password");
            }
            var admin = db.Admins.Where(u => u.Login == login && u.Password == password).ToList();
            if (admin.Count != 0)
            {
                HttpContext.Session.SetString("login", login);
                HttpContext.Session.SetString("password", password);
                return true;
            }
            else
            {
                return false;
            }
        }
        private Models.Objects.Contact GetContact()
        {
            var contacts = db.Contacts.ToList().LastOrDefault();
            if (contacts == null)
            {
                contacts = new Models.Objects.Contact();
            }

            return contacts;
        }
        private List<Models.Objects.Baner> GetBaners()
        {
            List<Models.Objects.Baner> baners = db.Baners.ToList();
            if (baners.Count != 0)
            {
                baners.OrderByDescending(o => o.Id);
            }

            return baners;
        }

        public IActionResult Index()
        {
            if (Auth())
            {
                return View("Index");
            }
            else
            {
                return View("Login");
            }
        }
        public IActionResult Contacts()
        {
           
            if (Auth())
            {
                Models.Objects.Contact contacts = GetContact();
                return View("Contacts", contacts);
            }
            else 
            {
                return View("Login");
            }
        }
        public IActionResult Baners()
        {
            if (Auth())
            {
                List<Models.Objects.Baner> baners = GetBaners();
                return View("Baners", baners);
            }
            else
            {
                return View("Login");
            }
        }
        public IActionResult Categories()
        {
            if (Auth())
            {
                List<Models.Objects.Category> category = db.Categories.ToList();
                return View("Categories", category);
            }

            else
            {
                return View("Login");
            }
        }
        public IActionResult Products()
        {
            Models.PageObjects.MarketModel marketModel = new Models.PageObjects.MarketModel();
            marketModel.Categories = db.Categories.ToList();
            marketModel.Products = db.Products.ToList();

            return View("Products", marketModel);
        }
        public IActionResult Products(string category)
        {
            Models.PageObjects.MarketModel marketModel = new Models.PageObjects.MarketModel();
            marketModel.Categories = db.Categories.ToList();
            try
            {
                marketModel.Products = db.Products.Where(p => p.Category.Name == category).ToList();
            }
            catch
            {
                marketModel.Products = db.Products.ToList();
            }
            return View("Products", marketModel);
        }


        #region REST CONTROL
        [HttpPost]
        public IActionResult Categories(string name, string url)
        {
            if (Auth())
            {

                db.Categories.Add(new Models.Objects.Category() { Name = name, Url = url, CountProducts = 0, LastEdit = DateTime.Now });
                db.SaveChanges();
                List<Models.Objects.Category> category = db.Categories.ToList();

                return View("Categories", category);
            }
            else 
            {
                return View("Login");
            }
        }
        [HttpPost]
        public IActionResult DeleteCategories(string name, string url)
        {
            if (Auth())
            {
                var categoryd = db.Categories.Where(c => c.Name == name && c.Url == url).FirstOrDefault();
                db.Categories.Remove(categoryd);
                db.SaveChanges();
                List<Models.Objects.Category> category = db.Categories.ToList();
                return View("Categories", category);
            }
            else
            {
                return View("Login");
            }
        }
        [HttpPost]
        public IActionResult Baners(IFormFile file)
        {
            if (file != null && file.Length != 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            db.Baners.Add(new Models.Objects.Baner() { URL = file.FileName });
            db.SaveChanges();
            List<Models.Objects.Baner> baners = GetBaners();
            return View("Baners", baners);
        }
        [HttpPost]
        public IActionResult DeleteBaners(string url)
        {
            var baner = db.Baners.Where(b => b.URL == url).FirstOrDefault();
            db.Baners.Remove(baner);
            db.SaveChanges();
            List<Models.Objects.Baner> baners = GetBaners();
            return View("Baners", baners);
        }
        [HttpPost]
        public IActionResult Contacts(string EmailInfo, string EmailMarketing, string PhoneInfo, string PhoneMarketing, string VKLink, string OKLink, string FacebookLink, string InstagramLink)
        {
            db.Contacts.Add(new Models.Objects.Contact() 
            {
                EmailInfo = EmailInfo,
                EmailMarketing = EmailMarketing,
                PhoneInfo = PhoneInfo,
                PhoneMarketing = PhoneMarketing,
                VKLink = VKLink,
                OKLink = OKLink,
                FacebookLink = FacebookLink,
                InstagramLink = InstagramLink,
                LastDateEdit = DateTime.Now
            });
            db.SaveChanges();

            Models.Objects.Contact contacts = GetContact();
            if (Auth())
            {
                return View("Contacts", contacts);
            }
            else
            {
                return View("Login");
            }
        }
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (Auth(login, password))
            {
                return View("Index");
            }
            else
            {
                return View("Login");
            }
        }
        #endregion
    }
}