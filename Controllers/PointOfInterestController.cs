using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers {
    [Route ("api/cities")]
    public class PointOfInterestController : Controller {
        private ILogger<PointOfInterestController> _logger;
        private IMaiService _mailServices;

        public PointOfInterestController (ILogger<PointOfInterestController> logger,  IMaiService mailServices) {
            _logger = logger;
            _mailServices = mailServices;
        }

        [HttpGet ("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest (int cityId) {
            try {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
                if (city == null) {
                    _logger.LogInformation ($"City with id {cityId} wasn't found.");
                    return NotFound ();
                }
                return Ok (city.PointOfInterest);
            } catch {
                _logger.LogCritical ($"Exception while getting points of interest for city with id {cityId}");
                return StatusCode (500, "A problem happens while handling your request");
            }
        }

        [HttpGet ("{cityId}/pointsOfInterest/{interestId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest (int cityId, int interestId) {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
            if (city == null) {
                return NotFound ();
            }
            var interestPoint = city.PointOfInterest.FirstOrDefault (interest => interest.Id == interestId);
            if (interestPoint == null) {
                return NotFound ();
            }
            return Ok (interestPoint);
        }

        [HttpPost ("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest (int cityId, [FromBody] PointsOfInterestForCreationDto pointOfInterest) {
            if (pointOfInterest == null) {
                return BadRequest ();
            }

            if (pointOfInterest.Description == pointOfInterest.Name) {
                ModelState.AddModelError ("Description", "Description should be different from name.");
            }

            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
            if (city == null) {
                return NotFound ();
            }

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany (c => c.PointOfInterest).Max (p => p.Id);
            var finalPointOfInterest = new PointsOfInterestDto () {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointOfInterest.Add (finalPointOfInterest);

            return CreatedAtRoute (
                "GetPointOfInterest",
                new { cityId = cityId, interestId = finalPointOfInterest.Id },
                finalPointOfInterest);
        }

        [HttpPut ("{cityId}/pointsOfInterest/{interestId}")]
        public IActionResult UpdatePointOfInterest (int cityId, int interestId, [FromBody] PointsOfInterestForUpdateDto pointOfInterest) {
            if (pointOfInterest == null) {
                return BadRequest ();
            }
            if (pointOfInterest.Description == pointOfInterest.Name) {
                ModelState.AddModelError ("Description", "Description should be different from name.");
            }
            if (!ModelState.IsValid) {
                return BadRequest ();
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
            if (city == null) {
                return NotFound ();
            }
            var interestPointFromStore = city.PointOfInterest.FirstOrDefault (interest => interest.Id == interestId);
            if (interestPointFromStore == null) {
                return NotFound ();
            }
            interestPointFromStore.Name = pointOfInterest.Name;
            interestPointFromStore.Description = pointOfInterest.Description;
            return NoContent ();
        }

        [HttpPatch ("{cityId}/pointsOfInterest/{id}")]
        public IActionResult partiallyUpdatePointOfInterest (int cityId, int id, [FromBody] JsonPatchDocument<PointsOfInterestForUpdateDto> patchDoc) {

            if (patchDoc == null) {
                return BadRequest ();
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
            if (city == null) {
                return NotFound ();
            }
            var interestPointFromStore = city.PointOfInterest.FirstOrDefault (interest => interest.Id == id);
            if (interestPointFromStore == null) {
                return NotFound ();
            }

            var interestPointToPatch = new PointsOfInterestForUpdateDto {
                Name = interestPointFromStore.Name,
                Description = interestPointFromStore.Description
            };
            patchDoc.ApplyTo (interestPointToPatch, ModelState);

            if (interestPointToPatch.Description == interestPointToPatch.Name) {
                ModelState.AddModelError ("Description", "Name and description cannot be the same");
            }
            TryValidateModel (interestPointToPatch);

            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            interestPointFromStore.Name = interestPointToPatch.Name;
            interestPointFromStore.Description = interestPointToPatch.Description;

            return NoContent ();
        }

        [HttpDelete ("{cityId}/pointsOfInterest/{id}")]
        public IActionResult deletePointOfInterest (int cityId, int id) {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == cityId);
            if (city == null) {
                return NotFound ();
            }

            var interestPointFromStore = city.PointOfInterest.FirstOrDefault (interest => interest.Id == id);
            if (interestPointFromStore == null) {
                return NotFound ();
            }

            city.PointOfInterest.Remove (interestPointFromStore);

            _mailServices.send("Point of interest deleted", $"point of interest with id {id} was deleted");

            return NoContent ();
        }
    }
}