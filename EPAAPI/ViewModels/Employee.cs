using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAAPI.ViewModels
{
    public class Employee:PersonCustomer
    {
       
        public string PID { get; set; }
        public string Phone { get; set; }

        List<EmployeeLocation> locations = null;
        public List<EmployeeLocation> Locations
        {
            get
            {
                if (locations == null)
                    locations = new List<EmployeeLocation>();
                return locations;

            }
            set { locations = value; }
        }
        public static void Fill(Models.Employee entity, ViewModels.Employee employee)
        {
            entity.Id = employee.Id;
            entity.PID = employee.PID;
            entity.Phone = employee.Phone;
        }
    }
}