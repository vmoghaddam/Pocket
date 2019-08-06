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
using System.Threading;
using Newtonsoft.Json;
using System.Globalization;

namespace EPAAPI.DAL
{
    public class FlightRepository : GenericRepository<Models.FlightInformation>
    {
        public FlightRepository(EPAGRIFFINEntities context)
       : base(context)
        {
        }

        public IQueryable<ViewFlightInformation> GetViewFlights()
        {
            return this.GetQuery<ViewFlightInformation>();
        }
        public IQueryable<ViewFlightDelay> GetViewFlightDelays()
        {
            return this.GetQuery<ViewFlightDelay>();
        }
        public IQueryable<ViewFlightPlan> GetViewFlightPlans()
        {
            return this.GetQuery<ViewFlightPlan>();
        }
        public IQueryable<ViewFlightPlanCalander> GetViewFlightPlansCalendar()
        {
            return this.GetQuery<ViewFlightPlanCalander>();
        }
        public IQueryable<ViewBoxCrew> GetViewBoxCrews()
        {
            return this.GetQuery<ViewBoxCrew>();
        }
        public IQueryable<ViewCrewCalendar> GetViewCrewCalendar()
        {
            return this.GetQuery<ViewCrewCalendar>();
        }
        public IQueryable<EmployeeCalendar> GetEmployeeCalendar()
        {
            return this.GetQuery<EmployeeCalendar>();
        }
        public IQueryable<ViewFlightFuel> GetViewFlightFuel()
        {
            return this.GetQuery<ViewFlightFuel>();
        }
        public IQueryable<ViewFlightCrew2> GetViewFlightCrew2()
        {
            return this.GetQuery<ViewFlightCrew2>();
        }
        public IQueryable<ViewCrewTimeDetail> GetViewCrewTimeDetail()
        {
            return this.GetQuery<ViewCrewTimeDetail>();
        }
        public IQueryable<ViewFlightPlanItem> GetViewFlightPlanItems()
        {
            return this.GetQuery<ViewFlightPlanItem>();
        }
        public IQueryable<ViewFlightPlanItemCalander> GetViewFlightPlanItemsCalander()
        {
            return this.GetQuery<ViewFlightPlanItemCalander>();
        }
        public IQueryable<ViewFlightPlanItemPermit> GetViewFlightPlanItemPermits()
        {
            return this.GetQuery<ViewFlightPlanItemPermit>();
        }
        public IQueryable<ViewFlightPlanCalanderCrew> GetViewFlightPlanCalanderCrew()
        {
            return this.GetQuery<ViewFlightPlanCalanderCrew>();
        }

        public IQueryable<FlightPlanCalanderCrew> GetFlightPlanCalanderCrew()
        {
            return this.GetQuery<FlightPlanCalanderCrew>();
        }
        public IQueryable<FlightPermit> GetFlightPermits()
        {
            return this.GetQuery<FlightPermit>();
        }
        public IQueryable<ViewFlighPlanAssignedRegister> GetViewFlightPlanAssignedRegisters()
        {
            return this.GetQuery<ViewFlighPlanAssignedRegister>();
        }


        public IQueryable<ViewDelayCode> GetViewDelayCodes()
        {
            return this.GetQuery<ViewDelayCode>();
        }
        public IQueryable<ViewFlightsAcType> GetViewFlightsAcTypes()
        {
            return this.GetQuery<ViewFlightsAcType>();
        }
        public IQueryable<ViewFlightsRegister> GetViewFlightsRegisters()
        {
            return this.GetQuery<ViewFlightsRegister>();
        }
        public IQueryable<ViewFlightsFrom> GetViewFlightsFrom()
        {
            return this.GetQuery<ViewFlightsFrom>();
        }
        public IQueryable<ViewFlightsTo> GetViewFlightsTo()
        {
            return this.GetQuery<ViewFlightsTo>();
        }
        public IQueryable<ViewFlightRoute> GetViewFlightRoutes()
        {
            return this.GetQuery<ViewFlightRoute>();
        }
        public IQueryable<ViewRouteFromAirport> GetViewRouteFromAirport()
        {
            return this.GetQuery<ViewRouteFromAirport>();
        }
        public IQueryable<ViewFlightDelayCode> GetViewFlightDelayCode(int flightId)
        {
            var delayCodes = this.context.ViewDelayCodes.ToList();
            var flightDelays = this.context.FlightDelays.Where(q => q.FlightId == flightId).ToList();
            var result = new List<ViewFlightDelayCode>();
            foreach (var x in delayCodes)
            {
                var fd = flightDelays.FirstOrDefault(q => q.DelayCodeId == x.Id);
                var item = new ViewFlightDelayCode()
                {
                    FlightId=(fd==null)?-1:fd.FlightId,
                    FlighrDelayId = (fd == null)?null:(Nullable<int>)fd.ID,
                    HH= (fd == null) ? null :  fd.HH,
                    MM = (fd == null) ? null : fd.MM,
                    UserId= (fd == null) ? null : fd.UserId,
                    Category=x.Category,
                    Title=x.Title,
                    Code=x.Code,
                    DelayCategoryId=x.DelayCategoryId,
                    AirlineId=x.AirlineId,
                    DelayCodeId= x.Id,
                    Selected= (fd == null) ? 0 : 1,

                    Remark=x.Remark,
 
                };
                result.Add(item);
            }
            return result.AsQueryable();
            //return this.GetQuery<ViewFlightDelayCode>().Where(q => q.FlightId == flightId || q.FlightId == -1);
        }
        public IQueryable<ViewFlightPlanItem> GetViewFlightPlanItems(int pid)
        {
            return this.GetQuery<ViewFlightPlanItem>().Where(q => q.FlightPlanId == pid);
        }

        public async Task<Models.FlightPlan> GetPlanById(int id)
        {
            return await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Models.EmployeeCalendar> GetEmployeeCalendarById(int id)
        {
            return await this.context.EmployeeCalendars.FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<Models.EmployeeCalendarSplited> GetEmployeeCalendarSplitedById(int id)
        {
            return await this.context.EmployeeCalendarSpliteds.FirstOrDefaultAsync(q => q.Id == id);
        }


        public async Task<Models.EmployeeCalendar> GetEmployeeCalendar(int eid,DateTime date)
        {
            return await this.context.EmployeeCalendars.FirstOrDefaultAsync(q => q.EmployeeId==eid && q.Date==date);
        }

        public async Task<Models.FlightPlanRegister> GetFlightPlanRegisterById(int id)
        {
            return await this.context.FlightPlanRegisters.FirstOrDefaultAsync(q => q.Id == id);
        }
        public bool IsBoxCrewAllAssigned(int bid)
        {
            return this.context.ViewBoxCrewRequirements.Where(q => q.Id == bid && q.Assigned != q.Min).FirstOrDefault() == null;
        }
        public async Task<Models.FlightPlan> GetPlanByTitle(string title)
        {
            return await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Title == title);
        }
        public async Task<Models.FlightPlanItem> GetPlanItemById(int id)
        {
            return await this.context.FlightPlanItems.FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<Models.FlightInformation> GetFlightById(int id)
        {
            return await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == id);
        }

        public async Task<Models.FlightPlanItemPermit> GetFlightPlanItemPermitById(int id)
        {
            return await this.context.FlightPlanItemPermits.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Models.FlightPlanCalanderCrew> GetFlightPlanCalanderCrewById(int id)
        {
            return await this.context.FlightPlanCalanderCrews.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Models.BoxCrew> GetBoxCrewById(int id)
        {
            return await this.context.BoxCrews.FirstOrDefaultAsync(q => q.Id == id);
        }


        public async Task<Models.AvgFlight> GetFlightAVG(int from, int to)
        {
            return await this.context.AvgFlights.FirstOrDefaultAsync(q => q.FromAirport == from && q.ToAirport == to);
        }

        public async Task<ViewModels.FlightPlanDto> GetFlightPlanById(int Id)
        {
            var plan = await this.context.ViewFlightPlans.FirstOrDefaultAsync(q => q.Id == Id);
            if (plan == null)
                return null;
            var plandto = new ViewModels.FlightPlanDto();
            ViewModels.FlightPlanDto.FillDto(plan, plandto);
            plandto.Months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == Id).Select(q => q.Month).ToListAsync();
            plandto.Days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == Id).Select(q => q.Day).ToListAsync();


            return plandto;
        }


        public List<ViewCrewTime> GetOverDuty(string date, int pid, int duty, int flight)
        {
           return this.context.GetOverDuty(date, duty, flight, pid).ToList();
        }

        public async Task<ViewModels.FlightPlanSummary> GetFlightPlanSummary(int Id, int tzoffset)
        {
            var plan = await this.context.ViewFlightPlans.FirstOrDefaultAsync(q => q.Id == Id);
            if (plan == null)
                return null;
            var plandto = new ViewModels.FlightPlanSummary();
            ViewModels.FlightPlanSummary.FillDto(plan, plandto);
            var month = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == Id).ToListAsync();
            var day = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == Id).ToListAsync();
            plandto.Months = month.Select(q => q.Month).ToList();
            plandto.Days = day.Select(q => q.Day).ToList();

            plandto.MonthsStr = string.Join(", ", month.Select(q => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(q.Month + 1)).ToList());
            plandto.DaysStr = string.Join(", ", day.Select(q => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName((DayOfWeek)q.Day)).ToList());

            var aregs = GetViewFlightPlanAssignedRegisters().Where(q => q.FlightPlanId == Id).ToList();
            foreach (var x in aregs)
            {
                x.STA = ((DateTime)x.STA).AddMinutes(tzoffset);
                x.STD = ((DateTime)x.STD).AddMinutes(tzoffset);
            }
            plandto.AssignedRegisters = aregs;
            return plandto;
        }
        public async Task<List<int>> GetFlightPlanMonth(int Id)
        {


            //plandto.Months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == Id).Select(q => q.Month).ToListAsync();
            // plandto.Days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == Id).Select(q => q.Day).ToListAsync();


            return await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == Id).Select(q => q.Month).ToListAsync();
        }
        public async Task<List<int>> GetFlightPlanDays(int Id)
        {


            //plandto.Months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == Id).Select(q => q.Month).ToListAsync();
            // plandto.Days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == Id).Select(q => q.Day).ToListAsync();


            return await this.context.FlightPlanDays.Where(q => q.FlightPlanId == Id).Select(q => q.Day).ToListAsync();
        }

        public async Task<List<Models.FlightPlanItem>> GetPlanItemsByIds(List<int> Ids)
        {
            //return await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == id);
            var query = from x in this.context.FlightPlanItems
                        where Ids.Contains(x.Id)
                        select x;
            return await query.ToListAsync();
        }

        public List<Models.FlightPlanMonth> GetPlanMonths(int Id)
        {
            //return await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == id);
            var query = from x in this.context.FlightPlanMonths
                        where x.FlightPlanId == Id
                        select x;
            return query.ToList();
        }
        public List<Models.FlightPlanDay> GetPlanDays(int Id)
        {
            //return await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == id);
            var query = from x in this.context.FlightPlanDays
                        where x.FlightPlanId == Id
                        select x;
            return query.ToList();
        }

        public void ClearPlanMonths(int id)
        {
            var months = GetPlanMonths(id);
            while (months.Count > 0)
            {
                var f = months.First();
                this.context.FlightPlanMonths.Remove(f);
                months.Remove(f);
            }
        }

        public void ClearPlanDays(int id)
        {
            var days = GetPlanDays(id);
            while (days.Count > 0)
            {
                var f = days.First();
                this.context.FlightPlanDays.Remove(f);
                days.Remove(f);
            }
        }

        private FlightInformation CreateFlight(FlightPlanItem item, int customer, DateTime date, int? register = null)
        {
            var STDPlan = (DateTime)item.STD;
            var STAPlan = (DateTime)item.STA;
            var STD = new DateTime(date.Year, date.Month, date.Day, STDPlan.Hour, STDPlan.Minute, STDPlan.Second);
            var STA = STD.AddHours(item.FlightH).AddMinutes(item.FlightM);
            var entity = new FlightInformation();
            entity.FlightPlanId = item.Id;
            entity.FlightStatusID = (int)item.StatusId;
            entity.CustomerId = customer;
            entity.STD = STD;
            entity.STA = STA;
            // entity.DateCreate = DateTime.Now;
            if (register != null)
                entity.RegisterID = register;


            return entity;
        }


        internal async Task<bool> CheckPlanRegister(int pid, int vid, int rid, DateTime? dfrom, DateTime? dto)
        {
            var plan = await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == pid);
            if (plan == null)
                return false;
            if (dfrom == null)
                dfrom = plan.DateFrom;
            if (dto == null)
                dto = plan.DateTo;
            var overlaps = await (from x in this.context.ViewFlightPlanCalendarRegisters
                                  join y in this.context.FlighPlanCalendars on x.Date equals y.Date
                                  where y.FlightPlanId == pid && x.FlightPlanId != y.FlightPlanId && y.Date >= dfrom && y.Date <= dto && x.FlightPlanId != y.FlightPlanId
                                  && x.RegisterId == rid
                                  select x).CountAsync();
            if (overlaps > 0)
                return false;

            return true;
        }
        //ati bani
        internal async Task<bool> ApproveFlightPlanRegisters(int pid)
        {
            var plan = await this.context.FlightPlans.SingleAsync(q => q.Id == pid);
            var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == pid).ToListAsync();
            var plannedRegisters = flights.Select(q => q.RegisterID).Distinct().ToList();
            var registers = await this.context.ViewFlightPlanRegisters.Where(q => q.FlightPlanId == pid).ToListAsync();

            if (registers.Select(q => q.PlannedRegisterId).Distinct().Count() != plannedRegisters.Count)
                return false;
            var plannedRegistersGroups = (from x in registers
                                          group x by x.PlannedRegisterId into g
                                          select g).ToList();
            foreach (var g in plannedRegistersGroups)
            {
                var dates = g.OrderBy(q => q.DateFrom).ToList();
                if (!(dates.First().DateFrom.Date == ((DateTime)plan.DateFrom).Date && dates.Last().DateTo.Date == ((DateTime)plan.DateTo).Date))
                    return false;
            }

            var fpstatus = new FlightPlanStatu()
            {
                ApproveTypeId = 100,
                DateApproved = DateTime.Now,
                FlighPlanId = pid,

            };
            this.context.FlightPlanStatus.Add(fpstatus);
            var flightCreationResult = await InsertFlightsByPlan(plan, registers);
            if (flightCreationResult.Code != HttpStatusCode.OK)
                return false;
            return true;
        }
        private FlightInformation CreateFlight(ViewFlightPlanItem item, int customer, DateTime date, int register, FlightPlanRegister fpr)
        {
            var STDPlan = (DateTime)item.STD;
            var STAPlan = (DateTime)item.STA;
            var STD = new DateTime(date.Year, date.Month, date.Day, STDPlan.Hour, STDPlan.Minute, STDPlan.Second);
            var STA = STD.AddHours(item.FlightH).AddMinutes(item.FlightM);
            var entity = new FlightInformation();
            entity.FlightPlanId = item.Id;
            entity.FlightStatusID = 1;
            entity.CustomerId = customer;
            entity.STD = STD;
            entity.STA = STA;
            // entity.DateCreate = DateTime.Now;

            entity.RegisterID = register;
            entity.FlightPlanRegisterId = fpr.Id;

            return entity;
        }

        private FlightInformation CreateFlight(ViewFlightPlanItemCalander item, int customer, DateTime date, int register, FlightPlanRegister fpr)
        {
            var STDPlan = (DateTime)item.STD;
            var STAPlan = (DateTime)item.STA;
            var STD = new DateTime(date.Year, date.Month, date.Day, STDPlan.Hour, STDPlan.Minute, STDPlan.Second);
            var STA = STD.AddHours(item.FlightH).AddMinutes(item.FlightM);
            var entity = new FlightInformation();
            entity.FlightPlanId = item.Id;
            entity.FlightStatusID = 1;
            entity.CustomerId = customer;
            entity.STD = STD;
            entity.STA = STA;
            // entity.DateCreate = DateTime.Now;

            entity.RegisterID = register;
            entity.FlightPlanRegisterId = fpr.Id;
            entity.CalendarId = item.CalendarId;
            entity.BoxId = item.BoxId;

            return entity;
        }
        internal async Task<CustomActionResult> InsertFlightsByPlanCalander(List<ViewFlightPlanItem> planItems, FlightPlanRegister planRegsiter)
        {

            //  var planRegsiters =await this.context.FlightPlanRegisters.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            var plancrew = await (from x in this.context.FlightPlanCalanderCrews
                                  where x.CalanderId == planRegsiter.CalendarId && x.FlightPlanId == planRegsiter.FlightPlanId
                                  select x).ToListAsync();


            var planitemids = planItems.Select(q => (Nullable<int>)q.Id).ToList();
            var deletedFlights = from x in this.context.FlightInformations

                                 where x.FlightStatusID == 1 && x.FlightPlanRegisterId == planRegsiter.Id && planitemids.Contains(x.FlightPlanId)
                                 select x;
            this.context.FlightInformations.RemoveRange(deletedFlights);


            var otherFlights = await (from x in this.context.FlightInformations

                                      where x.FlightStatusID != 1 && x.FlightPlanRegisterId == planRegsiter.Id && planitemids.Contains(x.FlightPlanId)
                                      select x).ToListAsync();
            var flights = new List<FlightInformation>();
            var flightcrews = new List<FlightCrew>();
            foreach (var item in planItems)
            {

                FlightInformation flight = CreateFlight(item, item.CustomerId, (DateTime)planRegsiter.Date, planRegsiter.RegisterId, planRegsiter);
                if (flight != null && otherFlights.FirstOrDefault(q => q.FlightPlanRegisterId == planRegsiter.Id && q.FlightPlanId == item.Id) == null)
                {
                    flights.Add(flight);
                    foreach (var x in plancrew)
                    {
                        flightcrews.Add(new FlightCrew()
                        {
                            FlightInformation = flight,
                            CreateDate = DateTime.Now,
                            Status = 1,
                            FlightPlanCrewId = x.Id,
                            EmployeeId = x.EmployeeId,

                        });
                    }
                }
            }
            this.context.FlightInformations.AddRange(flights);
            this.context.FlightCrews.AddRange(flightcrews);
            //   watch.Stop();
            //  var elapsedMs = watch.ElapsedMilliseconds;
            // this.context.Configuration.AutoDetectChangesEnabled = true;  
            return new CustomActionResult(HttpStatusCode.OK, "");
            // this.context.FlightInformations.RemoveRange(deletedFlights);
            // var months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Month).ToListAsync();
            // var days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Day).ToListAsync();
            //// var planItems = await this.context.FlightPlanItems.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            // var errors = planItems.Count(q => q.StatusId != 1);
            // if (errors > 0)
            //     return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            // var minDate = (DateTime)plan.DateFrom;
            // var adddays = 1;
            // switch (plan.Interval)
            // {
            //     case 1:


            //         adddays = 1;
            //         break;
            //     case 2:

            //         adddays = 7;
            //         break;
            //     case 3:

            //         adddays = 10;
            //         break;
            //     case 4:

            //         adddays = 14;
            //         break;
            //     case 5:

            //         adddays = 15;
            //         break;
            //     case 100:


            //         adddays = 1;
            //         break;
            //     case 101:


            //         adddays = 1;
            //         break;
            //     default:
            //         break;
            // }

            // var watch = System.Diagnostics.Stopwatch.StartNew();
            // while (minDate <= (DateTime)plan.DateTo)
            // {
            //     bool addFlight = false;
            //     //   flight = CreateFlight(item, customer, minDate);
            //     if (plan.Interval == 100)
            //     {
            //         //if (months != null && days != null && months.Count > 0 && days.Count > 0)
            //         {
            //             var d = (int)minDate.DayOfWeek;
            //             var m = minDate.Month - 1;
            //             if (months.IndexOf(m) != -1 && days.IndexOf(d) != -1)
            //             {
            //                 addFlight = true;
            //             }
            //         }

            //     }
            //     else
            //         addFlight = true;

            //     //
            //     foreach (var item in planItems)
            //     {
            //         if (addFlight)
            //         {
            //             var register = planRegsiters.FirstOrDefault(q => minDate >= q.DateFrom && minDate <= q.DateTo && item.RegisterID == q.PlannedRegisterId);
            //             FlightInformation flight = CreateFlight(item, plan.CustomerId, minDate, register.RegisterId);
            //             if (flight != null && otherFlights.FirstOrDefault(q => q.STD == flight.STD) == null)
            //             {
            //                 flights.Add(flight);
            //             }
            //         }




            //     }
            //     minDate = minDate.AddDays(adddays);

            // }
            // this.context.FlightInformations.AddRange(flights);
            // //   watch.Stop();
            // //  var elapsedMs = watch.ElapsedMilliseconds;
            // // this.context.Configuration.AutoDetectChangesEnabled = true;  
            // return new CustomActionResult(HttpStatusCode.OK, "");
        }



        internal async Task<CustomActionResult> InsertFlightsByPlanCalander(List<ViewFlightPlanItemCalander> planItems, FlightPlanRegister planRegsiter)
        {

            //  var planRegsiters =await this.context.FlightPlanRegisters.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            var plancrew = await (from x in this.context.FlightPlanCalanderCrews
                                  where x.CalanderId == planRegsiter.CalendarId && x.FlightPlanId == planRegsiter.FlightPlanId
                                  select x).ToListAsync();


            var planitemids = planItems.Select(q => (Nullable<int>)q.Id).ToList();
            var deletedFlights = from x in this.context.FlightInformations

                                 where x.FlightStatusID == 1 && x.FlightPlanRegisterId == planRegsiter.Id && planitemids.Contains(x.FlightPlanId)
                                 select x;
            this.context.FlightInformations.RemoveRange(deletedFlights);


            var otherFlights = await (from x in this.context.FlightInformations

                                      where x.FlightStatusID != 1 && x.FlightPlanRegisterId == planRegsiter.Id && planitemids.Contains(x.FlightPlanId)
                                      select x).ToListAsync();
            var flights = new List<FlightInformation>();
            var flightcrews = new List<FlightCrew>();
            foreach (var item in planItems)
            {

                FlightInformation flight = CreateFlight(item, item.CustomerId, (DateTime)planRegsiter.Date, planRegsiter.RegisterId, planRegsiter);
                if (flight != null && otherFlights.FirstOrDefault(q => q.FlightPlanRegisterId == planRegsiter.Id && q.FlightPlanId == item.Id) == null)
                {
                    flights.Add(flight);
                    var flightcrew = plancrew.Where(q => q.BoxId == item.BoxId).ToList();
                    foreach (var x in flightcrew)
                    {
                        flightcrews.Add(new FlightCrew()
                        {
                            FlightInformation = flight,
                            CreateDate = DateTime.Now,
                            Status = 1,
                            FlightPlanCrewId = x.Id,

                        });
                    }
                }
            }
            this.context.FlightInformations.AddRange(flights);
            this.context.FlightCrews.AddRange(flightcrews);
            //   watch.Stop();
            //  var elapsedMs = watch.ElapsedMilliseconds;
            // this.context.Configuration.AutoDetectChangesEnabled = true;  
            return new CustomActionResult(HttpStatusCode.OK, "");
            // this.context.FlightInformations.RemoveRange(deletedFlights);
            // var months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Month).ToListAsync();
            // var days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Day).ToListAsync();
            //// var planItems = await this.context.FlightPlanItems.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            // var errors = planItems.Count(q => q.StatusId != 1);
            // if (errors > 0)
            //     return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            // var minDate = (DateTime)plan.DateFrom;
            // var adddays = 1;
            // switch (plan.Interval)
            // {
            //     case 1:


            //         adddays = 1;
            //         break;
            //     case 2:

            //         adddays = 7;
            //         break;
            //     case 3:

            //         adddays = 10;
            //         break;
            //     case 4:

            //         adddays = 14;
            //         break;
            //     case 5:

            //         adddays = 15;
            //         break;
            //     case 100:


            //         adddays = 1;
            //         break;
            //     case 101:


            //         adddays = 1;
            //         break;
            //     default:
            //         break;
            // }

            // var watch = System.Diagnostics.Stopwatch.StartNew();
            // while (minDate <= (DateTime)plan.DateTo)
            // {
            //     bool addFlight = false;
            //     //   flight = CreateFlight(item, customer, minDate);
            //     if (plan.Interval == 100)
            //     {
            //         //if (months != null && days != null && months.Count > 0 && days.Count > 0)
            //         {
            //             var d = (int)minDate.DayOfWeek;
            //             var m = minDate.Month - 1;
            //             if (months.IndexOf(m) != -1 && days.IndexOf(d) != -1)
            //             {
            //                 addFlight = true;
            //             }
            //         }

            //     }
            //     else
            //         addFlight = true;

            //     //
            //     foreach (var item in planItems)
            //     {
            //         if (addFlight)
            //         {
            //             var register = planRegsiters.FirstOrDefault(q => minDate >= q.DateFrom && minDate <= q.DateTo && item.RegisterID == q.PlannedRegisterId);
            //             FlightInformation flight = CreateFlight(item, plan.CustomerId, minDate, register.RegisterId);
            //             if (flight != null && otherFlights.FirstOrDefault(q => q.STD == flight.STD) == null)
            //             {
            //                 flights.Add(flight);
            //             }
            //         }




            //     }
            //     minDate = minDate.AddDays(adddays);

            // }
            // this.context.FlightInformations.AddRange(flights);
            // //   watch.Stop();
            // //  var elapsedMs = watch.ElapsedMilliseconds;
            // // this.context.Configuration.AutoDetectChangesEnabled = true;  
            // return new CustomActionResult(HttpStatusCode.OK, "");
        }

        //kati2
        private FlightInformation CreateFlight(ViewFlightPlanItemCalander item)
        {
            var date = (DateTime)item.Date;
            var STDPlan = (DateTime)item.STD;
            var STAPlan = (DateTime)item.STA;
            var STD = STDPlan; //new DateTime(date.Year, date.Month, date.Day, STDPlan.Hour, STDPlan.Minute, STDPlan.Second);
            var STA = STD.AddHours(item.FlightH).AddMinutes(item.FlightM);
            var entity = new FlightInformation();
            entity.CalendarId = item.CalendarId;
            entity.FlightPlanId = item.Id;
            entity.FlightStatusID = (int)item.StatusId;
            entity.CustomerId = item.CustomerId;
            entity.STD = STD;
            entity.STA = STA;
            // entity.DateCreate = DateTime.Now;
            entity.FlightPlanRegisterId = item.RegisterID;
            entity.RegisterID = item.RegisterID;


            return entity;
        }
        internal async Task<CustomActionResult> InsertFlightsByPlanCalander(List<ViewFlightPlanItemCalander> planItems)
        {

            //  var planRegsiters =await this.context.FlightPlanRegisters.Where(q => q.FlightPlanId == plan.Id).ToListAsync();



            var planitemids = planItems.Select(q => (Nullable<int>)q.Id).ToList();
            var deletedFlights = from x in this.context.FlightInformations

                                 where x.FlightStatusID == 1 && planitemids.Contains(x.FlightPlanId)
                                 select x;
            this.context.FlightInformations.RemoveRange(deletedFlights);


            var otherFlights = await (from x in this.context.FlightInformations

                                      where x.FlightStatusID != 1 && planitemids.Contains(x.FlightPlanId)
                                      select x).ToListAsync();
            var flights = new List<FlightInformation>();
            var flightcrews = new List<FlightCrew>();
            foreach (var item in planItems)
            {

                FlightInformation flight = CreateFlight(item);
                if (flight != null && otherFlights.FirstOrDefault(q => q.CalendarId == item.CalendarId && q.FlightPlanId == item.Id) == null)
                {
                    flights.Add(flight);

                }
            }
            this.context.FlightInformations.AddRange(flights);
            
            //   watch.Stop();
            //  var elapsedMs = watch.ElapsedMilliseconds;
            // this.context.Configuration.AutoDetectChangesEnabled = true;  
            return new CustomActionResult(HttpStatusCode.OK, "");

        }


        internal async Task<bool> ApproveFlightPlanRegisterCalander(int id)
        {
            var flightplanregister = await this.context.FlightPlanRegisters.SingleAsync(q => q.Id == id);

            var plan = await this.context.FlightPlans.SingleAsync(q => q.Id == flightplanregister.FlightPlanId);
            // var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            var flights = await this.context.ViewFlightPlanItemCalanders.Where(q => q.FlightPlanId == plan.Id && q.CalendarId == flightplanregister.CalendarId).ToListAsync();


            flightplanregister.IsApproved = true;
            flightplanregister.DateApproved = DateTime.Now;


            var flightCreationResult = await InsertFlightsByPlanCalander(flights, flightplanregister);
            if (flightCreationResult.Code != HttpStatusCode.OK)
                return false;
            return true;
        }
        //zati2
        internal async Task<CustomActionResult> CloseFlightPlan(int pid)
        {
            var plan = await this.context.ViewFlightPlans.SingleAsync(q => q.Id == pid);
            var planitems = await this.context.ViewFlightPlanItemCalanders.Where(q => q.FlightPlanId == plan.Id).ToListAsync();



            if (plan.GapOverlaps > 0 || plan.Gaps > 0 || plan.Overlaps > 0)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            var plant = await this.context.FlightPlans.SingleAsync(q => q.Id == pid);
            plant.Title = plant.Title + "_A50-" + (DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            var fpstatus = new FlightPlanStatu()
            {
                ApproveTypeId = 50,
                DateApproved = DateTime.Now,
                FlighPlanId = pid,


            };
            this.context.FlightPlanStatus.Add(fpstatus);
            await InsertFlightsByPlanCalander(planitems);

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        internal async Task<CustomActionResult> ApproveFlightPlan(int pid, int type)
        {
            var plan = await this.context.ViewFlightPlans.SingleAsync(q => q.Id == pid);

            var fpstatus = new FlightPlanStatu()
            {
                ApproveTypeId = type,
                DateApproved = DateTime.Now,
                FlighPlanId = pid,


            };
            this.context.FlightPlanStatus.Add(fpstatus);

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        internal List<FlightPlanRegister> InsertFlightPlanRegisters(List<ViewFlightPlanRegister> items, List<int> deleted)
        {
            var deletedQ = from x in this.context.FlightPlanRegisters
                           where deleted.Contains(x.Id)
                           select x;
            this.context.FlightPlanRegisters.RemoveRange(deletedQ);
            var newItems = new List<FlightPlanRegister>();
            foreach (var x in items)
            {
                var df = ((DateTime)x.DateFrom).Date;
                var dt = ((DateTime)x.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                var entity = new FlightPlanRegister()
                {
                    DateFrom = df,
                    DateTo = dt,
                    FlightPlanId = x.FlightPlanId,
                    RegisterId = x.RegisterId,
                    PlannedRegisterId = x.PlannedRegisterId,
                    Remark = x.Remark
                };
                this.context.FlightPlanRegisters.Add(entity);
                newItems.Add(entity);
            }

            return newItems;
        }
        internal FlightPlanRegister InsertFlightPlanRegister(int id, DateTime date, int planId, int registerId, int virtualId, int calanderId)
        {
            var entity = this.context.FlightPlanRegisters.FirstOrDefault(q => q.Id == id);
            if (entity == null)
            {
                entity = new FlightPlanRegister();
                this.context.FlightPlanRegisters.Add(entity);
            }
            var df = ((DateTime)date).Date;
            var dt = ((DateTime)date).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            entity.Date = df;
            entity.DateFrom = df;
            entity.DateTo = dt;
            entity.FlightPlanId = planId;
            entity.RegisterId = registerId;
            entity.PlannedRegisterId = virtualId;
            entity.CalendarId = calanderId;

            int noOfRowUpdated = this.context.Database.ExecuteSqlCommand("Update FlightInformation set RegisterID = "+ registerId + " where CalendarId = "+ calanderId);


            return entity;
        }
        internal async Task<CustomActionResult> InsertFlightsByPlan(FlightPlan plan, List<ViewFlightPlanRegister> planRegsiters)
        {
            //  var planRegsiters =await this.context.FlightPlanRegisters.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            plan.IsActive = true;
            plan.DateActive = DateTime.Now;
            var deletedFlights = from x in this.context.FlightInformations
                                 join y in this.context.FlightPlanItems on x.FlightPlanId equals y.Id
                                 where y.FlightPlanId == plan.Id && x.FlightStatusID == 1
                                 select x;
            var otherFlights = await (from x in this.context.FlightInformations
                                      join y in this.context.FlightPlanItems on x.FlightPlanId equals y.Id
                                      where y.FlightPlanId == plan.Id && x.FlightStatusID != 1
                                      select x).ToListAsync();

            this.context.FlightInformations.RemoveRange(deletedFlights);
            var months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Month).ToListAsync();
            var days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Day).ToListAsync();
            var planItems = await this.context.FlightPlanItems.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            var errors = planItems.Count(q => q.StatusId != 1);
            if (errors > 0)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            var minDate = (DateTime)plan.DateFrom;
            var adddays = 1;
            switch (plan.Interval)
            {
                case 1:


                    adddays = 1;
                    break;
                case 2:

                    adddays = 7;
                    break;
                case 3:

                    adddays = 10;
                    break;
                case 4:

                    adddays = 14;
                    break;
                case 5:

                    adddays = 15;
                    break;
                case 100:


                    adddays = 1;
                    break;
                case 101:


                    adddays = 1;
                    break;
                default:
                    break;
            }
            var flights = new List<FlightInformation>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (minDate <= (DateTime)plan.DateTo)
            {
                bool addFlight = false;
                //   flight = CreateFlight(item, customer, minDate);
                if (plan.Interval == 100)
                {
                    //if (months != null && days != null && months.Count > 0 && days.Count > 0)
                    {
                        var d = (int)minDate.DayOfWeek;
                        var m = minDate.Month - 1;
                        if (months.IndexOf(m) != -1 && days.IndexOf(d) != -1)
                        {
                            addFlight = true;
                        }
                    }

                }
                else
                    addFlight = true;

                //
                foreach (var item in planItems)
                {
                    if (addFlight)
                    {
                        var register = planRegsiters.FirstOrDefault(q => minDate >= q.DateFrom && minDate <= q.DateTo && item.RegisterID == q.PlannedRegisterId);
                        FlightInformation flight = CreateFlight(item, plan.CustomerId, minDate, register.RegisterId);
                        if (flight != null && otherFlights.FirstOrDefault(q => q.STD == flight.STD) == null)
                        {
                            flights.Add(flight);
                        }
                    }




                }
                minDate = minDate.AddDays(adddays);

            }
            this.context.FlightInformations.AddRange(flights);
            //   watch.Stop();
            //  var elapsedMs = watch.ElapsedMilliseconds;
            // this.context.Configuration.AutoDetectChangesEnabled = true;  
            return new CustomActionResult(HttpStatusCode.OK, "");
        }



        public async Task<CustomActionResult> ApplyPlan(int planId, int customer)
        {
            //  return new CustomActionResult(HttpStatusCode.OK, "");
            var plan = await this.context.FlightPlans.FirstOrDefaultAsync(q => q.Id == planId);
            if (plan == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            plan.IsActive = true;
            plan.DateActive = DateTime.Now;
            var deletedFlights = from x in this.context.FlightInformations
                                 join y in this.context.FlightPlanItems on x.FlightPlanId equals y.Id
                                 where y.FlightPlanId == plan.Id && x.FlightStatusID == 1
                                 select x;
            var otherFlights = await (from x in this.context.FlightInformations
                                      join y in this.context.FlightPlanItems on x.FlightPlanId equals y.Id
                                      where y.FlightPlanId == plan.Id && x.FlightStatusID != 1
                                      select x).ToListAsync();

            this.context.FlightInformations.RemoveRange(deletedFlights);


            var months = await this.context.FlightPlanMonths.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Month).ToListAsync();
            var days = await this.context.FlightPlanDays.Where(q => q.FlightPlanId == plan.Id).Select(q => q.Day).ToListAsync();
            var planItems = await this.context.FlightPlanItems.Where(q => q.FlightPlanId == plan.Id).ToListAsync();
            var errors = planItems.Count(q => q.StatusId != 1);
            if (errors > 0)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            var minDate = (DateTime)plan.DateFrom;
            while (minDate <= (DateTime)plan.DateTo)
            {
                bool addFlight = false;
                //   flight = CreateFlight(item, customer, minDate);
                if (plan.Interval == 100)
                {
                    if (months != null && days != null && months.Count > 0 && days.Count > 0)
                    {
                        var d = (int)minDate.DayOfWeek;
                        var m = minDate.Month - 1;
                        if (months.IndexOf(m) != -1 && days.IndexOf(d) != -1)
                        {
                            addFlight = true;
                        }
                    }

                }
                else
                    addFlight = true;

                //
                foreach (var item in planItems)
                {
                    if (addFlight)
                    {
                        FlightInformation flight = CreateFlight(item, customer, minDate);
                        if (flight != null && otherFlights.FirstOrDefault(q => q.STD == flight.STD) == null)
                        {
                            this.context.FlightInformations.Add(flight);
                        }
                    }




                }
                switch (plan.Interval)
                {
                    case 1:


                        minDate = minDate.AddDays(1);
                        break;
                    case 2:

                        minDate = minDate.AddDays(7);
                        break;
                    case 3:

                        minDate = minDate.AddDays(10);
                        break;
                    case 4:

                        minDate = minDate.AddDays(14);
                        break;
                    case 5:

                        minDate = minDate.AddDays(15);
                        break;
                    case 100:


                        minDate = minDate.AddDays(1);
                        break;
                    default:
                        break;
                }
            }

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        //zati
        public void CreatePlanCalendar(int id, Models.FlightPlan plan, List<int> months, List<int> days)
        {
            if (id != -1)
                this.context.FlighPlanCalendars.RemoveRange(this.context.FlighPlanCalendars.Where(q => q.FlightPlanId == plan.Id));
            // $scope.intervalTypes = [{ Id: 1, Title: 'Daily' }, { Id: 2, Title: 'Weekly' }, { Id: 3, Title: 'Every 10 days' }, { Id: 4, Title: 'Every 14 days' }, { Id: 5, Title: 'Every 15 days' }, { Id: 100, Title: 'Custom' }];
            var minDate = (DateTime)plan.DateFrom;
            while (minDate <= (DateTime)plan.DateTo)
            {
                switch (plan.Interval)
                {
                    case 1:
                    case 101:
                        plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                        minDate = minDate.AddDays(1);
                        break;
                    case 2:
                        plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                        minDate = minDate.AddDays(7);
                        break;
                    case 3:
                        plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                        minDate = minDate.AddDays(10);
                        break;
                    case 4:
                        plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                        minDate = minDate.AddDays(14);
                        break;
                    case 5:
                        plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                        minDate = minDate.AddDays(15);
                        break;
                    case 100:
                        if (months != null && days != null && months.Count > 0 && days.Count > 0)
                        {
                            var d = (int)minDate.DayOfWeek;
                            var m = minDate.Month - 1;
                            if (months.IndexOf(m) != -1 && days.IndexOf(d) != -1)
                            {
                                plan.FlighPlanCalendars.Add(new FlighPlanCalendar() { FlightPlan = plan, Date = minDate, });
                            }
                        }

                        minDate = minDate.AddDays(1);
                        break;
                    default:
                        break;
                }

            }
        }
        public async Task<CustomActionResult> GetCrewSummary(int id)
        {

            var past24 = await (from x in this.context.ViewFlightCrews
                                where x.EmployeeId == id && x.FlightStatusID != null && x.PastHoursFromOffBlock <= 24
                                select x).SumAsync(q => q.TotalFlightHoursOffBlock);
            var past48 = await (from x in this.context.ViewFlightCrews
                                where x.EmployeeId == id && x.FlightStatusID != null && x.PastHoursFromOffBlock <= 48
                                select x).SumAsync(q => q.TotalFlightHoursOffBlock);
            var pastweek = await (from x in this.context.ViewFlightCrews
                                  where x.EmployeeId == id && x.FlightStatusID != null && x.PastHoursFromOffBlock <= 168
                                  select x).SumAsync(q => q.TotalFlightHoursOffBlock);
            var pastmonth = await (from x in this.context.ViewFlightCrews
                                   where x.EmployeeId == id && x.FlightStatusID != null && x.PastHoursFromOffBlock <= 720
                                   select x).SumAsync(q => q.TotalFlightHoursOffBlock);

            var p24 = past24 != null ? Math.Round(Convert.ToDouble(past24), 1) : 0;
            var p48 = past48 != null ? Math.Round(Convert.ToDouble(past48), 1) : 0;
            var pweek = pastweek != null ? Math.Round(Convert.ToDouble(pastweek), 1) : 0;
            var pmonth = pastmonth != null ? Math.Round(Convert.ToDouble(pastmonth), 1) : 0;

            dynamic summary = new
            {
                Past24 = p24,
                Past48 = p48,
                PastWeek = pweek,
                PastMonth = pmonth,
                EmployeeId = id,

            };
            return new CustomActionResult(HttpStatusCode.OK, summary);
        }
        public async Task<CustomActionResult> GetPlanBase(int customer, DateTime date, int register)
        {
            var baseid = await (from x in this.context.ViewFlightPlanItems.AsNoTracking()
                                where x.CustomerId == customer && x.DateFrom == date && x.RegisterID == register
                                select x.BaseId).FirstOrDefaultAsync();
            if (baseid == null)
                baseid = -1;
            return new CustomActionResult(HttpStatusCode.OK, baseid);
        }
        //boosk
        public async Task<ViewFlightPlanItem> GetPlanLastItem(int customer, DateTime date, int register, int offset)
        {
            var baseid = await (from x in this.context.ViewFlightPlanItems.AsNoTracking()
                                where x.CustomerId == customer && x.DateFrom == date && x.RegisterID == register
                                orderby x.STD descending
                                select x).FirstOrDefaultAsync();
            if (baseid != null)
            {
                baseid.STA = ((DateTime)baseid.STA).AddMinutes(offset);
                baseid.STD = ((DateTime)baseid.STD).AddMinutes(offset);
            }
            else
                baseid = new ViewFlightPlanItem() { Id = -1 };

            return baseid;
        }
        public  List<ViewFlightPlanItem>  GetPlanItems(int customer, DateTime date, int register, int offset)
        {
            try
            {
                var baseid =   (from x in this.context.ViewFlightPlanItems.AsNoTracking()
                                    where x.CustomerId == customer && x.DateFrom == date && x.RegisterID == register
                                    orderby x.STD descending
                                    select x).ToList();
                foreach (var x in baseid)
                {

                    x.STA = ((DateTime)x.STA).AddMinutes(offset);
                    x.STD = ((DateTime)x.STD).AddMinutes(offset);
                }


                return baseid;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        public async Task<CustomActionResult> ProcessPlanErrors(FlightPlan plan, FlightPlanItem item)
        {
            var dbitems_query = from x in this.context.ViewFlightPlanItems
                                join y in this.context.FlightPlanItems on x.Id equals y.Id
                                where x.RegisterID == item.RegisterID && x.DateFrom == plan.DateFrom
                                // orderby y.STD
                                select y;
            var dbitems = await dbitems_query.ToListAsync();
            if (item.Id == -1)
                dbitems.Add(item);
            dbitems = dbitems.OrderBy(q => q.STD).ToList();
            List<int> overlaps = new List<int>();
            List<int> gaps = new List<int>();
            if (dbitems.Count > 1)
            {
                for (int i = 0; i < dbitems.Count; i++)
                {


                    var o = dbitems[i];
                    var _overlaps = (from x in dbitems
                                     where x.Id != o.Id && (
                                     (x.STD >= o.STD && x.STD <= o.STA) || (x.STA >= o.STD && x.STA <= o.STA)
                                     || (o.STD >= x.STD && o.STD <= x.STA) || (o.STA >= x.STD && o.STA <= x.STA)
                                     )

                                     select x).Count();
                    var isoverlap = _overlaps > 0;

                    var isgap = i > 0 && dbitems[i - 1].ToAirport != o.FromAirport;
                    if (!isoverlap && !isgap)
                        o.StatusId = 1;
                    else if (isoverlap && !isgap)
                        o.StatusId = 10;
                    else if (!isoverlap && isgap)
                        o.StatusId = 11;
                    else o.StatusId = 16;



                }


            }

            // ( (STA>='2019-02-01 05:00:00.000' AND STA<='2019-02-01 06:10:00.000') or (STD>='2019-02-01 05:00:00.000' AND STD<='2019-02-01 06:10:00.000') )


            //16 gap and overlap
            //10 overlap
            //11 gap
            return new CustomActionResult(HttpStatusCode.OK, "");
        }


        public async Task<CustomActionResult> ProcessPlanErrors(FlightPlan plan)
        {
            var dbitems_query = from x in this.context.ViewFlightPlanItems
                                join y in this.context.FlightPlanItems on x.Id equals y.Id
                                where x.FlightPlanId == plan.Id
                                // orderby y.STD
                                select y;
            var dbitems = await dbitems_query.ToListAsync();

            dbitems = dbitems.OrderBy(q => q.STD).ToList();
            List<int> overlaps = new List<int>();
            List<int> gaps = new List<int>();
            //if (dbitems.Count > 1)
            {
                for (int i = 0; i < dbitems.Count; i++)
                {


                    var o = dbitems[i];
                    var _overlaps = (from x in dbitems
                                     where x.Id != o.Id && (
                                     (x.STD >= o.STD && x.STD <= o.STA) || (x.STA >= o.STD && x.STA <= o.STA)
                                     || (o.STD >= x.STD && o.STD <= x.STA) || (o.STA >= x.STD && o.STA <= x.STA)
                                     )

                                     select x).Count();
                    var isoverlap = _overlaps > 0;

                    var isgap = i > 0 && dbitems[i - 1].ToAirport != o.FromAirport;
                    if (!isoverlap && !isgap)
                        o.StatusId = 1;
                    else if (isoverlap && !isgap)
                        o.StatusId = 10;
                    else if (!isoverlap && isgap)
                        o.StatusId = 11;
                    else o.StatusId = 16;



                }


            }

            // ( (STA>='2019-02-01 05:00:00.000' AND STA<='2019-02-01 06:10:00.000') or (STD>='2019-02-01 05:00:00.000' AND STD<='2019-02-01 06:10:00.000') )


            //16 gap and overlap
            //10 overlap
            //11 gap
            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public CustomActionResult UpdateDelays(ViewModels.FlightSaveDto dto)
        {

            var currentDelays = this.context.FlightDelays.Where(q => q.FlightId == dto.ID);
            this.context.FlightDelays.RemoveRange(currentDelays);
            foreach (var x in dto.Delays)
            {
                this.context.FlightDelays.Add(new FlightDelay()
                {
                    DelayCodeId = x.DelayCodeId,
                    FlightId = dto.ID,
                    MM = x.MM,
                    HH = x.HH,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<CustomActionResult> UpdateEstimatedDelays(ViewModels.FlightSaveDto dto)
        {
            if (dto.EstimatedDelays == null || dto.EstimatedDelays.Count == 0)
                return new CustomActionResult(HttpStatusCode.OK, "");
            var flightIds = dto.EstimatedDelays.Select(q => q.FlightId).ToList();
            var flights = await this.context.FlightInformations.Where(q => flightIds.Contains(q.ID)).ToListAsync();
            foreach (var x in flights)
            {
                var f = dto.EstimatedDelays.FirstOrDefault(q => q.FlightId == x.ID);
                if (f != null)
                    x.EstimatedDelay = f.Delay;
            }

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        public async Task<CustomActionResult> UpdateFlightDeparture(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var takeOffChanged = flight.Takeoff != dto.Takeoff;
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && (q.FlightStatusID == 2 || q.FlightStatusID == 14));
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }
            if (flight.FlightStatusID != 4)
            {
                if (flight.FlightStatusID != dto.FlightStatusID)
                {
                    if (dto.StatusLog.Count > 0)
                    {
                        flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                    }
                    else
                    {
                        flight.DateCreate = DateTime.UtcNow;
                    }
                    flight.FlightStatusUserId = dto.UserId;
                }
                flight.FlightStatusID = dto.FlightStatusID;
            }

            flight.ChocksOut = dto.ChocksOut;
            flight.Takeoff = dto.Takeoff;
            flight.GWTO = dto.GWTO;
            flight.FuelDeparture = dto.FuelDeparture;
            flight.PaxAdult = dto.PaxAdult;
            flight.PaxInfant = dto.PaxInfant;
            flight.PaxChild = dto.PaxChild;
            flight.CargoWeight = dto.CargoWeight;
            flight.CargoUnitID = dto.CargoUnitID;
            flight.BaggageCount = dto.BaggageCount;
            flight.CargoCount = dto.CargoCount;
            flight.BaggageWeight = dto.BaggageWeight;
            flight.FuelUnitID = dto.FuelUnitID;
            flight.DepartureRemark = dto.DepartureRemark;

            var result = UpdateDelays(dto);

            if (result.Code != HttpStatusCode.OK)
                return result;

            var result2 = await UpdateEstimatedDelays(dto);

            if (result2.Code != HttpStatusCode.OK)
                return result2;


            return new CustomActionResult(HttpStatusCode.OK, takeOffChanged);
        }

        public async Task<CustomActionResult> UpdateFlightLog(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            //var takeOffChanged = flight.Takeoff != dto.Takeoff;
            //var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            //var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && (q.FlightStatusID == 2 || q.FlightStatusID == 14));
            //this.context.FlightStatusLogs.RemoveRange(currentStatus);
            //foreach (var x in dto.StatusLog)
            //{
            //    this.context.FlightStatusLogs.Add(new FlightStatusLog()
            //    {
            //        FlightID = dto.ID,
            //        Date = x.Date,
            //        FlightStatusID = x.FlightStatusID,
            //        Remark = x.Remark,
            //        UserId = x.UserId
            //    });
            //}
            //if (flight.FlightStatusID != 4)
            //{
            //    if (flight.FlightStatusID != dto.FlightStatusID)
            //    {
            //        if (dto.StatusLog.Count > 0)
            //        {
            //            flight.DateCreate = dto.StatusLog.Max(q => q.Date);
            //        }
            //        else
            //        {
            //            flight.DateCreate = DateTime.UtcNow;
            //        }
            //        flight.FlightStatusUserId = dto.UserId;
            //    }
            //    flight.FlightStatusID = dto.FlightStatusID;
            //}

            flight.ChocksIn = dto.ChocksIn;
            flight.Landing = dto.Landing;
            flight.ChocksOut = dto.ChocksOut;
            flight.Takeoff = dto.Takeoff;
            flight.GWTO = dto.GWTO;
            flight.FuelDeparture = dto.FuelDeparture;
            flight.FuelArrival = dto.FuelArrival;
            flight.PaxAdult = dto.PaxAdult;
            flight.PaxInfant = dto.PaxInfant;
            flight.PaxChild = dto.PaxChild;
            flight.CargoWeight = dto.CargoWeight;
            flight.CargoUnitID = dto.CargoUnitID;
            flight.BaggageCount = dto.BaggageCount;
            flight.CargoCount = dto.CargoCount;
            flight.BaggageWeight = dto.BaggageWeight;
            flight.FuelUnitID = dto.FuelUnitID;
            flight.DepartureRemark = dto.DepartureRemark;
            flight.FPFlightHH = dto.FPFlightHH;
            flight.FPFlightMM = dto.FPFlightMM;
            flight.FPFuel = dto.FPFuel;
            flight.Defuel = dto.Defuel;
            flight.FlightStatusID = dto.FlightStatusID;
            if (flight.FlightStatusID == 4)
            {
                flight.CancelDate = dto.CancelDate;
                flight.CancelReasonId = dto.CancelReasonId;
            }
            if (flight.FlightStatusID == 17)
            {
                var vflight = await this.context.ViewFlightInformations.FirstOrDefaultAsync(q => q.ID == flight.ID);
                flight.RedirectDate = dto.RedirectDate;
                flight.RedirectReasonId = dto.RedirectReasonId;
                flight.OSTA = flight.STA;
                flight.OToAirportId = vflight.ToAirport;
                var airport = await this.context.Airports.FirstOrDefaultAsync(q => q.Id == flight.OToAirportId);
                flight.ToAirportId = dto.ToAirportId;
                if (airport != null)
                    flight.OToAirportIATA = airport.IATA;
            }
            if (flight.FlightStatusID == 9)
            {
                flight.RampDate = dto.RampDate;
                flight.RampReasonId = dto.RampReasonId;
            }

            if (flight.ChocksIn != null && flight.FlightStatusID==15)
            {
                var vflight = await this.context.ViewFlightInformations.FirstOrDefaultAsync(q => q.ID == flight.ID);
                //var flightCrew = await this.context.ViewFlightCrew2.Where(q => q.FlightId == flight.ID).ToListAsync();
                var flightCrewEmployee = await (from x in this.context.Employees
                                                join y in this.context.ViewFlightCrew2 on x.Id equals y.EmployeeId
                                                where y.FlightId == flight.ID
                                                select x).ToListAsync();
                //foreach (var x in flightCrew)
                //     x.Status = 15;
                foreach (var x in flightCrewEmployee)
                    x.CurrentLocationAirport = vflight.ToAirport;

            }

            var result = UpdateDelays(dto);

            if (result.Code != HttpStatusCode.OK)
                return result;

            var result2 = await UpdateEstimatedDelays(dto);

            if (result2.Code != HttpStatusCode.OK)
                return result2;


            return new CustomActionResult(HttpStatusCode.OK,null);
        }
        public async Task<CustomActionResult> UpdateFlightFuelDeparture(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);


            flight.FuelDeparture = dto.FuelDeparture;

            flight.FuelUnitID = dto.FuelUnitID;



            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        public async Task<CustomActionResult> UpdateFlightFuelArrival(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            flight.FuelArrival = dto.FuelArrival;

            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        public async Task<CustomActionResult> UpdateFlightPax(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            flight.PaxAdult = dto.PaxAdult;
            flight.PaxInfant = dto.PaxInfant;
            flight.PaxChild = dto.PaxChild;



            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        public async Task<CustomActionResult> UpdateFlightCargo(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");


            flight.GWTO = dto.GWTO;


            flight.CargoWeight = dto.CargoWeight;
            flight.CargoUnitID = dto.CargoUnitID;
            flight.BaggageCount = dto.BaggageCount;
            flight.CargoCount = dto.CargoCount;
            flight.BaggageWeight = dto.BaggageWeight;



            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<CustomActionResult> UpdateFlightOffBlock(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }
            if (flight.FlightStatusID != dto.FlightStatusID)
            {
                if (dto.StatusLog.Count > 0)
                {
                    flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                }
                else
                {
                    flight.DateCreate = DateTime.UtcNow;
                }
                flight.FlightStatusUserId = dto.UserId;
            }
            flight.FlightStatusID = dto.FlightStatusID;
            flight.ChocksOut = dto.ChocksOut;

            var result2 = await UpdateEstimatedDelays(dto);

            if (result2.Code != HttpStatusCode.OK)
                return result2;


            return new CustomActionResult(HttpStatusCode.OK, "");
        }


        public async Task<CustomActionResult> UpdateReportingTime(int id,int eid,DateTime date,int offset)
        {
            var flight = await this.context.BoxCrews.FirstOrDefaultAsync(q => q.Id==id);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
           date= date.AddMinutes(-offset);
            flight.ReportingTime = date;


            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<bool> IsRegisterAvailable(int id, DateTime sta, DateTime std)
        {
            var query = await this.context.ViewFlightInformations.Where(q => q.RegisterID == id && q.CancelDate == null
               && ((std >= q.STD && std <= q.STA) || (sta >= q.STD && sta <= q.STA))
            ).CountAsync();

            return query == 0;
        }

        public async Task<CustomActionResult> AssignFlightRegister(ViewModels.FlightRegisterDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.FlightId);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, false);
          //  var isavailable = await IsRegisterAvailable(dto.RegisterId, (DateTime)flight.STA, (DateTime)flight.STD);
          //  if (!isavailable)
           //     return new CustomActionResult(HttpStatusCode.NotAcceptable, false);
            flight.TypeID = dto.TypeId;
            flight.RegisterID = dto.RegisterId;

            return new CustomActionResult(HttpStatusCode.OK, true);
        }

        public async Task<CustomActionResult> ChangeFlightRegister(ViewModels.FlightRegisterChangeLogDto dto)
        {


            DateTime From = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(dto.From).Date;
            DateTime To = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(dto.To).Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);


            var flights = await this.context.FlightInformations.Where(q => dto.Flights.Contains(q.ID)).ToListAsync();
            if (flights == null || flights.Count == 0)
                return new CustomActionResult(HttpStatusCode.NotFound, "Flights Not Found");
            var existFlights = await this.context.ViewFlightInformations.Where(q => q.RegisterID == dto.NewRegisterId && q.STA >= From && q.STA <= To && !dto.Flights.Contains(q.ID)).ToListAsync();
            var isvalid = true;
            foreach (var x in flights)
            {
                var exist = existFlights.FirstOrDefault(q => (q.STA >= x.STD && q.STA <= x.STA) || (q.STD >= x.STD && q.STD <= x.STA)
                || (x.STD >= q.STD && x.STA <= q.STA) || (x.STA >= q.STD && x.STD <= q.STA)
                );
                if (exist != null)
                {
                    isvalid = false;
                    break;
                }
                x.RegisterID = dto.NewRegisterId;
                this.context.FlightRegisterChangeLogs.Add(new FlightRegisterChangeLog()
                {
                    Date = DateTime.UtcNow,
                    FlightId = x.ID,
                    NewRegisterId = dto.NewRegisterId,
                    OldRegisterId = (int)x.RegisterID,
                    ReasonId = dto.ReasonId,
                    Remark = dto.Remark,
                    UserId = dto.UserId,
                });
            }
            if (!isvalid)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "Conflict Error");

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<CustomActionResult> ApplyFlight(int id)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == id);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, false);

            flight.IsApplied = true;
            flight.DateApplied = DateTime.UtcNow;

            return new CustomActionResult(HttpStatusCode.OK, true);
        }

        public async Task<CustomActionResult> UpdateFlightCancel(ViewModels.FlightCancelDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.FlightId);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.FlightId && q.FlightStatusID == 4);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);

            var log = new FlightStatusLog()
            {
                FlightID = flight.ID,
                Date = dto.Date,
                FlightStatusID = 4,
                UserId = (int)dto.UserId,

            };
            this.context.FlightStatusLogs.Add(log);
            flight.CancelDate = dto.CancelDate;
            flight.CancelReasonId = dto.CancelReasonId;
            flight.CancelRemark = dto.CancelRemark;
            flight.FlightStatusID = 4;
            flight.FlightStatusUserId = dto.UserId;



            return new CustomActionResult(HttpStatusCode.OK, true);
        }


        public async Task<CustomActionResult> UpdateFlightRamp(ViewModels.FlightRampDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.FlightId);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

           //var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.FlightId).OrderBy(q=>q.Date).ToList();
           // var rampstatus = currentStatus.Where(q => q.FlightStatusID == 9).OrderByDescending(q => q.Date).FirstOrDefault();
           // var index = currentStatus.IndexOf(rampstatus);
           //   this.context.FlightStatusLogs.RemoveRange(currentStatus);

            var log = new FlightStatusLog()
            {
                FlightID = flight.ID,
                Date = dto.Date,
                FlightStatusID = 9,
                UserId = (int)dto.UserId,

            };
            this.context.FlightStatusLogs.Add(log);
            flight.RampDate = dto.RampDate;
            flight.RampReasonId = dto.RampReasonId;
            flight.RampRemark = dto.RampRemark;
            flight.FlightStatusID = 9;
            flight.FlightStatusUserId = dto.UserId;



            return new CustomActionResult(HttpStatusCode.OK, true);
        }

        public async Task<CustomActionResult> UpdateFlightRedirect(ViewModels.FlightRedirectDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.FlightId);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.FlightId && q.FlightStatusID == 17);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);

            var log = new FlightStatusLog()
            {
                FlightID = flight.ID,
                Date = dto.Date,
                FlightStatusID = 17,
                UserId = (int)dto.UserId,

            };
            this.context.FlightStatusLogs.Add(log);
            flight.RedirectDate = dto.RedirectDate;
            flight.RedirectReasonId = dto.RedirectReasonId;
            flight.RedirectRemark = dto.RedirectRemark;

            flight.OToAirportId = flight.ToAirportId;
            flight.OToAirportIATA = dto.OAirportIATA;
            flight.ToAirportId = dto.AirportId;

            flight.OSTA = flight.STA;
            flight.STA = dto.STA;
            //old
            //flight.FlightH = Convert.ToInt32(dto.STA.Subtract((DateTime)flight.STD).TotalHours);
            //flight.FlightM = Convert.ToByte(dto.STA.Subtract((DateTime)flight.STD).TotalMinutes - flight.FlightH * 60);
            //new
            //  flight.FlightH = Convert.ToInt32(dto.STA.Subtract((DateTime)flight.ChocksOut).TotalHours);
            // flight.FlightM = Convert.ToByte(dto.STA.Subtract((DateTime)flight.ChocksOut).TotalMinutes - flight.FlightH * 60);
           
            flight.FlightH = Convert.ToInt32(dto.STA.Subtract((DateTime)flight.ChocksOut).Hours);
            flight.FlightM = Convert.ToByte(dto.STA.Subtract((DateTime)flight.ChocksOut).TotalHours*60 - flight.FlightH * 60);



            flight.FlightStatusID = 17;
            flight.FlightStatusUserId = dto.UserId;



            return new CustomActionResult(HttpStatusCode.OK, true);
        }

        public async Task<CustomActionResult> UpdateFlightTakeOff(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && q.FlightStatusID == 2);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            {
                if (flight.FlightStatusID != dto.FlightStatusID)
                {
                    if (dto.StatusLog.Count > 0)
                    {
                        flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                    }
                    else
                    {
                        flight.DateCreate = DateTime.UtcNow;
                    }
                    flight.FlightStatusUserId = dto.UserId;
                }
                flight.FlightStatusID = dto.FlightStatusID;
            }

            flight.Takeoff = dto.Takeoff;

            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<CustomActionResult> UpdateFlightArrival(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var landingChanged = flight.Landing != dto.Landing;
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && (q.FlightStatusID == 15 || q.FlightStatusID == 3));
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            if (flight.FlightStatusID != dto.FlightStatusID)
            {
                if (dto.StatusLog.Count > 0)
                {
                    flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                }
                else
                {
                    flight.DateCreate = DateTime.UtcNow;
                }
                flight.FlightStatusUserId = dto.UserId;
            }
            if (flight.FlightStatusID != 17 && flight.FlightStatusID != 7)
            {
                flight.FlightStatusID = dto.FlightStatusID;
            }
            else
            {
                if (dto.ChocksIn == null && dto.Landing != null)
                {
                    flight.FlightStatusID = 7;
                    var divertedStatus = this.context.FlightStatusLogs.FirstOrDefault(q => q.FlightID == dto.ID && (q.FlightStatusID == 7));
                    if (divertedStatus == null)
                    {
                        var dtoLandingStatus = dto.StatusLog.FirstOrDefault(q => q.FlightStatusID == 3);
                        var date = dtoLandingStatus != null ? dtoLandingStatus.Date : DateTime.UtcNow;
                        this.context.FlightStatusLogs.Add(new FlightStatusLog()
                        {
                            FlightID = dto.ID,
                            Date = date,
                            FlightStatusID = 7,
                            Remark = "",
                            UserId = dtoLandingStatus != null ? dtoLandingStatus.UserId : -1,
                        });
                    }
                }
                else if (dto.ChocksIn != null)
                    flight.FlightStatusID = 18;
            }


            flight.ChocksIn = dto.ChocksIn;
            flight.Landing = dto.Landing;
            flight.GWLand = dto.GWLand;
            flight.FuelArrival = dto.FuelArrival;


            flight.ArrivalRemark = dto.ArrivalRemark;
            if (flight.ChocksIn != null)
            {
                //var flightCrew = await this.context.ViewFlightCrew2.Where(q => q.FlightId == flight.ID).ToListAsync();
                var flightCrewEmployee = await (from x in this.context.Employees
                                                join y in this.context.ViewFlightCrew2 on x.Id equals y.EmployeeId
                                                where y.FlightId == flight.ID
                                                select x).ToListAsync();
                //foreach (var x in flightCrew)
                //     x.Status = 15;
                foreach (var x in flightCrewEmployee)
                    x.CurrentLocationAirport = flight.ToAirportId;

            }

            var result = UpdateDelays(dto);
            if (result.Code != HttpStatusCode.OK)
                return result;

            return new CustomActionResult(HttpStatusCode.OK, landingChanged);
        }

        public async Task<CustomActionResult> UpdateFlightLanding(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && q.FlightStatusID == 3);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            if (flight.FlightStatusID != dto.FlightStatusID)
            {
                if (dto.StatusLog.Count > 0)
                {
                    flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                }
                else
                {
                    flight.DateCreate = DateTime.UtcNow;
                }
                flight.FlightStatusUserId = dto.UserId;
            }
            if (flight.FlightStatusID != 17)
            {
                flight.FlightStatusID = dto.FlightStatusID;
            }
            else
            {
                flight.FlightStatusID = 7;
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = dto.StatusLog.Count > 0 ? dto.StatusLog.First().Date : DateTime.UtcNow,
                    FlightStatusID = 7,
                    Remark = "",
                    UserId = dto.StatusLog.Count > 0 ? dto.StatusLog.First().UserId : -1,
                });
            }




            flight.Landing = dto.Landing;




            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        //shati
        public async Task<CustomActionResult> UpdateFlightOnBlock(ViewModels.FlightSaveDto dto)
        {
            var flight = await this.context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");
            var stids = dto.StatusLog.Select(q => q.FlightStatusID).ToList();
            var currentStatus = this.context.FlightStatusLogs.Where(q => q.FlightID == dto.ID && q.FlightStatusID == 15);
            this.context.FlightStatusLogs.RemoveRange(currentStatus);
            foreach (var x in dto.StatusLog)
            {
                this.context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,
                    Date = x.Date,
                    FlightStatusID = x.FlightStatusID,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            if (flight.FlightStatusID != dto.FlightStatusID)
            {
                if (dto.StatusLog.Count > 0)
                {
                    flight.DateCreate = dto.StatusLog.Max(q => q.Date);
                }
                else
                {
                    flight.DateCreate = DateTime.UtcNow;
                }
                flight.FlightStatusUserId = dto.UserId;
            }
            if (flight.FlightStatusID != 7)
            {
                flight.FlightStatusID = dto.FlightStatusID;

            }



            flight.ChocksIn = dto.ChocksIn;
            var airport = await this.context.ViewFlightInformations.Where(q => q.ID == flight.ID).Select(q => q.ToAirport).SingleAsync();
           // var flightCrew = await this.context.ViewFlightCrew2.Where(q => q.FlightId == flight.ID).ToListAsync();
            var flightCrewEmployee = await (from x in this.context.Employees
                                            join y in this.context.ViewFlightCrew2 on x.Id equals y.EmployeeId
                                            where y.FlightId == flight.ID
                                            select x).ToListAsync();
            //foreach (var x in flightCrew)
           //     x.Status = 15;
            foreach (var x in flightCrewEmployee)
                x.CurrentLocationAirport = airport;



            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        public async Task<CustomActionResult> GetFlightWeather(int flightId, int status)
        {
            var weather = await this.context.FlightStatusWeathers.FirstOrDefaultAsync(q => q.FlightId == flightId && q.StatusId == status);
            if (weather != null)
            {
                dynamic myObject = JsonConvert.DeserializeObject<dynamic>(weather.Details);
                return new CustomActionResult(HttpStatusCode.OK, myObject);
            }

            var flight = await this.context.ViewFlightInformations.FirstOrDefaultAsync(q => q.ID == flightId);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            var result = SetFlightStatusWeather(flight, status);

            return new CustomActionResult(HttpStatusCode.OK, result);
        }
        public async Task<CustomActionResult> GetFlightWeathers(int flightId)
        {
            var weathers = await this.context.FlightStatusWeathers.Where(q => q.FlightId == flightId && q.StatusId > 0).ToListAsync();
            var result = new List<object>();
            foreach (var w in weathers)
            {
                //  dynamic myObject = JsonConvert.DeserializeObject<dynamic>(w.Details);

                result.Add(w);
            }


            return new CustomActionResult(HttpStatusCode.OK, result);
        }
        public object SetFlightStatusWeather(ViewFlightInformation flight, int status)
        {

            try
            {
                var entity = new Models.FlightStatusWeather()
                {
                    StatusId = status,
                    FlightId = flight.ID,

                };
                EPAAPI.Controllers.WeatherController weather = new Controllers.WeatherController();
                long unix = 0;
                dynamic weatherResult = null;
                if (status == -1)
                {
                    unix = (long)(((DateTime)flight.STD).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.FromLatitude, (decimal)flight.FromLongitude, unix);
                    entity.DateStatus = flight.STD;
                }
                if (status == -2)
                {
                    unix = (long)(((DateTime)flight.STA).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.ToLatitude, (decimal)flight.ToLongitude, unix);
                    entity.DateStatus = flight.STA;
                }
                if (status == 2)
                {
                    unix = (long)(((DateTime)flight.Takeoff).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.FromLatitude, (decimal)flight.FromLongitude, unix);
                    entity.DateStatus = flight.Takeoff;
                }
                if (status == 14)
                {
                    unix = (long)(((DateTime)flight.ChocksOut).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.FromLatitude, (decimal)flight.FromLongitude, unix);
                    entity.DateStatus = flight.ChocksOut;
                }
                if (status == 15)
                {
                    unix = (long)(((DateTime)flight.ChocksIn).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.ToLatitude, (decimal)flight.ToLongitude, unix);
                    entity.DateStatus = flight.ChocksIn;
                }
                if (status == 3)
                {
                    unix = (long)(((DateTime)flight.Landing).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.ToLatitude, (decimal)flight.ToLongitude, unix);
                    entity.DateStatus = flight.Landing;
                }

                entity.Temprature = (decimal)weatherResult.currently.temperature;
                entity.Pressure = (decimal)weatherResult.currently.pressure;


                entity.WindSpeed = (decimal)weatherResult.currently.windSpeed;
                entity.WindBearing = (decimal)weatherResult.currently.windBearing;
                entity.CloudCover = (decimal)weatherResult.currently.cloudCover;


                entity.Humidity = (decimal)weatherResult.currently.humidity;
                entity.DewPoint = (decimal)weatherResult.currently.dewPoint;
                entity.Summary = (string)weatherResult.currently.summary;
                entity.Icon = (string)weatherResult.currently.icon;
                entity.Visibility = (decimal)weatherResult.currently.visibility;
                entity.Details = JsonConvert.SerializeObject(weatherResult);

                this.context.FlightStatusWeathers.Add(entity);
                this.context.SaveChanges();

                return weatherResult;
            }
            catch(Exception ex)
            {
                return null;
            }
            

            // }).Start();

        }
        public object SetFlightStatusWeather(int flightId, DateTime? time, int status)
        {
            // new Thread(() =>
            // {
            try
            {
                var flight = this.context.ViewFlightInformations.FirstOrDefault(q => q.ID == flightId);
                if (flight == null)
                    return null;
                var current = this.context.FlightStatusWeathers.FirstOrDefault(q => q.FlightId == flightId && q.StatusId == status);
                if (current != null)
                    this.context.FlightStatusWeathers.Remove(current);
                if (time == null)
                {
                    this.context.SaveChanges();
                    return null;
                }
                var entity = new Models.FlightStatusWeather()
                {
                    StatusId = status,
                    FlightId = flightId,

                };
                EPAAPI.Controllers.WeatherController weather = new Controllers.WeatherController();
                long unix = 0;
                dynamic weatherResult = null;
                if (status == 2)
                {
                    // unix = ((DateTimeOffset)flight.Takeoff).ToUnixTimeSeconds();
                    unix = (long)(((DateTime)flight.Takeoff).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.FromLatitude, (decimal)flight.FromLongitude, unix);
                    entity.DateStatus = flight.Takeoff;
                }
                else if (status == 3)
                {
                    unix = (long)(((DateTime)flight.Landing).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.ToLatitude, (decimal)flight.ToLongitude, unix);
                    entity.DateStatus = flight.Landing;
                }
                else if (status == 4)
                {
                    unix = (long)(((DateTime)time).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.FromLatitude, (decimal)flight.FromLongitude, unix);
                    entity.DateStatus = time;
                }
                else
                {
                    unix = (long)(((DateTime)time).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    weatherResult = weather.GetWeatherByTimeInternal((decimal)flight.ToLatitude, (decimal)flight.ToLongitude, unix);
                    entity.DateStatus = time;
                }

                entity.Temprature = (decimal)weatherResult.currently.temperature;
                entity.Pressure = (decimal)weatherResult.currently.pressure;


                entity.WindSpeed = (decimal)weatherResult.currently.windSpeed;
                entity.WindBearing = (decimal)weatherResult.currently.windBearing;
                entity.CloudCover = (decimal)weatherResult.currently.cloudCover;


                entity.Humidity = (decimal)weatherResult.currently.humidity;
                entity.DewPoint = (decimal)weatherResult.currently.dewPoint;
                entity.Summary = (string)weatherResult.currently.summary;
                entity.Icon = (string)weatherResult.currently.icon;
                entity.Visibility = (decimal)weatherResult.currently.visibility;
                entity.Details = JsonConvert.SerializeObject(weatherResult);

                this.context.FlightStatusWeathers.Add(entity);
                this.context.SaveChanges();

                return weatherResult;
            }
            catch(Exception ex)
            {
                return null;
            }
            

            // }).Start();

        }

        public void RemoveFlightLink(int flightId)
        {
            var links = this.context.FlightLinks.Where(q => q.Flight1Id == flightId);
            this.context.FlightLinks.RemoveRange(links);
        }

        public virtual void Insert(Models.FlightInformation entity)
        {
            this.context.FlightInformations.Add(entity);
        }

        public virtual void Insert(Models.FlightPlanCalanderCrew entity)
        {
            this.context.FlightPlanCalanderCrews.Add(entity);
        }
        public virtual void Insert(Models.BoxCrew entity)
        {
            this.context.BoxCrews.Add(entity);
        }
        public virtual void Insert(Models.FlightPlanItemPermit entity)
        {
            this.context.FlightPlanItemPermits.Add(entity);
        }
        public virtual void Insert(Models.FlightLink entity)
        {
            this.context.FlightLinks.Add(entity);
        }
        public virtual void Insert(Models.FlightPlanItem entity)
        {
            this.context.FlightPlanItems.Add(entity);
        }
        public virtual void Insert(Models.FlightPlan entity)
        {
            this.context.FlightPlans.Add(entity);
        }
        public virtual void Insert(Models.FlightPlanMonth entity)
        {
            this.context.FlightPlanMonths.Add(entity);
        }
        public virtual void Insert(Models.EmployeeCalendar entity)
        {
            this.context.EmployeeCalendars.Add(entity);
        }
        public virtual void Insert(Models.EmployeeCalendarSplited entity)
        {
            this.context.EmployeeCalendarSpliteds.Add(entity);
        }
        public virtual void Insert(Models.FlightPlanDay entity)
        {
            this.context.FlightPlanDays.Add(entity);
        }
        public virtual void Delete(Models.FlightPlanItem entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.FlightPlanItems.Attach(entityToDelete);
            }
            this.context.FlightPlanItems.Remove(entityToDelete);
        }
        public virtual void Delete(Models.EmployeeCalendar entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.EmployeeCalendars.Attach(entityToDelete);
            }
            this.context.EmployeeCalendars.Remove(entityToDelete);
        }

        public virtual void Delete(Models.FlightPlanCalanderCrew entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.FlightPlanCalanderCrews.Attach(entityToDelete);
            }
            this.context.FlightPlanCalanderCrews.Remove(entityToDelete);
        }
        public virtual void Delete(Models.BoxCrew entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.BoxCrews.Attach(entityToDelete);
            }
            this.context.BoxCrews.Remove(entityToDelete);
        }

        public virtual void DeleteFlight(Models.FlightInformation entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.FlightInformations.Attach(entityToDelete);
            }
            this.context.FlightInformations.Remove(entityToDelete);
        }

        public virtual void Delete(Models.FlightPlanRegister entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.FlightPlanRegisters.Attach(entityToDelete);
            }
            this.context.FlightPlanRegisters.Remove(entityToDelete);
        }

        public virtual void Delete(Models.FlightPlanItemPermit entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.context.FlightPlanItemPermits.Attach(entityToDelete);
            }
            this.context.FlightPlanItemPermits.Remove(entityToDelete);
        }


        public async Task<object> GetFlightsGrouped(int cid, string sta, string std)
        {
            DateTime dateSTA = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(sta).Date;
            DateTime dateSTD = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(std).Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            var query = from x in this.context.ViewFlightInformations
                        where x.RegisterID != null && x.CustomerId == cid && x.FlightStatusID == 1 && (x.STA >= dateSTA && x.STA <= dateSTD)
                        group x by new
                        {
                            x.AircraftType,
                            x.TypeId,
                            x.Register,
                            x.RegisterID

                        }
                      into g
                        select new { g.Key.Register, g.Key.RegisterID, g.Key.TypeId, g.Key.AircraftType, TotalFlights = g.Count() };
            var result = await query.ToListAsync();
            return result;
        }

        internal async Task<object> GetGantt(int cid)
        {
            var flights = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid).ToListAsync();
            var flightsdto = new List<ViewModels.ViewFlightInformationDto>();
            foreach (var x in flights)
            {
                ViewModels.ViewFlightInformationDto dto = new ViewFlightInformationDto();
                ViewModels.ViewFlightInformationDto.FillDto(x, dto, 0);
                flightsdto.Add(dto);
            }

            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            //            var resourceGanttResources = [
            //    { resourceId: 1, resourceName: "EP-TAD", groupId: 1 },
            //    { resourceId: 2, resourceName: "EP-TAB", groupId: 1 },
            //    { resourceId: 3, resourceName: "EP-TAC", groupId: 2 },
            //    { resourceId: 4, resourceName: "EP-TAF", groupId: 2 },
            //    { resourceId: 5, resourceName: "EP-TAE", groupId: 2 },
            //    { resourceId: 6, resourceName: "EP-TAG", groupId: 3 },
            //    { resourceId: 7, resourceName: "UNKNOWN 1" },
            //    { resourceId: 8, resourceName: "UNKNOWN 2" },
            //];
            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new { grp.Key.RegisterID, grp.Key.Register, grp.Key.TypeId }).ToList();
            var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                if (x.RegisterID >= 0)
                    x.resourceId.Add((int)x.RegisterID);
                else
                    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }

            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroups.ToList(),
                resources = ress,
            };
            return result;
        }
        internal async Task<object> GetPlanGantt(int pid, int tzoffset)
        {
            var plan = await this.context.FlightPlans.SingleAsync(q => q.Id == pid);
            var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == pid).ToListAsync();
            var flightsdto = new List<ViewModels.ViewFlightPlanItemDto>();
            foreach (var x in flights)
            {
                ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                ViewModels.ViewFlightPlanItemDto.FillDto(x, dto, tzoffset);
                flightsdto.Add(dto);
            }

            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };

            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new resource() { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId, assigned = false, registers = string.Empty }).ToList();

            //var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }
            var _registers = await this.context.ViewFlightPlanRegisters.Where(q => q.FlightPlanId == pid).ToListAsync();
            var grpRegisters = (from x in _registers
                                group x by x.PlannedRegisterId into g
                                select g).ToList();
            foreach (var g in grpRegisters)
            {
                var res = ressq.FirstOrDefault(q => q.resourceId == g.Key);
                if (res != null)
                {
                    res.registers = string.Join(", ", g.Select(q => q.Register).ToList());
                    var dates = g.OrderBy(q => q.DateFrom).ToList();
                    if (dates.First().DateFrom.Date == ((DateTime)plan.DateFrom).Date && dates.Last().DateTo.Date == ((DateTime)plan.DateTo).Date)
                        res.assigned = true;
                }
            }
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroups.ToList(),
                resources = ressq,
                registers = _registers,
            };
            return result;
        }


        internal async Task<object> GetPlanItemsGantt(DateTime date, int tzoffset, bool design, int cid)
        {
            var plansQuery = from x in this.context.ViewFlightPlans
                             where x.DateFrom == date && x.CustomerId == cid
                             select x;
            var flightsQuery = from x in this.context.ViewFlightPlanItems
                               where x.DateFrom == date && x.CustomerId == cid
                               select x;
            if (design)
            {
                plansQuery = plansQuery.Where(q => q.IsApproved50 == 0);
                flightsQuery = flightsQuery.Where(q => q.IsApproved50 == 0);
            }

            var plans = await this.context.FlightPlans.ToListAsync();


            //var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == pid).ToListAsync();
            var flights = await flightsQuery.ToListAsync();
            var flightsdto = new List<ViewModels.ViewFlightPlanItemDto>();
            foreach (var x in flights)
            {
                ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                ViewModels.ViewFlightPlanItemDto.FillDto(x, dto, tzoffset);
                flightsdto.Add(dto);
            }

            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };

            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new resource() { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId, assigned = false, registers = string.Empty }).ToList();

            //var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }
            var planIds = plans.Select(q => q.Id).ToList();
            var _registers = await this.context.ViewFlightPlanRegisters.Where(q => planIds.Contains(q.FlightPlanId)).ToListAsync();
            var grpRegisters = (from x in _registers
                                group x by x.PlannedRegisterId into g
                                select g).ToList();
            foreach (var g in grpRegisters)
            {
                var res = ressq.FirstOrDefault(q => q.resourceId == g.Key);
                if (res != null)
                {
                    res.registers = string.Join(", ", g.Select(q => q.Register).ToList());
                    var dates = g.OrderBy(q => q.DateFrom).ToList();
                    var plan = plans.FirstOrDefault(q => q.Id == g.FirstOrDefault().FlightPlanId);
                    if (dates.First().DateFrom.Date == ((DateTime)plan.DateFrom).Date && dates.Last().DateTo.Date == ((DateTime)plan.DateTo).Date)
                        res.assigned = true;
                }
            }
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroups.ToList(),
                resources = ressq,
                registers = _registers,
            };
            return result;
        }
        public virtual async Task<Box> GetBoxByID(int id)
        {
            return await this.context.Boxes.FirstOrDefaultAsync(q => q.Id == id);
        }
        public virtual async Task<ViewBox> GetViewBoxByID(int id)
        {
            return await this.context.ViewBoxes.FirstOrDefaultAsync(q => q.Id == id);
        }


        internal async Task<bool> boxPlanItems(List<int> ids, int cid)
        {
            // var planitems = await this.context.FlightPlanItems.Where(q => ids.Contains(q.Id)).OrderBy(q=>q.STD).ToListAsync();
            var planitems = await (from x in this.context.ViewFlightPlanItemCalanders
                                   where ids.Contains(x.Id) && x.CalendarId == cid
                                   select new { x.Id, x.FlightPlanId, x.STA, x.STD, x.ToAirport, x.FromAirport }).ToListAsync();
            var IsPlanValid = planitems.Select(q => q.FlightPlanId).Distinct().Count() == 1;
            if (!IsPlanValid)
                return false;
            //,DateTime std,DateTime sta,int from,int to
            var std = planitems.First().STD;
            var sta = planitems.Last().STA;
            var from = planitems.First().FromAirport;
            var to = planitems.Last().ToAirport;
            var box = new Box();
            box.STA = sta;
            box.STD = std;
            box.FromAirportId = from;
            box.ToAirportId = to;
            box.Date = ((DateTime)std).Date;
            box.FlightPlanId = planitems.First().FlightPlanId;
            box.CalanderId = cid;
            this.context.Boxes.Add(box);
            foreach (var x in planitems)
            {
                //x.Box = box;
                var bfp = new BoxFlightPlanItem()
                {
                    CalanderId = cid,
                    ItemId = x.Id,
                    Box = box
                };
                this.context.BoxFlightPlanItems.Add(bfp);
            }
            return true;

        }
        //jooj
        internal async Task<string> boxFlights(List<int> ids, int cid)
        {
            // var planitems = await this.context.FlightPlanItems.Where(q => ids.Contains(q.Id)).OrderBy(q=>q.STD).ToListAsync();
            var flights = await (from x in this.context.ViewFlightInformations
                                   where ids.Contains(x.ID) //&& x.CalendarId == cid
                                   orderby x.STD
                                   select new { x.ID, x.PlanId, x.STA, x.STD, x.ToAirport, x.FromAirport,x.CalendarId }).ToListAsync();

           // var IsPlanValid = flights.Select(q => q.PlanId+"_"+(q.CalendarId==null?"0":q.CalendarId.ToString())).Distinct().Count() == 1;
           // if (!IsPlanValid)
            //    return false;


            //,DateTime std,DateTime sta,int from,int to
            var fids = flights.Select(q => q.ID).ToList();
            var tflights = await this.context.FlightInformations.Where(q => fids.Contains(q.ID)).OrderBy(q=>q.STD) .ToListAsync();
            var std = flights.First().STD;
            var sta = flights.Last().STA;
            var from = flights.First().FromAirport;
            var to = flights.Last().ToAirport;

            var breakGreaterThan10Hours = string.Empty;
            
            if (flights.Count > 1)
            {
                for (int i = 1; i < flights.Count; i++)
                {
                    var dt =(DateTime) flights[i].STD -(DateTime) flights[i - 1].STA;
                    var minuts = dt.TotalMinutes;
                    // – (0:30 + 0:15 + 0:45)
                    var brk = minuts - 30 -60; //30:travel time, post flight duty:15, pre flight duty:30
                    if (brk>=600)
                    {
                        //var tfi = tflights.FirstOrDefault(q => q.ID == flights[i].ID);
                        // var tfi1 = tflights.FirstOrDefault(q => q.ID == flights[i - 1].ID);
                        breakGreaterThan10Hours = "The break is greater than 10 hours.";
                    }
                      
                        else
                    if (brk >= 180)
                    {
                        
                        var tfi = tflights.FirstOrDefault(q => q.ID == flights[i].ID);
                        tfi.SplitDuty = true;
                        var tfi1 = tflights.FirstOrDefault(q => q.ID == flights[i-1].ID);
                        tfi1.SplitDuty = true;
                    }
                }
            }
            if (!string.IsNullOrEmpty(breakGreaterThan10Hours))
                return breakGreaterThan10Hours;
            var box = new Box();
            box.STA = sta;
            box.STD = std;
            box.FromAirportId = from;
            box.ToAirportId = to;
            box.Date = ((DateTime)std).AddMinutes(270).Date;
            box.FlightPlanId =(int) flights.First().PlanId;
            box.CalanderId = cid;
            this.context.Boxes.Add(box);
            var ci = 0;
            foreach (var x in flights)
            {
                var tf = tflights.FirstOrDefault(q => q.ID == x.ID);
                tf.Box = box;
                //x.Box = box;
                var bfp = new BoxFlightPlanItem()
                {
                    CalanderId = cid,
                    ItemId = x.ID,
                    Box = box,
                     SplitDuty=tf.SplitDuty,
                };
                if (bfp.SplitDuty==true && ci>0)
                {
                    var preitem = tflights[ci - 1];
                    if (preitem.SplitDuty == true)
                        bfp.SplitDutyPairId = preitem.ID;

                }
                this.context.BoxFlightPlanItems.Add(bfp);
                ci++;
            }
            return string.Empty;

        }


        internal async Task<bool> unboxPlanItems(int bid)
        {
            var box = await this.context.Boxes.FirstOrDefaultAsync(q => q.Id == bid);
            this.context.Boxes.Remove(box);
            //var planitems = await this.context.FlightPlanItems.Where(q => q.BoxId==bid).ToListAsync();

            //foreach (var x in planitems)
            //    x.BoxId = null;
            return true;

        }
        internal async Task<bool> unboxFlights(int bid)
        {
            int noOfRowUpdated = this.context.Database.ExecuteSqlCommand("Update FlightInformation set BoxId = null where BoxId = " + bid);

            var box = await this.context.Boxes.FirstOrDefaultAsync(q => q.Id == bid);
            this.context.Boxes.Remove(box);
            //var planitems = await this.context.FlightPlanItems.Where(q => q.BoxId==bid).ToListAsync();

            //foreach (var x in planitems)
            //    x.BoxId = null;
            return true;

        }
        public IQueryable<ViewBoxCrewFlight> GetViewBoxCrewFlights()
        {
            return this.GetQuery<ViewBoxCrewFlight>();
        }
        public IQueryable<ViewCrewTime> GetViewCrewTimes()
        {
            return this.GetQuery<ViewCrewTime>();
        }
        //jooj
        internal async Task<object> GetPlanItemsGanttCrewTestByFlights(DateTime date,DateTime dateTo, int tzoffset, bool design, int cid, int planid)
        {
            dateTo = dateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

            var _df = date.AddMinutes(-tzoffset);
            var _dt = dateTo.AddMinutes(-tzoffset);
            //var boxNos = new List<string>() { "2500","2501","2502"};
            var plansQuery = from x in this.context.ViewFlightPlans
                             where (x.DateFrom >= date && x.DateTo<=dateTo ) && x.CustomerId == cid
                             select x;
            var flightsQuery =// from x in this.context.ViewFlightPlanItems
                 from x in this.context.ViewFlightInformations
                 where (x.STD >= _df && x.STA<=_dt) && x.CustomerId == cid
                 select x;
            //if (design)
            //{
            //    plansQuery = plansQuery.Where(q => q.IsApproved50 == 0);
            //    flightsQuery = flightsQuery.Where(q => q.IsApproved50 == 0);
            //}
            if (planid != -1)
                flightsQuery = flightsQuery.Where(q => q.FlightPlanId == planid);
          //  flightsQuery = flightsQuery.Where(q => q.IsApproved50 == 1);
          //  var plans = await this.context.FlightPlans.ToListAsync();


            //var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == pid).ToListAsync();
            var flights = await flightsQuery.ToListAsync();

            var boxedFlights = flights.Where(q => q.BoxId != null).ToList();
            var unboxedFlights = flights.Where(q => q.BoxId == null).ToList();

            var flightsdto = new List<ViewModels.ViewFlightInformationDto>(); //new List<ViewModels.ViewFlightPlanItemDto>();

            foreach (var x in unboxedFlights)
            {
                // ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                //  ViewModels.ViewFlightPlanItemDto.FillDto(x, dto, tzoffset);

                ViewModels.ViewFlightInformationDto dto = new ViewFlightInformationDto();
                ViewModels.ViewFlightInformationDto.FillDto(x, dto, tzoffset);
                dto.HasCrew = false;
                dto.HasCrewProblem = false;
                dto.AllCrewAssigned = false;
                flightsdto.Add(dto);
                //if (boxNos.IndexOf(x.FlightNumber) == -1)
                //{

                //    flightsdto.Add(dto);
                //}
                //else
                //{
                //    box.Add(x);
                //    boxItems.Add(dto);
                //}

            }
            //oks
            var boxes = (from x in boxedFlights
                         group x by x.BoxId into g
                         select g).ToList();
            var boxids = boxes.Select(q => q.Key).ToList();
            var boxReqs = await this.context.ViewBoxCrewRequirements.Where(q => boxids.Contains(q.Id) && q.Assigned != q.Min).Select(q => q.Id).ToListAsync();
            var viewboxes = await this.context.ViewBoxes.Where(q => boxids.Contains(q.Id)).ToListAsync();
            foreach (var group in boxes)
            {
                var box = group.OrderBy(q => q.STD).ToList();
                var boxedFlight = new ViewFlightInformation(); //new ViewFlightPlanItem();
                string str = JsonConvert.SerializeObject(box.First());
                boxedFlight = JsonConvert.DeserializeObject<ViewFlightInformation>(str); //JsonConvert.DeserializeObject<ViewFlightPlanItem>(str);
                var boxItems = new List<ViewFlightInformationDto>(); //new List<ViewFlightPlanItemDto>();
                foreach (var _f in box)
                {
                    // ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                    //  ViewModels.ViewFlightPlanItemDto.FillDto(_f, dto, tzoffset);

                    ViewModels.ViewFlightInformationDto dto = new ViewFlightInformationDto();
                    ViewModels.ViewFlightInformationDto.FillDto(_f, dto, tzoffset);
                    boxItems.Add(dto);
                }
                boxedFlight.taskId = 1000000 + boxedFlight.taskId;
                boxedFlight.ID = 1000000 + boxedFlight.ID;
                boxedFlight.FlightNumber = "ATI" + boxedFlight.FlightNumber;
                boxedFlight.ToAirport = box.Last().ToAirport;
                boxedFlight.ToAirportCity = box.Last().ToAirportCity;
                boxedFlight.ToAirportCityId = box.Last().ToAirportCityId;
                boxedFlight.ToAirportIATA = box.Last().ToAirportIATA;
                boxedFlight.ToAirportName = box.Last().ToAirportName;
                boxedFlight.ToCity = box.Last().ToCity;
                boxedFlight.ToCountry = box.Last().ToCountry;
                boxedFlight.ToLatitude = box.Last().ToLatitude;
                boxedFlight.ToLongitude = box.Last().ToLongitude;
                boxedFlight.ToSortName = box.Last().ToSortName;
                boxedFlight.to = box.Last().ToAirportIATA;
                boxedFlight.STA = box.Last().STA;
                boxedFlight.duration = Convert.ToDecimal(((DateTime)box.Last().STA).Subtract((DateTime)boxedFlight.STD).TotalHours);
                boxedFlight.FlightH = Convert.ToInt32(Math.Truncate((decimal)boxedFlight.duration));
                boxedFlight.FlightM = Convert.ToByte((boxedFlight.duration - Math.Truncate((decimal)boxedFlight.duration)) * 60);
                boxedFlight.taskName = boxedFlight.FromAirportIATA + " -" + boxedFlight.FlightNumber + "- " + boxedFlight.ToAirportIATA;
               // boxedFlight.Date = viewbox.Date; //((DateTime)box.First().STD).Date;
                //ViewModels.ViewFlightPlanItemDto bdto = new ViewFlightPlanItemDto();
                // ViewModels.ViewFlightPlanItemDto.FillDto(boxedFlight, bdto, tzoffset);

                ViewModels.ViewFlightInformationDto bdto = new ViewFlightInformationDto();
                ViewModels.ViewFlightInformationDto.FillDto(boxedFlight, bdto, tzoffset);

                bdto.IsBox = true;
                bdto.BoxId = box.First().BoxId;
                var viewbox = viewboxes.FirstOrDefault(q => q.Id == bdto.BoxId);
                bdto.Date = viewbox.Date;
                bdto.Duty = viewbox.Duty;
                bdto.Flight = viewbox.Flight;
                bdto.IsDutyOver = viewbox.IsDutyOver;
                bdto.WOCLError = viewbox.WOCLError;
                bdto.MaxFDPExtended = viewbox.MaxFDPExtended;
                bdto.BoxItems = boxItems.ToList();
                bdto.HasCrew = viewbox.AssignedCrewCount>0; //box.First().AssignedCrewCount != 0;
                bdto.HasCrewProblem = viewbox.CrewProblemCount>0; //box.First().AssignedCrewProblemCount != 0;
                bdto.AllCrewAssigned = viewbox.NotAllAssignedCount==0; //!boxReqs.Contains((int)bdto.BoxId);
                flightsdto.Add(bdto);

            }

            //var box = new List<ViewFlightPlanItem>();

            //box = box.OrderBy(q => q.STD).ToList();








            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var resgroupsList = resgroups.ToList();
            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new resource() { resourceId =(int) grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId, assigned = false, registers = string.Empty }).ToList();

            ////var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }
            //var planIds = plans.Select(q => q.Id).ToList();
            //var _registers = await this.context.ViewFlightPlanRegisters.Where(q => planIds.Contains(q.FlightPlanId)).ToListAsync();
            //var grpRegisters = (from x in _registers
            //                    group x by x.PlannedRegisterId into g
            //                    select g).ToList();
            //foreach (var g in grpRegisters)
            //{
            //    var res = ressq.FirstOrDefault(q => q.resourceId == g.Key);
            //    if (res != null)
            //    {
            //        res.registers = string.Join(", ", g.Select(q => q.Register).ToList());
            //        var dates = g.OrderBy(q => q.DateFrom).ToList();
            //        var plan = plans.FirstOrDefault(q => q.Id == g.FirstOrDefault().FlightPlanId);
            //        if (dates.First().DateFrom.Date == ((DateTime)plan.DateFrom).Date && dates.Last().DateTo.Date == ((DateTime)plan.DateTo).Date)
            //            res.assigned = true;
            //    }
            //}
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroupsList,
                resources = ressq,
                //   registers = _registers,
            };
            return result;
        }
        internal async Task<object> GetPlanItemsGanttCrewTest(DateTime date, int tzoffset, bool design, int cid, int planid)
        {
            //var boxNos = new List<string>() { "2500","2501","2502"};
            var plansQuery = from x in this.context.ViewFlightPlans
                             where x.DateFrom == date && x.CustomerId == cid
                             select x;
            var flightsQuery =// from x in this.context.ViewFlightPlanItems
                 from x in this.context.ViewFlightPlanItemCalanders
                 where x.Date == date && x.CustomerId == cid
                 select x;
            if (design)
            {
                plansQuery = plansQuery.Where(q => q.IsApproved50 == 0);
                flightsQuery = flightsQuery.Where(q => q.IsApproved50 == 0);
            }
            if (planid != -1)
                flightsQuery = flightsQuery.Where(q => q.FlightPlanId == planid);
            flightsQuery = flightsQuery.Where(q => q.IsApproved50 == 1);
            var plans = await this.context.FlightPlans.ToListAsync();


            //var flights = await this.context.ViewFlightPlanItems.Where(q => q.FlightPlanId == pid).ToListAsync();
            var flights = await flightsQuery.ToListAsync();

            var boxedFlights = flights.Where(q => q.BoxId != null).ToList();
            var unboxedFlights = flights.Where(q => q.BoxId == null).ToList();

            var flightsdto = new List<ViewModels.ViewFlightPlanItemCalanderDto>(); //new List<ViewModels.ViewFlightPlanItemDto>();

            foreach (var x in unboxedFlights)
            {
                // ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                //  ViewModels.ViewFlightPlanItemDto.FillDto(x, dto, tzoffset);

                ViewModels.ViewFlightPlanItemCalanderDto dto = new ViewFlightPlanItemCalanderDto();
                ViewModels.ViewFlightPlanItemCalanderDto.FillDto(x, dto, tzoffset);
                dto.HasCrew = false;
                dto.HasCrewProblem = false;
                dto.AllCrewAssigned = false;
                flightsdto.Add(dto);
                //if (boxNos.IndexOf(x.FlightNumber) == -1)
                //{

                //    flightsdto.Add(dto);
                //}
                //else
                //{
                //    box.Add(x);
                //    boxItems.Add(dto);
                //}

            }

            var boxes = (from x in boxedFlights
                         group x by x.BoxId into g
                         select g).ToList();
            var boxids = boxes.Select(q => q.Key).ToList();
            var boxReqs = await this.context.ViewBoxCrewRequirements.Where(q => boxids.Contains(q.Id) && q.Assigned != q.Min).Select(q => q.Id).ToListAsync();

            foreach (var group in boxes)
            {
                var box = group.OrderBy(q => q.STD).ToList();
                var boxedFlight = new ViewFlightPlanItemCalander(); //new ViewFlightPlanItem();
                string str = JsonConvert.SerializeObject(box.First());
                boxedFlight = JsonConvert.DeserializeObject<ViewFlightPlanItemCalander>(str); //JsonConvert.DeserializeObject<ViewFlightPlanItem>(str);
                var boxItems = new List<ViewFlightPlanItemCalanderDto>(); //new List<ViewFlightPlanItemDto>();
                foreach (var _f in box)
                {
                    // ViewModels.ViewFlightPlanItemDto dto = new ViewFlightPlanItemDto();
                    //  ViewModels.ViewFlightPlanItemDto.FillDto(_f, dto, tzoffset);

                    ViewModels.ViewFlightPlanItemCalanderDto dto = new ViewFlightPlanItemCalanderDto();
                    ViewModels.ViewFlightPlanItemCalanderDto.FillDto(_f, dto, tzoffset);
                    boxItems.Add(dto);
                }
                boxedFlight.taskID = 1000000 + boxedFlight.taskID;
                boxedFlight.Id = 1000000 + boxedFlight.Id;
                boxedFlight.FlightNumber = "ATI" + boxedFlight.FlightNumber;
                boxedFlight.ToAirport = box.Last().ToAirport;
                boxedFlight.ToAirportCity = box.Last().ToAirportCity;
                boxedFlight.ToAirportCityId = box.Last().ToAirportCityId;
                boxedFlight.ToAirportIATA = box.Last().ToAirportIATA;
                boxedFlight.ToAirportName = box.Last().ToAirportName;
                boxedFlight.ToCity = box.Last().ToCity;
                boxedFlight.ToCountry = box.Last().ToCountry;
                boxedFlight.ToLatitude = box.Last().ToLatitude;
                boxedFlight.ToLongitude = box.Last().ToLongitude;
                boxedFlight.ToSortName = box.Last().ToSortName;
                boxedFlight.to = box.Last().ToAirportIATA;
                boxedFlight.STA = box.Last().STA;
                boxedFlight.duration = Convert.ToDecimal(((DateTime)box.Last().STA).Subtract((DateTime)boxedFlight.STD).TotalHours);
                boxedFlight.FlightH = Convert.ToInt32(Math.Truncate((decimal)boxedFlight.duration));
                boxedFlight.FlightM = Convert.ToInt32((boxedFlight.duration - Math.Truncate((decimal)boxedFlight.duration)) * 60);
                boxedFlight.taskName = boxedFlight.FromAirportIATA + " -" + boxedFlight.FlightNumber + "- " + boxedFlight.ToAirportIATA;

                //ViewModels.ViewFlightPlanItemDto bdto = new ViewFlightPlanItemDto();
                // ViewModels.ViewFlightPlanItemDto.FillDto(boxedFlight, bdto, tzoffset);

                ViewModels.ViewFlightPlanItemCalanderDto bdto = new ViewFlightPlanItemCalanderDto();
                ViewModels.ViewFlightPlanItemCalanderDto.FillDto(boxedFlight, bdto, tzoffset);

                bdto.IsBox = true;
                bdto.BoxId = box.First().BoxId;
                bdto.BoxItems = boxItems.ToList();
                bdto.HasCrew = box.First().AssignedCrewCount != 0;
                bdto.HasCrewProblem = box.First().AssignedCrewProblemCount != 0;
                bdto.AllCrewAssigned = !boxReqs.Contains((int)bdto.BoxId);
                flightsdto.Add(bdto);

            }

            //var box = new List<ViewFlightPlanItem>();

            //box = box.OrderBy(q => q.STD).ToList();








            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var resgroupsList = resgroups.ToList();
            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new resource() { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId, assigned = false, registers = string.Empty }).ToList();

            ////var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }
            //var planIds = plans.Select(q => q.Id).ToList();
            //var _registers = await this.context.ViewFlightPlanRegisters.Where(q => planIds.Contains(q.FlightPlanId)).ToListAsync();
            //var grpRegisters = (from x in _registers
            //                    group x by x.PlannedRegisterId into g
            //                    select g).ToList();
            //foreach (var g in grpRegisters)
            //{
            //    var res = ressq.FirstOrDefault(q => q.resourceId == g.Key);
            //    if (res != null)
            //    {
            //        res.registers = string.Join(", ", g.Select(q => q.Register).ToList());
            //        var dates = g.OrderBy(q => q.DateFrom).ToList();
            //        var plan = plans.FirstOrDefault(q => q.Id == g.FirstOrDefault().FlightPlanId);
            //        if (dates.First().DateFrom.Date == ((DateTime)plan.DateFrom).Date && dates.Last().DateTo.Date == ((DateTime)plan.DateTo).Date)
            //            res.assigned = true;
            //    }
            //}
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroupsList,
                resources = ressq,
                //   registers = _registers,
            };
            return result;
        }

        internal async Task<object> GetFlightGantt(int cid, DateTime dateFrom, DateTime dateTo, int tzoffset, int? airport, ViewModels.FlightsFilter filter)
        {
            var flightsQuery = this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.STD >= dateFrom && q.STD <= dateTo && q.RegisterID != null);
           // var flightsQueryAll = this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.STD >= dateFrom && q.STD <= dateTo && q.RegisterID != null);
            if (airport != null)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            if (filter != null)
            {
                if (filter.Status != null && filter.Status.Count > 0)
                    flightsQuery = flightsQuery.Where(q => filter.Status.Contains(q.FlightStatusID));
                if (filter.Types != null && filter.Types.Count > 0)
                    flightsQuery = flightsQuery.Where(q => filter.Types.Contains(q.TypeId));
                if (filter.Registers != null && filter.Registers.Count > 0)
                    flightsQuery = flightsQuery.Where(q => filter.Registers.Contains(q.RegisterID));
                if (filter.From != null && filter.From.Count > 0)
                    flightsQuery = flightsQuery.Where(q => filter.From.Contains(q.FromAirport));
                if (filter.To != null && filter.To.Count > 0)
                    flightsQuery = flightsQuery.Where(q => filter.To.Contains(q.ToAirport));
            }
            var flights = await flightsQuery.OrderBy(q=>q.STD).ToListAsync();
            var f11367 = flights.FirstOrDefault(q => q.ID == 11367);
            var flightsdto = new List<ViewModels.ViewFlightInformationDto>();
            foreach (var x in flights)
            {
                ViewModels.ViewFlightInformationDto dto = new ViewFlightInformationDto();
                ViewModels.ViewFlightInformationDto.FillDto(x, dto, tzoffset);
                flightsdto.Add(dto);
            }

            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };

            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();
            //var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }

            //var baseSum = (from x in flights
            //              group x by new { x.BaseId, x.BaseIATA, x.BaseName } into g
            //              select new
            //              {
            //                  BaseId=g.Key.BaseId,
            //                  BaseIATA=g.Key.BaseIATA,
            //                  BaseName=g.Key.BaseName,
            //                  Total=g.Count(),
            //                  TakeOff=g.Where(q=>q.Takeoff!=null).Count(),
            //                  Landing=g.Where(q=>q.Landing!=null).Count(),
            //                  Canceled=g.Where(q=>q.FlightStatusID==4).Count(),
            //                  Redirected= g.Where(q => q.FlightStatusID == 17).Count(),
            //                  Diverted = g.Where(q => q.FlightStatusID == 7).Count(),
            //                  TotalDelays=g.Where(q=>q.ChocksOut!=null).Sum(q=>q.DelayOffBlock),
            //                  DepartedPax= g.Where(q => q.Takeoff != null).Sum(q=>q.TotalPax),
            //                  ArrivedPax= g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

            //              }).ToList();
            var fromAirport = (from x in flights
                           group x by new { x.FromAirport, x.FromAirportIATA, x.FromAirportName } into g
                           select new BaseSummary()
                           {
                               BaseId = g.Key.FromAirport,
                               BaseIATA = g.Key.FromAirportIATA,
                               BaseName = g.Key.FromAirportName,
                               Total = g.Count(),
                               TakeOff = g.Where(q => q.Takeoff != null).Count(),
                               Landing =0, //g.Where(q => q.Landing != null).Count(),
                               Canceled = g.Where(q => q.FlightStatusID == 4).Count(),
                               Redirected = g.Where(q => q.FlightStatusID == 17).Count(),
                               Diverted = g.Where(q => q.FlightStatusID == 7).Count(),
                               TotalDelays = g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                               DepartedPax = g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                               ArrivedPax =0,// g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                           }).ToList();
            var toAirport = (from x in flights
                               group x by new { x.ToAirport, x.ToAirportIATA, x.ToAirportName } into g
                               select new BaseSummary()
                               {
                                   BaseId = g.Key.ToAirport,
                                   BaseIATA = g.Key.ToAirportIATA,
                                   BaseName = g.Key.ToAirportName,
                                   Total = g.Count(),
                                   TakeOff = 0,//g.Where(q => q.Takeoff != null).Count(),
                                   Landing = g.Where(q => q.Landing != null).Count(),
                                   Canceled = 0,//g.Where(q => q.FlightStatusID == 4).Count(),
                                   Redirected =0,// g.Where(q => q.FlightStatusID == 17).Count(),
                                   Diverted =0,// g.Where(q => q.FlightStatusID == 7).Count(),
                                   TotalDelays =0,// g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                                   DepartedPax =0,// g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                                   ArrivedPax =  g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                               }).ToList();

            var baseSum = new List<BaseSummary>();
            foreach (var x in fromAirport)
            {
                var to = toAirport.FirstOrDefault(q => q.BaseId == x.BaseId);
                if (to != null)
                {
                    x.ArrivedPax += to.ArrivedPax;
                    x.Canceled += to.Canceled;
                    x.DepartedPax += to.DepartedPax;
                    x.Diverted += to.Diverted;
                    x.Landing += to.Landing;
                    x.Redirected += to.Redirected;
                    x.TakeOff += to.TakeOff;
                    x.Total += to.Total;
                    x.TotalDelays += to.TotalDelays;
                    
                }
               
                    baseSum.Add(x);
            }
            var f9641 = flightsdto.FirstOrDefault(q => q.ID == 9641);
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroups.ToList(),
                resources = ressq,
                baseSummary=baseSum,
            };
            return result;
        }
        public async Task<CustomActionResult> GetUpdatedFlights(int airport, DateTime baseDate, DateTime? fromDate, DateTime? toDate, int customer, int tzoffset, int userid)
        {
            baseDate = baseDate.ToUniversalTime();
            fromDate = ((DateTime)fromDate).ToUniversalTime();
            toDate = ((DateTime)toDate).ToUniversalTime();


            var query = from x in this.context.ViewFlightInformations
                        where x.STD >= fromDate && x.STA <= toDate && x.CustomerId == customer && x.DateStatus > baseDate && x.FlightStatusUserId != userid
                        select x;
            if (airport != -1)
            {
                query = query.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            }

            var flightsQuery = this.context.ViewFlightInformations.Where(q => q.CustomerId == customer && q.STD >= fromDate && q.STD <= toDate && q.RegisterID != null);
         
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);


           


            var _flights = await query.ToListAsync();
            if (_flights.Count > 0)
            {
                //var baseSum = (from x in flightsQuery
                //               group x by new { x.BaseId, x.BaseIATA, x.BaseName } into g
                //               select new
                //               {
                //                   BaseId = g.Key.BaseId,
                //                   BaseIATA = g.Key.BaseIATA,
                //                   BaseName = g.Key.BaseName,
                //                   Total = g.Count(),
                //                   TakeOff = g.Where(q => q.Takeoff != null).Count(),
                //                   Landing = g.Where(q => q.Landing != null).Count(),
                //                   Canceled = g.Where(q => q.FlightStatusID == 4).Count(),
                //                   Redirected = g.Where(q => q.FlightStatusID == 17).Count(),
                //                   Diverted = g.Where(q => q.FlightStatusID == 7).Count(),
                //                   TotalDelays = g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                //                   DepartedPax = g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                //                   ArrivedPax = g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                //               }).ToList();
                /////////////////////////////////////////////////////
                var fromAirport = (from x in flightsQuery
                                   group x by new { x.FromAirport, x.FromAirportIATA, x.FromAirportName } into g
                                   select new BaseSummary()
                                   {
                                       BaseId = g.Key.FromAirport,
                                       BaseIATA = g.Key.FromAirportIATA,
                                       BaseName = g.Key.FromAirportName,
                                       Total = g.Count(),
                                       TakeOff = g.Where(q => q.Takeoff != null).Count(),
                                       Landing = 0, //g.Where(q => q.Landing != null).Count(),
                                       Canceled = g.Where(q => q.FlightStatusID == 4).Count(),
                                       Redirected = g.Where(q => q.FlightStatusID == 17).Count(),
                                       Diverted = g.Where(q => q.FlightStatusID == 7).Count(),
                                       TotalDelays = g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                                       DepartedPax = g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                                       ArrivedPax = 0,// g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                                   }).ToList();
                var toAirport = (from x in flightsQuery
                                 group x by new { x.ToAirport, x.ToAirportIATA, x.ToAirportName } into g
                                 select new BaseSummary()
                                 {
                                     BaseId = g.Key.ToAirport,
                                     BaseIATA = g.Key.ToAirportIATA,
                                     BaseName = g.Key.ToAirportName,
                                     Total = g.Count(),
                                     TakeOff = 0,//g.Where(q => q.Takeoff != null).Count(),
                                     Landing = g.Where(q => q.Landing != null).Count(),
                                     Canceled = 0,//g.Where(q => q.FlightStatusID == 4).Count(),
                                     Redirected = 0,// g.Where(q => q.FlightStatusID == 17).Count(),
                                     Diverted = 0,// g.Where(q => q.FlightStatusID == 7).Count(),
                                     TotalDelays = 0,// g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                                     DepartedPax = 0,// g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                                     ArrivedPax = g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                                 }).ToList();

                var baseSum = new List<BaseSummary>();
                foreach (var x in fromAirport)
                {
                    var to = toAirport.FirstOrDefault(q => q.BaseId == x.BaseId);
                    if (to != null)
                    {
                        x.ArrivedPax += to.ArrivedPax;
                        x.Canceled += to.Canceled;
                        x.DepartedPax += to.DepartedPax;
                        x.Diverted += to.Diverted;
                        x.Landing += to.Landing;
                        x.Redirected += to.Redirected;
                        x.TakeOff += to.TakeOff;
                        x.Total += to.Total;
                        x.TotalDelays += to.TotalDelays;

                    }

                    baseSum.Add(x);
                }
                /////////////////////////////////////////////////////
                var result = new
                {
                    flights = _flights.Select(q => ViewFlightInformationDto.GetDto(q, tzoffset)).ToList(),
                    summary = baseSum,
                };
                //  return new CustomActionResult(HttpStatusCode.OK, flights.Select(q => ViewFlightInformationDto.GetDto(q, tzoffset)).ToList());
                return new CustomActionResult(HttpStatusCode.OK, result);
            }
            else
            {
                var result = new
                {
                    flights = _flights.Select(q => ViewFlightInformationDto.GetDto(q, tzoffset)).ToList(),
                    summary = -1,
                };
                return new CustomActionResult(HttpStatusCode.OK, result);
            }

           
        }




        public async Task<dynamic> GetFlightsSummary(int cid,DateTime date)
        {
            var _count = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).CountAsync();
            var _departed = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date && q.Takeoff!=null).CountAsync();
            var _arrived = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date && q.Landing != null).CountAsync();
            var _canceled=   await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date && q.FlightStatusID==4).CountAsync();
            var _redirected = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date && (q.FlightStatusID == 7 || q.FlightStatusID==17)).CountAsync();
            var _plannedtime = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.duration*60).SumAsync();
            var _actualtime = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.ActualFlightHOffBlock!=null? q.ActualFlightHOffBlock * 60+q.ActualFlightMOffBlock:0).SumAsync();
            var _delay1 = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.DelayOffBlock == null ? 0 : q.DelayOffBlock).SumAsync();
            var _delay2 =await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.DelayOnBlock == null ? 0 : q.DelayOnBlock).SumAsync();
            var _delaytotal = _delay1 + _delay2;
            var _paxadult= await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.PaxAdult == null ? 0 : q.PaxAdult).SumAsync();
            var _paxchild = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.PaxChild == null ? 0 : q.PaxChild).SumAsync();
            var _paxinfant = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => q.PaxInfant == null ? 0 : q.PaxInfant).SumAsync();
            var _paxtotal = _paxadult + _paxchild + _paxinfant;
            var _fuel = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => (q.FuelDeparture !=null && q.FuelArrival!=null)? q.FuelDeparture -q.FuelArrival: 0).SumAsync();
            var _cargo = await this.context.ViewFlightInformations.Where(q => q.CustomerId == cid && q.Date == date).Select(q => (q.CargoWeight!=null?q.CargoWeight:0)+(q.BaggageWeight!=null?q.BaggageWeight:0)).SumAsync();
            TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble( _delaytotal));
            var _delaytotalstr = span.ToString(@"hh\:mm");

            var mostDelayQuery = await (from x in this.context.ViewFlightInformations
                                        where x.CustomerId == cid && x.Date == date && x.DelayOffBlock!=null
                                        orderby x.DelayOffBlock descending
                                        select new {x.ID,x.DelayOffBlock,x.FromAirportIATA,x.ToAirportIATA,x.Register,x.FlightNumber, Remark=x.Register+" "+ x.FromAirportIATA+"-"+x.FlightNumber+"-"+x.ToAirportIATA }).Take(10).ToListAsync();

            var paxLoadQuery = await (from x in this.context.ViewFlightInformations
                                        where x.CustomerId == cid && x.Date == date && x.TotalPax != null && x.PaxAdult!=null
                                        orderby x.TotalPax 
                                        select new { x.ID, x.TotalPax,x.TotalSeat,
                                             PaxLoad=  x.TotalPax/x.TotalSeat ,
                                            x.FromAirportIATA, x.ToAirportIATA, x.Register, x.FlightNumber, Remark = x.Register + " " + x.FromAirportIATA + "-" + x.FlightNumber + "-" + x.ToAirportIATA }).Take(10).ToListAsync();


            var result = new
            {
                count = _count,
                departed = _departed,
                arrived = _arrived,
                canceled = _canceled,
                redirected = _redirected,
                plannedtime=Math.Round(Convert.ToDouble( _plannedtime)),
                actualtime=Math.Round(Convert.ToDouble( _actualtime)),
                delay1=_delay1,
                delay2=_delay2,
                delaytotal=_delaytotal,
                delaytotalstr=_delaytotalstr,
                paxadult=_paxadult,
                paxchild=_paxchild,
                paxinfant=_paxinfant,
                paxtotal=_paxtotal,
                fuel=_fuel,
                cargo=_cargo,
                topdelays=mostDelayQuery,
                paxload=paxLoadQuery,
            };
            return  result ;
        }

        //public async Task<ViewModels.Ac_MSNDto> GetDto(int id)
        //{
        //    var msndto = new ViewModels.Ac_MSNDto();
        //    var dbmsn = await context.Ac_MSN.FirstOrDefaultAsync(q => q.ID == id);
        //    ViewModels.Ac_MSNDto.FillDto(dbmsn, msndto);
        //    return msndto;
        //}

        internal virtual CustomActionResult ValidateFlightPlan(FlightPlanDto dto)
        {
            return new CustomActionResult(HttpStatusCode.OK, "");
        }
        internal virtual CustomActionResult ValidateFlight(FlightDto dto)
        {
            return new CustomActionResult(HttpStatusCode.OK, "");
        }

        //internal virtual CustomActionResult CanDelete(Ac_MSN entity)
        //{
        //    return new CustomActionResult(HttpStatusCode.OK, "");
        //}
    }
}