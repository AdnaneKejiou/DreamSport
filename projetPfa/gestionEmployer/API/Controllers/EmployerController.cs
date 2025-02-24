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
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");
            return Ok(employee);
        }

        //GET ALL EMPLOYEES by idAdmin
      [HttpGet("admin/{AdminId}")]
public async Task<ActionResult<IEnumerable<Employer>>> GetEmployesByAdminId(int AdminId)
{
    var employes = await _employeeService.GetEmployesByAdminIdAsync(AdminId);
    if (employes == null)
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
            return Ok(employeeAjoute);
        }

       
        // PUT: api/Employee/{id}
        [HttpPut]
        [ValidationModels]
        public IActionResult UpdateEmployee([FromBody] UpdateEmployeeDTO employee)
        {
            Employer employer = EmployeeMapper.UpdateEmployeeDTOToEmployer(employee);
            _employeeService.UpdateEmployeeAsync(employer);
            return NoContent();
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
