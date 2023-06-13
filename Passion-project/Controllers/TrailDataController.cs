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


        // ---------------------------- LIST TRAILS----------
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
                LocationName = a.Location.LocationName
         

            }));

            return TrailDtos;
            
        }




        // ------------------------------ FIND TRAIL --------
        // GET: api/TrailData/FindTrail/5
        [HttpGet]
        [ResponseType(typeof(Trail))]
        public IHttpActionResult FindTrail(int id)
        {
            Trail trail = db.Trails.Find(id);
            TrailDto TrailDto = new TrailDto()
            {
                TrailID = trail.TrailID,
                TrailName = trail.TrailName,
                LocationName = trail.Location.LocationName
            };
            if (trail == null)
            {
                return NotFound();
            }

            return Ok(TrailDto);
        }



        // --------------------- UPDATE TRAIL----------------
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
    }
}