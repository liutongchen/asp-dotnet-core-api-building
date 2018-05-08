using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Services;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller { // GET api/values
        private ICityInfoRepository _cityInfoRepository;
        
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities() {
            var cityEntities = _cityInfoRepository.GetCities();
            var result = new List<CityWithoutPointsOfInterstDTO>();
            foreach (var cityEntity in cityEntities) {
                result.Add(new CityWithoutPointsOfInterstDTO
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description,
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id) {
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);
            if (cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn);
        }
    }
}
