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


        // ------------------------------ LIST FEATURES
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



        // ----------------------LIST FEATURES FOR TRAIL

        // GET: api/FeatureData/ListFeaturesForTrail
        [HttpGet]
        [ResponseType(typeof(FeatureDto))]
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


        /// GET: api/KeeperData/ListKeepersNotCaringForAnimal/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FeatureDto))]
        public IHttpActionResult ListFeaturesNotInTrail(int id)
        {
            List<Feature> Features = db.Features.Where(
                k => !k.Trails.Any(
                    a => a.TrailID == id)
                ).ToList();
            List<FeatureDto> FeatureDtos = new List<FeatureDto>();

            Features.ForEach(f => FeatureDtos.Add(new FeatureDto()
            {
                FeatureID = f.FeatureID,
                FeatureName = f.FeatureName
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
        // POST: api/UpdateFeature
        [ResponseType(typeof(Feature))]
        [HttpPost]
        public IHttpActionResult UpdateFeature(int id, Feature Feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != Feature.FeatureID)
            {

                return BadRequest();
            }

            db.Entry(Feature).State = EntityState.Modified;

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



        // ----------------------------------------------- DELETE FEATURE
        // DELETE: api/FeatureData/5
        [ResponseType(typeof(Feature))]
        [HttpPost]
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



        // ------------------------------------ ADD FEATURE
        /// <summary>
        /// Adds an Feature to the system
        /// </summary>
        /// <param name="Feature">JSON FORM DATA of an Feature</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Feature ID, Feature Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/FeatureData/AddFeature
        /// FORM DATA: Feature JSON Object
        /// </example>
        [ResponseType(typeof(Feature))]
        [HttpPost]
        public IHttpActionResult AddFeature(Feature Feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Features.Add(Feature);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Feature.FeatureID }, Feature);
        }


    }
}