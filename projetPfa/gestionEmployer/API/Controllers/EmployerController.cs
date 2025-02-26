using gestionEmployer.API.DTOs.DTOs;
using gestionEmployer.API.Filters;
using gestionEmployer.Core.Interfaces;
using gestionEmployer.Core.Models;
using gestionEmployer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using gestionEmployer.API.Mappers;
using gestionEmployer.API.DTOs.EmployeeDTO;

namespace gestionEmployer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
       

        // GET: api/Employee/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id cant be < 0");
            }
            Employer employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            GetEmployeeDTO dto = EmployeeMapper.ModelToGetEmployee(employee);
            return Ok(dto);
        }

        //GET ALL EMPLOYEES by idAdmin
        [HttpGet("admin/{AdminId}")]
        public async Task<ActionResult<IEnumerable<ReturnAddedEmployee>>> GetEmployesByAdminId(int AdminId)
        {
            var employes = await _employeeService.GetEmployesByAdminIdAsync(AdminId);
            if (employes == null || employes.Count()==0)
            {
                return NotFound($"Aucun employé trouvé pour l'ID Admin : {AdminId}");
            }

            return Ok(employes);
        }



        // POST: api/Employee
        [HttpPost]
        [ValidationModels]
        public async Task<ActionResult<ReturnAddedEmployee>> AddEmployee([FromBody] AddEmployeeDTO employee)
        {
            
            var employerr = EmployeeMapper.AddEmployeeDTOToEmployer(employee);

            var employeeAjoute = await _employeeService.AddEmployeeAsync(employerr);
            if (employeeAjoute.errors.Count()>0)
            {
                return BadRequest(employeeAjoute);
            }
            return Ok(employeeAjoute);
        }

       
        // PUT: api/Employee/{id}
        [HttpPut]
        [ValidationModels]
        public async Task <IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDTO employee)
        {
            Employer employer = EmployeeMapper.UpdateEmployeeDTOToEmployer(employee);

            try
            {
                ReturnUpdatedEmpDto emp = await _employeeService.UpdateEmployeeAsync(employer);
                if(emp.Errors.Count()>0)
                {
                    return BadRequest(emp);
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeService.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }


    }
}
