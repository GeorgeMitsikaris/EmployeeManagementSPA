using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagementSPA.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
    }
}