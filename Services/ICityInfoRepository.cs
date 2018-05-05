using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId);
        IEnumerable<City> GetPointsOfInterestForCity(int cityId); 
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
    }
}