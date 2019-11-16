using EmployeeManagementSPA.Models;
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

        //Is used for getting the total rows of the table for paging
        //The presentation of employees is done by the Paging method
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

        [HttpGet]
        [Route("employeeNames")]
        public IHttpActionResult GetEmployeesNames(string searchTerm)
        {
            var employeeNames = db.Employees.Where(e => e.Name.Contains(searchTerm)).Select(e => e.Name);
            return Ok(employeeNames);
        }

        [HttpGet]
        [Route("employeeEmails")]
        public IHttpActionResult GetEmployeesEmails(string searchTerm)
        {
            var employeeEmails = db.Employees.Where(e => e.Email.Contains(searchTerm)).Select(e => e.Email);
            return Ok(employeeEmails);
        }

        [HttpGet]
        [Route("employeeSalaries")]
        public IHttpActionResult GetEmployeesSalaries(string searchTerm)
        {
            var employeeSalaries = db.Employees.Where(e => e.Salary.ToString().Contains(searchTerm)).Select(e => e.Salary);
            List<string> empSalaries = new List<string>();
            foreach (var salary in employeeSalaries)
            {
                empSalaries.Add(salary.ToString());
            }
            return Ok(empSalaries);
        }

        //Returns an http response with a list of employees because two or more employees can have the same name
        [HttpGet]
        [Route("searchByName")]
        public IHttpActionResult SearchEmployeesByName(string name)
        {
            var employees = db.Employees.Where(e => e.Name.Equals(name));
            return Ok(employees);
        }

        //Returns an http response with a list of employees because ajax function expects an array of employees to create the table
        [HttpGet]
        [Route("searchByEmail")]
        public IHttpActionResult SearchEmployeesByEmail(string email)
        {
            var employees = db.Employees.Where(e => e.Email.Equals(email));
            return Ok(employees);
        }

        //Returns an http response with a list of employees because two or more employees can have the same name
        [HttpGet]
        [Route("searchBySalary")]
        public IHttpActionResult SearchEmployeesBySalary(string salary)
        {
            decimal salaryDecimal = 0;
            if (decimal.TryParse(salary, out salaryDecimal))
            {
                var employees = db.Employees.Where(e => e.Salary.Equals(salaryDecimal));
                return Ok(employees);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, salary);
            }
        }

        [HttpGet]
        [Route("sortByName")]
        public IHttpActionResult SortByName(string sort, int pageNumber, int numberOfRowsPerPage)
        {
            var employees = db.Employees.ToList();
            if (sort == "asc")
                employees = employees.OrderBy(e => e.Name).ToList();
            else if (sort == "desc")
                employees = employees.OrderByDescending(e => e.Name).ToList();
            else
            {
                return Content(HttpStatusCode.BadRequest, "Invalid sort data");
            }

            int numberOfRowsToSkip = (pageNumber - 1) * numberOfRowsPerPage;
            employees = employees.Skip(numberOfRowsToSkip).Take(numberOfRowsPerPage).ToList();

            return Ok(employees);
        }

        [HttpGet]
        [Route("sortByEmail")]
        public IHttpActionResult SortByEmail(string sort, int pageNumber, int numberOfRowsPerPage)
        {
            var employees = db.Employees.ToList();
            if (sort == "asc")
                employees = employees.OrderBy(e => e.Email).ToList();
            else if (sort == "desc")
                employees = employees.OrderByDescending(e => e.Email).ToList();
            else
            {
                return Content(HttpStatusCode.BadRequest, "Invalid sort data");
            }

            int numberOfRowsToSkip = (pageNumber - 1) * numberOfRowsPerPage;
            employees = employees.Skip(numberOfRowsToSkip).Take(numberOfRowsPerPage).ToList();

            return Ok(employees);
        }

        [HttpGet]
        [Route("sortBySalary")]
        public IHttpActionResult SortBySalary(string sort, int pageNumber, int numberOfRowsPerPage)
        {
            var employees = db.Employees.ToList();
            if (sort == "asc")
                employees = employees.OrderBy(e => e.Salary).ToList();
            else if (sort == "desc")
                employees = employees.OrderByDescending(e => e.Salary).ToList();
            else
            {
                return Content(HttpStatusCode.BadRequest, "Invalid sort data");
            }

            int numberOfRowsToSkip = (pageNumber - 1) * numberOfRowsPerPage;
            employees = employees.Skip(numberOfRowsToSkip).Take(numberOfRowsPerPage).ToList();

            return Ok(employees);
        }

        [HttpGet]
        [Route("pagination")]
        public IHttpActionResult Paging(int pageNumber, int numberOfRowsPerPage)
        {
            var employees = db.Employees.ToList();
            int numberOfRowsToSkip = (pageNumber - 1) * numberOfRowsPerPage;
            employees = employees.Skip(numberOfRowsToSkip).Take(numberOfRowsPerPage).ToList();
            return Ok(employees);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Content(HttpStatusCode.Created, employee);
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateEmployee(int id, [FromBody]Employee employee)
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
