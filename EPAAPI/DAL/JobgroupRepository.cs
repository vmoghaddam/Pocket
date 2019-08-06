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
    public class JobgroupRepository : GenericRepository<Models.JobGroup>
    {
        public JobgroupRepository(EPAGRIFFINEntities context)
      : base(context)
        {
        }

        public IQueryable<ViewJobGroup> GetViewJobGroup()
        {
            return this.GetQuery<ViewJobGroup>();
        }
         

        
        public virtual CustomActionResult Validate(ViewModels.JobGroup dto)
        {
            var exist = dbSet.FirstOrDefault(q => q.Id != dto.Id && q.CustomerId==dto.CustomerId && q.Title.ToLower().Trim() == dto.Title.ToLower().Trim());
             if (exist != null)
                 return Exceptions.getDuplicateException("JobGroup-01", "Title");
             var fullcode= dbSet.FirstOrDefault(q => q.Id != dto.Id && q.CustomerId == dto.CustomerId && q.FullCode == dto.FullCode);
            if (fullcode != null)
                return Exceptions.getDuplicateException("JobGroup-02", "FullCode");

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public virtual CustomActionResult CanDelete(Models.JobGroup entity)
        {
            
                var employees = this.context.PersonCustomers.Count(q => q.GroupId == entity.Id);
                if (employees > 0)
                    return Exceptions.getCanNotDeleteException("JobGroup-03:Employees found");

            var children = this.context.JobGroups.Count(q => q.ParentId == entity.Id);
            if (children > 0)
                return Exceptions.getCanNotDeleteException("JobGroup-04:Children found");
            var books = this.context.BookRelatedGroups.Count(q => q.GroupId == entity.Id);
            if (books > 0)
                return Exceptions.getCanNotDeleteException("JobGroup-03:Library item found");
            var courses = this.context.CourseRelatedGroups.Count(q => q.GroupId == entity.Id);
            if (courses > 0)
                return Exceptions.getCanNotDeleteException("JobGroup-03:Course found");
            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public virtual void UpdateChildren(Models.JobGroup entity, string fullcode)
        {
            var children = dbSet.Where(q => q.ParentId == entity.Id).ToList();
            foreach (var x in children)
            {

                x.FullCode = fullcode + x.Code;
                UpdateChildren(x, x.FullCode);
            }

        }
    }
}