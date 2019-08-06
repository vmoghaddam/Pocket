﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPAAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EPAGRIFFINEntities : DbContext
    {
        public EPAGRIFFINEntities()
            : base("name=EPAGRIFFINEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AircraftModel> AircraftModels { get; set; }
        public virtual DbSet<AircraftType> AircraftTypes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CaoBasic> CaoBasics { get; set; }
        public virtual DbSet<CaoBasicLicenseType> CaoBasicLicenseTypes { get; set; }
        public virtual DbSet<CaoBasicType> CaoBasicTypes { get; set; }
        public virtual DbSet<CaoCategory> CaoCategories { get; set; }
        public virtual DbSet<CaoType> CaoTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CourseCategory> CourseCategories { get; set; }
        public virtual DbSet<CourseType> CourseTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<EmployeeLocation> EmployeeLocations { get; set; }
        public virtual DbSet<FileType> FileTypes { get; set; }
        public virtual DbSet<LicenseResultBasic> LicenseResultBasics { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<PersonAircraftType> PersonAircraftTypes { get; set; }
        public virtual DbSet<PersonCaoLicenceHistory> PersonCaoLicenceHistories { get; set; }
        public virtual DbSet<PersonCaoLicense> PersonCaoLicenses { get; set; }
        public virtual DbSet<PersonEducationDocument> PersonEducationDocuments { get; set; }
        public virtual DbSet<PersonExperiense> PersonExperienses { get; set; }
        public virtual DbSet<PersonRatingDocument> PersonRatingDocuments { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<RoleOrganizational> RoleOrganizationals { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<HelperPerson> HelperPersons { get; set; }
        public virtual DbSet<ViewAircraftType> ViewAircraftTypes { get; set; }
        public virtual DbSet<ViewManufacturer> ViewManufacturers { get; set; }
        public virtual DbSet<ViewCaoType> ViewCaoTypes { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CourseAircraftType> CourseAircraftTypes { get; set; }
        public virtual DbSet<CourseCatRate> CourseCatRates { get; set; }
        public virtual DbSet<CourseRelatedAircraftType> CourseRelatedAircraftTypes { get; set; }
        public virtual DbSet<CourseRelatedCourse> CourseRelatedCourses { get; set; }
        public virtual DbSet<CourseRelatedCourseType> CourseRelatedCourseTypes { get; set; }
        public virtual DbSet<CourseRelatedEmployee> CourseRelatedEmployees { get; set; }
        public virtual DbSet<CourseRelatedGroup> CourseRelatedGroups { get; set; }
        public virtual DbSet<CourseRelatedStudyField> CourseRelatedStudyFields { get; set; }
        public virtual DbSet<ViewPersonAircraftType> ViewPersonAircraftTypes { get; set; }
        public virtual DbSet<ViewPersonCaoLicense> ViewPersonCaoLicenses { get; set; }
        public virtual DbSet<ViewPersonExperiense> ViewPersonExperienses { get; set; }
        public virtual DbSet<ViewPersonDocument> ViewPersonDocuments { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<PersonDocument> PersonDocuments { get; set; }
        public virtual DbSet<ViewPersonDocumentFile> ViewPersonDocumentFiles { get; set; }
        public virtual DbSet<PersonRating> PersonRatings { get; set; }
        public virtual DbSet<ViewPersonRating> ViewPersonRatings { get; set; }
        public virtual DbSet<PersonCustomer> PersonCustomers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<ViewCourse> ViewCourses { get; set; }
        public virtual DbSet<PersonCourse> PersonCourses { get; set; }
        public virtual DbSet<ViewCourseNotificationEnabled> ViewCourseNotificationEnableds { get; set; }
        public virtual DbSet<ViewEmployeeSimple> ViewEmployeeSimples { get; set; }
        public virtual DbSet<ViewApplicableCourse> ViewApplicableCourses { get; set; }
        public virtual DbSet<ViewApplicableCoursePerson> ViewApplicableCoursePersons { get; set; }
        public virtual DbSet<ViewCertificate> ViewCertificates { get; set; }
        public virtual DbSet<ViewCourseType> ViewCourseTypes { get; set; }
        public virtual DbSet<BookAutor> BookAutors { get; set; }
        public virtual DbSet<BookFile> BookFiles { get; set; }
        public virtual DbSet<BookKeyword> BookKeywords { get; set; }
        public virtual DbSet<BookRelatedAircraftType> BookRelatedAircraftTypes { get; set; }
        public virtual DbSet<BookRelatedEmployee> BookRelatedEmployees { get; set; }
        public virtual DbSet<BookRelatedGroup> BookRelatedGroups { get; set; }
        public virtual DbSet<BookRelatedStudyField> BookRelatedStudyFields { get; set; }
        public virtual DbSet<ViewBookFile> ViewBookFiles { get; set; }
        public virtual DbSet<ViewBookAuthor> ViewBookAuthors { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<ViewBook> ViewBooks { get; set; }
        public virtual DbSet<ViewBookApplicableEmployee> ViewBookApplicableEmployees { get; set; }
        public virtual DbSet<ViewBookApplicableEmployeeAb> ViewBookApplicableEmployeeAbs { get; set; }
        public virtual DbSet<PersonMisc> PersonMiscs { get; set; }
        public virtual DbSet<ViewOrganization> ViewOrganizations { get; set; }
        public virtual DbSet<ViewPersonMisc> ViewPersonMiscs { get; set; }
        public virtual DbSet<ViewJournal> ViewJournals { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserActivityMenuHit> UserActivityMenuHits { get; set; }
        public virtual DbSet<UserActivity> UserActivities { get; set; }
        public virtual DbSet<ViewUserActivity> ViewUserActivities { get; set; }
        public virtual DbSet<SumCarelessEmployee> SumCarelessEmployees { get; set; }
        public virtual DbSet<SumCarelessEmployeeTotal> SumCarelessEmployeeTotals { get; set; }
        public virtual DbSet<SumLibraryDownloadByMonth> SumLibraryDownloadByMonths { get; set; }
        public virtual DbSet<SumLibraryAddedByMonth> SumLibraryAddedByMonths { get; set; }
        public virtual DbSet<JobGroup> JobGroups { get; set; }
        public virtual DbSet<ViewPost> ViewPosts { get; set; }
        public virtual DbSet<ViewStudyField> ViewStudyFields { get; set; }
        public virtual DbSet<ViewLocation> ViewLocations { get; set; }
        public virtual DbSet<SumEmployeeAge> SumEmployeeAges { get; set; }
        public virtual DbSet<SumEmployeeDegree> SumEmployeeDegrees { get; set; }
        public virtual DbSet<SumEmployeeJobGroup> SumEmployeeJobGroups { get; set; }
        public virtual DbSet<SumEmployeeLocation> SumEmployeeLocations { get; set; }
        public virtual DbSet<SumEmployeeSex> SumEmployeeSexes { get; set; }
        public virtual DbSet<SumEmployeeStudyField> SumEmployeeStudyFields { get; set; }
        public virtual DbSet<ViewEmployeeAge> ViewEmployeeAges { get; set; }
        public virtual DbSet<ViewEmployeeLocation> ViewEmployeeLocations { get; set; }
        public virtual DbSet<ViewJobGroup> ViewJobGroups { get; set; }
        public virtual DbSet<SumEmployeeExp> SumEmployeeExps { get; set; }
        public virtual DbSet<ViewEmployeeExp> ViewEmployeeExps { get; set; }
        public virtual DbSet<SumEmployeeMaritalStatu> SumEmployeeMaritalStatus { get; set; }
        public virtual DbSet<SumActiveCourse> SumActiveCourses { get; set; }
        public virtual DbSet<SumCertificateStatu> SumCertificateStatus { get; set; }
        public virtual DbSet<SumEmployeeDateAlert> SumEmployeeDateAlerts { get; set; }
        public virtual DbSet<SumCertificateType> SumCertificateTypes { get; set; }
        public virtual DbSet<EmployeeBookStatu> EmployeeBookStatus { get; set; }
        public virtual DbSet<SumEmployeeLibraryAlert> SumEmployeeLibraryAlerts { get; set; }
        public virtual DbSet<ViewPersonActiveCourse> ViewPersonActiveCourses { get; set; }
        public virtual DbSet<ViewNotification> ViewNotifications { get; set; }
        public virtual DbSet<FlightGroup> FlightGroups { get; set; }
        public virtual DbSet<G_DelayCode> G_DelayCode { get; set; }
        public virtual DbSet<G_OpOneDelayType> G_OpOneDelayType { get; set; }
        public virtual DbSet<FlightStatu> FlightStatus { get; set; }
        public virtual DbSet<ViewMSN> ViewMSNs { get; set; }
        public virtual DbSet<AvgFlight> AvgFlights { get; set; }
        public virtual DbSet<FlightPlanGroup> FlightPlanGroups { get; set; }
        public virtual DbSet<FlightPlanDay> FlightPlanDays { get; set; }
        public virtual DbSet<FlightPlanMonth> FlightPlanMonths { get; set; }
        public virtual DbSet<FlighPlanCalendar> FlighPlanCalendars { get; set; }
        public virtual DbSet<FlightPlanStatu> FlightPlanStatus { get; set; }
        public virtual DbSet<ViewCity> ViewCities { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<DelayCode> DelayCodes { get; set; }
        public virtual DbSet<DelayCodeCategory> DelayCodeCategories { get; set; }
        public virtual DbSet<ViewDelayCode> ViewDelayCodes { get; set; }
        public virtual DbSet<Ac_MSN> Ac_MSN { get; set; }
        public virtual DbSet<FlightDelay> FlightDelays { get; set; }
        public virtual DbSet<ViewFlightDelayCode> ViewFlightDelayCodes { get; set; }
        public virtual DbSet<FlightStatusLog> FlightStatusLogs { get; set; }
        public virtual DbSet<ViewAirport> ViewAirports { get; set; }
        public virtual DbSet<ViewFlightsAcType> ViewFlightsAcTypes { get; set; }
        public virtual DbSet<ViewFlightsFrom> ViewFlightsFroms { get; set; }
        public virtual DbSet<ViewFlightsRegister> ViewFlightsRegisters { get; set; }
        public virtual DbSet<ViewFlightsTo> ViewFlightsToes { get; set; }
        public virtual DbSet<FlightStatusWeather> FlightStatusWeathers { get; set; }
        public virtual DbSet<ViewFlightRoute> ViewFlightRoutes { get; set; }
        public virtual DbSet<ViewRouteFromAirport> ViewRouteFromAirports { get; set; }
        public virtual DbSet<ViewFlightPlanRegisterAssigned> ViewFlightPlanRegisterAssigneds { get; set; }
        public virtual DbSet<ViewRouteToAirport> ViewRouteToAirports { get; set; }
        public virtual DbSet<ViewFlighPlanAssignedRegister> ViewFlighPlanAssignedRegisters { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<ViewOption> ViewOptions { get; set; }
        public virtual DbSet<FlightLink> FlightLinks { get; set; }
        public virtual DbSet<FlightRegisterChangeLog> FlightRegisterChangeLogs { get; set; }
        public virtual DbSet<FlightPlan> FlightPlans { get; set; }
        public virtual DbSet<FlightPermit> FlightPermits { get; set; }
        public virtual DbSet<ViewFlightPlan> ViewFlightPlans { get; set; }
        public virtual DbSet<ViewFlightPlanCalendarRegisterAll> ViewFlightPlanCalendarRegisterAlls { get; set; }
        public virtual DbSet<ViewFlightPlanCalander> ViewFlightPlanCalanders { get; set; }
        public virtual DbSet<ViewFlightPlanRegister> ViewFlightPlanRegisters { get; set; }
        public virtual DbSet<ViewFlightPlanCalendarRegister> ViewFlightPlanCalendarRegisters { get; set; }
        public virtual DbSet<FlightPlanItemPermit> FlightPlanItemPermits { get; set; }
        public virtual DbSet<ViewFlightPlanItemPermit> ViewFlightPlanItemPermits { get; set; }
        public virtual DbSet<FlightPlanRegister> FlightPlanRegisters { get; set; }
        public virtual DbSet<FlightCrew> FlightCrews { get; set; }
        public virtual DbSet<FlightCrewChangeHistory> FlightCrewChangeHistories { get; set; }
        public virtual DbSet<ViewFlightPlanItem> ViewFlightPlanItems { get; set; }
        public virtual DbSet<FlightPlanItem> FlightPlanItems { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<CrewAvailability> CrewAvailabilities { get; set; }
        public virtual DbSet<CrewAvailabilityStatu> CrewAvailabilityStatus { get; set; }
        public virtual DbSet<FlightPlanCalanderCrew> FlightPlanCalanderCrews { get; set; }
        public virtual DbSet<ViewFlightPlanItemCalander> ViewFlightPlanItemCalanders { get; set; }
        public virtual DbSet<Box> Boxes { get; set; }
        public virtual DbSet<ViewEmployeeLight> ViewEmployeeLights { get; set; }
        public virtual DbSet<CrewTypeRequirement> CrewTypeRequirements { get; set; }
        public virtual DbSet<ViewBoxCrewRequirement> ViewBoxCrewRequirements { get; set; }
        public virtual DbSet<ViewFlightPlanCalanderCrew> ViewFlightPlanCalanderCrews { get; set; }
        public virtual DbSet<ViewFlightCrew> ViewFlightCrews { get; set; }
        public virtual DbSet<ViewBoxCrewFlight> ViewBoxCrewFlights { get; set; }
        public virtual DbSet<ViewFlightDelay> ViewFlightDelays { get; set; }
        public virtual DbSet<PersonEducation> PersonEducations { get; set; }
        public virtual DbSet<ViewPersonEducation> ViewPersonEducations { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<ViewCrew> ViewCrews { get; set; }
        public virtual DbSet<ViewEmployee> ViewEmployees { get; set; }
        public virtual DbSet<BoxCrew> BoxCrews { get; set; }
        public virtual DbSet<ViewBoxCrew> ViewBoxCrews { get; set; }
        public virtual DbSet<ViewFlightCrew2> ViewFlightCrew2 { get; set; }
        public virtual DbSet<ViewFlightFuel> ViewFlightFuels { get; set; }
        public virtual DbSet<FlightInformation> FlightInformations { get; set; }
        public virtual DbSet<ViewFlightInformation> ViewFlightInformations { get; set; }
        public virtual DbSet<BoxFlightPlanItem> BoxFlightPlanItems { get; set; }
        public virtual DbSet<ViewBox> ViewBoxes { get; set; }
        public virtual DbSet<ViewCrewCalendar> ViewCrewCalendars { get; set; }
        public virtual DbSet<ViewCrewCalendarSplited> ViewCrewCalendarSpliteds { get; set; }
        public virtual DbSet<EmployeeCalendar> EmployeeCalendars { get; set; }
        public virtual DbSet<EmployeeCalendarSplited> EmployeeCalendarSpliteds { get; set; }
        public virtual DbSet<ViewCrewTime> ViewCrewTimes { get; set; }
        public virtual DbSet<ViewCrewTimeDetail> ViewCrewTimeDetails { get; set; }
    
        public virtual ObjectResult<ViewCrewTime> GetOverDuty(string aDate, Nullable<int> aDuty, Nullable<int> aFlight, Nullable<int> aPID)
        {
            var aDateParameter = aDate != null ?
                new ObjectParameter("ADate", aDate) :
                new ObjectParameter("ADate", typeof(string));
    
            var aDutyParameter = aDuty.HasValue ?
                new ObjectParameter("ADuty", aDuty) :
                new ObjectParameter("ADuty", typeof(int));
    
            var aFlightParameter = aFlight.HasValue ?
                new ObjectParameter("AFlight", aFlight) :
                new ObjectParameter("AFlight", typeof(int));
    
            var aPIDParameter = aPID.HasValue ?
                new ObjectParameter("APID", aPID) :
                new ObjectParameter("APID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ViewCrewTime>("GetOverDuty", aDateParameter, aDutyParameter, aFlightParameter, aPIDParameter);
        }
    
        public virtual ObjectResult<ViewCrewTime> GetOverDuty(string aDate, Nullable<int> aDuty, Nullable<int> aFlight, Nullable<int> aPID, MergeOption mergeOption)
        {
            var aDateParameter = aDate != null ?
                new ObjectParameter("ADate", aDate) :
                new ObjectParameter("ADate", typeof(string));
    
            var aDutyParameter = aDuty.HasValue ?
                new ObjectParameter("ADuty", aDuty) :
                new ObjectParameter("ADuty", typeof(int));
    
            var aFlightParameter = aFlight.HasValue ?
                new ObjectParameter("AFlight", aFlight) :
                new ObjectParameter("AFlight", typeof(int));
    
            var aPIDParameter = aPID.HasValue ?
                new ObjectParameter("APID", aPID) :
                new ObjectParameter("APID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ViewCrewTime>("GetOverDuty", mergeOption, aDateParameter, aDutyParameter, aFlightParameter, aPIDParameter);
        }
    
        public virtual ObjectResult<Nullable<double>> GetFDPReduction(Nullable<int> boxId, Nullable<int> sTBId)
        {
            var boxIdParameter = boxId.HasValue ?
                new ObjectParameter("BoxId", boxId) :
                new ObjectParameter("BoxId", typeof(int));
    
            var sTBIdParameter = sTBId.HasValue ?
                new ObjectParameter("STBId", sTBId) :
                new ObjectParameter("STBId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<double>>("GetFDPReduction", boxIdParameter, sTBIdParameter);
        }
    }
}