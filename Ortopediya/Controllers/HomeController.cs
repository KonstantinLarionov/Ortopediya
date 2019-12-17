using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ortopediya.Models;
using Ortopediya.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Ortopediya.Models.PageObjects;

namespace Ortopediya.Controllers
{
    public class HomeController : Controller
    {
        private FrontContext db { get; set; }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            db = new FrontContext(new DbContextOptions<FrontContext>());
            _logger = logger;
        }
        private IndexModel GetModelIndex()
        {
            IndexModel indexModel = new IndexModel();
            var contact = db.Contacts.OrderByDescending(x => x).FirstOrDefault();
            indexModel.Contacts = contact == null ? new Models.Objects.Contact() : contact;
            indexModel.Products = db.Products.Where(o => o.Category.Name == "Рекомендуемые").OrderByDescending(x => x).ToList();
            indexModel.Baners = db.Baners.ToList();
            indexModel.Categories = db.Categories.ToList();
            return indexModel;
        }
        private ContactModel GetModelContact()
        {
            ContactModel contactModel = new ContactModel();
            var contact = db.Contacts.OrderByDescending(x => x).FirstOrDefault();
            contactModel.Contacts = contact == null ? new Models.Objects.Contact() : contact;
            contactModel.Categories = db.Categories.ToList();
            return contactModel;
        }
        private MarketModel GetModelMarket(List<Models.Objects.Product> product, string selectCoty)
        {
            MarketModel marketModel = new MarketModel();
            var contact = db.Contacts.OrderByDescending(x => x).FirstOrDefault();
            marketModel.Contacts = contact == null ? new Models.Objects.Contact() : contact;
            marketModel.Categories = db.Categories.ToList();
            marketModel.Products = product;
            marketModel.SelectedCategory = selectCoty;
            return marketModel;
        }
        private PartnerModel GetModelPartner()
        {
            PartnerModel partnerModel = new PartnerModel();
            partnerModel.Categories = db.Categories.ToList();
            var contact = db.Contacts.OrderByDescending(x => x).FirstOrDefault();
            partnerModel.Contacts = contact == null ? new Models.Objects.Contact() : contact;
            return partnerModel;
        }

        #region GETMETHODE
        public IActionResult Index()
        {
            db.Database.EnsureCreated();
            //TODO: Отладить функцию котроля IP
            //Models.Objects.IP IP = new Models.Objects.IP()
            //{
            //    Address = System.Text.Encoding.UTF8.GetString(HttpContext.Connection.RemoteIpAddress.GetAddressBytes())
            //};
            //var ipDb = db.IPs.Where(i => i.Address == IP.Address).FirstOrDefault();
            //db.Users.Add(new Models.Objects.User()
            //{
            //    DateCreate = DateTime.Now,
            //    IP = ipDb != null && IP.Address == ipDb.Address ? ipDb.Address : IP.Address
            //});
            //db.SaveChanges();
            IndexModel indexModel = GetModelIndex();
            return View(indexModel);
        }
        public IActionResult Contact()
        {
            ContactModel contactModel = GetModelContact();
            return View("Contact", contactModel);
        }
        public IActionResult Market(string id)
        {
            var Id = id;
            var product = db.Products.Where(p => p.Category.Url == id).ToList();
            var selectCategory = db.Categories.Where(c => c.Url == id).Select(s => s.Name).FirstOrDefault();
            MarketModel marketModel = GetModelMarket(product, selectCategory);
            return View("Market", marketModel);
        }
        public IActionResult Partner()
        {
            var partner = GetModelPartner();
            return View("Partner", partner);
        }
        [HttpGet]
        public ActionResult BalancePartner(string code)
        {
            var balance = db.Partners.Where(o => o.Code == code).Select(x=>x.Balance).FirstOrDefault();
            return Json(balance);
        }
        [HttpPost]
        public ActionResult OutOfMoneyPartners(string code, string card, string password)
        {
            var User = db.Partners.Where(x => x.Code == code && x.Password == password).FirstOrDefault();
            if (User != null)
            {
                Models.Objects.Request request = new Models.Objects.Request();
                request.Message = User.Balance.ToString() + "\n" + card;
                request.Theme = "Запрос на вывод средств";
                request.User = User;
                db.Requests.Add(request);
                db.SaveChanges();
            }
         
            var indexModel = GetModelIndex();

            return View("Index",indexModel);
        }
        #endregion

        #region POSTMETHODE
        [HttpPost]
        public IActionResult Contact(string name, string contact, string theme, string text)
        {
            int phone = 0;
            try
            {
                phone = Convert.ToInt32(contact);
            }
            catch 
            {    }
            Models.Objects.Request request = new Models.Objects.Request()
            {
                Message = text,
                Theme = theme
            };
            Models.Objects.User user = new Models.Objects.User();
            if (db.Users.Any(u => u.Phone == phone.ToString() || u.Email == contact))
            {
                user = db.Users.Where(u => u.Phone == phone.ToString() || u.Email == contact).FirstOrDefault();
            }
            else
            {
                user = new Models.Objects.User() { Email = phone == 0 ? contact : "", Phone = phone.ToString() };
            }
            request.User = user;
            db.Requests.Add(request);
            db.SaveChanges();
            ContactModel contactModel = GetModelContact();
            return View("Contact", contactModel);
        }
        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            if (!db.Users.Any(u => u.Email == email))
            {
                db.Users.Add(new Models.Objects.User()
                {
                    IP = System.Text.Encoding.UTF8.GetString(HttpContext.Connection.RemoteIpAddress.GetAddressBytes()),
                    Email = email,
                    DateCreate = DateTime.Now
                }) ;
                db.SaveChanges();
            }
            IndexModel indexModel = GetModelIndex();
            return View("Index",indexModel);
        }
        #endregion

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
