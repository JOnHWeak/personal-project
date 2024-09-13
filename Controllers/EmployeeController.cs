using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personal_project.Helper;
using personal_project.Models;

namespace personal_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly pubsContext _context;

        public EmployeesController(pubsContext context)
        {
            _context = context;
        }

        public class EmployeeForm
        {
            [Required]
            [EmpIdFormat(ErrorMessage = "emp_id format is incorrect. Must follow [A-Z][A-Z][A-Z][1-9][0-9][0-9][0-9][0-9][FM] or [A-Z]-[A-Z][1-9][0-9][0-9][0-9][0-9][FM].")]
            public string emp_id { get; set; }

            public string fname { get; set; }
            public string minit { get; set; }
            public string lname { get; set; }
            public short job_id { get; set; }
            public byte? job_lvl { get; set; }
            public string pub_id { get; set; }
            public DateTime hire_date { get; set; }
        }
        public class UpdateEmployee
        {
            public string fname { get; set; }
            public string minit { get; set; }
            public string lname { get; set; }
            public short job_id { get; set; }
            public byte? job_lvl { get; set; }
            public string pub_id { get; set; }
            public DateTime hire_date { get; set; }
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<employee>>> GetEmployees()
        {
            var employees = await _context.employees.ToListAsync();
            if (!employees.Any())
            {
                return NoContent();
            }
            return Ok(employees);
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<employee>> GetEmployee(string id)
        {
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with ID '{id}' not found." });
            }

            return Ok(employee);
        }

        // PUT: api/employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, UpdateEmployee form)
        {
            // Find the existing employee
            var existingEmployee = await _context.employees.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound(new { Message = $"Employee with ID '{id}' not found." });
            }

            // Validate the input model along with any custom validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the job_id exists in the job table
                var jobExists = await _context.jobs.AnyAsync(j => j.job_id == form.job_id);
                if (!jobExists)
                {
                    return BadRequest(new { Message = "The specified Job ID does not exist." });
                }

                // Check if the pub_id exists in the publisher table
                var publisherExists = await _context.publishers.AnyAsync(p => p.pub_id == form.pub_id);
                if (!publisherExists)
                {
                    return BadRequest(new { Message = "The specified Publisher ID does not exist." });
                }

                // Update the employee's details                 
                existingEmployee.fname = form.fname;
                existingEmployee.minit = form.minit;
                existingEmployee.lname = form.lname;
                existingEmployee.job_id = form.job_id;
                existingEmployee.job_lvl = form.job_lvl;
                existingEmployee.pub_id = form.pub_id;
                existingEmployee.hire_date = form.hire_date;

                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Employee updated successfully." });
            }
            catch (DbUpdateException ex)
            {
                // Check for specific SQL constraint violations
                if (ex.InnerException != null && ex.InnerException.Message.Contains("The level for job_id"))
                {
                    return BadRequest(new { Message = "job_lvl is out of the allowed range." });
                }
                else if (ex.InnerException != null && ex.InnerException.Message.Contains("minit"))
                {
                    return BadRequest(new { Message = "Error: minit must be the 2nd character of emp_id" });
                }
                throw;
            }
        }



        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<employee>> PostEmployee(EmployeeForm form)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if emp_id already exists
                var empExists = await _context.employees.AnyAsync(e => e.emp_id == form.emp_id);
                if (empExists)
                {
                    return BadRequest(new { Message = "The emp_id already exists." });
                }

                // Check if the job_id exists in the job table
                var jobExists = await _context.jobs.AnyAsync(j => j.job_id == form.job_id);
                if (!jobExists)
                {
                    return BadRequest(new { Message = "The specified Job ID does not exist." });
                }

                // Check if the pub_id exists in the publisher table
                var publisherExists = await _context.publishers.AnyAsync(p => p.pub_id == form.pub_id);
                if (!publisherExists)
                {
                    return BadRequest(new { Message = "The specified Publisher ID does not exist." });
                }

                // Create a new employee entity
                var employee = new employee
                {
                    emp_id = form.emp_id,
                    fname = form.fname,
                    minit = form.minit,
                    lname = form.lname,
                    job_id = form.job_id,
                    job_lvl = form.job_lvl,
                    pub_id = form.pub_id,
                    hire_date = form.hire_date
                };

                // Add the employee to the context
                _context.employees.Add(employee);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return successfull status
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.emp_id }, new { Message = "Employee created successfully.", employee });
            }
            catch (DbUpdateException ex)
            {
                // Check for specific SQL constraint violations                
                if (ex.InnerException != null && ex.InnerException.Message.Contains("The level for job_id"))
                {
                    return BadRequest(new { Message = "Error: job_lvl is out of the allowed range." });
                }
                else if (ex.InnerException != null && ex.InnerException.Message.Contains("minit"))
                {
                    return BadRequest(new { Message = "Error: minit must be the 2nd character of emp_id" });
                }
                throw;
            }
        }



        // DELETE: api/employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with ID '{id}' not found." });
            }

            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Employee deleted successfully." });
        }

        private bool EmployeeExists(string id)
        {
            return _context.employees.Any(e => e.emp_id == id);
        }
    }
}
