using ExperIklimdendirmeApp.Context;
using ExperIklimdendirmeApp.Models;
using ExperIklimdendirmeApp.Models.Logs;
using ExperIklimdendirmeApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExperIklimdendirmeApp.Controllers
{
    public class CalendarController : Controller
    {
        ProjectContext _context = new ProjectContext();
        // GET: Calendar
        public ActionResult GetCalendarEvents()
        {
            return View();
        }
        //Takvim Ekleme
        [HttpPost]
        public JsonResult GetCalendarEvents(string start, string end)
        {
            try
            {
                CalendarEventsViewModel res = new CalendarEventsViewModel();

                var query = (from c in _context.CalendarEvents
                             select new CalendarEventsViewModel
                             {
                                 id = int.Parse(c.id.ToString()),
                                 title = c.title.ToString(),
                                 start = DateTime.Parse(Convert.ToString(c.start)).ToString("yyyy-MM-dd"),
                                 end = DateTime.Parse(Convert.ToString(c.end)).AddDays(1).ToString("yyyy-MM-dd"),
                                 customerid = int.Parse(c.Customerid.ToString())
                             }).ToList();

                return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public JsonResult AddOrEditItem(CalendarEvents item)
        {
            try
            {
                CalendarEvents _event = new CalendarEvents();
                _event.title = item.title;
                _event.start = item.start;
                _event.end = item.end;
                _event.Customerid = item.Customerid;
                _context.CalendarEvents.Add(_event);
                _context.SaveChanges();


                #region SaveLogs
                LogsCalendarEvent _log = new LogsCalendarEvent();
                _log.title = item.title;
                _log.start = item.start;
                _log.end = item.end;
                _log.Customerid = item.Customerid;
                _context.LogsCalendarEvent.Add(_log);
                _context.SaveChanges();
                #endregion

                return Json(_event, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult UpdateItemDate(CalendarEvents item)
        {
            try
            {
                CalendarEvents updateevent = _context.CalendarEvents.Where(x => x.id == item.id).FirstOrDefault();
                if(updateevent != null)
                {
                    updateevent.title = item.title;
                    updateevent.start = item.start;
                    updateevent.end = item.end;
                    updateevent.Customerid = item.Customerid;
                }
                _context.CalendarEvents.Update(updateevent);
                _context.SaveChanges();

                #region UpdateLogs
                var updateeventlog = _context.LogsCalendarEvent.Where(x => x.id == item.id).FirstOrDefault();
                if (updateevent != null)
                {
                    updateeventlog.title = item.title;
                    updateeventlog.start = item.start;
                    updateeventlog.end = item.end;
                    updateeventlog.Customerid = item.Customerid;
                }
                _context.LogsCalendarEvent.Update(updateeventlog);
                _context.SaveChanges();
                #endregion

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult DeleteItemDate(int id)
        {

            try
            {
                var deleteid = _context.CalendarEvents.Where(x=>x.id == id).FirstOrDefault();
                if (deleteid != null)
                {
                    _context.CalendarEvents.Remove(deleteid);
                    _context.SaveChanges();
                }

                
                                        
                //_context.CalendarEvents.Remove();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult GetCalendarItemEvent(int id)
        {
            try
            {
                CalendarEventsViewModel res = new CalendarEventsViewModel();
                var query = (from c in _context.CalendarEvents
                             where c.id == id
                             select new CalendarEventsViewModel
                             {
                                 title = c.title,
                                 // start= DateTime.Parse(Convert.ToString(c.start)).ToString(),
                                 // end= Convert.ToString(DateTime.Parse(c.end)),
                                 start = DateTime.Parse(Convert.ToString(c.start)).ToString("yyyy-MM-dd"),
                                 end = DateTime.Parse(Convert.ToString(c.end)).ToString("yyyy-MM-dd"),
                                 calendarid = c.id,
                                 customerid = c.Customerid
                             }).ToList();
                if (query.Count != 0)
                {
                    res.title = query[0].title;
                    res.start = query[0].start;
                    res.end = query[0].end;
                    res.calendarid = query[0].calendarid;
                    res.customerid = query[0].customerid;
                    res.sonuc = 1;
                }

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult GetCustomersList()
        {
            List<CustomerListViewModel> customres = new List<CustomerListViewModel>();
            var query = (from cus in _context.Customers
                         where cus.IsActive == 1
                         select new CustomerListViewModel()
                         {
                             customerid = cus.Id,
                             Name = cus.FirstName +' '+ cus.LastName
                         }).ToList();

            if (query.Count != 0)
            {
                customres.AddRange(query);
            }
            return Json(customres, JsonRequestBehavior.AllowGet);

        }
    }
}