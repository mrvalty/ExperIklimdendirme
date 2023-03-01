using ExperIklimdendirmeApp.Context;
using ExperIklimdendirmeApp.Models;
using ExperIklimdendirmeApp.Models.ViewModel;
using Microsoft.Win32;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace ExperIklimdendirmeApp.Controllers
{
    public class HomeController : Controller
    {
        ProjectContext _context = new ProjectContext();

        public ActionResult Index()
        {
            List<Customer> _customers = _context.Customers.Where(x=>x.IsActive==1).ToList();
            var _customercount = _customers.Count();
            ViewBag.CountCustomer = _customercount;

            var _tarih = DateTime.Now;
            ViewBag.Tarih = _tarih;
            return View();
        }

        public ActionResult GetCustomerList()
        {

            var query = (from cus in _context.Customers
                         where cus.IsActive == 1
                         select new QRCustomerViewModel()
                         {
                             Id = cus.Id,
                             Address = cus.Address,
                             Explanation = cus.Explanation,
                             FaultReason = cus.FaultReason,
                              UsedMaterial=cus.UsedMaterial,
                             FirstName = cus.FirstName,
                             LastName = cus.LastName,
                             PhoneNumber = cus.PhoneNumber

                         }).ToList();

            return View(query);
        }

        public ActionResult AddCustomer()
        {
            GetLastId();
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(string FirstName, string LastName, string PhoneNumber, string Address, string FaultReason, string UsedMaterial, string Explanation, string lastId)
        {

            Customer _customer = new Customer();
            _customer.Address = Address;
            _customer.Explanation = Explanation;
            _customer.FaultReason = FaultReason;
            _customer.FirstName = FirstName;
            _customer.LastName = LastName;
            _customer.PhoneNumber = PhoneNumber;
            _customer.UsedMaterial = UsedMaterial;
            _customer.IsActive = 1;
            ProjectContext _context = new ProjectContext();
            _context.Customers.Add(_customer);

            CreateQR(lastId);

            _context.SaveChanges();

            //return RedirectToAction("Index","QRCode");
            TempData["Message"] = $"{_customer.FirstName} {_customer.LastName} İsimli Müşteri Kaydı Başarılı.";
            return View();
        }

        public ActionResult AddedCustomer()
        {
            return View();
        }
        public ActionResult CustomerUpdateDetail(int id)
        {
            Customer _customer = _context.Customers.Find(id);
            return View(_customer);
        }

        [HttpPost]
        public ActionResult CustomerUpdate(int id, string FirstName, string LastName, string PhoneNumber, string Address, string FaultReason, string UsedMaterial, string Explanation)
        {

            Customer _customer = _context.Customers.Find(id);
            _customer.Explanation = Explanation;
            _customer.FaultReason = FaultReason;
            _customer.FirstName = FirstName;
            _customer.LastName = LastName;
            _customer.PhoneNumber = PhoneNumber;
            _customer.UsedMaterial = UsedMaterial;
            _customer.Address = Address;
            _context.Customers.Update(_customer);

            _context.SaveChanges();

            TempData["Message"] = $"{_customer.FirstName} {_customer.LastName} İsimli Müşteri Kaydı Güncellendi.";

            return RedirectToAction("GetCustomerList", "Home");
        }
        [HttpGet]
        public ActionResult CustomerDeleteDetail(int id)
        {

            Customer _customer = _context.Customers.Find(id);
            return View(_customer);
        }

        public ActionResult CustomerDelete(int id)
        {

            Customer _customer = _context.Customers.Find(id);
            if (_customer != null)
            {
                var active = _customer.IsActive = 0;
                
            }
            _context.SaveChanges();
            TempData["Message"] = $"{_customer.FirstName} {_customer.LastName} İsimli Müşteri Kaydı Silindi.";
            //ViewBag.Message = "Müşteri Başarı ile Silindi";
            return RedirectToAction("GetCustomerList", "Home");
        }

        public void GetLastId()
        {
            int lastid = _context.Customers.OrderBy(x => x.Id).Select(x => x.Id).Last();
            lastid++;
            ViewBag.lastId = lastid;

        }

        public void CreateQR(string id)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCustomer qRCustomer = new QRCustomer();

                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(id, QRCodeGenerator.ECCLevel.H);
                QRCode qRCode = new QRCode(qRCodeData);

                using (Bitmap image = qRCode.GetGraphic(10))
                {
                    image.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

                    //img= new byte[ms.ToArray().Length];
                    //img = ms.ToArray();

                    qRCustomer.CustomerId = Convert.ToInt32(id);
                    qRCustomer.QRImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    _context.QRCustomers.Add(qRCustomer);

                    _context.SaveChanges();
                }

            }

        }

        public static Bitmap GetImageFromByteArray(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            string base64String = Convert.ToBase64String(bytes);
            Bitmap _bitmap = new Bitmap(base64String);
            return _bitmap;
        }
       

        //public void QRGetDetails(int id)
        //{
        //    Customer customer = _context.Customers.Find(id);
        //    var text = customer.FirstName + " " + customer.LastName + " " + (Environment.NewLine) + "" + customer.PhoneNumber + " " + customer.Address;
        //    ViewBag.text = text;
        //}

    }
}