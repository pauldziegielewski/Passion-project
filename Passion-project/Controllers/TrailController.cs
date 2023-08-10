using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Passion_project.Models;
using Passion_project.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Security.Policy;

namespace Passion_project.Controllers
{
   
    public class TrailController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static TrailController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44367/api/");
        }

        // ---------------------------------- TRAIL LIST
        // GET: Trail/List
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of trails
            //curl https://localhost:44367/api/traildata/listtrails

            //client is anything that's accessing any information from server
            // HttpClient client = new HttpClient() { }; ==> this line of code was in use before (client = new HttpClient();) was added on line 17
            string url = "traildata/listtrails";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine(response.StatusCode);
            //Debug.WriteLine("The response code is");

            //using Passion_project.Models; is needed for <TrailDto> to link up with this TrailController.cs
            IEnumerable<TrailDto> trails = response.Content.ReadAsAsync<IEnumerable<TrailDto>>().Result;

            //Debug.WriteLine("Number of trails received: ");
            //Debug.WriteLine(trails.Count());

            return View(trails);
        }


        // ------------------------------ TRAIL DETAILS
        // GET: Trail/Show/5
        public ActionResult Show(int id)
        {
            TrailDetails ViewModel = new TrailDetails();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false; //  this code is set to always be false for a guest user
          
            //objective: communicate with our animal data api to retrieve one trail by ID
            //curl https://localhost:44367/api/traildata/findtrail/{id}

            //client is anything that's accessing any information from server
            //HttpClient client = new HttpClient() { };
            // url here is the end point for the full url string
            string url = "traildata/findtrail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //using Passion_project.Models; is needed for <TrailDto> to link up with this TrailController.cs
            TrailDto SelectedTrail = response.Content.ReadAsAsync<TrailDto>().Result;

            ViewModel.SelectedTrail = SelectedTrail;



            url = "featuredata/listfeaturesfortrail/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<FeatureDto> AvailableFeatures = response.Content.ReadAsAsync<IEnumerable<FeatureDto>>().Result;

      ViewModel.AvailableFeatures = AvailableFeatures;



            url = "FeatureData/ListFeaturesNotInTrail/" + id;

            response = client.GetAsync(url).Result;
            IEnumerable<FeatureDto> AFeatures = response.Content.ReadAsAsync<IEnumerable<FeatureDto>>().Result;

            ViewModel.AFeatures = AFeatures;

     
            return View(ViewModel);
        }




        // ---------------------------------- ERROR PAGE
        public ActionResult Error()
        {
            return View();
        }




        // -------------------------- NEW TRAIL GET
        // GET: Trail/NewTrail
        public ActionResult NewTrail()
        {
            //Information about all locations in the system
            //GET api/locationdata/listlocations
            string url = "locationdata/listlocations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;
            return View(LocationOptions);
        }


        // -----------------------PostNewTrail
        // POST: Trail/PostNewTrail
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostNewTrail(Trail trail)
        {
            Debug.WriteLine("The json payload is: ");
            Debug.WriteLine(trail.TrailName);
            //objective: add a new trail into our system using API

            //curl -d @animal.json -H "Content-Type:application/json" https://localhost:44367/api/traildata/addtrail
            string url = "traildata/addtrail";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            
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


        // ----------------------------------- EDIT
        // GET: Trail/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateTrail ViewModel = new UpdateTrail();

          // The existing trail information
            string url = "traildata/findtrail/"+ id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrailDto SelectedTrail = response.Content.ReadAsAsync<TrailDto>().Result;
            ViewModel.SelectedTrail = SelectedTrail;

            url = "locationdata/listlocations/";
            response = client.GetAsync(url).Result;
            IEnumerable<LocationDto> LocationOptions = response.Content.ReadAsAsync<IEnumerable<LocationDto>>().Result;

            ViewModel.LocationOptions = LocationOptions;

            // FEATURE TO BE INCLUDED HERE?

            return View(ViewModel);
        }


        //---------------------------------- UPDATE
        // POST: Trail/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, TrailDto Trail, HttpPostedFileBase TrailPic)
        {
            // Construct the URL for updating the trail
            string updateUrl = "traildata/updatetrail/" + id;

            // Serialize the Trail object to JSON
            string jsonPayload = jss.Serialize(Trail);

            // Create the HTTP content for the API request
            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            // Send the API request to update the trail
            HttpResponseMessage updateResponse = client.PostAsync(updateUrl, content).Result;

            // Check if the trail update was successful
            if (!updateResponse.IsSuccessStatusCode)
            {
                // Redirect to the "Error" action if the update failed
                return RedirectToAction("Error");
            }

            // If an image was provided, upload it
            if (TrailPic != null)
            {
                // Construct the URL for uploading the image
                string uploadImageUrl = "TrailData/UploadTrailPic/"+ id;

                // Create the content for the image upload
                MultipartFormDataContent requestContent = new MultipartFormDataContent();
                HttpContent imageContent = new StreamContent(TrailPic.InputStream);

                requestContent.Add(imageContent, "TrailPic", TrailPic.FileName);

                // Upload the image
                HttpResponseMessage imageUploadResponse = client.PostAsync(uploadImageUrl, requestContent).Result;

                // Check if the image upload was successful
                if (!imageUploadResponse.IsSuccessStatusCode)
                {
                    // Redirect to the "Error" action if the image upload failed
                    return RedirectToAction("Error");
                }
            }

            // Redirect to the "List" action after successful update or image upload
            return RedirectToAction("List");
        }




        //--------------------DELETE CONFIRM TRAIL
        // GET: Trail/Delete/5

        public ActionResult DeleteConfirm(int id)
        {
            string url = "traildata/findtrail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TrailDto selectedTrail = response.Content.ReadAsAsync<TrailDto>().Result;

            return View(selectedTrail);
        }

        // POST: Trail/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "traildata/deletetrail/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }










    }
}
