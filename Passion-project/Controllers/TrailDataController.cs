using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_project.Models;

namespace Passion_project.Controllers
{
    public class TrailDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // ----------------------- LIST TRAILS----------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/TrailData/ListTrails
        [HttpGet]
        public IEnumerable<TrailDto> ListTrails()
        {
            List<Trail> Trails = db.Trails.ToList();
            List<TrailDto> TrailDtos = new List<TrailDto>();

            Trails.ForEach(a => TrailDtos.Add(new TrailDto()
            {
                TrailID = a.TrailID,
                TrailName = a.TrailName,
                LocationID = a.LocationID,
                LocationName = a.Location.LocationName


            }));

            return TrailDtos;

        }



        // ---------- LIST TRAILS FOR LOCATIONS----------
        /// <summary>
        /// Gather information about all the trails related to a particular location
        /// </summary>
        /// <returns>
        /// Header: 200 (ok)
        /// Content: all trails in the data, including their associated location
        /// </returns>
        /// <param> name="id">Location ID </param>
        // GET: api/TrailData/ListTrailsForLocation
        [HttpGet]
        [ResponseType(typeof(TrailDto))]

        public IHttpActionResult ListTrailsForLocation(int? id) {

            if (id == null)
            {
                return BadRequest("Location ID is required");
            }
        

            List<Trail> Trails = db.Trails.Where(a=>a.LocationID==id).ToList();
            List<TrailDto> TrailDtos = new List<TrailDto>();

            Trails.ForEach(a => TrailDtos.Add(new TrailDto()
            {
                TrailID = a.TrailID,
                TrailName = a.TrailName,
                LocationID = a.LocationID,
                LocationName = a.Location.LocationName


            }));

            return Ok(TrailDtos);

        }






        // ----------------------------------------------------------------- LIST TRAILS Associated with FEATURES----------
        /// <summary>
        /// Gathers trails that are assocaited with a particular feature
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all trails in the database, including their location and feature
        /// </returns>
        /// <param name="id">FeatureID</param>
        /// <example>
        /// GET: api/traildata/ListTrailFeatures
        /// </example>
   
        [HttpGet]
        [ResponseType(typeof(TrailDto))]
        public IHttpActionResult ListTrailFeatures(int id)
        {
            //All trails that match with a particular feature
            List<Trail> Trails = db.Trails.Where(
                t => t.Features.Any(
                    f=>f.FeatureID==id
                    )).ToList();
            List<TrailDto> TrailDtos = new List<TrailDto>();

            Trails.ForEach(a => TrailDtos.Add(new TrailDto()
            {
                TrailID = a.TrailID,
                TrailName = a.TrailName,
                LocationID = a.LocationID,
                LocationName = a.Location.LocationName


            }));

            return Ok(TrailDtos);

        }



        // ------------------------------ FIND TRAIL --------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/TrailData/FindTrail/5
        [HttpGet]
        [ResponseType(typeof(TrailDto))]
        public IHttpActionResult FindTrail(int id)
        {
            List<Trail> Trails = db.Trails.ToList();
            List<TrailDto> TrailDtos = new List<TrailDto>();

            Trails.ForEach(a => TrailDtos.Add(new TrailDto()
            {
                TrailID = a.TrailID,
                TrailName = a.TrailName,
                LocationID = a.Location.LocationID,
                LocationName = a.Location.LocationName

            }));
            

            return Ok(TrailDtos.FirstOrDefault(t => t.TrailID == id));
        }


        // --------------------- UPDATE TRAIL----------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trail"></param>
        /// <returns></returns>
        // POST: api/TrailData/UpdateTrail/5
        // {
        //"trailID": 1,
        //"trialName": "Hockley Valley",
        //"locationID": 2
        // }
        // copy json folder path then in command prompt cd C:\Users\paulj\Desktop\Web Development\5112\Christine\Passion-project\Passion-project\JSON => then

        //curl -d @trail.json -H "Content-type:application/json" https://localhost:44367/api/traildata/updatetrail/1
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTrail(int id, Trail trail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trail.TrailID)
            {
                return BadRequest();
            }

            db.Entry(trail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrailExists(id))
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
       


        // -------------------------------- ADD TRAIL--------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        // POST: api/TrailData/AddTrail
        // copy json folder path then in command prompt cd C:\Users\paulj\Desktop\Web Development\5112\Christine\Passion-project\Passion-project\JSON => then
        //curl -d @trail.json -H "Content-type:application/json" https://localhost:44367/api/traildata/addtrail
        [ResponseType(typeof(Trail))]
        [HttpPost]
        public IHttpActionResult AddTrail(Trail trail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trails.Add(trail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trail.TrailID }, trail);
        }



        // ----------------------- DELETE TRAIL -------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/TrailData/DeleteTrail/5
        //curl -d "" https://localhost:44367/api/traildata/deletetrail/1 is the alternative in command prompt
        [ResponseType(typeof(Trail))]
        [HttpPost]
        public IHttpActionResult DeleteTrail(int id)
        {
            Trail trail = db.Trails.Find(id);
            if (trail == null)
            {
                return NotFound();
            }

            db.Trails.Remove(trail);
            db.SaveChanges();

            return Ok(trail);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrailExists(int id)
        {
            return db.Trails.Count(e => e.TrailID == id) > 0;
        }

        /// <summary>
        /// Associates a particular feature with a particular trail
        /// </summary>
        /// <param name="TrailID">The trail ID</param>
        /// <param name="FeatureID">The feature id</param>
        /// <returns>
        /// Header: 200 (ok)
        /// or
        /// Header: 404 (not found)
        /// </returns>
        [HttpPost]
        [Route("api/traildata/AssociateTrailWithFeature")]
        public IHttpActionResult AssociateTrailWithFeature(int TrailID, int FeatureID)
        {
            Trail SelectedTrail = db.Trails.Include(t => t.Features).Where(t=>t.TrailID == TrailID).FirstOrDefault();

            Feature SelectedFeature = db.Features.Find(FeatureID);

            // SelectedFeature.Trail.Add(SelectedTrail); -> if this method is in the FeatureController

            SelectedTrail.Features.Add(SelectedFeature);
            db.SaveChanges();
            return Ok();
        }




        /// <summary>
        /// Associates a particular feature with a particular trail
        /// </summary>
        /// <param name="TrailID">The trail ID</param>
        /// <param name="FeatureID">The feature id</param>
        /// <returns>
        /// Header: 200 (ok)
        /// or
        /// Header: 404 (not found)
        /// </returns>
        [HttpPost]
        [Route("api/traildata/UnAssociateTrailWithFeature")]
        public IHttpActionResult UnAssociateTrailWithFeature(int TrailID, int FeatureID)
        {
            Trail SelectedTrail = db.Trails.Include(t => t.Features).Where(t => t.TrailID == TrailID).FirstOrDefault();

            Feature SelectedFeature = db.Features.Find(FeatureID);

            // SelectedFeature.Trail.Add(SelectedTrail); -> if this method is in the FeatureController

            SelectedTrail.Features.Remove(SelectedFeature);
            db.SaveChanges();
            return Ok();
        }
    }
}