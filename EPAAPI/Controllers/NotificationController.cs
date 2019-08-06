using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using EPAAPI.Models;
using System.Web.Http.Description;
using System.Collections.Generic;
using System;
using System.Data.Entity.Validation;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using EPAAPI.DAL;
using System.Text;
using System.Configuration;

namespace EPAAPI.Controllers
{
    public class NotificationController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("odata/notifications/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewNotification> GetNotificationsByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.NotificationRepository.GetViewViewNotification().Where(q =>   q.CustomerId == cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/notifications/employee/{eid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewNotification> GetNotificationsByEmployeeId(int eid)
        {
            try
            {
                return unitOfWork.NotificationRepository.GetViewViewNotification().Where(q => q.UserId == eid).OrderByDescending(q=>q.DateSent);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/notifications/module/{cid}/{mid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewNotification> GetModuleNotificationsByCustomerId(int cid,int mid)
        {
            try
            {
                return unitOfWork.NotificationRepository.GetViewViewNotification().Where(q => q.CustomerId == cid && q.ModuleId==mid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/notification/{id}")]

        // [Authorize]
        public async Task<Models.ViewNotification> GetNotification(int  id)
        {
            try
            {
                return await unitOfWork.NotificationRepository.GetNotification(id);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }



        [Route("odata/notifications/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostNotification(ViewModels.NotificationX dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.NotificationRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;
            int c = 0;
            foreach(var x in dto.Employees)
            {
                var entity = new Notification();
                unitOfWork.NotificationRepository.Insert(entity);
                ViewModels.NotificationX.Fill(entity, dto,x);
                if (dto.Names!=null && dto.Names.Count > 0)
                {
                    var name = dto.Names[c];
                    entity.Message=entity.Message.Replace("[#Name]", name);
                }

                c++;
                

            }

            
           

             
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
 

             
            return Ok(dto);
        }

    }
}
