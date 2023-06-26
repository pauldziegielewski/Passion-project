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
    public class FeatureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // -------------------------------------------- LIST FEATURES
        // GET: api/FeatureData/ListFeatures
        [HttpGet]
        public IHttpActionResult ListFeatures()
        {
            List<Feature> features = db.Features.ToList();
            List<FeatureDto> featureDtos = new List<FeatureDto>();
            foreach (var feature in features)
            {
                featureDtos.Add(new FeatureDto()
                {
                    FeatureID = feature.FeatureID,
                    FeatureName = feature.FeatureName,

                });
            }
            return Ok(featureDtos);
        }



        // ------------------------------------LIST FEATURES
        // GET: api/FeatureData/ListFeatures
        [HttpGet]
        public IHttpActionResult ListFeaturesForTrail(int id)
        {
            List<Feature> Features = db.Features.Where(
                f => f.Trails.Any(
                    t => t.TrailID == id)
                ).ToList();

            List<FeatureDto> FeatureDtos = new List<FeatureDto>();

            Features.ForEach(f => FeatureDtos.Add(new FeatureDto()

            { 
                    FeatureID = f.FeatureID,
                    FeatureName = f.FeatureName,



                }));
 
            return Ok(FeatureDtos);
        }

        // ------------------------------------------- FIND FEATURE GET
        // GET: api/FeatureData/FindFeature/5
        [ResponseType(typeof(Feature))]
        [HttpGet]
        public IHttpActionResult FindFeature(int id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }

            return Ok(feature);
        }
         
    


        // ------------------------------------------- FIND FEATURE POST
        // POST: api/FeatureData/FindFeature/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult FindFeature(int id, Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feature.FeatureID)
            {
                return BadRequest();
            }

            db.Entry(feature).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(id))
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


        // --------------------------------------------- UPDATE FEATURE
        // POST: api/FeatureData
        [ResponseType(typeof(Feature))]
        public IHttpActionResult PostFeature(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Features.Add(feature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feature.FeatureID }, feature);
        }



        // ----------------------------------------------- DELETE FEATURE
        // DELETE: api/FeatureData/5
        [ResponseType(typeof(Feature))]
        public IHttpActionResult DeleteFeature(int id)
        {
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }

            db.Features.Remove(feature);
            db.SaveChanges();

            return Ok(feature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatureExists(int id)
        {
            return db.Features.Count(e => e.FeatureID == id) > 0;
        }
    }
}