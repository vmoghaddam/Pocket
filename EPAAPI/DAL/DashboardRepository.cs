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
    public class DashboardRepository : Repository
    {
        public DashboardRepository(EPAGRIFFINEntities context)
               : base(context)
        {

        }

        public async Task<DashboardLibrary> GetDashboardLibrary(int cid)
        {
            var item = new DashboardLibrary();
            item.Publishers = await this.context.Organizations.CountAsync(q => q.TypeId == 77);
            item.Authors = await this.context.PersonMiscs.CountAsync(q => q.TypeId == 75);
            item.Journals = await this.context.Journals.CountAsync();
            //////////////////////////////////////////////////////////
            var libraryGroup = await (from x in context.ViewBooks
                                      where x.CustomerId == cid
                                      group x by new { x.TypeId, x.IsExposed } into g

                                      select new
                                      {
                                          TypeId = g.Key.TypeId,
                                          Exposed = g.Key.IsExposed,
                                          count = g.Count()
                                      }).ToListAsync();

            var libraryGroupSum = (from x in libraryGroup
                                   group x by x.TypeId into g
                                   orderby g.Key
                                   select new { TypeId = g.Key, count = g.Sum(e => e.count) }).ToList();

            item.DocumentsTotal = 0;
            item.BooksTotal = 0;
            item.PapersTotal = 0;
            item.VideosTotal = 0;

            item.DocumentsNotExposed = 0;
            item.BooksNotExposed = 0;
            item.PapersNotExposed = 0;
            item.VideosNotExposed = 0;


            foreach (var x in libraryGroupSum)
            {
                var notexposed = libraryGroup.FirstOrDefault(q => q.TypeId == x.TypeId && q.Exposed == 0);
                switch (x.TypeId)
                {
                    case 83:
                        item.BooksNotExposed = notexposed == null ? 0 : notexposed.count;
                        item.BooksTotal = x.count;
                        break;
                    case 84:
                        item.PapersTotal = x.count;
                        item.PapersNotExposed = notexposed == null ? 0 : notexposed.count;
                        break;
                    case 85:
                        item.VideosNotExposed = notexposed == null ? 0 : notexposed.count;
                        item.VideosTotal = x.count;
                        break;
                    case 86:
                        item.DocumentsNotExposed = notexposed == null ? 0 : notexposed.count;
                        item.DocumentsTotal = x.count;
                        break;
                    default:
                        break;
                }



            }
            var careless = await this.context.SumCarelessEmployeeTotals.FirstOrDefaultAsync(q => q.CustomerId == cid);
            item.Careless = careless == null ? 0 : (int)careless.Count;

            var carelessTypes = await this.context.SumCarelessEmployees.Where(q => q.CustomerId == cid).ToListAsync();
            foreach (var y in carelessTypes)
            {
                switch (y.TypeId)
                {
                    case 83:
                        item.CarelessBook = (int)y.Count;
                        break;
                    case 84:
                        item.CarelessPaper = (int)y.Count;
                        break;
                    case 85:
                        item.CarelessVideo = (int)y.Count;
                        break;
                    case 86:
                        item.CarelessDocument = (int)y.Count;
                        break;
                    default:
                        break;
                }
            }

            item.DownloadTotal = await this.context.ViewBookApplicableEmployees.Where(q => q.CustomerId == cid && q.IsDownloaded).CountAsync();

            var download = await this.context.SumLibraryDownloadByMonths.Where(q => q.CustomerId == cid).OrderBy(q => q.Year).ThenBy(q => q.Month).ToListAsync();
            item.Download = new List<DateRate>();
            foreach (var x in download)
            {
                item.Download.Add(new DateRate()
                {
                    Month = x.Month,
                    MonthName = x.MonthName,
                    Year = x.Year,
                    Total = (int)x.Count
                });
            }
            var add = await this.context.SumLibraryAddedByMonths.Where(q => q.CustomerId == cid).OrderBy(q => q.Year).ThenBy(q => q.Month).ToListAsync();
            item.Add = new List<DateRate>();
            foreach (var x in add)
            {
                item.Add.Add(new DateRate()
                {
                    Month = x.Month,
                    MonthName = x.MonthName,
                    Year = x.Year,
                    Total = (int)x.Count
                });
            }
            //////////////////////////////////////////////


            return item;
            // return await this.context.UserActivityMenuHits.FirstOrDefaultAsync(q => q.UserId == dto.UserId && q.CustomerId == dto.CustomerId && q.ModuleId == dto.ModuleId && q.Key == dto.Key);
        }

        internal async Task<DashboardClienApp> GetAppDashboard(int cid, int eid)
        {
            var item = new DashboardClienApp() {
                 Library=new List<TileAlert>(),
            };
            var library = await this.context.SumEmployeeLibraryAlerts.Where(q => q.EmployeeId == eid && q.CustomerId == cid).OrderBy(q=>q.TypeId).ToListAsync();
            foreach (var x in library)
            {
                if (x.TypeId == 83 && x.Count>0)
                {
                    item.Library.Add( new TileAlert()
                    {
                        Type = 83,
                        Caption = "Book" + (x.Count > 1 ? "s" : ""),
                        Total = x.Count,
                        Remark = "need" + (x.Count == 1 ? "s" : "") + " your attention.",

                    });
                }
                if (x.TypeId == 84 && x.Count > 0)
                {
                    item.Library.Add(new TileAlert()
                    {
                        Type = 84,
                        Caption = "Paper" + (x.Count > 1 ? "s" : ""),
                        Total = x.Count,
                        Remark = "need" + (x.Count == 1 ? "s" : "") + " your attention.",

                    });
                }
                if (x.TypeId == 85 && x.Count > 0)
                {
                    item.Library.Add(new TileAlert()
                    {
                        Type = 85,
                        Caption = "Video" + (x.Count > 1 ? "s" : ""),
                        Total = x.Count,
                        Remark = "need" + (x.Count == 1 ? "s" : "") + " your attention.",

                    });
                }
                if (x.TypeId == 86 && x.Count > 0)
                {
                    item.Library.Add(new TileAlert()
                    {
                        Type = 86,
                        Caption = "Document" + (x.Count > 1 ? "s" : ""),
                        Total = x.Count,
                        Remark = "need" + (x.Count == 1 ? "s" : "") + " your attention.",

                    });
                }
            }

            var notification =await this.context.Notifications.Where(q => q.UserId == eid && q.DateAppVisited == null).CountAsync();
            var lastNote = await this.context.ViewNotifications.Where(q => q.UserId == eid && q.DateAppVisited == null).OrderByDescending(q => q.DateSent).FirstOrDefaultAsync();
            item.Nots = notification;
            if (lastNote!=null)
            {
                item.LastNot = lastNote.Type;
                item.LastNotDate = lastNote.DateSent;
                item.LastNotSender = lastNote.Sender;
                item.LastNotAbs = lastNote.Abstract;
            }
            return item;
        }

        public async Task<DashboardProfile> GetDashboardProfile(int cid)
        {
            var item = new DashboardProfile();

            item.EmployeesJobGroup = await this.context.SumEmployeeJobGroups.Where(q => q.CustomerId == cid).ToListAsync();
            item.EmployeesLocation = await this.context.SumEmployeeLocations.Where(q => q.CustomerId == cid).ToListAsync();
            item.EmployeesStudyField = await this.context.SumEmployeeStudyFields.Where(q => q.CustomerId == cid).ToListAsync();

            item.EmployeesDegree = await this.context.SumEmployeeDegrees.Where(q => q.CustomerId == cid).ToListAsync();
            item.EmployeeSex = await this.context.SumEmployeeSexes.Where(q => q.CustomerId == cid).ToListAsync();
            item.EmployeesAge = await this.context.SumEmployeeAges.Where(q => q.Id == cid).FirstOrDefaultAsync();
            item.EmployeeMaritalStatus = await this.context.SumEmployeeMaritalStatus.Where(q => q.CustomerId == cid).ToListAsync();
            item.EmployeesExp = await this.context.SumEmployeeExps.Where(q => q.Id == cid).FirstOrDefaultAsync();

            var courses = await this.context.SumActiveCourses.Where(q => q.CustomerId == cid).ToListAsync();
            foreach (var x in courses)
            {
                switch (x.StatusId)
                {
                    case 1:
                        item.RegisteringCourse = new TitleCourse()
                        {
                            Assigned = x.Assigned,
                            Registered = x.Registered,
                            UnRegistered = x.Unregistered,
                            Total = x.Count

                        };
                        break;
                    case 2:
                        item.ActiveCourse = new TitleCourse()
                        {
                            Total = x.Count,
                            Assigned = x.Assigned,
                            Learner = x.ActiveLearner,
                            Canceled = x.Canceled,
                        };
                        break;
                    case 3:
                        item.CompletedCourse = new TitleCourse()
                        {
                            Learner = x.DoneLearner,
                            Failed = x.Failed,
                            Passed = x.Passed,
                            Total = x.Count
                        };
                        break;
                    default:
                        break;
                }
            }

            var dateAlert = await this.context.SumEmployeeDateAlerts.FirstOrDefaultAsync(q => q.Id == cid);
            item.Passport = new TileAlert() { Expired = dateAlert.PassportExpired, Expiring = dateAlert.PassportExpiring };
            item.NDT = new TileAlert() { Expiring = dateAlert.NDTExpiring, Expired = dateAlert.NDTExpired };
            item.CAO = new TileAlert() { Expired = dateAlert.CAOExpired, Expiring = dateAlert.CAOExpiring };
            item.Medical = new TileAlert() { Expired = dateAlert.MedicalExpired, Expiring = dateAlert.MedicalExpiring };

            var certificate = await this.context.SumCertificateStatus.FirstOrDefaultAsync(q => q.Id == cid);
            item.Certificate = new TileAlert() { Expired = certificate.Expired, Expiring = certificate.Expiring, Valid = certificate.Valid, Total = certificate.Valid + certificate.Expiring + certificate.Expired };

            item.CertificatesTypes = await this.context.SumCertificateTypes.Where(q => q.CustomerId == cid).ToListAsync();



            return item;
        }
    }
}