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
    public class EmployeeController : ApiController
    {
        private TimeRegistrationEntities db = new TimeRegistrationEntities();

        // GET: api/Employee
        [HttpGet]
        [Route("Employees")]
        [ResponseType(typeof(List<Employee>))]
        // public IQueryable<Employee> GetEmployees()
        public HttpResponseMessage GetEmployees()
        {
            var result = new List<Employee>();
            try
            {
                result = db.Employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            // breturn db.Employees;
        }

        // GET: api/Employee/5
        [HttpGet]
        [Route("Employee/Get/{id}")]
        [ResponseType(typeof(Employee))]
        //public IHttpActionResult GetEmployee(int id)
        public HttpResponseMessage GetEmployee(int id)
        {
            try
            {
                // Employee employee = db.Employees.Find(id);
                Employee result = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (result == null)
                {
                    // return NotFound();
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Employee not found");
                }

                // return Ok(employee);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT: api/Employee/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employee
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employee/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}