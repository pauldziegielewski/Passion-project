using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_project.Models;
using Passion_project.Models.ViewModels;

namespace Passion_project.Controllers
{
    public class LocationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // ------------------------------ LIST LOCATIONS

        /// <summary>
        /// Returns all Locations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Locations in the database, including their associated hiking trails.
        /// </returns>
        /// <example>
        /// GET: api/LocationData/ListLocations
        /// </example>

        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocations()
        {
            List<Location> locations = db.Locations.ToList();
            List<LocationDto> locationDtos = new List<LocationDto>();

            foreach (Location location in locations)
            {
                LocationDto locationDto = new LocationDto()
                {
                    LocationID = location.LocationID,
                    LocationName = location.LocationName
                };

                locationDtos.Add(locationDto);
            }

            return Ok(locationDtos);
        }



        // ------------------------------------------ FIND LOCATION
        /// <summary>
        /// Returns a particular location based on locationID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Location in the database matching a particular ID
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of a location</param>
        /// <example>
        /// GET: api/SpeciesData/FindSpecies/5
        /// </example>

        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult FindLocation(int id)
        {
            Location Location = db.Locations.Find(id);
            LocationDto LocationDto = new LocationDto()
            {
                LocationID = Location.LocationID,
                LocationName = Location.LocationName
            };
            if (Location == null)
            {
                return NotFound();
            }

            return Ok(LocationDto);
        }

    // ------------------------------------------ UPDATE LOCATION
    /// <summary>
    /// Updates a location in the database with Post data input
    /// </summary>
    /// <param name="id">Represents the Location ID primary key</param>
    /// <param name="Location">JSON FORM DATA of an location</param>
    /// <returns>
    /// HEADER: 204 (Success, No Content Response)
    /// or
    /// HEADER: 400 (Bad Request)
    /// or
    /// HEADER: 404 (Not Found)
    /// </returns>
    /// <example>
    /// POST: api/LocationData/UpdateLocation/5
    /// FORM DATA: Species JSON Object
    /// </example>
    // PUT: api/LocationData/UpdateLocation/5
    [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationID)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


    // -------------------------------------------- ADD LOCATION
    // POST: api/LocationData/AddLocation
    /// <summary>
    /// Adds a location to the system
    /// </summary>
    /// <param name="Location">JSON FORM DATA of an location</param>
    /// <returns>
    /// HEADER: 201 (Created)
    /// CONTENT: LocationID, location Data
    /// or
    /// HEADER: 400 (Bad Request)
    /// </returns>
    /// <example>
    /// POST: api/LocationData/AddLocation
    /// FORM DATA: Location JSON Object
    /// </example>
    [HttpPost]
        [ResponseType(typeof(Location))]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationID }, location);
        }


    // --------------------------------------------- DELETE LOCATION
    /// <summary>
    /// Deletes a location from database by its ID
    /// </summary>
    /// <param name="id">The primary key of the Location</param>
    /// <returns>
    /// HEADER: 200 (OK)
    /// or
    /// HEADER: 404 (NOT FOUND)
    /// </returns>
    /// <example>
    /// POST: api/LocationData/DeleteLocation/5
    /// FORM DATA: (empty)
    /// </example>
    // DELETE: api/LocationData/DeleteLocation/5
    [HttpPost]
        [ResponseType(typeof(Location))]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Any(e => e.LocationID == id);
        }
    }
}
