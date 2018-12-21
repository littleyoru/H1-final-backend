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

namespace TimeRegistration.Controllers
{
    [RoutePrefix("API")]
    public class TaskController : ApiController
    {
        private TimeRegistrationEntities db = new TimeRegistrationEntities();

        // GET: api/Task
        [AllowAnonymous]
        [HttpGet]
        [Route("Tasks")]
        [ResponseType(typeof(List<Task>))]
        public HttpResponseMessage GetTasks()
        {
            var result = new List<Task>();
            try
            {
                result = db.Tasks.ToList();
                // Request.Headers.Add("Access-Control-Allow-Origin", "*");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET: api/Task/5
        [AllowAnonymous]
        [HttpGet]
        [Route("Task/Get/{id}")]
        [ResponseType(typeof(Task))]
        public HttpResponseMessage GetTask(int id)
        {
            // Task task = db.Tasks.Find(id);
            try
            {
                Task result = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                if (result == null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Task not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            //if (task == null)
            //{
            //    return NotFound();
            //}

            //return Ok(task);
        }

        // PUT: api/Task/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Task
        [AllowAnonymous]
        [HttpPost]
        [Route("Task/Post")]
        [ResponseType(typeof(Task))]
        public HttpResponseMessage PostTask([FromBody]TaskDetail task)
        {
            try
            {
                var result = new Task();
                result.TaskName = task.TaskName;
                result.HoursEstimated = task.HoursEstimated;
                result.Status = task.Status;

                // persist data 
                db.Tasks.Add(result);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        //public IHttpActionResult PostTask(Task task)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Tasks.Add(task);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        //}

        // DELETE: api/Task/5
        [AllowAnonymous]
        [HttpPost]
        [Route("Task/Delete/{id}")]
        [ResponseType(typeof(Task))]
        public HttpResponseMessage DeleteTask(int id)
        {
            try
            {
                var result = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                if (result == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Task does not exit!");
                }
                else
                {
                    db.Tasks.Remove(result);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
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

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }

        public class TaskDetail
        {
            // public int TaskId { get; set; }
            public string TaskName { get; set; }
            public int HoursEstimated { get; set; }
            public int Status { get; set; }
        }
    }
}