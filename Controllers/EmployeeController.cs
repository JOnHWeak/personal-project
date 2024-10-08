﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personal_project.Helper;
using personal_project.Models;
using static personal_project.DTO.EmployeeDTO;

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
                // Use CheckIDExisted helper methods
                if (form.job_id.HasValue && !await CheckIDExisted.JobExists(_context, form.job_id.Value))
                {
                    return BadRequest(new { Message = "The specified Job ID does not exist." });
                }

                if (!string.IsNullOrEmpty(form.pub_id) && !await CheckIDExisted.PublisherExists(_context, form.pub_id))
                {
                    return BadRequest(new { Message = "The specified Publisher ID does not exist." });
                }

                // Update the employee's details
                existingEmployee.fname = form.fname;
                existingEmployee.minit = form.minit;
                existingEmployee.lname = form.lname;
                existingEmployee.job_id = form.job_id ?? existingEmployee.job_id; // Use existing if null
                existingEmployee.job_lvl = form.job_lvl ?? existingEmployee.job_lvl; // Use existing if null
                existingEmployee.pub_id = form.pub_id ?? existingEmployee.pub_id; // Use existing if null
                existingEmployee.hire_date = form.hire_date ?? existingEmployee.hire_date; // Use existing if null

                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Employee updated successfully." });
            }
            catch (DbUpdateException ex)
            {
                // Check for specific SQL constraint violations
                if (ex.InnerException != null && ex.InnerException.Message.Contains("minit"))
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
                // Set default values if they are not provided 
                if (form.job_id == 0)
                {
                    form.job_id = 1; // Set a valid default job_id
                }

                if (form.job_lvl == 0 || form.job_lvl == null)
                {
                    form.job_lvl = 10; // Set a valid default job_lvl
                }

                if (string.IsNullOrEmpty(form.pub_id))
                {
                    form.pub_id = "9952"; // Apply default value "9952"
                }
                // Use CheckIDExisted helper methods
                if (await CheckIDExisted.EmployeeExists(_context, form.emp_id))
                {
                    return BadRequest(new { Message = "The emp_id already exists." });
                }

                if (!await CheckIDExisted.JobExists(_context, form.job_id))
                {
                    return BadRequest(new { Message = "The specified Job ID does not exist." });
                }

                if (!await CheckIDExisted.PublisherExists(_context, form.pub_id))
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
               
                if (ex.InnerException != null && ex.InnerException.Message.Contains("minit"))
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
       
    }
}
