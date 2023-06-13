using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Passion_project.Models;
using System.Web.Script.Serialization;


namespace Passion_project.Controllers
{
   
    public class TrailController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static TrailController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44367/api/traildata/");
        }

        // ---------------------- TRAIL LIST
        // GET: Trail/List
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of trails
            //curl https://localhost:44367/api/traildata/listtrails

            //client is anything that's accessing any information from server
            // HttpClient client = new HttpClient() { }; ==> this line of code was in use before (client = new HttpClient();) was added on line 17
            string url = "listtrails";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine("The response code is");

            //using Passion_project.Models; is needed for <TrailDto> to link up with this TrailController.cs
            IEnumerable<TrailDto> trails = response.Content.ReadAsAsync<IEnumerable<TrailDto>>().Result;

            Debug.WriteLine("Number of trails received: ");
            Debug.WriteLine(trails.Count());

            return View(trails);
        }


        // ----------------------- TRAIL DETAILS
        // GET: Trail/Show/5
        public ActionResult Show(int id)
        {
            //objective: communicate with our animal data api to retrieve one trail by ID
            //curl https://localhost:44367/api/traildata/findtrail/{id}

            //client is anything that's accessing any information from server
            //HttpClient client = new HttpClient() { };
            // url here is the end point for the full url string
            string url = "findtrail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine(response.StatusCode);
            //Debug.WriteLine("The response code is");

            //using Passion_project.Models; is needed for <TrailDto> to link up with this TrailController.cs
            TrailDto selectedTrail = response.Content.ReadAsAsync <TrailDto>().Result;

            //Debug.WriteLine(selectedTrail.TrailName);
            //Debug.WriteLine("trail received");

            return View(selectedTrail);
        }


        // ---------------------------------- ERROR PAGE
        public ActionResult Error()
        {
            return View();
        }


        // --------------------------------- NEW TRAIL GET
        // GET: Trail/NewTrail
        public ActionResult NewTrail()
        {
            return View();
        }


        // ----------------------------- PostNewTrail POST
        // POST: Trail/PostNewTrail
        [HttpPost]
        public ActionResult PostNewTrail(Trail trail)
        {
            Debug.WriteLine("The json payload is: ");
            Debug.WriteLine(trail.TrailName);
            //objective: add a new trail into our system using API

            //curl -d @animal.json -H "Content-Type:application/json" https://localhost:44367/api/traildata/addtrail
            string url = "addtrail";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            
            string JsonPayload = jss.Serialize(trail);

            Debug.WriteLine(JsonPayload);

            HttpContent content = new StringContent(JsonPayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("ERROR");
            }

            //HttpContent content = new StringContent(jsonpayload);
            //content.Headers.ContentType.MediaType = "application/json";

            //return RedirectToAction("List");
        }

        // GET: Trail/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trail/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trail/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trail/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
