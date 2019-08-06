using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EPAAPI.ViewModels
{
    public class Airport
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
       // [Required(ErrorMessage = "IATA is required")]
        public string IATA { get; set; }
        public string ICAO { get; set; }
        [Required(ErrorMessage = "City is required")]
        public int CityId { get; set; }

       // public string ImportId { get; set; }
       // public string Type { get; set; }
        public static void Fill(Models.Airport entity, ViewModels.Airport airport)
        {
            entity.Id = airport.Id;
            entity.Name = airport.Name;
            entity.IATA = airport.IATA;
            entity.ICAO = airport.ICAO;
            entity.CityId = airport.CityId;
           // entity.ImportId = airport.ImportId;
            //entity.Type = airport.Type;
        }
    }
}