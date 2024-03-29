﻿using ExperIklimdendirmeApp.Context;
using ExperIklimdendirmeApp.Models;
using ExperIklimdendirmeApp.Models.Logs;
using ExperIklimdendirmeApp.Models.ViewModel;
using Microsoft.Win32;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            List<Customer> _customers = _context.Customers.Where(x => x.IsActive == 1).ToList();
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
                             UsedMaterial = cus.UsedMaterial,
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

        public BaseViewModel CustomerDelete(int id)
        {
            BaseViewModel _res = new BaseViewModel();
            Customer _customer = _context.Customers.Find(id);
            if (_customer != null)
            {
                var active = _customer.IsActive = 0;

            }
            _context.SaveChanges();
            TempData["Message"] = $"{_customer.FirstName} {_customer.LastName} İsimli Müşteri Kaydı Silindi.";
            return _res;
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

        ////Takvim Ekleme
        //public ActionResult GetCalendarEvents()
        //{
        //    List<CalendarEvents> eventItems = new List<CalendarEvents>();
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
        //        //SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
        //        con.Open();
        //        var query = $@"select * from CalendarEvents";

        //        SqlCommand command = new SqlCommand(query, con);
        //        DataTable dt = new DataTable();
        //        SqlDataAdapter da = new SqlDataAdapter(command);
        //        da.Fill(dt);

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            DataRow dr = dt.Rows[i];
        //            CalendarEvents item = new CalendarEvents();

        //            item.id = int.Parse(dr["id"].ToString());
        //            item.title = dr["title"].ToString();
        //            item.start =DateTime.Parse(Convert.ToString(dr["start"])).ToString("yyyy-MM-dd HH:mm:ss");
        //            item.end = DateTime.Parse(Convert.ToString(dr["end"])).ToString("yyyy-MM-dd HH:mm:ss");
        //            //item.start = string.Format("{0:s}", dr["start"]);
        //            //item.end = string.Format("{0:s}", dr["end"]);
        //            item.color = dr["color"].ToString();
        //            item.IsActive = 1;

        //            eventItems.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return Json(eventItems,JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult AddOrEditItem(CalendarEvents item)
        //{
        //    ProjectContext connect = new ProjectContext();
        //    try
        //    {
        //        CalendarEvents _event = new CalendarEvents();
        //        _event.title = item.title;
        //        _event.start = item.start;
        //        _event.end = item.end;
        //        _event.IsActive = 1;
        //        _event.Customerid = item.Customerid;
        //        _context.CalendarEvents.Add(_event);
        //        _context.SaveChanges();


        //        #region SaveLogs
        //        LogsCalendarEvent _log = new LogsCalendarEvent();
        //        _log.title = item.title;
        //        _log.start = item.start;
        //        _log.end = item.end;
        //        _log.Customerid = item.Customerid;
        //        _context.LogsCalendarEvent.Add(_log);
        //        _context.SaveChanges();
        //        #endregion



        //        return Json(_event, JsonRequestBehavior.AllowGet);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public JsonResult UpdateItemDate(int? eventid, string startDate, string endDate, string title,int customerid)
        //{
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
        //        //SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
        //        con.Open();
        //        List<SqlParameter> param = new List<SqlParameter>();
        //        param.Add(new SqlParameter("@id", eventid));
        //        param.Add(new SqlParameter("@startdate", string.Format("{0:s}", startDate)));
        //        param.Add(new SqlParameter("@enddate", string.Format("{0:s}", endDate)));
        //        param.Add(new SqlParameter("@title", title));
        //        param.Add(new SqlParameter("@customerid", customerid));

        //        var query = $@"update CalendarEvents set start=@startdate, [end]=@enddate, title=@title,Customerid =@customerid  where id =@id";

        //        RunSqlCommand(query, param);


        //        #region UpdateLogs
        //        //List<SqlParameter> param1 = new List<SqlParameter>();
        //        //param1.Add(new SqlParameter("@id", eventid));
        //        //param1.Add(new SqlParameter("@startdate", string.Format("{0:s}", startDate)));
        //        //param1.Add(new SqlParameter("@enddate", string.Format("{0:s}", endDate)));
        //        //param1.Add(new SqlParameter("@title", title));
        //        //param1.Add(new SqlParameter("@customerid", customerid));
        //        //var query1 = $@"update LogsCalendarEvent set start=@startdate, [end]=@enddate, title=@title,Customerid =@customerid  where id =@id";

        //        //RunSqlCommand(query1, param1);
        //        #endregion

        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public JsonResult DeleteItemDate(int? eventid)
        //{
        //    try
        //    {

        //        SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
        //        //SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
        //        con.Open();
        //        List<SqlParameter> param = new List<SqlParameter>();
        //        param.Add(new SqlParameter("@id", eventid));

        //        var query = $@"Delete  CalendarEvents where id =@id";

        //        //SqlCommand command = new SqlCommand(query, con);
        //        //CalendarEvents _update = _context.CalendarEvents.First(x => x.id == eventid);
        //        //var query = from events in _context.CalendarEvents
        //        //             where events.id == eventid
        //        //             select events;

        //        //foreach (var item in query)
        //        //{
        //        //    item.title = title;
        //        //    item.start = startDate;
        //        //    item.end = endDate;
        //        //}

        //        //_context.SaveChanges();

        //        RunSqlCommand(query, param);

        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //public JsonResult GetCalendarItemEvent(int id)
        //{
        //    try
        //    {
        //        CalendarEventsViewModel res = new CalendarEventsViewModel();
        //        var query = (from c in _context.CalendarEvents
        //                     where c.id == id
        //                     select new CalendarEventsViewModel
        //                     {
        //                         title = c.title,
        //                         // start= DateTime.Parse(Convert.ToString(c.start)).ToString(),
        //                         // end= Convert.ToString(DateTime.Parse(c.end)),
        //                         start = DateTime.Parse(Convert.ToString(c.start)).ToString("yyyy-MM-dd"),
        //                         end = DateTime.Parse(Convert.ToString(c.end)).ToString("yyyy-MM-dd"),
        //                         calendarid = c.id,
        //                         customerid = c.Customerid
        //                     }).ToList();
        //        if (query.Count != 0)
        //        {
        //            res.title = query[0].title;
        //            res.start = query[0].start;
        //            res.end = query[0].end;
        //            res.calendarid = query[0].calendarid;
        //            res.customerid = query[0].customerid;
        //            res.sonuc = 1;
        //        }

        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        //public void RunSqlCommand(string sql, List<SqlParameter> param = null)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        SqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = sql;
        //        if (param != null)
        //            cmd.Parameters.AddRange(param.ToArray());
        //        cmd.CommandType = CommandType.Text;
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //public void OpenConnection()
        //{
        //    connection = new SqlConnection(connectionString);

        //    if (connection.State != ConnectionState.Open)
        //        connection.Open();

        //}


        //public JsonResult GetCustomersList()
        //{
        //    List<CustomerListViewModel> customres = new List<CustomerListViewModel>();
        //    var query = (from cus in _context.Customers
        //                 where cus.IsActive == 1
        //                 select new CustomerListViewModel()
        //                 {
        //                     customerid = cus.Id,
        //                     Name = cus.FirstName + cus.LastName
        //                 }).ToList();

        //    if (query.Count != 0)
        //    {
        //        customres.AddRange(query);
        //    }
        //    return Json(customres, JsonRequestBehavior.AllowGet);

        //}
    }
}