using EPAAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using EPAAPI.ViewModels;

namespace EPAAPI.DAL
{
    public class NotificationRepository : GenericRepository<Models.Notification>
    {
        public NotificationRepository(EPAGRIFFINEntities context)
      : base(context)
        {
        }

        public async Task<Notification> VisitMessage(int id)
        {
            var notification = await this.context.Notifications.FirstOrDefaultAsync(q => q.Id==id);
            if (notification != null)
                notification.DateAppVisited = DateTime.Now;
            return notification;
            // return await this.context.UserActivityMenuHits.FirstOrDefaultAsync(q => q.UserId == dto.UserId && q.CustomerId == dto.CustomerId && q.ModuleId == dto.ModuleId && q.Key == dto.Key);
        }
        public async Task<Models.ViewNotification> GetNotification(int id)
        {
            var view = await this.context.ViewNotifications.FirstOrDefaultAsync(q => q.Id == id);
            if (view != null)
            {
                var notification = await dbSet.Where(q=>q.Id==id).Select(q=>q.Message).FirstOrDefaultAsync();
                view.Abstract = notification;
            }
            return view;
        }

        public IQueryable<ViewNotification> GetViewViewNotification()
        {
            return this.GetQuery<ViewNotification>();
        }

        

        public virtual CustomActionResult Validate(ViewModels.NotificationX dto)
        {
            //var exist = dbSet.FirstOrDefault(q => q.Id != dto.Id && q.Title.ToLower().Trim() == dto.Title.ToLower().Trim());
            //if (exist != null)
            //    return Exceptions.getDuplicateException("Organization-01", "Title");

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
    }
}