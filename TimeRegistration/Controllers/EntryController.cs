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
using TimeRegistration.Models;
using TimeRegistration.Enums;

namespace TimeRegistration.Controllers
{
    [RoutePrefix("API")]
    public class EntryController : ApiController
    {
        private TimeRegistrationEntities db = new TimeRegistrationEntities();

        // GET: api/Entry
        [AllowAnonymous]
        [HttpGet]
        [Route("Entries")]
        [ResponseType(typeof(List<Entry>))]
        public HttpResponseMessage GetEntries()
        {
            var result = new List<Entry>();
            try
            {
                result = db.Entries.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET: api/Entry/5
        [AllowAnonymous]
        [HttpGet]
        [Route("Entry/Get/{id}")]
        [ResponseType(typeof(Entry))]
        public HttpResponseMessage GetEntry(int id)
        {
            try
            {
                Entry result = db.Entries.Where(x => x.Id == id).FirstOrDefault();
                if (result == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Entry not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT: api/Entry/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEntry(int id, Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entry.Id)
            {
                return BadRequest();
            }

            db.Entry(entry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(id))
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

        // POST: api/Entry
        [AllowAnonymous]
        [HttpPost]
        [Route("Entry/Post")]
        [ResponseType(typeof(Entry))]
        public HttpResponseMessage PostEntry([FromBody]EntryDetail entry)
        {
            try
            {
                var result = new Entry();
                result.Hours = entry.Hours;
                result.DateOfEntry = entry.DateOfEntry;
                result.Message = entry.Message;
                // TO DO - include enums in entities
                switch (entry.AbsenceReason)
                {
                    case "None":
                        result.AbsenceReason = (int)AbsenceReason.None;
                        break;
                    case "Sickness":
                        result.AbsenceReason = (int)AbsenceReason.Sickness;
                        break;
                    case "Doctor":
                        result.AbsenceReason = (int)AbsenceReason.DoctorTime;
                        break;
                    case "VacationPayed":
                        result.AbsenceReason = (int)AbsenceReason.VacationPayed;
                        break;
                    case "VacationNotPayed":
                        result.AbsenceReason = (int)AbsenceReason.VacationNotPayed;
                        break;
                    case "Other":
                        result.AbsenceReason = (int)AbsenceReason.Other;
                        break;
                    default:
                        result.AbsenceReason = (int)AbsenceReason.None;
                        break;
                }

                // redundancy.. to be fixed later
                result.EmployeeId = entry.EmployeeId;
                result.TaskId = entry.TaskId;

                result.Employee = db.Employees.Where(x => x.Id == entry.EmployeeId).FirstOrDefault();
                result.Task = db.Tasks.Where(x => x.Id == entry.TaskId).FirstOrDefault();

                // persist data
                db.Entries.Add(result);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            // return CreatedAtRoute("DefaultApi", new { id = entry.Id }, entry);
        }

        // DELETE: api/Entry/5
        [AllowAnonymous]
        [HttpPost]
        [Route("Entry/Delete/{id}")]
        [ResponseType(typeof(Entry))]
        public HttpResponseMessage DeleteEntry(int id)
        {
            try
            {
                Entry entry = db.Entries.Find(id);
                if (entry == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Entry was not found!");
                }

                db.Entries.Remove(entry);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, entry);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntryExists(int id)
        {
            return db.Entries.Count(e => e.Id == id) > 0;
        }

        public class EntryDetail
        {
            //public int Id { get; set; }
            public int Hours { get; set; }
            public DateTime DateOfEntry { get; set; }
            public string AbsenceReason { get; set; }
            public string Message { get; set; }
            public int EmployeeId { get; set; }
            public int? TaskId { get; set; }

            //public virtual Employee Employee { get; set; }
            //public virtual Task Task { get; set; }
        }
    }
}