﻿using EmployeeManagementSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeManagementSPA.ApiControllers
{
    [RoutePrefix("employees")]
    public class EmployeesController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetEmployees()
        {
            var employees = db.Employees.ToList();
            return Ok(employees);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetEmployee(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Id.Equals(id));
            if (employee != null)
                return Ok(employee);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Ok(employee);
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateEmployee(int id, Employee employee)
        {
            var employeeFromDb = db.Employees.Find(id);
            if (employeeFromDb != null)
            {
                employeeFromDb.Name = employee.Name;
                employeeFromDb.Email = employee.Email;
                employeeFromDb.Salary = employee.Salary;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            var employeeToDelete = db.Employees.Find(id);
            if (employeeToDelete != null)
            {
                db.Employees.Remove(employeeToDelete);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}