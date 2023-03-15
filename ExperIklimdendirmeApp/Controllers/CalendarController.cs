using ExperIklimdendirmeApp.Context;
using ExperIklimdendirmeApp.Models;
using ExperIklimdendirmeApp.Models.Logs;
using ExperIklimdendirmeApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExperIklimdendirmeApp.Controllers
{
    public class CalendarController : Controller
    {
        ProjectContext _context = new ProjectContext();
        private SqlConnection connection = null;

        private string connectionString = @"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;";
        //private string connectionString = @"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;";
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        //Takvim Ekleme

        public ActionResult GetCalendarEvents(string start,string end)
        {
            List<CalendarEvents> eventItems = new List<CalendarEvents>();
            try
            {
                //SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
                SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
                con.Open();
                var query = $@"select * from CalendarEvents";

                SqlCommand command = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    CalendarEvents item = new CalendarEvents();

                    item.id = int.Parse(dr["id"].ToString());
                    item.title = dr["title"].ToString();
                    item.start = DateTime.Parse(Convert.ToString(dr["start"])).ToString("yyyy-MM-dd HH:mm:ss");
                    item.end = DateTime.Parse(Convert.ToString(dr["end"])).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                    //item.start = string.Format("{0:s}", dr["start"]);
                    //item.end = string.Format("{0:s}", dr["end"]);
                    item.color = dr["color"].ToString();
                    item.IsActive = 1;

                    eventItems.Add(item);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(eventItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddOrEditItem(CalendarEvents item)
        {
            ProjectContext connect = new ProjectContext();
            try
            {
                CalendarEvents _event = new CalendarEvents();
                _event.title = item.title;
                _event.start = item.start;
                _event.end = item.end;
                _event.IsActive = 1;
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
        public JsonResult UpdateItemDate(int? eventid, string startDate, string endDate, string title, int customerid)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
                //SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
                con.Open();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@id", eventid));
                param.Add(new SqlParameter("@startdate", string.Format("{0:s}", startDate)));
                param.Add(new SqlParameter("@enddate", string.Format("{0:s}", endDate)));
                param.Add(new SqlParameter("@title", title));
                param.Add(new SqlParameter("@customerid", customerid));

                var query = $@"update CalendarEvents set start=@startdate, [end]=@enddate, title=@title,Customerid =@customerid  where id =@id";

                RunSqlCommand(query, param);


                #region UpdateLogs
                //List<SqlParameter> param1 = new List<SqlParameter>();
                //param1.Add(new SqlParameter("@id", eventid));
                //param1.Add(new SqlParameter("@startdate", string.Format("{0:s}", startDate)));
                //param1.Add(new SqlParameter("@enddate", string.Format("{0:s}", endDate)));
                //param1.Add(new SqlParameter("@title", title));
                //param1.Add(new SqlParameter("@customerid", customerid));
                //var query1 = $@"update LogsCalendarEvent set start=@startdate, [end]=@enddate, title=@title,Customerid =@customerid  where id =@id";

                //RunSqlCommand(query1, param1);
                #endregion

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult DeleteItemDate(int? eventid)
        {
            try
            {

                SqlConnection con = new SqlConnection(@"server=104.247.162.242\\MSSQLSERVER2019;database=fuatomay_exprDB1; user=fuatomay_admndb1; password=ANkara12345//.*;");
                //SqlConnection con = new SqlConnection(@"server=.;database=exprDB1; user=admndb1;password=ANkara12345//.*;");
                con.Open();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@id", eventid));

                var query = $@"Delete  CalendarEvents where id =@id";

                //SqlCommand command = new SqlCommand(query, con);
                //CalendarEvents _update = _context.CalendarEvents.First(x => x.id == eventid);
                //var query = from events in _context.CalendarEvents
                //             where events.id == eventid
                //             select events;

                //foreach (var item in query)
                //{
                //    item.title = title;
                //    item.start = startDate;
                //    item.end = endDate;
                //}

                //_context.SaveChanges();

                RunSqlCommand(query, param);

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


        public void RunSqlCommand(string sql, List<SqlParameter> param = null)
        {
            try
            {
                OpenConnection();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                if (param != null)
                    cmd.Parameters.AddRange(param.ToArray());
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void OpenConnection()
        {
            connection = new SqlConnection(connectionString);

            if (connection.State != ConnectionState.Open)
                connection.Open();

        }


        public JsonResult GetCustomersList()
        {
            List<CustomerListViewModel> customres = new List<CustomerListViewModel>();
            var query = (from cus in _context.Customers
                         where cus.IsActive == 1
                         select new CustomerListViewModel()
                         {
                             customerid = cus.Id,
                             Name = cus.FirstName + cus.LastName
                         }).ToList();

            if (query.Count != 0)
            {
                customres.AddRange(query);
            }
            return Json(customres, JsonRequestBehavior.AllowGet);

        }
    }
}