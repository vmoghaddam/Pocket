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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LibraryController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("odata/library/books/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewBook> GetBooksByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.BookRepository.GetViewBooks().Where(q => (q.CustomerId == null || q.CustomerId == cid) && q.TypeId!=86);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/library/exposed/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewBook> GetExposedLibraryByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.BookRepository.GetViewBooks().Where(q => (q.CustomerId == null || q.CustomerId == cid) && q.IsExposed==1 ).OrderByDescending(q=>q.DateExposure);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/library/documents/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewBook> GetDocumentsByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.BookRepository.GetViewBooks().Where(q => (q.CustomerId == null || q.CustomerId == cid) && q.TypeId == 86);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/library/books/applicable/employees/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewBookApplicableEmployeeAb> GetBooksApplicableEmployees(int id)
        {
            try
            {
                return unitOfWork.BookRepository.GetViewBookApplicableEmployeeAb().Where(q => q.BookId==id);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        //GetBookFileIrls
        [AcceptVerbs("POST")]
        [Route("odata/library/books/file/{id}")]
         
        // [Authorize]
        public IHttpActionResult GetBookFileUrls(int id)
        {
            try
            {
                var files= unitOfWork.BookRepository.GetBookFileUrls(id);
                return Ok(files);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }


        [Route("odata/library/book/expose")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostExposeBook(ViewModels.BookExpose dto)
        {
            var result = await unitOfWork.BookRepository.ExposeBook(dto);
            if (result)
            {
                var saveResult = await unitOfWork.SaveAsync();
                if (saveResult.Code != HttpStatusCode.OK)
                    return saveResult;


                return Ok(dto);
            }

            else
                return InternalServerError();
        }


        [Route("odata/library/keywords")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<string> GetKeywords()
        {
            try
            {
                return unitOfWork.BookRepository.GetKeywords();
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/library/book/{id}")]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            var course = await unitOfWork.BookRepository.GetBookDto(id);
            return Ok(course);
        }

        [Route("odata/library/book/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostBook(ViewModels.BookDto dto)
        {
            // return Ok(client);
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return Exceptions.getModelValidationException(ModelState);
            }
            var validate = unitOfWork.BookRepository.Validate(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            Book entity = null;

            if (dto.Id == -1)
            {
                entity = new Book();
                unitOfWork.BookRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.BookRepository.GetByID(dto.Id);

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.BookDto.Fill(entity, dto);
            

            unitOfWork.BookRepository.FillBookRelatedAircraftTypes(entity, dto);

            unitOfWork.BookRepository.FillBookRelatedEmployees(entity, dto);
            unitOfWork.BookRepository.FillBookRelatedGroups(entity, dto);
            unitOfWork.BookRepository.FillBookRelatedStudyFields(entity, dto);
            unitOfWork.BookRepository.FillBookAuthors(entity, dto);
            unitOfWork.BookRepository.FillBookKeywords(entity, dto);
            unitOfWork.BookRepository.FillBookFiles(entity, dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            string webUrl = ConfigurationManager.AppSettings["web"] + "downloadhandler.ashx?t=bookarchive&id=" + entity.Id;
            object input = new
            {

            };
            string inputJson =Newtonsoft.Json.JsonConvert.SerializeObject(input);
            //string inputJson = (new JavaScriptSerializer()).Serialize(input);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string json = client.UploadString(webUrl, inputJson);


            dto.Id = entity.Id;
            return Ok(dto);
        }






    }
}
