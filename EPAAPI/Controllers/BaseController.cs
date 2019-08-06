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

namespace EPAAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseController : ODataController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("odata/base/caotypes")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewCaoType> GetCaoTypes()
        {
            try
            {
                return unitOfWork.ViewCaoTypeRepository.GetQuery();
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/base/airlines")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<Organization> GetAirlines()
        {
            try
            {
                return unitOfWork.ViewOrganizationRepository.GetQuery().Where(q=>q.TypeId==58);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        [Route("odata/base/ratingorganization")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<Organization> GetRatingOrganizations()
        {
            try
            {
                return unitOfWork.ViewOrganizationRepository.GetQuery().Where(q => q.TypeId == 58);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/base/publishers")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewOrganization> GetPublishers()
        {
            try
            {
                return unitOfWork.OrganizationRepository.GetViewOrganization().Where(q => q.TypeId == 77);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

       

       

        [Route("odata/base/currencies")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<Currency> GetCurrencies()
        {
            try
            {
                return unitOfWork.ViewCurrencyRepository.GetQuery();
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        [Route("odata/base/studyfields/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewStudyField> GetViewStudyField(int cid)
        {
            try
            {
                return unitOfWork.StudyFieldRepository.GetQuery().Where(q => q.CustomerId == cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/base/posts/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewPost> GetViewPost(int cid)
        {
            try
            {
                return unitOfWork.PostRepository.GetQuery().Where(q => q.CustomerId == cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        ///JobGroups
        [Route("odata/base/jobgroups/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewJobGroup> GetJobGroups(int cid)
        {
            try
            {
                return unitOfWork.ViewJobGroupRepository.GetQuery().Where(q=>q.CustomerId==cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        [Route("odata/base/jobgroups/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteJobGroup(ViewModels.Location dto)
        {
            var entity = await unitOfWork.JobgroupRepository.GetByID(dto.Id);

            if (entity == null)
            {
                return NotFound();
            }



          //  var canDelete = unitOfWork.JobgroupRepository.CanDelete(entity);
          //  if (canDelete.Code != HttpStatusCode.OK)
          //      return canDelete;

            unitOfWork.JobgroupRepository.Delete(entity);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            return Ok(dto);
        }
        [Route("odata/base/jobgroups/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostJobGroup(ViewModels.JobGroup dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.JobgroupRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            JobGroup entity = null;
            var oldFullCode = string.Empty;
            if (dto.Id == -1)
            {
                entity = new JobGroup();
                unitOfWork.JobgroupRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.JobgroupRepository.GetByID(dto.Id);
                oldFullCode = entity.FullCode;

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.JobGroup.Fill(entity, dto);

            if (dto.Id != -1 && entity.FullCode != oldFullCode)
                unitOfWork.JobgroupRepository.UpdateChildren(entity, entity.FullCode);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            dto.Id = entity.Id;
            return Ok(dto);
        }
        ////////////////////////
        [Route("odata/organizations/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostOrganization(ViewModels.Organization dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.OrganizationRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            Organization entity = null;

            if (dto.Id == -1)
            {
                entity = new Organization();
                unitOfWork.OrganizationRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.OrganizationRepository.GetByID(dto.Id);

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.Organization.Fill(entity, dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            dto.Id = entity.Id;
            return Ok(dto);
        }

        [Route("odata/organizations/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteOrganization(ViewModels.Organization dto)
        {
            var entity = await unitOfWork.OrganizationRepository.GetByID(dto.Id);

            if (entity == null)
            {
                return NotFound();
            }



            var canDelete = unitOfWork.OrganizationRepository.CanDelete(entity);
            if (canDelete.Code != HttpStatusCode.OK)
                return canDelete;

            unitOfWork.OrganizationRepository.Delete(entity);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            return Ok(dto);
        }
        
        ///////// Journal ///////////////
         [Route("odata/base/journals")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewJournal> GetJournals()
        {
            try
            {
                return unitOfWork.JournalRepository.GetViewJournal().Where(q=>q.TypeId==1);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        [Route("odata/base/conferences")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewJournal> GetConferences()
        {
            try
            {
                return unitOfWork.JournalRepository.GetViewJournal().Where(q => q.TypeId == 2);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        [Route("odata/journals/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostJournal(ViewModels.JournalX dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.JournalRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            Journal entity = null;

            if (dto.Id == -1)
            {
                entity = new Journal();
                unitOfWork.JournalRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.JournalRepository.GetByID(dto.Id);

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.JournalX.Fill(entity, dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            dto.Id = entity.Id;
            return Ok(dto);
        }

        [Route("odata/journals/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteJournal(ViewModels.JournalX dto)
        {
            var entity = await unitOfWork.JournalRepository.GetByID(dto.Id);

            if (entity == null)
            {
                return NotFound();
            }



            var canDelete = unitOfWork.JournalRepository.CanDelete(entity);
            if (canDelete.Code != HttpStatusCode.OK)
                return canDelete;

            unitOfWork.JournalRepository.Delete(entity);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            return Ok(dto);
        }


        ////////////////////////////


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
