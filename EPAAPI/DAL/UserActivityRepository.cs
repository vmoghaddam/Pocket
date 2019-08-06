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
    public class UserActivityRepository : GenericRepository<Models.UserActivity>
    {
        public UserActivityRepository(EPAGRIFFINEntities context)
      : base(context)
        {
        }

        public IQueryable<ViewUserActivity> GetViewUserActivity()
        {
            return this.GetQuery<ViewUserActivity>();
        }
        public IQueryable<UserActivityMenuHit> GetUserActivityMenuHit()
        {
            return this.GetQuery<UserActivityMenuHit>();
        }
        public virtual void Insert(UserActivityMenuHit entity)
        {
            this.context.UserActivityMenuHits.Add(entity);
        }

        public  async Task<UserActivityMenuHit> GetMenuHitsByDto(UserActivityX dto)
        {
            return await this.context.UserActivityMenuHits.FirstOrDefaultAsync(q => q.UserId == dto.UserId && q.CustomerId == dto.CustomerId && q.ModuleId == dto.ModuleId && q.Key == dto.Key);
        }

        public async Task<EmployeeBookStatu> VisitLibraryItem(int employeeId,int itemId)
        {
            var status = await this.context.EmployeeBookStatus.FirstOrDefaultAsync(q => q.EmployeeId == employeeId && q.BookId == itemId);
            if (status == null)
            {
                status = new EmployeeBookStatu()
                {
                    BookId = itemId,
                    DateVisit = DateTime.Now,
                    EmployeeId = employeeId,
                    IsVisited = true,
                    IsDownloaded = false

                };
                this.context.EmployeeBookStatus.Add(status);
            }
            if (!status.IsVisited)
            {
                status.IsVisited = true;
                status.DateVisit = DateTime.Now;
            }
            return status;
           // return await this.context.UserActivityMenuHits.FirstOrDefaultAsync(q => q.UserId == dto.UserId && q.CustomerId == dto.CustomerId && q.ModuleId == dto.ModuleId && q.Key == dto.Key);
        }


        public async Task<EmployeeBookStatu> DownloadLibraryItem(int employeeId, int itemId)
        {
            var status = await this.context.EmployeeBookStatus.FirstOrDefaultAsync(q => q.EmployeeId == employeeId && q.BookId == itemId);
            if (status == null)
            {
                status = new EmployeeBookStatu()
                {
                    BookId = itemId,
                    DateVisit = DateTime.Now,
                    EmployeeId = employeeId,
                    IsVisited = true,
                    IsDownloaded = true,
                    DateDownload=DateTime.Now,

                };
                this.context.EmployeeBookStatus.Add(status);
            }
            if (!status.IsVisited)
            {
                status.IsVisited = true;
                status.DateVisit = DateTime.Now;
            }
            if (!status.IsDownloaded)
            {
                status.IsDownloaded = true;
                status.DateDownload = DateTime.Now;
            }
            return status;
            // return await this.context.UserActivityMenuHits.FirstOrDefaultAsync(q => q.UserId == dto.UserId && q.CustomerId == dto.CustomerId && q.ModuleId == dto.ModuleId && q.Key == dto.Key);
        }

        public virtual CustomActionResult Validate(ViewModels.UserActivityX dto)
        {
            //var exist = dbSet.FirstOrDefault(q => q.Id != dto.Id && q.Title.ToLower().Trim() == dto.Title.ToLower().Trim());
            //if (exist != null)
            //    return Exceptions.getDuplicateException("Organization-01", "Title");

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
    }
}