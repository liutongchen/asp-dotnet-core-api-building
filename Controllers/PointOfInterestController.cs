using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller {
        [HttpGet("{cityId}/pointOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId) {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }
            return Ok(city.PointOfInterest);
        }

        [HttpGet("{cityId}/pointOfInterest/{interestId}")]
        public IActionResult GetPointOfInterest(int cityId, int interestId) {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            } 
            var interestPoint = city.PointOfInterest.FirstOrDefault(interest => interest.Id == interestId);
            if (interestPoint == null) {
                return NotFound();
            }
            return Ok(interestPoint);
        }
    }
}
