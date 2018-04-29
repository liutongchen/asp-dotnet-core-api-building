using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore {
        // the static constructor below make sure the data is immutable;
        public static CitiesDataStore Current { get; }= new CitiesDataStore(); 
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore() {
            // init dummy data
            Cities = new List<CityDto> {
                new CityDto {
                    Id = 1,
                    Name = "New York City",
                    Description = "The Apple City",
                    PointOfInterest = new List<PointOfInterestDto> {
                        new PointOfInterestDto{
                            Id = 1,
                            Name = "Central Park",
                            Description = "Most visited place in NY"
                        }
                    }
                },
                new CityDto {
                    Id = 2, 
                    Name = "Vancouver",
                    Description = "Our First Home",
                    PointOfInterest = new List<PointOfInterestDto> {
                        new PointOfInterestDto{
                            Id = 1,
                            Name = "English Bay",
                            Description = "A pretty beach near home"
                        },
                        new PointOfInterestDto{
                            Id = 1,
                            Name = "Costco",
                            Description = "A place to buy food"
                        }
                    }
                },
                new CityDto {
                    Id = 3,
                    Name = "Zhanjiang",
                    Description = "My Hometown",
                    PointOfInterest = new List<PointOfInterestDto> {
                        new PointOfInterestDto{
                            Id = 1,
                            Name = "Chang Da Change",
                            Description = "A supermarket I used to hangout with my parents."
                        }
                    }
                }
            };
        }
    }
}