using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAAPI.ViewModels
{
   
    public class NotificationX
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public int? SenderId { get; set; }
        public bool SMS { get; set; }
        public bool Email { get; set; }
        public bool App { get; set; }
        public DateTime? DateSMSSent { get; set; }
        public DateTime? DateEmailSent { get; set; }
        public DateTime? DateAppSent { get; set; }
        public string SMSIssue { get; set; }
        public string EmailIssue { get; set; }
        public string AppIssue { get; set; }
        public DateTime? DateAppVisited { get; set; }
        public int TypeId { get; set; }
        public string Subject { get; set; }
        public int? ModuleId { get; set; }

        List<int> employees;
        public List<int> Employees
        {
            get
            {
                if (employees == null)
                    employees = new List<int>();
                return employees;
            }
            set
            {
                employees = value;
            }
        }


        List<string> names;
        public List<string> Names
        {
            get
            {
                if (names == null)
                    names = new List<string>();
                return names;
            }
            set
            {
                names = value;
            }
        }
        public static void Fill(Models.Notification entity, ViewModels.NotificationX notification,int userid)
        {
            entity.Id = notification.Id;
            entity.UserId = userid;
            entity.CustomerId = notification.CustomerId;
            entity.Message = notification.Message;
            entity.DateSent = DateTime.Now;
            entity.SenderId = notification.SenderId;
            entity.SMS = notification.SMS;
            entity.Email = notification.Email;
            entity.App = notification.App;
            entity.DateSMSSent = null;
            entity.DateEmailSent = null;
            entity.DateAppSent = null;
            entity.SMSIssue = null;
            entity.EmailIssue = null;
            entity.AppIssue = null;
            entity.DateAppVisited = null;
            entity.TypeId = notification.TypeId;
            entity.Subject = notification.Subject;
            entity.ModuleId = notification.ModuleId;
        }
        public static void FillDto(Models.Notification entity, ViewModels.NotificationX notification)
        {
            notification.Id = entity.Id;
            notification.UserId = entity.UserId;
            notification.CustomerId = entity.CustomerId;
            notification.Message = entity.Message;
            notification.DateSent = DateTime.Now;
            notification.SenderId = entity.SenderId;
            notification.SMS = entity.SMS;
            notification.Email = entity.Email;
            notification.App = entity.App;
            notification.DateSMSSent = null;
            notification.DateEmailSent = null;
            notification.DateAppSent = null;
            notification.SMSIssue = null;
            notification.EmailIssue = null;
            notification.AppIssue = null;
            notification.DateAppVisited = null;
            notification.TypeId = entity.TypeId;
            notification.Subject = entity.Subject;
            notification.ModuleId = entity.ModuleId;
        }
    }
}