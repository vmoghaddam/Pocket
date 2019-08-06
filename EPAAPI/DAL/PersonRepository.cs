using EPAAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace EPAAPI.DAL
{
    public class PersonRepository : GenericRepository<Person>
    {
        public PersonRepository(EPAGRIFFINEntities context)
         : base(context)
        {
        }
        public virtual async Task<Person> GetPersonByNID(string nid)
        {
            return await dbSet.FirstOrDefaultAsync(q => q.NID == nid);
        }
        public virtual async Task<Person> GetPersonById(int id)
        {
            return await dbSet.FirstOrDefaultAsync(q => q.Id == id);
        }

        public virtual async Task<PersonCustomer> GetPersonCustomer(int cid, int pid)
        {
            return await this.context.PersonCustomers.FirstOrDefaultAsync(q => q.CustomerId == cid && q.PersonId == pid);
        }
        public virtual async Task<Employee> GetEmployee(int eid)
        {
            return await this.context.Employees.FirstOrDefaultAsync(q => q.Id == eid);
        }

        public virtual async Task<ViewEmployee> GetViewEmployeesByUserId(string userid, int cid)
        {
            return await this.context.ViewEmployees.FirstOrDefaultAsync(q => q.UserId == userid && q.CustomerId == cid);
        }
        

        public IQueryable<ViewEmployee> GetViewEmployees()
        {
            return this.GetQuery<ViewEmployee>();
        }

        public IQueryable<ViewEmployeeLight> GetViewEmployeesLight()
        {
            return this.GetQuery<ViewEmployeeLight>();
        }
        public IQueryable<ViewCrew> GetViewCrews()
        {
            return this.GetQuery<ViewCrew>();
        }
       


        public IQueryable<ViewPersonAircraftType> GetViewPersonAircraftType()
        {
            return this.GetQuery<ViewPersonAircraftType>();
        }

        internal async Task<ViewModels.Employee> GetEmployeeDtoByNID(string nid, int cid)
        {
            ViewModels.Employee employee = null;
            var entity = await this.context.People.SingleOrDefaultAsync(q => q.NID == nid && !q.IsDeleted);
            if (entity == null)
                return null;
            employee = new ViewModels.Employee();
            employee.Person = new ViewModels.Person();
            ViewModels.Person.FillDto(entity, employee.Person);
            var actypes = await context.ViewPersonAircraftTypes.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.AircraftTypes = ViewModels.PersonAircraftType.GetDtos(actypes);
            var educations = await context.ViewPersonEducations.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Educations = ViewModels.PersonEducation.GetDtos(educations);
            var exp = await context.ViewPersonExperienses.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Expreienses = ViewModels.PersonExperiense.GetDtos(exp);
            var rating = await context.ViewPersonRatings.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Ratings = ViewModels.PersonRating.GetDtos(rating);
            var doc = await context.ViewPersonDocuments.Where(q => q.PersonId == entity.Id).ToListAsync();
            var docIds = doc.Select(q => q.Id).ToList();
            var files = await context.ViewPersonDocumentFiles.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Documents = ViewModels.PersonDocument.GetDtos(doc, files);
            var certificates = await this.context.ViewCertificates.Where(q => q.PersonId == entity.Id && q.IsLast == 1).OrderBy(q => q.ExpireStatus).ThenBy(q => q.Remain).ToListAsync();
            employee.Person.Certificates = certificates;
            var pc = context.PersonCustomers.SingleOrDefault(q => q.CustomerId == cid && q.PersonId == entity.Id && !q.IsDeleted);

            if (pc != null)
            {
                var emp = await context.Employees.FirstOrDefaultAsync(q => q.Id == pc.Id);
                if (emp != null)
                {
                    employee.CustomerId = cid;
                    employee.DateActiveEnd = pc.DateActiveEnd;
                    employee.DateActiveStart = pc.DateActiveStart;
                    employee.DateJoinCompany = pc.DateJoinCompany;
                    employee.DateJoinCompanyP = pc.DateJoinCompanyP;
                    employee.DateConfirmedP = pc.DateConfirmedP;
                    employee.DateConfirmed = pc.DateConfirmed;
                    employee.DateLastLogin = pc.DateLastLogin;
                    employee.DateLastLoginP = pc.DateLastLoginP;
                    employee.DateRegister = pc.DateRegister;
                    employee.DateRegisterP = pc.DateRegisterP;
                    employee.Id = pc.Id;
                    employee.IsActive = pc.IsActive;
                    employee.Password = pc.Password;
                    employee.PersonId = entity.Id;
                    employee.GroupId = pc.GroupId;
                    employee.PID = emp.PID;
                    employee.Phone = emp.Phone;
                    var locs = await context.ViewEmployeeLocations.Where(q => q.EmployeeId == pc.Id).ToListAsync();
                    employee.Locations = ViewModels.EmployeeLocation.GetDtos(locs);


                }


            }



            return employee;
        }

        internal async Task<ViewModels.Employee> GetEmployeeViewDtoByNID(string nid, int cid)
        {
            ViewModels.Employee employee = null;
            var entity = await this.context.People.SingleOrDefaultAsync(q => q.NID == nid && !q.IsDeleted);
            if (entity == null)
                return null;
            employee = new ViewModels.Employee();
            employee.Person = new ViewModels.Person();
            ViewModels.Person.FillDto(entity, employee.Person);
            var actypes = await context.ViewPersonAircraftTypes.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.AircraftTypes = ViewModels.PersonAircraftType.GetDtos(actypes);
            var educations = await context.ViewPersonEducations.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Educations = ViewModels.PersonEducation.GetDtos(educations);
            var exp = await context.ViewPersonExperienses.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Expreienses = ViewModels.PersonExperiense.GetDtos(exp);
            var rating = await context.ViewPersonRatings.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Ratings = ViewModels.PersonRating.GetDtos(rating);
            var doc = await context.ViewPersonDocuments.Where(q => q.PersonId == entity.Id).ToListAsync();
            var docIds = doc.Select(q => q.Id).ToList();
            var files = await context.ViewPersonDocumentFiles.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Documents = ViewModels.PersonDocument.GetDtos(doc, files);


            var certificates=await this.context.ViewCertificates.Where(q => q.PersonId == entity.Id && q.IsLast == 1).OrderBy(q => q.ExpireStatus).ThenBy(q => q.Remain).ToListAsync();
            employee.Person.Certificates = certificates;

            var courses = await this.context.ViewPersonActiveCourses.Where(q => q.PersonId == entity.Id).OrderBy(q => q.StatusId).ToListAsync();
            employee.Person.Courses = courses;


            var pc = context.PersonCustomers.SingleOrDefault(q => q.CustomerId == cid && q.PersonId == entity.Id && !q.IsDeleted);

            if (pc != null)
            {
                var emp = await context.Employees.FirstOrDefaultAsync(q => q.Id == pc.Id);
                if (emp != null)
                {
                    employee.CustomerId = cid;
                    employee.DateActiveEnd = pc.DateActiveEnd;
                    employee.DateActiveStart = pc.DateActiveStart;
                    employee.DateJoinCompany = pc.DateJoinCompany;
                    employee.DateJoinCompanyP = pc.DateJoinCompanyP;
                    employee.DateConfirmedP = pc.DateConfirmedP;
                    employee.DateConfirmed = pc.DateConfirmed;
                    employee.DateLastLogin = pc.DateLastLogin;
                    employee.DateLastLoginP = pc.DateLastLoginP;
                    employee.DateRegister = pc.DateRegister;
                    employee.DateRegisterP = pc.DateRegisterP;
                    employee.Id = pc.Id;
                    employee.IsActive = pc.IsActive;
                    employee.Password = pc.Password;
                    employee.PersonId = entity.Id;
                    employee.GroupId = pc.GroupId;
                    employee.PID = emp.PID;
                    employee.Phone = emp.Phone;
                    var locs = await context.ViewEmployeeLocations.Where(q => q.EmployeeId == pc.Id).ToListAsync();
                    employee.Locations = ViewModels.EmployeeLocation.GetDtos(locs);


                }


            }



            return employee;
        }


        internal async Task<object> GetEmployeeDtoByID(int id)
        {
            var employee = await this.context.ViewEmployees.SingleOrDefaultAsync(q => q.Id == id);
            if (employee == null)
                return null;
            var dto = new
            {
                Employee=employee
            };

            return dto;
            //ViewModels.Employee employee = null;
            //var entity = await this.context.People.SingleOrDefaultAsync(q => q.NID == nid && !q.IsDeleted);
            //if (entity == null)
            //    return null;
            //employee = new ViewModels.Employee();
            //employee.Person = new ViewModels.Person();
            //ViewModels.Person.FillDto(entity, employee.Person);
            //var actypes = await context.ViewPersonAircraftTypes.Where(q => q.PersonId == entity.Id).ToListAsync();
            //employee.Person.AircraftTypes = ViewModels.PersonAircraftType.GetDtos(actypes);
            //var educations = await context.ViewPersonEducations.Where(q => q.PersonId == entity.Id).ToListAsync();
            //employee.Person.Educations = ViewModels.PersonEducation.GetDtos(educations);
            //var exp = await context.ViewPersonExperienses.Where(q => q.PersonId == entity.Id).ToListAsync();
            //employee.Person.Expreienses = ViewModels.PersonExperiense.GetDtos(exp);
            //var rating = await context.ViewPersonRatings.Where(q => q.PersonId == entity.Id).ToListAsync();
            //employee.Person.Ratings = ViewModels.PersonRating.GetDtos(rating);
            //var doc = await context.ViewPersonDocuments.Where(q => q.PersonId == entity.Id).ToListAsync();
            //var docIds = doc.Select(q => q.Id).ToList();
            //var files = await context.ViewPersonDocumentFiles.Where(q => q.PersonId == entity.Id).ToListAsync();
            //employee.Person.Documents = ViewModels.PersonDocument.GetDtos(doc, files);

            //var pc = context.PersonCustomers.SingleOrDefault(q => q.CustomerId == cid && q.PersonId == entity.Id && !q.IsDeleted);

            //if (pc != null)
            //{
            //    var emp = await context.Employees.FirstOrDefaultAsync(q => q.Id == pc.Id);
            //    if (emp != null)
            //    {
            //        employee.CustomerId = cid;
            //        employee.DateActiveEnd = pc.DateActiveEnd;
            //        employee.DateActiveStart = pc.DateActiveStart;
            //        employee.DateJoinCompany = pc.DateJoinCompany;
            //        employee.DateJoinCompanyP = pc.DateJoinCompanyP;
            //        employee.DateConfirmedP = pc.DateConfirmedP;
            //        employee.DateConfirmed = pc.DateConfirmed;
            //        employee.DateLastLogin = pc.DateLastLogin;
            //        employee.DateLastLoginP = pc.DateLastLoginP;
            //        employee.DateRegister = pc.DateRegister;
            //        employee.DateRegisterP = pc.DateRegisterP;
            //        employee.Id = pc.Id;
            //        employee.IsActive = pc.IsActive;
            //        employee.Password = pc.Password;
            //        employee.PersonId = entity.Id;
            //        employee.GroupId = pc.GroupId;
            //        employee.PID = emp.PID;
            //        employee.Phone = emp.Phone;
            //        var locs = await context.ViewEmployeeLocations.Where(q => q.EmployeeId == pc.Id).ToListAsync();
            //        employee.Locations = ViewModels.EmployeeLocation.GetDtos(locs);


            //    }


            //}



             
        }

        public void FillEmployeeLocations(Employee employee, ViewModels.Employee dto)
        {
            var exists = this.context.EmployeeLocations.Where(q => q.EmployeeId == employee.Id).ToList();
            var dtoLocation = dto.Locations.First();
            if (exists == null || exists.Count == 0)
            {
                employee.EmployeeLocations.Add(new EmployeeLocation()
                {
                    DateActiveEnd = dtoLocation.DateActiveEnd,
                    DateActiveEndP = dtoLocation.DateActiveEnd != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveEnd)) : null,
                    DateActiveStart = dtoLocation.DateActiveStart,
                    DateActiveStartP = dtoLocation.DateActiveStart != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveStart)) : null,
                    IsMainLocation = dtoLocation.IsMainLocation,
                    LocationId = dtoLocation.LocationId,
                    OrgRoleId = dtoLocation.OrgRoleId,
                    Phone = dtoLocation.Phone,
                    Remark = dtoLocation.Remark

                });
            }
            else
            {
                exists[0].DateActiveEnd = dtoLocation.DateActiveEnd;
                exists[0].DateActiveEndP = dtoLocation.DateActiveEnd != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveEnd)) : null;
                exists[0].DateActiveStart = dtoLocation.DateActiveStart;
                exists[0].DateActiveStartP = dtoLocation.DateActiveStart != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveStart)) : null;
                exists[0].IsMainLocation = dtoLocation.IsMainLocation;
                exists[0].LocationId = dtoLocation.LocationId;
                exists[0].OrgRoleId = dtoLocation.OrgRoleId;
                exists[0].Phone = dtoLocation.Phone;
                exists[0].Remark = dtoLocation.Remark;
            }

        }

        public void FillAircraftTypes(Person person, ViewModels.Employee dto)
        {
            var existing = this.context.PersonAircraftTypes.Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.AircraftTypes
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonAircraftTypes.Remove(x);
            foreach (var x in added)
                context.PersonAircraftTypes.Add(new PersonAircraftType()
                {
                    Person = person,
                    AircraftTypeId = x.AircraftTypeId,
                    IsActive = x.IsActive,
                    Remark = x.Remark,
                    DateLimitBegin = x.DateLimitBegin,
                    DateLimitEnd = x.DateLimitEnd

                });
            foreach (var x in edited)
            {
                var item = dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.AircraftTypeId = item.AircraftTypeId;
                    x.DateLimitBegin = item.DateLimitBegin;
                    x.DateLimitEnd = item.DateLimitEnd;
                    x.IsActive = item.IsActive;
                    x.Remark = item.Remark;

                }
            }
        }
        public void FillEducations(Person person, ViewModels.Employee dto)
        {
            var existing = this.context.PersonEducations.Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.Educations.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.Educations
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.Educations.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonEducations.Remove(x);
            foreach (var x in added)
                context.PersonEducations.Add(new PersonEducation()
                {
                    Person = person,
                    Remark = x.Remark,
                    College = x.College,
                    DateCatch = x.DateCatch,
                    EducationDegreeId = x.EducationDegreeId,
                    StudyFieldId = x.StudyFieldId,
                    Title = x.Title,
                     FileTitle=x.FileTitle,
                      FileType=x.FileType,
                       FileUrl=x.FileUrl,
                        SysUrl=x.SysUrl,

                });
            foreach (var x in edited)
            {
                var item = dto.Person.Educations.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.College = item.College;
                    x.DateCatch = item.DateCatch;
                    x.EducationDegreeId = item.EducationDegreeId;
                    x.StudyFieldId = item.StudyFieldId;
                    x.Remark = item.Remark;
                    x.FileTitle = item.FileTitle;
                    x.FileType = item.FileType;
                    x.FileUrl = item.FileUrl;
                    x.SysUrl = item.SysUrl;

                }
            }
        }

        public void FillExps(Person person, ViewModels.Employee dto)
        {
            var existing = this.context.PersonExperienses.Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.Expreienses.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.Expreienses
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.Expreienses.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonExperienses.Remove(x);
            foreach (var x in added)
                context.PersonExperienses.Add(new PersonExperiense()
                {
                    Person = person,
                    Remark = x.Remark,
                    AircraftTypeId = x.AircraftTypeId,
                    DateEnd = x.DateEnd,
                    DateStart = x.DateStart,
                    Employer = x.Employer,
                    JobTitle = x.JobTitle,
                    Organization = x.Organization,
                    OrganizationId = x.OrganizationId,


                });
            foreach (var x in edited)
            {
                var item = dto.Person.Expreienses.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.AircraftTypeId = item.AircraftTypeId;
                    x.DateEnd = item.DateEnd;
                    x.DateStart = item.DateStart;
                    x.Employer = item.Employer;
                    x.JobTitle = item.JobTitle;
                    x.Organization = item.Organization;
                    x.OrganizationId = item.OrganizationId;
                    x.Remark = item.Remark;

                }
            }
        }

        public void FillRatings(Person person, ViewModels.Employee dto)
        {
            var existing = this.context.PersonRatings.Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.Ratings.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.Ratings
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.Ratings.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonRatings.Remove(x);
            foreach (var x in added)
                context.PersonRatings.Add(new PersonRating()
                {
                    Person = person,

                    AircraftTypeId = x.AircraftTypeId,

                    OrganizationId = x.OrganizationId,
                    CategoryId = x.CategoryId,
                    DateExpire = x.DateExpire,
                    DateIssue = x.DateIssue,
                    RatingId = x.RatingId,

                });
            foreach (var x in edited)
            {
                var item = dto.Person.Ratings.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.AircraftTypeId = item.AircraftTypeId;

                    x.OrganizationId = item.OrganizationId;
                    x.CategoryId = item.CategoryId;
                    x.DateExpire = item.DateExpire;
                    x.DateIssue = item.DateIssue;
                    x.RatingId = item.RatingId;

                }
            }
        }
        public void FillDocuments(Person person, ViewModels.Employee dto)
        {
            var existing = this.context.PersonDocuments.Include("Documents").Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.Documents
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonDocuments.Remove(x);
            foreach (var x in added)
            {
                var pd = new PersonDocument()
                {
                    Person = person,

                    Remark = x.Remark,
                    DocumentTypeId = x.DocumentTypeId,
                    Title = x.Title,


                };
                foreach (var file in x.Documents)
                {
                    pd.Documents.Add(new Document()
                    {
                        FileType = file.FileType,
                        FileUrl = file.FileUrl,
                        SysUrl = file.SysUrl,
                        Title = file.Title,

                    });
                }
                context.PersonDocuments.Add(pd);
            }
            foreach (var x in edited)
            {
                var item = dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.DocumentTypeId = item.DocumentTypeId;
                    x.Title = item.Title;
                    x.Remark = item.Remark;
                    //bani
                    while (x.Documents.Count > 0)
                    {
                        var f = x.Documents.First();
                        this.context.Documents.Remove(f);
                    }
                    foreach (var f in item.Documents)
                        x.Documents.Add(new Document()
                        {
                            FileType = f.FileType,
                            FileUrl = f.FileUrl,
                            SysUrl = f.SysUrl,
                            Title = f.Title,

                        });
                }
            }
        }

        public virtual CustomActionResult Validate(ViewModels.Employee dto)
        {
            //return Exceptions.getDuplicateException("Location-01", "Title");
            // var c = dbSet.FirstOrDefault(q => q.Id != dto.Id && q.DateStart == dto.DateStart && q.CourseTypeId == dto.CourseTypeId && q.OrganizationId == dto.OrganizationId);
            var c = dbSet.FirstOrDefault(q => q.Id != dto.Person.PersonId && q.NID == dto.Person.NID);
            if (c != null)
                return Exceptions.getDuplicateException("Person-01", "NID");
            if (!string.IsNullOrEmpty(dto.Person.IDNo))
            {
                var idno = dbSet.FirstOrDefault(q => q.Id != dto.Person.PersonId && q.IDNo == dto.Person.IDNo);
                if (idno != null)
                    return Exceptions.getDuplicateException("Person-02", "IDNo");
            }


            var pc = context.ViewEmployees.FirstOrDefault(q => q.CustomerId == dto.CustomerId && q.Id != dto.Id && q.PID == dto.PID);
            if (pc != null)
                return Exceptions.getDuplicateException("Employee-01", "PID");

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
    }
}