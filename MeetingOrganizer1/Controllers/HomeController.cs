using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingOrganizer1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMeeting()
        {
            using (DatabaseOrganizerEntities dc = new DatabaseOrganizerEntities())
            {
                var meeting = dc.Meetings.ToList();
                return new JsonResult { Data = meeting, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveMeeting(Meeting e)
        {
            var status = false;
            using (DatabaseOrganizerEntities dc = new DatabaseOrganizerEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                Console.WriteLine(e.MeetingID);
                if (e.MeetingID > 0)
                {
                    //Update the event
                    var v = dc.Meetings.Where(a => a.MeetingID == e.MeetingID).FirstOrDefault();
                    if (v != null)
                    {
                        v.MeetingSubject = e.MeetingSubject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Participants = e.Participants;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Meetings.Add(e);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteMeeting(int meetingID)
        {
            var status = false;
            using (DatabaseOrganizerEntities dc = new DatabaseOrganizerEntities())
            {
                var v = dc.Meetings.Where(a => a.MeetingID == meetingID).FirstOrDefault();
                if (v != null)
                {
                    dc.Meetings.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}