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
    public class PersonController : ODataController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("odata/employees/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewEmployee> GetEmployeesByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.PersonRepository.GetViewEmployees().Where(q => q.CustomerId == cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/light/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewEmployeeLight> GetEmployeesLightByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.PersonRepository.GetViewEmployeesLight().Where(q => q.CustomerId == cid);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }


        [Route("odata/employees/group/{groupId}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewEmployee> GetEmployeesByGroupId(int groupId)
        {
            try
            {
                return unitOfWork.PersonRepository.GetViewEmployees().Where(q => q.GroupId==groupId);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/group/code/{code}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewEmployee> GetEmployeesByGroupCode(string code)
        {
            try
            {
                return unitOfWork.PersonRepository.GetViewEmployees().Where(q => q.JobGroupCode.StartsWith(code));
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }


        [Route("odata/authors")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewPersonMisc> GetAuthors( )
        {
            try
            {
                return unitOfWork.PersonMiscRepository.GetViewPersonMisc().Where(q => q.TypeId == 75);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/activecourses/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewPersonActiveCourse> GetEmployeesActiveCourses(int id)
        {
            try
            {
                return unitOfWork.CourseRepository.GetViewPersonActiveCourse().Where(q => q.PersonId == id).OrderBy(q=>q.StatusId);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
       
        [Route("odata/employees/pendingcourses/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewPersonActiveCourse> GetEmployeesPendingCourses(int id)
        {
            try
            {
                return unitOfWork.CourseRepository.GetViewPersonActiveCourse().Where(q => q.PersonId == id && q.StatusId==null && q.CourseStatusId==1).OrderBy(q=>q.Remain);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/library/{id}/{type?}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewBookApplicableEmployee> GetEmployeesLibrary(int id,int? type=null)
        {
            try
            {
                if (type==null)
                return unitOfWork.BookRepository.GetViewBookApplicableEmployee().Where(q =>q.IsExposed==1 && q.EmployeeId == id).OrderBy(q=>q.IsVisited).ThenBy(q=>q.IsDownloaded).ThenByDescending(q=>q.DateExposure);
                else
                    return unitOfWork.BookRepository.GetViewBookApplicableEmployee().Where(q => q.IsExposed == 1 && q.EmployeeId == id && q.TypeId==(int)type).OrderBy(q => q.IsVisited).ThenBy(q => q.IsDownloaded).ThenByDescending(q => q.DateExposure);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/library/item/{id}/{itemId}")]
        public async Task<IHttpActionResult> GetBook(int id,int itemId)
        {
            var course = await unitOfWork.BookRepository.GetEmployeeBookDto(itemId, id);
            return Ok(course);
        }
       
        

        [Route("odata/employees/certificates/last/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewCertificate> GetEmployeesCertificates(int id)
        {
            //soosk
            try
            {
                return unitOfWork.CourseRepository.GetViewCertificates().Where(q => q.PersonId == id && q.IsLast==1).OrderBy(q=>q.ExpireStatus).ThenBy(q=>q.Remain);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        [Route("odata/employees/expiringcertificates/last/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewCertificate> GetEmployeesExpiringCertificates(int id)
        {
            try
            {
                return unitOfWork.CourseRepository.GetViewCertificates().Where(q => q.PersonId == id && q.IsLast == 1 && q.Remain!=null && q.Remain<=30).OrderBy(q => q.ExpireStatus).ThenBy(q => q.Remain);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }


        [Route("odata/employees/certificates/all/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewCertificate> GetEmployeesCertificatesAll(int id)
        {
            try
            {
                return unitOfWork.CourseRepository.GetViewCertificates().Where(q => q.PersonId == id  ).OrderByDescending(q=>q.IsLast).ThenBy(q => q.ExpireStatus).ThenBy(q => q.Remain).ThenByDescending(q=>q.DateIssue);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }


        [Route("odata/employee/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostEmployee(ViewModels.Employee dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.PersonRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            ///////////////////////////////////////
         //   if (dto.Person.VisaExpireDate!=null)
            //    dto.Person.VisaExpireDate=((DateTime)dto.Person.VisaExpireDate).AddHours(3).AddMinutes(30);
            ///////////////////////////////////////////
            Person person = null;
            if (dto.PersonId != -1)
                person = await unitOfWork.PersonRepository.GetPersonById(dto.PersonId);
            else
                person = await unitOfWork.PersonRepository.GetPersonByNID(dto.Person.NID);
            if (person == null)
            {
                person = new Person();
                person.DateCreate = DateTime.Now;
                unitOfWork.PersonRepository.Insert(person);
            }
            ViewModels.Person.Fill(person, dto.Person);
            PersonCustomer personCustomer = await unitOfWork.PersonRepository.GetPersonCustomer((int)dto.CustomerId, dto.Person.PersonId);
            if (personCustomer == null)
            {
                personCustomer = new PersonCustomer();
              
                person.PersonCustomers.Add(personCustomer);
            }
            ViewModels.PersonCustomer.Fill(personCustomer, dto);
            Employee employee =await unitOfWork.PersonRepository.GetEmployee(personCustomer.Id);
            if (employee == null)
                employee = new Employee();
            personCustomer.Employee = employee;
            ViewModels.Employee.Fill(employee, dto);
            unitOfWork.PersonRepository.FillEmployeeLocations(employee, dto);

            unitOfWork.PersonRepository.FillAircraftTypes(person, dto);
            unitOfWork.PersonRepository.FillDocuments(person, dto);
            unitOfWork.PersonRepository.FillEducations(person, dto);
            unitOfWork.PersonRepository.FillExps(person, dto);
            unitOfWork.PersonRepository.FillRatings(person, dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            dto.Id = employee.Id;
            return Ok(dto);



            
        }

        [Route("odata/person/misc/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostPersonMisc(ViewModels.PersonMiscellaneous dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.PersonMiscRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            PersonMisc entity = null;

            if (dto.Id == -1)
            {
                entity = new PersonMisc();
                unitOfWork.PersonMiscRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.PersonMiscRepository.GetByID(dto.Id);

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.PersonMiscellaneous.Fill(entity, dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            dto.Id = entity.Id;
            return Ok(dto);
        }

        [Route("odata/person/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeletePerson(dynamic dto)
        {
            var PersonId = Convert.ToInt32(dto.PersonId);
            var entity = await unitOfWork.PersonRepository.GetByID(PersonId);

            if (entity == null)
            {
                return NotFound();
            }



           

            unitOfWork.PersonRepository.Delete(entity);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
            {
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "The selected item can not be deleted because of its related logs in Flight Pockect system.");
               // return saveResult;
            }

            return Ok(dto);
        }


        [Route("odata/person/misc/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeletePersonMisc(ViewModels.PersonMiscellaneous dto)
        {
            var entity = await unitOfWork.PersonMiscRepository.GetByID(dto.Id);

            if (entity == null)
            {
                return NotFound();
            }



            var canDelete = unitOfWork.PersonMiscRepository.CanDelete(entity);
            if (canDelete.Code != HttpStatusCode.OK)
                return canDelete;

            unitOfWork.PersonMiscRepository.Delete(entity);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            return Ok(dto);
        }

        [Route("odata/employee/nid/{cid}/{nid}")]
        public async Task<IHttpActionResult> GetEmployee(string nid,int cid)
        {
            //soosk
            var employee = await unitOfWork.PersonRepository.GetEmployeeDtoByNID(nid,cid);
            return Ok(employee);
        }

        [Route("odata/employee/view/nid/{cid}/{nid}")]
        public async Task<IHttpActionResult> GetEmployeeForView(string nid, int cid)
        {
            //soosk
            var employee = await unitOfWork.PersonRepository.GetEmployeeViewDtoByNID(nid, cid);
            return Ok(employee);
        }

        [Route("odata/employee/{id}")]
        public async Task<IHttpActionResult> GetEmployeeById(int id)
        {
            var employee = await unitOfWork.PersonRepository.GetEmployeeDtoByID(id);
            return Ok(employee);
        }

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
