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
using Newtonsoft.Json;

namespace EPAAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FlightController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("odata/flights/actypes/{customer}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightsAcType> GetViewFlightsAcTypes(int customer)
        {

            return unitOfWork.FlightRepository.GetViewFlightsAcTypes().Where(q => q.CustomerId == customer);

        }

        [Route("odata/flights/routes/airline/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightRoute> GetViewFlightRoutes(int id)
        {

            return unitOfWork.FlightRepository.GetViewFlightRoutes().Where(q => q.AirlineId == id);

        }

        [Route("odata/route/{from}/{to}")]
        [EnableQuery]
        // [Authorize]
        public IHttpActionResult GetViewFlightRoute(int from, int to)
        {


            var result = unitOfWork.FlightRepository.GetViewFlightRoutes().Where(q => q.FromAirportId == from && q.ToAirportId == to).FirstOrDefault();
            return Ok(result);

        }

        [Route("odata/fuel/report/")]
        [EnableQuery]

        public IQueryable<ViewFlightFuel> GetViewFlightFuel(DateTime df, DateTime dt)
        {
            df = df.Date;
            dt = dt.Date;
            var result = unitOfWork.FlightRepository.GetViewFlightFuel().Where(q => q.Date >= df && q.Date <= dt).ToList();
            return result.AsQueryable();

        }


        [Route("odata/crew/report/main")]
        [EnableQuery]

        public IQueryable<ViewCrewTimeDetail> GetViewCrewTimeDetails(DateTime date,string jc)
        {
            date = date.Date;
            var result = unitOfWork.FlightRepository.GetViewCrewTimeDetail().Where(q => q.CDate == date);
            if (!string.IsNullOrEmpty(jc))
            {
                result = result.Where(q => q.JobGroupCode.StartsWith(jc));
            }
            return result.OrderBy(q=>q.JobGroupCode).ThenBy(q=>q.Name);

        }


        [Route("odata/flights/plan/item/permits/{id}/{calanderId}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanItemPermit> GetViewFlightPlanItemPermit(int id, int calanderId)
        {

            return unitOfWork.FlightRepository.GetViewFlightPlanItemPermits().Where(q => q.Id == id && q.CalanderId == calanderId);

        }

        [Route("odata/flights/plan/permits/{id}/{calanderId}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanItemPermit> GetViewFlightPlanPermit(int id, int calanderId)
        {

            return unitOfWork.FlightRepository.GetViewFlightPlanItemPermits().Where(q => q.FlightPlanId == id && q.CalanderId == calanderId);

        }



        [Route("odata/flights/plan/permits/")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<FlightPermit> GetPlanPermits()
        {

            return unitOfWork.FlightRepository.GetFlightPermits();

        }


        [Route("odata/flights/{cid}/{airport}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetViewFlights(int cid, int airport)
        {

            var flightsQuery = unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid);
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            return flightsQuery;

        }
        [Route("odata/flights/grouped/{cid}/{from}/{to}")]
        [EnableQuery]
        // [Authorize]
        public async Task<IHttpActionResult> GetFlightsGrouped(int cid, string from, string to)
        {

            var result = await this.unitOfWork.FlightRepository.GetFlightsGrouped(cid, from, to);
            return Ok(result);

        }

        [Route("odata/flights/register/{cid}/{airport}/{register}/{from}/{to}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetViewFlightsByRegister(int cid, int airport, int register, string from, string to)
        {

            var flightsQuery = unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid && q.RegisterID == register);
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            if (from != "-1" && to != "-1")
            {
                DateTime dateFrom = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(from).Date;
                DateTime dateTo = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(to).Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                flightsQuery = flightsQuery.Where(q => q.STA >= dateFrom && q.STA <= dateTo);
            }
            return flightsQuery;

        }


        [Route("odata/flights/irregular/{cid}/{airport}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetViewIrregularFlights(int cid, int airport)
        {

            var flightsQuery = unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid && q.FlightPlanId == null);
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            return flightsQuery;

        }
        [Route("odata/flights/{cid}/{airport}/{std}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetViewFlights(int cid, int airport, string std)
        {
            DateTime dateSTD = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(std).Date;
            DateTime dateStD2 = dateSTD.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            var flightsQuery = unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid && q.STDDay >= dateSTD && q.STDDay <= dateStD2);
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            return flightsQuery;

        }
        [Route("odata/flights/abnormal/{cid}/{airport}/{std}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetViewAbnormalFlights(int cid, int airport, string std)
        {
            DateTime dateSTD = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(std).Date;
            DateTime dateStD2 = dateSTD.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            var flightsQuery = unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid && q.STDDay >= dateSTD && q.STDDay <= dateStD2
             && (q.FlightStatusID == 4 || q.FlightStatusID == 7 || q.FlightStatusID == 17 || q.RedirectReasonId != null)
            );
            if (airport != -1)
                flightsQuery = flightsQuery.Where(q => q.FromAirport == airport || q.ToAirport == airport);
            return flightsQuery;

        }
        [Route("odata/flightplans/registers/assigned/{id}/{tzoffset}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlighPlanAssignedRegister> GetViewFlightPlanRegisterAssigneds(int id, int tzoffset)
        {

            var result = unitOfWork.FlightRepository.GetViewFlightPlanAssignedRegisters().Where(q => q.FlightPlanId == id).ToList();
            foreach (var x in result)
            {
                x.STA = ((DateTime)x.STA).AddMinutes(tzoffset);
                x.STD = ((DateTime)x.STD).AddMinutes(tzoffset);
            }
            return result.AsQueryable();

        }

        [Route("odata/flights/routes/destination/airline/{id}/{from}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightRoute> GetViewFlightRoutesDestination(int id, int from)
        {

            return unitOfWork.FlightRepository.GetViewFlightRoutes().Where(q => q.AirlineId == id && q.FromAirportId == from);

        }
        [Route("odata/flights/routes/from/airline/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewRouteFromAirport> GetViewRouteFromAirport(int id)
        {

            return unitOfWork.FlightRepository.GetViewRouteFromAirport().Where(q => q.AirlineId == id);

        }
        [Route("odata/flights/registers/{customer}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightsRegister> GetViewFlightsRegisters(int customer)
        {

            return unitOfWork.FlightRepository.GetViewFlightsRegisters().Where(q => q.CustomerId == customer);

        }
        [Route("odata/flights/from/{customer}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightsFrom> GetViewFlightsFrom(int customer)
        {

            return unitOfWork.FlightRepository.GetViewFlightsFrom().Where(q => q.CustomerId == customer);

        }
        [Route("odata/flights/to/{customer}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightsTo> GetViewFlightsTo(int customer)
        {

            return unitOfWork.FlightRepository.GetViewFlightsTo().Where(q => q.CustomerId == customer);

        }

        [Route("odata/flights/delaycodes")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewDelayCode> GetFlightDelayCodes()
        {
            try
            {
                var result = unitOfWork.FlightRepository.GetViewDelayCodes().OrderBy(q => q.DelayCategoryId).ThenBy(q => q.Code).ToList();
                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }



        [Route("odata/flights/delaycodes/{flightId}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightDelayCode> GetFlightDelayCodes(int flightId)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlightDelayCode(flightId).OrderBy(q => q.DelayCodeId);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }


        [Route("odata/flights/customer/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetFlightsByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlights().Where(q => q.CustomerId == cid);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        [Route("odata/flights/box/{bid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightInformation> GetFlightsByBoxId(int bid)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlights().Where(q => q.BoxId == bid).OrderBy(q => q.STD);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/flight/{id}/{tzoffset}")]
        public async Task<IHttpActionResult> GetViewFlightDto(int id, int tzoffset)
        {
            var flight = await unitOfWork.FlightRepository.GetViewFlights().FirstOrDefaultAsync(q => q.ID == id);
            if (flight == null)
                return NotFound();
            var dto = ViewModels.ViewFlightInformationDto.GetDto(flight, tzoffset);


            return Ok(dto);
        }

        [Route("odata/crew/summary/{id}")]
        public async Task<IHttpActionResult> GetCrewSummary(int id)
        {

            var result = await unitOfWork.FlightRepository.GetCrewSummary(id);


            return Ok(result.data);
        }


        [Route("odata/flightplans/customer/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlan> GetFlightPlansByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        [Route("odata/flightplans/calendar/{cid}/{type}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanCalander> GetFlightPlansCalendarByCustomerId(int cid, int type)
        {

            try
            {
                switch (type)
                {
                    case -1:
                        return unitOfWork.FlightRepository.GetViewFlightPlansCalendar().Where(q => q.CustomerId == cid);
                    case 50:
                        return unitOfWork.FlightRepository.GetViewFlightPlansCalendar().Where(q => q.CustomerId == cid && q.IsApproved50 == 1);
                    case 60:
                        return unitOfWork.FlightRepository.GetViewFlightPlansCalendar().Where(q => q.CustomerId == cid && q.IsApproved60 == 1);
                    case 70:
                        return unitOfWork.FlightRepository.GetViewFlightPlansCalendar().Where(q => q.CustomerId == cid && q.IsApproved70 == 1);
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }


        }


        [Route("odata/flight/plans/items/calendar/{cid}/{type}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanItemCalander> GetFlightPlanItemsCalendarByCustomerId(int cid, int type)
        {

            try
            {
                switch (type)
                {
                    case -1:
                        return unitOfWork.FlightRepository.GetViewFlightPlanItemsCalander().Where(q => q.CustomerId == cid);
                    case 50:
                        return unitOfWork.FlightRepository.GetViewFlightPlanItemsCalander().Where(q => q.CustomerId == cid && q.IsApproved50 == 1);
                    case 60:
                        return unitOfWork.FlightRepository.GetViewFlightPlanItemsCalander().Where(q => q.CustomerId == cid && q.IsApproved60 == 1);
                    case 70:
                        return unitOfWork.FlightRepository.GetViewFlightPlanItemsCalander().Where(q => q.CustomerId == cid && q.IsApproved70 == 1);
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }


        }


        [Route("odata/flightplans/opened/customer/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlan> GetOpenedFlightPlansByCustomerId(int cid)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved50 == 0);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }





        [Route("odata/flightplans/items/opened/{cid}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanItem> GetOpenedFlightPlanItemsByCustomerId(int cid)
        {
            try
            {
                var xxxxxx = unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid && q.IsApproved50 == 0).ToList();
                //return unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid && q.IsApproved50 == 0);
                return xxxxxx.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        [Route("odata/flightplans/approved/customer/{cid}/{type}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlan> GetFlightPlansByCustomerId(int cid, int type)
        {
            try
            {
                switch (type)
                {
                    case -1:
                        return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid);
                    case 50:
                        return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved50 == 1);
                    case 60:
                        return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved60 == 1);
                    case 70:
                        return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved70 == 1);
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        //gati
        [Route("odata/plan/items/approved/{cid}/{type}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanItem> GetFlightPlanItemsByCustomerId(int cid, int type)
        {
            try
            {
                switch (type)
                {
                    case -1:
                        // return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid);
                        return unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid);
                    case 50:
                        // return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved50 == 1);
                        return unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid && q.IsApproved50 == 1);

                    case 60:
                        // return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved60 == 1);
                        return unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid && q.IsApproved60 == 1);
                    case 70:
                        // return unitOfWork.FlightRepository.GetViewFlightPlans().Where(q => q.CustomerId == cid && q.IsApproved70 == 1);
                        return unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.CustomerId == cid && q.IsApproved70 == 1);
                    default:
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }

        [Route("odata/flightplan/base/")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightPlanBase(dynamic dto)
        {
            var cid = Convert.ToInt32(dto.CustomerId);
            var date = ((DateTime)dto.Date).Date;
            var register = Convert.ToInt32(dto.RegisterId);
            var result = await unitOfWork.FlightRepository.GetPlanBase(cid, date, register);
            return Ok(result.data);
        }

        [Route("odata/flightplan/last/")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightPlanLastItem(dynamic dto)
        {
            var cid = Convert.ToInt32(dto.CustomerId);
            var date = ((DateTime)dto.Date).Date;
            var register = Convert.ToInt32(dto.RegisterId);
            var offset = Convert.ToInt32(dto.Offset);
            var result = await unitOfWork.FlightRepository.GetPlanLastItem(cid, date, register, offset);
            return Ok(result);
        }
        [Route("odata/flightplan/items/")]
        [AcceptVerbs("POST", "GET")]
        public IHttpActionResult GetFlightPlanItems(dynamic dto)
        {
            var cid = Convert.ToInt32(dto.CustomerId);
            var date = ((DateTime)dto.Date).Date;
            var register = Convert.ToInt32(dto.RegisterId);
            var offset = Convert.ToInt32(dto.Offset);
            var result = unitOfWork.FlightRepository.GetPlanItems(cid, date, register, offset);
            return Ok(result);
        }

        [Route("odata/flightplan/{id}")]

        public async Task<IHttpActionResult> GetFlightPlan(int id)
        {
            var result = await unitOfWork.FlightRepository.GetFlightPlanById(id);
            return Ok(result);
        }
        [Route("odata/flight/plan/item/{id}/{tzoffset}")]

        public async Task<IHttpActionResult> GetFlightPlanItem(int id, int tzoffset)
        {
            var x = await unitOfWork.FlightRepository.GetViewFlightPlanItems().FirstOrDefaultAsync(q => q.Id == id);
            if (x == null)
                return NotFound();
            x.STA = ((DateTime)x.STA).AddMinutes(tzoffset);
            x.STD = ((DateTime)x.STD).AddMinutes(tzoffset);
            return Ok(x);
        }
        [Route("odata/flightplan/summary/{id}/{tzoffset}")]
        public async Task<IHttpActionResult> GetFlightPlanSummary(int id, int tzoffset)
        {
            var result = await unitOfWork.FlightRepository.GetFlightPlanSummary(id, tzoffset);
            return Ok(result);
        }
        [Route("odata/flightplan/view/{id}")]
        public async Task<IHttpActionResult> GetViewFlightPlan(int id)
        {
            var _plan = await unitOfWork.FlightRepository.GetViewFlightPlans().FirstOrDefaultAsync(q => q.Id == id);
            var ms = await unitOfWork.FlightRepository.GetFlightPlanMonth(id);
            var ds = await unitOfWork.FlightRepository.GetFlightPlanDays(id);
            dynamic result = new { plan = _plan, Month = ms, Days = ds };


            return Ok(result);
        }

        [Route("odata/plan/checkerrors/{id}")]
        public IHttpActionResult GetFlightPlanErrors(int id)
        {
            var errors = unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.FlightPlanId == id && q.StatusId != 1).Count();
            var firstFlight = unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.FlightPlanId == id).OrderBy(q => q.STD).FirstOrDefault();
            var lastflight = unitOfWork.FlightRepository.GetViewFlightPlanItems().Where(q => q.FlightPlanId == id).OrderByDescending(q => q.STD).FirstOrDefault();
            var plan = unitOfWork.FlightRepository.GetViewFlightPlans().FirstOrDefault(q => q.Id == id);
            if (firstFlight.FromAirport != plan.BaseId)
                errors++;
            if (lastflight.ToAirport != plan.BaseId)
                errors++;


            return Ok(errors);
        }


        [Route("odata/flights/updated/")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightsGanttByCustomerId(dynamic dto)
        {


            var result = await unitOfWork.FlightRepository.GetUpdatedFlights(
                 (int)dto.airport,
                 (DateTime)dto.baseDate,
                 (DateTime)dto.from,
                 (DateTime)dto.to,
                 (int)dto.customer,
                 (int)dto.tzoffset,
                 (int)dto.userid
                );
            var data = result.data;
            return Ok(data);
        }


        [Route("odata/flights/gantt/customer/{cid}/{from}/{to}/{tzoffset}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightsGanttByCustomerId(int cid, string from, string to, int tzoffset)
        {
            DateTime dateFrom = EPAAPI.Helper.BuildDateTimeFromYAFormat(from);
            DateTime dateTo = EPAAPI.Helper.BuildDateTimeFromYAFormat(to);
            var result = await unitOfWork.FlightRepository.GetFlightGantt(cid, dateFrom, dateTo, tzoffset, null, null);
            return Ok(result);
        }
        [Route("odata/flights/gantt/customer/{cid}/{from}/{to}/{tzoffset}/{airport}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightsGanttByCustomerId(int cid, string from, string to, int tzoffset, int airport
            , ViewModels.FlightsFilter filter)
        {
            DateTime dateFrom = EPAAPI.Helper.BuildDateTimeFromYAFormat(from);
            DateTime dateTo = EPAAPI.Helper.BuildDateTimeFromYAFormat(to);
            var result = await unitOfWork.FlightRepository.GetFlightGantt(cid, dateFrom, dateTo, tzoffset, airport, filter);
            return Ok(result);
        }

        [Route("odata/flightplanitems/gantt/plan/{pid}/{tzoffset}")]
        public async Task<IHttpActionResult> GetFlightPlanItemsGanttByPlanId(int pid, int tzoffset)
        {
            var result = await unitOfWork.FlightRepository.GetPlanGantt(pid, tzoffset);
            return Ok(result);
        }

        [Route("odata/plan/items/gantt/")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightPlanItemsGanttByPlanId(dynamic dto)
        {
            //DateTime date, int tzoffset,bool design,int cid
            var cid = Convert.ToInt32(dto.CustomerId);
            var date = ((DateTime)dto.Date).Date;
            var tzoffset = Convert.ToInt32(dto.Offset);
            var design = Convert.ToBoolean(dto.Design);

            var result = await unitOfWork.FlightRepository.GetPlanItemsGantt(date, tzoffset, design, cid);
            return Ok(result);
        }
        //xati
        [Route("odata/plan/items/gantt/crewtest/")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightPlanItemsGanttByPlanIdCrewTest(dynamic dto)
        {
            //DateTime date, int tzoffset,bool design,int cid
            var cid = Convert.ToInt32(dto.CustomerId);
            var date = ((DateTime)dto.Date).Date;
            var dateTo = ((DateTime)dto.DateTo).Date;
            var tzoffset = Convert.ToInt32(dto.Offset);
            var design = Convert.ToBoolean(dto.Design);
            var planid = Convert.ToInt32(dto.PlanId);
            //  var result = await unitOfWork.FlightRepository.GetPlanItemsGanttCrewTest(date, tzoffset, design, cid,planid);
            var result = await unitOfWork.FlightRepository.GetPlanItemsGanttCrewTestByFlights(date, dateTo, tzoffset, design, cid, planid);

            return Ok(result);
        }

        [Route("odata/plan/items/box")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostBoxItems(dynamic dto)
        {
            List<int> ids = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(dto.ids));
            int cid = Convert.ToInt32(dto.cid);

            // var result = await unitOfWork.FlightRepository.boxPlanItems(ids,cid);
            var result = await unitOfWork.FlightRepository.boxFlights(ids, cid);
            if (!string.IsNullOrEmpty(result))
                return new CustomActionResult(HttpStatusCode.NotAcceptable, result);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(true);
        }
        [Route("odata/plan/items/unbox")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostUnBoxItems(dynamic dto)
        {
            var id = Convert.ToInt32(dto.id);
            //  var result = await unitOfWork.FlightRepository.unboxPlanItems(id);
            var result = await unitOfWork.FlightRepository.unboxFlights(id);


            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(true);
        }

        [Route("odata/flightplanitems/plan/{pid}/{tzoffset}")]
        public IQueryable<ViewFlightPlanItem> GetFlightPlanItemsByPlanId(int pid, int tzoffset)
        {

            return unitOfWork.FlightRepository.GetViewFlightPlanItems(pid).OrderBy(q => q.TypeId).ThenBy(q => q.Register).ThenBy(q => q.STD);
        }
        [Route("odata/flights/routes/averagetime/{from}/{to}")]
        public async Task<IHttpActionResult> GetRouteAverageTime(int from, int to)
        {
            var result = await unitOfWork.FlightRepository.GetFlightAVG(from, to);
            return Ok(result);
        }


        [Route("odata/flightplan/apply/customer/{cid}/{id}")]

        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> PostApplyPlan(int cid, int id)
        {

            var result = await unitOfWork.FlightRepository.ApplyPlan(id, cid);
            if (result.Code != HttpStatusCode.OK)
                return result;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(id);
        }
        [Route("odata/flightplan/close/{id}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> PostClosePlan(int id)
        {

            var result = await unitOfWork.FlightRepository.CloseFlightPlan(id);
            if (result.Code != HttpStatusCode.OK)
                return result;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(id);
        }
        [Route("odata/flightplan/approve/60/{id}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> PostApprovePlan60(int id)
        {

            var result = await unitOfWork.FlightRepository.ApproveFlightPlan(id, 60);
            if (result.Code != HttpStatusCode.OK)
                return result;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(id);
        }
        [Route("odata/flightplan/approve/70/{id}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> PostApprovePlan70(int id)
        {

            var result = await unitOfWork.FlightRepository.ApproveFlightPlan(id, 70);
            if (result.Code != HttpStatusCode.OK)
                return result;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(id);
        }
        [Route("odata/flightplan/register/overlaps/{pid}/{vid}/{rid}/{from?}/{to?}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetCheckPlanRegister(int pid, int vid, int rid, string from = null, string to = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (!string.IsNullOrEmpty(from))
                dateFrom = EPAAPI.Helper.BuildDateTimeFromYAFormat(from);
            if (!string.IsNullOrEmpty(to))
                dateTo = EPAAPI.Helper.BuildDateTimeFromYAFormat(to);


            var result = await unitOfWork.FlightRepository.CheckPlanRegister(pid, vid, rid, dateFrom, dateTo);
            // if (result.Code != HttpStatusCode.OK)
            //    return result;
            // var saveResult = await unitOfWork.SaveAsync();
            // if (saveResult.Code != HttpStatusCode.OK)
            //    return saveResult;
            return Ok(result);

        }



        [Route("odata/flightplan/registers/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanRegisters(ViewModels.FlightPlanRegistersSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }
            var items = dto.Items;
            var newItems = unitOfWork.FlightRepository.InsertFlightPlanRegisters(dto.Items, dto.Deleted);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            var result = new List<ViewModels.OldNewPair>();
            var c = 0;
            foreach (var x in newItems)
            {
                result.Add(new ViewModels.OldNewPair() { NewId = x.Id, OldId = items[c].Id });
                c++;
            }


            return Ok(result);
        }
        //rati
        [Route("odata/flightplan/register/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanRegister(dynamic dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var id = Convert.ToInt32(dto.Id);
            var planId = Convert.ToInt32(dto.PlanId);
            var registerId = Convert.ToInt32(dto.RegisterId);
            var virtualId = Convert.ToInt32(dto.VirtualId);
            var date = ((DateTime)dto.Date);
            var CalendarId = Convert.ToInt32(dto.CalendarId);
            var item = unitOfWork.FlightRepository.InsertFlightPlanRegister(id, date, planId, registerId, virtualId, CalendarId);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(item);
        }
        [Route("odata/flightplan/register/lock")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanRegisterLock(dynamic dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var id = Convert.ToInt32(dto.id);

            FlightPlanRegister item = await unitOfWork.FlightRepository.GetFlightPlanRegisterById(id);
            item.IsLocked = true;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(item);
        }



        [Route("odata/flightplan/register/unlock")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanRegisterUnLock(dynamic dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var id = Convert.ToInt32(dto.Id);

            FlightPlanRegister item = await unitOfWork.FlightRepository.GetFlightPlanRegisterById(id);
            item.IsLocked = false;
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(item);
        }

        [Route("odata/flight/plan/register/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFlightPlanRegister(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            FlightPlanRegister item = await unitOfWork.FlightRepository.GetFlightPlanRegisterById(id);


            unitOfWork.FlightRepository.Delete(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(dto);
        }

        [Route("odata/flightplan/registers/approve")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanRegistersApprove(ViewModels.Dto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.ApproveFlightPlanRegisters(dto.Id);
            if (!result)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(true);
        }



        [Route("odata/flightplan/calander/apply")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanCalanderApply(ViewModels.Dto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.ApproveFlightPlanRegisterCalander(dto.Id);
            if (!result)
                return new CustomActionResult(HttpStatusCode.NotAcceptable, "");
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(true);
        }

        [Route("odata/flightplan/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlan(ViewModels.FlightPlanDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }
            dto.DateFrom = ((DateTime)dto.DateFrom).Date;
            dto.DateTo = ((DateTime)dto.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dto.DateFirst = ((DateTime)dto.DateFirst).Date;
            var validate = unitOfWork.FlightRepository.ValidateFlightPlan(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            FlightPlan entity = null;

            if (dto.Id == -1)
            {
                entity = new FlightPlan();
                unitOfWork.FlightRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.FlightRepository.GetPlanById(dto.Id);

            }

            if (entity == null)
                return Exceptions.getNotFoundException();

            ViewModels.FlightPlanDto.Fill(entity, dto);

            if (dto.Id != -1)
            {
                unitOfWork.FlightRepository.ClearPlanMonths(dto.Id);
                unitOfWork.FlightRepository.ClearPlanDays(dto.Id);
            }

            if (dto.Months != null)
                foreach (var x in dto.Months)
                    unitOfWork.FlightRepository.Insert(new FlightPlanMonth() { FlightPlan = entity, Month = x });

            if (dto.Days != null)
                foreach (var x in dto.Days)
                    unitOfWork.FlightRepository.Insert(new FlightPlanDay() { FlightPlan = entity, Day = x });

            unitOfWork.FlightRepository.CreatePlanCalendar(dto.Id, entity, dto.Months, dto.Days);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);
        }

        //crewati
        [Route("odata/flights/plan/crew/{id}/{calanderId}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightPlanCalanderCrew> GetViewFlightPlanCalanderCrew(int id, int calanderId)
        {

            return unitOfWork.FlightRepository.GetViewFlightPlanCalanderCrew().Where(q => q.FlightPlanId == id && q.CalanderId == calanderId).OrderBy(q => q.JobGroupCode).ThenBy(q => q.Name);

        }
        [Route("odata/flight/crew/2/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightCrew2> GetFlightViewBoxCrew(int id)
        {

            return unitOfWork.FlightRepository.GetViewFlightCrew2().Where(q => q.FlightId == id).OrderBy(q => q.Position).ThenBy(q => q.Name);

        }

        //poosk
        [Route("odata/crew/report/flights/{id}")]
        [EnableQuery]

        public IQueryable<ViewFlightCrew2> GetCrewFlightsReport(DateTime from, DateTime to, int id)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightCrew2()
                        where x.EmployeeId == id && x.Date >= dfrom && x.Date <= dto
                        //&& x.FlightStatusID==15
                        orderby x.Date, x.STD
                        select x;

            return query;

        }
        [Route("odata/crew/report/flights/total/")]
        [EnableQuery]

        public IHttpActionResult GetCrewFlightsTotalReport(DateTime from, DateTime to)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = (from x in unitOfWork.FlightRepository.GetViewFlightCrew2()
                         where x.Date >= dfrom && x.Date <= dto
                         //&& x.FlightStatusID == 15
                         group x by new { x.EmployeeId, x.FirstName, x.LastName, x.PID, x.NID, x.Mobile, x.JobGroup, x.JobGroupCode } into grp

                         select new
                         {
                             grp.Key.EmployeeId,
                             Id = grp.Key.EmployeeId,
                             grp.Key.FirstName,
                             grp.Key.LastName,
                             grp.Key.NID,
                             grp.Key.PID,
                             grp.Key.Mobile,
                             grp.Key.JobGroup,
                             grp.Key.JobGroupCode,
                             FlightTime = grp.Sum(q => q.FlightTime),
                             Duty = grp.Sum(q => q.Duty),
                             FlightsCount = grp.Count(),
                             FlightH = grp.Sum(q => q.FlightH),
                             FlightM = grp.Sum(q => q.FlightM),
                             BlockH = grp.Sum(q => q.ActualFlightHOffBlock),
                             BlockM = grp.Sum(q => q.ActualFlightMOffBlock),
                         }).ToList();
            var result = query.OrderBy(q => q.JobGroupCode).ThenByDescending(q => q.FlightTime).ThenBy(q => q.LastName).ToList();
            return Ok(result);

        }

        [Route("odata/delays")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightDelay> GetViewFlightDelays()
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlightDelays();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        [Route("odata/delays/{id}")]
        [EnableQuery]
        // [Authorize]
        public IQueryable<ViewFlightDelay> GetViewFlightDelays(int id)
        {
            try
            {
                return unitOfWork.FlightRepository.GetViewFlightDelays().Where(q => q.FlightId == id).OrderByDescending(q => q.DelayHH).ThenByDescending(q => q.DelayMM);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }



        }
        [Route("odata/delays/total/code/{cid}")]
        [EnableQuery]

        //public async Task<IHttpActionResult> GetTotalDelaysByCode(DateTime from, DateTime to, int cid)
        public IQueryable<dynamic> GetTotalDelaysByCode(DateTime from, DateTime to, int cid, int? top = null, int? skip = null)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid
                        group x by new { x.DelayCodeId, x.Category, x.Title, x.Code, x.DelayCategoryId, x.DelayRemark } into grp
                        select new
                        {
                            grp.Key.DelayCodeId,
                            grp.Key.Category,
                            grp.Key.Title,
                            grp.Key.Code,
                            grp.Key.DelayCategoryId,
                            grp.Key.DelayRemark,
                            Flights = grp.Count(),
                            TotalDelay = grp.Sum(q => q.DelayMM),
                            DurationOffBlock = grp.Sum(q => q.ActualFlightHOffBlock * 60 + q.ActualFlightMOffBlock),
                            DurationTakeOff = grp.Sum(q => q.ActualFlightHTakeoff * 60 + q.ActualFlightMTakeoff)
                        };
            var result = query.OrderByDescending(q => q.TotalDelay).ThenBy(q => q.Code);
            if (skip != null && top != null)
                return result.Skip((int)skip).Take((int)top);
            else
            if (top != null)
                return result.Take((int)top);

            return result;

        }

        [Route("odata/delays/details/code/{cid}/{code}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysByCodeDetails(DateTime from, DateTime to, int cid, int code)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid && x.DelayCodeId == code
                        select x;
            var result = await query.OrderByDescending(q => q.DelayMM).ThenBy(q => q.FlightNumber).ToListAsync();

            return Ok(result);

        }
        //============================================//

        [Route("odata/delays/total/source/{cid}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysBySourceAirport(DateTime from, DateTime to, int cid)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid
                        group x by new { x.FromAirport, x.FromAirportIATA, x.DelayCodeId, x.Category, x.Title, x.Code, x.DelayCategoryId, x.DelayRemark } into grp
                        select new
                        {
                            grp.Key.FromAirport,
                            grp.Key.FromAirportIATA,
                            grp.Key.DelayCodeId,
                            grp.Key.Category,
                            grp.Key.Title,
                            grp.Key.Code,
                            grp.Key.DelayCategoryId,
                            grp.Key.DelayRemark,
                            Flights = grp.Count(),
                            TotalDelay = grp.Sum(q => q.DelayMM),
                            DurationOffBlock = grp.Sum(q => q.ActualFlightHOffBlock * 60 + q.ActualFlightMOffBlock),
                            DurationTakeOff = grp.Sum(q => q.ActualFlightHTakeoff * 60 + q.ActualFlightMTakeoff)
                        };
            var result = await query.OrderByDescending(q => q.TotalDelay).ThenBy(q => q.FromAirportIATA).ThenBy(q => q.Code).ToListAsync();

            return Ok(result);

        }
        [Route("odata/delays/details/source/{cid}/{source}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysBySourceDetails(DateTime from, DateTime to, int cid, int source)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid && x.FromAirport == source
                        select x;
            var result = await query.OrderByDescending(q => q.DelayMM).ThenBy(q => q.Code).ThenBy(q => q.FlightNumber).ToListAsync();

            return Ok(result);

        }
        //==================================================//

        [Route("odata/delays/total/register/{cid}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysByRegister(DateTime from, DateTime to, int cid)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid
                        group x by new { x.RegisterID, x.Register, x.DelayCodeId, x.Category, x.Title, x.Code, x.DelayCategoryId, x.DelayRemark } into grp
                        select new
                        {
                            grp.Key.RegisterID,
                            grp.Key.Register,
                            grp.Key.DelayCodeId,
                            grp.Key.Category,
                            grp.Key.Title,
                            grp.Key.Code,
                            grp.Key.DelayCategoryId,
                            grp.Key.DelayRemark,
                            Flights = grp.Count(),
                            TotalDelay = grp.Sum(q => q.DelayMM),
                            DurationOffBlock = grp.Sum(q => q.ActualFlightHOffBlock * 60 + q.ActualFlightMOffBlock),
                            DurationTakeOff = grp.Sum(q => q.ActualFlightHTakeoff * 60 + q.ActualFlightMTakeoff)
                        };
            var result = await query.OrderByDescending(q => q.TotalDelay).ThenBy(q => q.Register).ThenBy(q => q.Code).ToListAsync();

            return Ok(result);

        }
        [Route("odata/delays/details/register/{cid}/{register}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysByRegisterDetails(DateTime from, DateTime to, int cid, int register)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid && x.RegisterID == register
                        select x;
            var result = await query.OrderByDescending(q => q.DelayMM).ThenBy(q => q.Code).ThenBy(q => q.FlightNumber).ToListAsync();

            return Ok(result);

        }
        //================================================//
        [Route("odata/delays/total/route/{cid}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysByRoute(DateTime from, DateTime to, int cid)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid
                        group x by new { x.Route, x.DelayCodeId, x.Category, x.Title, x.Code, x.DelayCategoryId, x.DelayRemark } into grp
                        select new
                        {
                            grp.Key.Route,

                            grp.Key.DelayCodeId,
                            grp.Key.Category,
                            grp.Key.Title,
                            grp.Key.Code,
                            grp.Key.DelayCategoryId,
                            grp.Key.DelayRemark,
                            Flights = grp.Count(),
                            TotalDelay = grp.Sum(q => q.DelayMM),
                            DurationOffBlock = grp.Sum(q => q.ActualFlightHOffBlock * 60 + q.ActualFlightMOffBlock),
                            DurationTakeOff = grp.Sum(q => q.ActualFlightHTakeoff * 60 + q.ActualFlightMTakeoff)
                        };
            var result = await query.OrderByDescending(q => q.TotalDelay).ThenBy(q => q.Route).ThenBy(q => q.Code).ToListAsync();

            return Ok(result);

        }
        [Route("odata/delays/details/route/{cid}/{route}")]
        [EnableQuery]

        public async Task<IHttpActionResult> GetTotalDelaysByRegisterDetails(DateTime from, DateTime to, int cid, string route)
        {
            var dfrom = from.Date;
            var dto = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var query = from x in unitOfWork.FlightRepository.GetViewFlightDelays()
                        where x.Date >= dfrom && x.Date <= dto && x.CustomerId == cid && x.Route == route
                        select x;
            var result = await query.OrderByDescending(q => q.DelayMM).ThenBy(q => q.Code).ThenBy(q => q.FlightNumber).ToListAsync();

            return Ok(result);

        }
        //=============================================//
        [Route("odata/box/crew/{boxid}")]
        [EnableQuery]
        // [Authorize]
        public IHttpActionResult GetViewBoxCrew(int boxid)
        {
            var crew = unitOfWork.FlightRepository.GetViewBoxCrews().Where(q => q.BoxId == boxid).OrderBy(q => q.Position).ThenBy(q => q.Name).ToList();
            var query = (from x in crew
                         group x by x.Position into g
                         select new { Title = g.Key, Value = g.Count() }).ToList();
            var _allassigned = unitOfWork.FlightRepository.IsBoxCrewAllAssigned(boxid);
            dynamic result = new
            {
                Crew = crew,
                Summary = query,
                HasCrewProblem = crew.Where(q => q.AvailabilityId != 1).Count() > 0,
                AllCrewAssigned = _allassigned,
            };
            return Ok(result);
            // return unitOfWork.FlightRepository.GetViewBoxCrews().Where(q => q.BoxId == boxid);

        }
        //oks
        [Route("odata/flights/plan/crew/box/{bid}")]
        // [EnableQuery]
        // [Authorize]
        public IHttpActionResult GetViewFlightPlanCalanderCrewBox(int bid)
        {
            var crew = unitOfWork.FlightRepository.GetViewFlightPlanCalanderCrew().Where(q => q.BoxId == bid).OrderBy(q => q.JobGroupCode).ThenBy(q => q.Name).ToList();
            var query = (from x in crew
                         group x by x.JobGroup into g
                         select new { Title = g.Key, Value = g.Count() }).ToList();
            var _allassigned = unitOfWork.FlightRepository.IsBoxCrewAllAssigned(bid);
            dynamic result = new
            {
                Crew = crew,
                Summary = query,
                HasCrewProblem = crew.Where(q => q.AvStatusId > 1).Count() > 0,
                AllCrewAssigned = _allassigned,
            };
            return Ok(result);

        }


        [Route("odata/crew/available/{cid}/{type}/{boxid}")]
        [EnableQuery]
        // [Authorize]
        public async Task<IHttpActionResult> GetCrewAvailable(int cid, int type, int boxid)
        {
            try
            {
                //DateTime date = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(day).Date;
                var box = await unitOfWork.FlightRepository.GetBoxByID(boxid);
                var boxCrewIds = await unitOfWork.FlightRepository.GetViewFlightPlanCalanderCrew().Where(q => q.BoxId == boxid).Select(q => q.PersonId).ToListAsync();
                var query = from x in unitOfWork.PersonRepository.GetViewCrews()
                                // join y in unitOfWork.PersonRepository.GetViewPersonAircraftType() on x.PersonId equals y.PersonId
                            where x.CustomerId == cid && !boxCrewIds.Contains(x.PersonId)

                            select x;
                if (type != -1)
                    query = from x in query
                            join y in unitOfWork.PersonRepository.GetViewPersonAircraftType() on x.PersonId equals y.PersonId
                            where y.AircraftTypeId == type

                            select x;
                //ViewFlightPlanCalanderCrew
                var dutyQuery = (from x in unitOfWork.FlightRepository.GetViewFlightPlanCalanderCrew()
                                 where (
                                 x.STD >= box.STD && x.STA <= box.STA
                                 ) ||
                                 (
                                 box.STD >= x.STD && box.STA <= x.STA
                                 ) ||
                                 (x.BoxId != box.Id && x.Date == box.Date)
                                 select x.PersonId).ToList();

                var result = query.ToList();
                var pids = new List<string> { "66", "64" };
                foreach (var x in result)
                {
                    //get availability
                    x.AvStatus = "Available";
                    x.AvStatusId = 1;
                    if (x.CurrentLocationAirportId != box.FromAirportId)
                    {
                        x.AvStatusId = 2;
                        x.AvStatus = "Location";
                    }
                    if (dutyQuery.IndexOf(x.PersonId) != -1)
                    {
                        x.AvStatus = "Duty";
                        x.AvStatusId = 3;
                    }
                    if (pids.IndexOf(x.PID) != -1)
                    {
                        x.AvStatus = "Rest";
                        x.AvStatusId = 4;
                    }
                }

                // return query.OrderBy(x=>x.Name);
                return Ok(result);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }
        //dool
        [Route("odata/crew/over/{date}/{pid}/{duty}/{flight}")]
        [EnableQuery]

        public dynamic GetOverDuty(string date, int pid, int duty, int flight)
        {

            var _result = unitOfWork.FlightRepository.GetOverDuty(date, pid, duty, flight).AsQueryable();
            if (_result == null || _result.Count() == 0)
                return new
                {
                    status = true,
                };
            var _first = _result.First();
            var overType = new List<string>();
            if (_first.Day1_Duty > 780)
                overType.Add("Duty-Day");
            if (_first.Day7_Duty > 3600)
                overType.Add("Duty-Day 7");
            if (_first.Day14_Duty > 6600)
                overType.Add("Duty-Day 14");
            if (_first.Day28_Duty > 11400)
                overType.Add("Duty-Day 28");

            if (_first.Day28_Flight > 6000)
                overType.Add("Flight-Day 28");
            if (_first.Year_Flight > 60000)
                overType.Add("Flight-Year");
            dynamic dto = new
            {
                status = false,
                result = _result,
                first = _first,
                remark = overType,
            };
            return dto;

        }


        [Route("odata/crew/rest/validation/{boxid}/{pid}")]
        [EnableQuery]
        public async Task<dynamic> GetNextRestValidation(int boxid, int pid)
        {
            var box = await unitOfWork.FlightRepository.GetViewBoxByID(boxid);
            var query = (from x in unitOfWork.FlightRepository.GetViewBoxCrews()
                         where x.BoxId != boxid && pid == x.EmployeeId && (x.DefaultStart >= box.RestFrom && x.DefaultStart <= box.RestUntil)
                         select x).ToList();
            var firstbox = unitOfWork.FlightRepository.GetViewBoxCrews().OrderBy(q => q.DefaultStart).Select(q => q.DefaultStart).FirstOrDefault();
            var temp = true;
            if (firstbox != null && ((DateTime)box.DefaultStart - (DateTime)firstbox).TotalHours >= 168)
            {
                var date168 = ((DateTime)box.DefaultStart).AddHours(-168);
                var rest168 = (from x in unitOfWork.FlightRepository.GetEmployeeCalendar()
                               where x.EmployeeId == pid && x.StatusId == 1166 && x.Date >= date168 && x.Date <= box.DefaultStart
                               select x).Count();
                if (rest168 == 0)
                    temp = false;
            }

            return new
            {
                nextRest = query,
                dayoff = temp,
            };
        }
        //jool
        [Route("odata/crew/rest/check/{date}/{pid}")]
        [EnableQuery]
        public int GetRestCheck(string date, int pid)
        {
            //DateTime value = new DateTime(2017, 1, 18);
            // var dates = date.Split('-').Select(q=>Convert.ToInt32(q)).ToList();
            //var _date =( new DateTime(dates[0], dates[1], dates[2])).Date;
            var y = Convert.ToInt32(date.Substring(0, 4));
            var m = Convert.ToInt32(date.Substring(4, 2));
            var d = Convert.ToInt32(date.Substring(6, 2));

            var _date = (new DateTime(y, m, d)).Date;
            var _dateTo = _date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = (from x in unitOfWork.FlightRepository.GetViewBoxCrews()
                         where pid == x.EmployeeId && ((x.RestUntil >= _date && x.RestUntil <= _dateTo) || ((x.RestFrom >= _date && x.RestFrom <= _dateTo)))
                         select x).Count();
            return query == 0 ? 1 : -1;
        }
        class RestReq
        {
            public int EmployeeId { get; set; }
            public DateTime? RestFrom { get; set; }
            public DateTime? RestUntil { get; set; }
        }
        //getcrew dool
        [Route("odata/crew")]
        [AcceptVerbs("POST", "GET")]
        [EnableQuery]
        // [Authorize]
        public async Task<IHttpActionResult> GetCrew(/*int cid, int type, int boxid*/dynamic dto)
        {
            try
            {
                var dateFrom = ((DateTime)dto.DateFrom).Date;
                var dateTo = ((DateTime)dto.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                var cid = (Nullable<int>)Convert.ToInt32(dto.cid);
                int type = Convert.ToInt32(dto.type);
                int boxid = Convert.ToInt32(dto.boxid);
                bool cockpit = Convert.ToBoolean(dto.cockpit);

                var code = cockpit ? "00101" : "00102";
                //DateTime date = EPAAPI.Helper.BuildDateTimeFromYAFormatUTC(day).Date;
                var box = await unitOfWork.FlightRepository.GetViewBoxByID(boxid);
                // var boxCrewIds = await unitOfWork.FlightRepository.GetViewFlightPlanCalanderCrew().Where(q => q.BoxId == boxid).Select(q => q.PersonId).ToListAsync();
                var query = from x in unitOfWork.PersonRepository.GetViewCrews()
                                // join y in unitOfWork.PersonRepository.GetViewPersonAircraftType() on x.PersonId equals y.PersonId
                            where x.CustomerId == cid && x.JobGroupCode.StartsWith(code)
                            select x;
                if (type != -1)
                    query = from x in query
                            join y in unitOfWork.PersonRepository.GetViewPersonAircraftType() on x.PersonId equals y.PersonId
                            where y.AircraftTypeId == type

                            select x;

                var crews = query.ToList();

                var eids = crews.Select(q => q.Id).ToList();


                var requiredRests = (from x in unitOfWork.FlightRepository.GetViewBoxCrews()
                                     where x.BoxId != boxid && eids.Contains(x.EmployeeId) && (box.DefaultStart >= x.RestFrom && box.DefaultStart <= x.RestUntil)
                                     select new RestReq() { EmployeeId = x.EmployeeId, RestFrom = x.RestFrom, RestUntil = x.RestUntil }).ToList();


                var stbyRests = (from x in unitOfWork.FlightRepository.GetViewCrewCalendar()
                                 where eids.Contains(x.EmployeeId) && (x.StatusId == 1168 || x.StatusId == 1167) && x.IsCeased == 0
                                 && (box.DefaultStart >= x.DateEnd && box.DefaultStart <= x.RestUntil)
                                 select new RestReq() { EmployeeId = x.EmployeeId, RestFrom = x.DateEnd, RestUntil = x.RestUntil }).ToList();

                var otherrest = (from x in unitOfWork.FlightRepository.GetViewCrewCalendar()
                                 where eids.Contains(x.EmployeeId) && (x.StatusId == 5000 || x.StatusId == 5001) && x.IsCeased == 0
                                 && (box.DefaultStart >= x.DateEnd && box.DefaultStart <= x.RestUntil)
                                 select new RestReq() { EmployeeId = x.EmployeeId, RestFrom = x.DateEnd, RestUntil = x.RestUntil }).ToList();

                List<RestReq> AllRests = requiredRests.Concat(stbyRests).Concat(otherrest).ToList();

                var tasks = await unitOfWork.FlightRepository.GetViewBoxCrewFlights().Where(q => eids.Contains(q.EmployeeId) && (q.Date >= dateFrom && q.Date <= dateTo)).ToListAsync();
                var c1 = from x in tasks
                         group x by new { x.DateStr, x.EmployeeId } into g

                         select new
                         {
                             DateStr = g.Key.DateStr,
                             EmployeeId = g.Key.EmployeeId,
                             Boxed = (from z in g
                                      group z by z.BoxId into bg
                                      select new { BoxId = bg.Key, Items = bg.ToList() }
                                   )
                         };
                var _times = await (from x in unitOfWork.FlightRepository.GetViewCrewTimes()
                                    where x.CDate >= dateFrom && x.CDate <= dateTo && eids.Contains(x.Id)
                                    select x).ToListAsync();

                var dayTimes = (from x in _times
                                where (x.CalendarStatusId == 1167 || x.CalendarStatusId == 1168 || x.CalendarStatusId==5000 || x.CalendarStatusId==5001) && x.CDate == box.Date && box.DefaultStart >= x.ECDateStart && box.DefaultStart <= x.ECDateEnd && x.ECBoxId == null
                                select x).ToList();
                foreach (var x in dayTimes)
                {
                    x.Day1_Duty -= Convert.ToDouble(x.ECDuty);
                    x.Day7_Duty -= Convert.ToDouble(x.ECDuty);
                    x.Day14_Duty -= Convert.ToDouble(x.ECDuty);
                    x.Day28_Duty -= Convert.ToDouble(x.ECDuty);
                    x.Year_Duty -= Convert.ToDouble(x.ECDuty);

                    var newduty = 0.25 * ((DateTime)box.DefaultStart - (DateTime)x.ECDateStart).TotalMinutes;
                    x.ECDuty = Convert.ToDecimal(newduty);
                    // x.Day1_Duty -= Convert.ToDouble(x.ECDuty);
                    // x.Day7_Duty -= Convert.ToDouble(x.ECDuty);
                    // x.Day14_Duty -= Convert.ToDouble(x.ECDuty);
                    // x.Day28_Duty -= Convert.ToDouble(x.ECDuty);
                    // x.Year_Duty -= Convert.ToDouble(x.ECDuty);

                    if (x.CalendarStatusId == 1168 && ((DateTime)box.DefaultStart).AddMinutes(270).Hour >= 6)
                    {
                        //stby am
                        x.FDPReduction = 360;
                    }
                    if (x.CalendarStatusId == 1167 && ((DateTime)box.DefaultStart).AddMinutes(270).Hour >= 18)
                    {
                        //stby pm
                        x.FDPReduction = 360;
                    }

                }

                dynamic result = new
                {
                    crew = crews,
                    calendar = c1.ToList(),
                    times = _times,
                    reqrests = AllRests,

                };

                return Ok(result);
                // return db.ViewAirports.AsNoTracking() ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }



        //sati
        [Route("odata/flight/plan/crew/save/x")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanCrew(ViewModels.FlightPlanCalanderCrewDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            //var validate = unitOfWork.FlightRepository.ValidateFlightPlan(dto);
            //if (validate.Code != HttpStatusCode.OK)
            //    return validate;

            FlightPlanCalanderCrew entity = null;

            if (dto.Id == -1)
            {
                entity = new FlightPlanCalanderCrew();
                unitOfWork.FlightRepository.Insert(entity);
            }



            if (entity == null)
                return Exceptions.getNotFoundException();

            ViewModels.FlightPlanCalanderCrewDto.Fill(entity, dto);


            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);
        }

        [Route("odata/flight/plan/crew/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostBoxCrew(ViewModels.FlightPlanCalanderCrewDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            //var validate = unitOfWork.FlightRepository.ValidateFlightPlan(dto);
            //if (validate.Code != HttpStatusCode.OK)
            //    return validate;

            BoxCrew entity = null;

            if (dto.Id == -1)
            {
                entity = new BoxCrew();
                unitOfWork.FlightRepository.Insert(entity);
            }



            if (entity == null)
                return Exceptions.getNotFoundException();

            //ViewModels.FlightPlanCalanderCrewDto.Fill(entity, dto);
            entity.EmployeeId = dto.EmployeeId;
            entity.AvailabilityId = dto.AvailabilityId;
            entity.BoxId = (int)dto.BoxId;
            entity.JobGroupId = dto.GroupId;
            entity.Remark = string.Empty;

            if ((int)dto.ECSplitedId != -1)
            {
                var ecsplited = await unitOfWork.FlightRepository.GetEmployeeCalendarSplitedById((int)dto.ECSplitedId);
                ecsplited.BoxCrew = entity;
            }

            if ((int)dto.ECId != -1)
            {
                var ec = await unitOfWork.FlightRepository.GetEmployeeCalendarById((int)dto.ECId);
                ec.BoxCrew = entity;
            }



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);
        }

        [Route("odata/flight/plan/crew/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFlightPlanCrew(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            FlightPlanCalanderCrew item = await unitOfWork.FlightRepository.GetFlightPlanCalanderCrewById(id);


            unitOfWork.FlightRepository.Delete(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(dto);
        }

        [Route("odata/box/crew/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteBoxCrew(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            BoxCrew item = await unitOfWork.FlightRepository.GetBoxCrewById(id);


            unitOfWork.FlightRepository.Delete(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(dto);
        }
        [Route("odata/flight/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFlight(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            FlightInformation item = await unitOfWork.FlightRepository.GetFlightById(id);


            unitOfWork.FlightRepository.DeleteFlight(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(dto);
        }

        [Route("odata/crew/calendar/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostCrewCalendar(dynamic dto)
        {
            //xox
            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            int employeeId = Convert.ToInt32(dto.EmployeeId);
            string date = Convert.ToString(dto.Date);
            int status = Convert.ToInt32(dto.Status);

            

            var y = Convert.ToInt32(date.Substring(0, 4));
            var m = Convert.ToInt32(date.Substring(4, 2));
            var d = Convert.ToInt32(date.Substring(6, 2));

            var _date = (new DateTime(y, m, d)).Date;


            EmployeeCalendarSplited detail =new EmployeeCalendarSplited() ;
                        EmployeeCalendar entity = await unitOfWork.FlightRepository.GetEmployeeCalendar(employeeId, _date);
            if (status != -1)
            {
                if (entity == null)
                {
                    entity = new EmployeeCalendar();
                    entity.EmployeeId = employeeId;
                    entity.Date = _date;

                    unitOfWork.FlightRepository.Insert(entity);

                      detail = new EmployeeCalendarSplited()
                    {
                        EmployeeCalendar = entity,
                        EmployeeId = employeeId,
                        IsDismissed = false,
                        StatusId = status,

                    };
                    var _ds = new DateTime(y, m, d, 0, 0, 0);
                    var _de = new DateTime(y, m, d, 23, 59, 59, 999);
                    detail.DateStart = _ds;
                    detail.DateEnd = _de;
                    unitOfWork.FlightRepository.Insert(detail);
                }

                entity.StatusId = status;
                if (status == 1168)
                {
                    var _ds = new DateTime(y, m, d, 0, 0, 0);
                    var _de = new DateTime(y, m, d, 12, 0, 0);
                    _ds = _ds.AddMinutes(-270);
                    _de = _de.AddMinutes(-270);
                    entity.DateStart = _ds;
                    entity.DateEnd = _de;

                    detail.DateStart = _ds;
                    detail.DateEnd = _de;
                    

                }
                if (status == 1167)
                {
                    var _ds = new DateTime(y, m, d, 12, 0, 0);
                    var _de = new DateTime(y, m, d, 23, 59, 59, 999);
                    _ds = _ds.AddMinutes(-270);
                    _de = _de.AddMinutes(-270);
                    entity.DateStart = _ds;
                    entity.DateEnd = _de;

                    detail.DateStart = _ds;
                    detail.DateEnd = _de;
                }
                if (status==5001 || status == 5000)
                {
                    var _ds = Convert.ToDateTime(dto.DateStart);
                    var _de = Convert.ToDateTime(dto.DateEnd);
                    entity.DateStart = _ds;
                    entity.DateEnd = _de;

                    detail.DateStart = _ds;
                    detail.DateEnd = _de;
                }
            }
            else
            {
                if (entity == null)
                    return Exceptions.getNotFoundException();
                unitOfWork.FlightRepository.Delete(entity);
            }



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);
        }



        [Route("odata/plan/interval/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanInterval(ViewModels.FlightPlanDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }
            dto.DateFrom = ((DateTime)dto.DateFrom).Date;
            dto.DateTo = ((DateTime)dto.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            dto.DateFirst = ((DateTime)dto.DateFirst).Date;
            var validate = unitOfWork.FlightRepository.ValidateFlightPlan(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            FlightPlan entity = await unitOfWork.FlightRepository.GetPlanById(dto.Id);



            if (entity == null)
                return Exceptions.getNotFoundException();


            entity.DateFrom = dto.DateFrom;
            entity.DateTo = dto.DateTo;
            entity.DateFirst = dto.DateFirst;
            entity.DateLast = dto.DateLast;
            entity.Interval = dto.Interval;



            unitOfWork.FlightRepository.ClearPlanMonths(dto.Id);
            unitOfWork.FlightRepository.ClearPlanDays(dto.Id);


            if (dto.Months != null)
                foreach (var x in dto.Months)
                    unitOfWork.FlightRepository.Insert(new FlightPlanMonth() { FlightPlan = entity, Month = x });

            if (dto.Days != null)
                foreach (var x in dto.Days)
                    unitOfWork.FlightRepository.Insert(new FlightPlanDay() { FlightPlan = entity, Day = x });

            unitOfWork.FlightRepository.CreatePlanCalendar(dto.Id, entity, dto.Months, dto.Days);

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);
        }
        [Route("odata/flightplanitems/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanItems(ViewModels.FlightPlanSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var errors = new List<int>() { 10, 11, 16 };
            var flightPlan = await unitOfWork.FlightRepository.GetPlanById(dto.Plan.Id);
            if (flightPlan == null)
            {
                return Exceptions.getNotFoundException();
            }

            var newItems = (from x in dto.Items
                            where x.Id >= 1000000
                            select x).ToList();

            foreach (var x in newItems)
            {
                var nfpi = new FlightPlanItem();
                ViewModels.FlightPlanItemDto.Fill(nfpi, x);

                if (errors.IndexOf((int)nfpi.StatusId) == -1)
                    nfpi.StatusId = 1;

                unitOfWork.FlightRepository.Insert(nfpi);
                x.PlanItem = nfpi;
            }

            var updatedItems = dto.Items.Where(q => q.Id < 1000000).ToList();
            var updatedEntities = await unitOfWork.FlightRepository.GetPlanItemsByIds(updatedItems.Select(q => q.Id).ToList());
            foreach (var x in updatedEntities)
            {
                var dtoitem = updatedItems.Single(q => q.Id == x.Id);
                ViewModels.FlightPlanItemDto.Fill(x, dtoitem);
                if (errors.IndexOf((int)x.StatusId) == -1)
                    x.StatusId = 1;
            }

            var deletedEntities = await unitOfWork.FlightRepository.GetPlanItemsByIds(dto.Deleted);
            foreach (var x in deletedEntities)
            {
                unitOfWork.FlightRepository.Delete(x);
            }

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            var result = (from x in newItems
                          select new { InitId = x.Id, Id = x.PlanItem.Id }).ToList();

            return Ok(result);
        }


        [Route("odata/flight/planitem/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFlightPlanItem(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            FlightPlanItem item = await unitOfWork.FlightRepository.GetPlanItemById(id);

            FlightPlan plan = await unitOfWork.FlightRepository.GetPlanById(item.FlightPlanId);
            unitOfWork.FlightRepository.Delete(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            await unitOfWork.FlightRepository.ProcessPlanErrors(plan);
            saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            return Ok(dto);
        }


        [Route("odata/flight/plan/item/permit/delete")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFlightPlanItemPermit(dynamic dto)
        {
            var id = Convert.ToInt32(dto.Id);
            FlightPlanItemPermit item = await unitOfWork.FlightRepository.GetFlightPlanItemPermitById(id);


            unitOfWork.FlightRepository.Delete(item);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(dto);
        }

        [Route("odata/flight/planitem/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPlanItem(ViewModels.FlightPlanningDto obj)
        {

            if (obj == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            //   string str = JsonConvert.SerializeObject(dto);
            //  dynamic obj = JsonConvert.DeserializeObject(str);

            var dateFrom = ((DateTime)obj.DateFrom).Date;
            var dateTo = ((DateTime)obj.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime? dateFirst = null;
            if (obj.DateFirst != null)
                dateFirst = ((DateTime)obj.DateFirst).Date;
            //dto.DateFrom = ((DateTime)dto.DateFrom).Date;
            //dto.DateTo = ((DateTime)dto.DateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            //dto.DateFirst = ((DateTime)dto.DateFirst).Date;
            //var validate = unitOfWork.FlightRepository.ValidateFlightPlan(dto);
            //if (validate.Code != HttpStatusCode.OK)
            //    return validate;

            FlightPlan entity = null;
            FlightPlanItem entityItem = null;
            var plan_title = obj.CustomerId.ToString() + "_" + obj.BaseId.ToString() + "_" + obj.RegisterID + "_" + dateFrom.ToShortDateString().Replace("/", "-");
            entity = await unitOfWork.FlightRepository.GetPlanByTitle(plan_title);

            if (entity == null)
            {
                entity = new FlightPlan();
                unitOfWork.FlightRepository.Insert(entity);
                //////Fill Plan ///////////////////
                ///  entity.Id = flightplan.Id;
                entity.Title = plan_title;
                entity.DateFrom = dateFrom;
                entity.DateTo = dateTo;
                entity.DateFirst = dateFirst;
                entity.DateLast = null;
                entity.CustomerId = obj.CustomerId;
                entity.IsActive = obj.IsActive;
                entity.DateActive = obj.DateActive;
                entity.Interval = obj.Interval;
                entity.BaseId = obj.BaseId;
                //if (obj.Id != -1)
                //{
                //    unitOfWork.FlightRepository.ClearPlanMonths(obj.Id);
                //    unitOfWork.FlightRepository.ClearPlanDays(obj.Id);
                //}
                var month = obj.Months as List<int>;
                var days = obj.Days as List<int>;
                if (month != null)
                    foreach (var x in month)
                        unitOfWork.FlightRepository.Insert(new FlightPlanMonth() { FlightPlan = entity, Month = x });

                if (days != null)
                    foreach (var x in days)
                        unitOfWork.FlightRepository.Insert(new FlightPlanDay() { FlightPlan = entity, Day = x });

                unitOfWork.FlightRepository.CreatePlanCalendar(obj.Id, entity, month, days);
                ////////////////////////////////////
            }

            if (obj.Id == -1)
            {


                entityItem = new FlightPlanItem();

                unitOfWork.FlightRepository.Insert(entityItem);
            }

            else
            {


                entityItem = await unitOfWork.FlightRepository.GetPlanItemById(obj.Id);

            }



            if (entity == null)
                return Exceptions.getNotFoundException();
            if (entityItem == null)
                return Exceptions.getNotFoundException();




            ////Fill Plan Item ////////////////////
            entityItem.Id = obj.Id;
            entityItem.FlightPlan = entity;
            entityItem.TypeId = obj.TypeId;
            entityItem.RegisterID = obj.RegisterID;
            entityItem.FlightTypeID = obj.FlightTypeID;
            entityItem.AirlineOperatorsID = obj.AirlineOperatorsID;
            entityItem.FlightNumber = obj.FlightNumber;
            entityItem.FromAirport = obj.FromAirport;
            entityItem.ToAirport = obj.ToAirport;
            entityItem.STD = ((DateTime)obj.STD);//.AddHours(3).AddMinutes(30);
            entityItem.STA = ((DateTime)obj.STA);//.AddHours(3).AddMinutes(30);
            entityItem.FlightH = obj.FlightH;
            entityItem.FlightM = obj.FlightM;
            entityItem.Unknown = obj.Unknown;
            entityItem.StatusId = 1;
            entityItem.RouteId = obj.RouteId;
            //////////////////////////////////////

            await unitOfWork.FlightRepository.ProcessPlanErrors(entity, entityItem);

            //    ViewModels.FlightPlanDto.Fill(entity, dto);







            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            obj.FlightPlanId = entity.Id;
            obj.Id = entityItem.Id;
            var view = unitOfWork.FlightRepository.GetViewFlightPlanItems().FirstOrDefault(q => q.Id == obj.Id);

            return Ok(view);
        }



        [Route("odata/flight/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlight(ViewModels.FlightDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var validate = unitOfWork.FlightRepository.ValidateFlight(dto);
            if (validate.Code != HttpStatusCode.OK)
                return validate;

            FlightInformation entity = null;

            if (dto.ID == -1)
            {
                entity = new FlightInformation();
                unitOfWork.FlightRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.FlightRepository.GetFlightById(dto.ID);
                if (entity == null)
                    return Exceptions.getNotFoundException();
                unitOfWork.FlightRepository.RemoveFlightLink(dto.ID);
            }



            ViewModels.FlightDto.Fill(entity, dto);
            entity.BoxId = dto.BoxId;

            if (dto.LinkedFlight != null)
            {
                var link = new FlightLink()
                {
                    FlightInformation = entity,
                    Flight2Id = (int)dto.LinkedFlight,
                    ReasonId = (int)dto.LinkedReason,
                    Remark = dto.LinkedRemark

                };
                unitOfWork.FlightRepository.Insert(link);
            }

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(/*entity*/true);

        }
        [Route("odata/flight/departure/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightDeparture(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightDeparture(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            if ((bool)result.data)
            {
                unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, dto.Takeoff, 2);
            }

            return Ok(result);

        }

        [Route("odata/flight/log/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightLog(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightLog(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            //if ((bool)result.data)
            //{
            //    unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, dto.Takeoff, 2);
            //}

            return Ok(result);

        }


        [Route("odata/flight/delays/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightDelays(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = unitOfWork.FlightRepository.UpdateDelays(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(true);

        }

        [Route("odata/flight/register/assign")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightRegisterAssign(ViewModels.FlightRegisterDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.AssignFlightRegister(dto);


            if (!(bool)result.data)
                return result;

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }


        [Route("odata/flight/register/change")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightRegisterChange(ViewModels.FlightRegisterChangeLogDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.ChangeFlightRegister(dto);


            if (!string.IsNullOrEmpty(result.data.ToString()))
                return result;

            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }

        [Route("odata/flight/apply/{id}")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightAppy(int id)
        {


            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.ApplyFlight(id);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }

        [Route("odata/flight/offblock")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightOffBlock(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightOffBlock(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, (DateTime)dto.ChocksOut, 14);

            return Ok(result);

        }
        [Route("odata/crew/reportingtime")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostReportingTime(dynamic dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            //  var result = await unitOfWork.FlightRepository.UpdateFlightOffBlock(dto);

            var Id = Convert.ToInt32(dto.Id);
            var eid = Convert.ToInt32(dto.Eid);
            var offset = Convert.ToInt32(dto.Offset);

            var Date = Convert.ToDateTime(dto.Date);
            await unitOfWork.FlightRepository.UpdateReportingTime(Id, eid, Date, offset);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(true);

        }

        [Route("odata/flight/onblock")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightOnBlock(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightOnBlock(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, (DateTime)dto.ChocksIn, 15);

            return Ok(result);

        }

        [Route("odata/flight/takeoff")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightTakeOff(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightTakeOff(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, (DateTime)dto.Takeoff, 2);

            return Ok(result);

        }

        [Route("odata/flight/landing")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightLanding(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightLanding(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, (DateTime)dto.Landing, 3);

            return Ok(result);

        }


        [Route("odata/flight/arrival/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightArrival(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }


            var result = await unitOfWork.FlightRepository.UpdateFlightArrival(dto);
            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            if ((bool)result.data)
            {
                unitOfWork.FlightRepository.SetFlightStatusWeather(dto.ID, dto.Landing, 3);
            }

            return Ok(result);

        }


        [Route("odata/flights/plan/item/permits/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPermit(dynamic dto)
        {



            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var Id = Convert.ToInt32(dto.Id);
            var FlightPlanId = Convert.ToInt32(dto.FlightPlanId);
            var PermitId = Convert.ToInt32(dto.PermitId);
            var Date = Convert.ToDateTime(dto.Date).Date;
            var DateFlight = Convert.ToDateTime(dto.DateFlight).Date;
            var CalanderId = Convert.ToInt32(dto.CalanderId);
            var Remark = dto.Remark.ToString();


            //  var validate = unitOfWork.FlightRepository.ValidateFlight(dto);
            //  if (validate.Code != HttpStatusCode.OK)
            //   return validate;

            FlightPlanItemPermit entity = null;

            if (Id == -1)
            {
                entity = new FlightPlanItemPermit();
                entity.Id = Id;
                unitOfWork.FlightRepository.Insert(entity);
            }

            else
            {
                entity = await unitOfWork.FlightRepository.GetFlightPlanItemPermitById(Id);
                if (entity == null)
                    return Exceptions.getNotFoundException();

            }

            entity.FlightPlanId = FlightPlanId;
            entity.PermitId = PermitId;
            entity.Remark = Remark;
            entity.Date = Date;
            entity.DateFlight = DateFlight;
            entity.CalanderId = CalanderId;




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;



            return Ok(entity);

        }
        [Route("odata/flight/cancel")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightCancel(ViewModels.FlightCancelDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightCancel(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            if ((bool)result.data)
            {
                unitOfWork.FlightRepository.SetFlightStatusWeather(dto.FlightId, dto.CancelDate, 4);
            }

            return Ok(result);

        }


        [Route("odata/flight/ramp")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightRamp(ViewModels.FlightRampDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightRamp(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            if ((bool)result.data)
            {
                unitOfWork.FlightRepository.SetFlightStatusWeather(dto.FlightId, dto.RampDate, 9);
            }

            return Ok(result);

        }

        [Route("odata/flight/redirect")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightRedirect(ViewModels.FlightRedirectDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightRedirect(dto);




            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            if ((bool)result.data)
            {
                unitOfWork.FlightRepository.SetFlightStatusWeather(dto.FlightId, dto.RedirectDate, 17);
            }

            return Ok(result);

        }


        [Route("odata/flight/pax")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightPax(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightPax(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }

        [Route("odata/flight/cargo")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightCargo(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightCargo(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }

        [Route("odata/flight/fuel/departure")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightFuelDeparture(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightFuelDeparture(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

        }

        [Route("odata/flight/fuel/arrival")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightFuelArrival(ViewModels.FlightSaveDto dto)
        {

            if (dto == null)
                return Exceptions.getNullException(ModelState);
            if (!ModelState.IsValid)
            {

                return Exceptions.getModelValidationException(ModelState);
            }

            var result = await unitOfWork.FlightRepository.UpdateFlightFuelArrival(dto);



            var saveResult = await unitOfWork.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(result);

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
