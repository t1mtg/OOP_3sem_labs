using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DTO;
using DTO.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ReportsServer.Controllers
{
    [ApiController] 
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        
        [Route("hireEmployee")]
        [HttpPost]
        public Employee HireEmployee([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new IncorrectNameException();
            return _employeeService.HireEmployee(name);
        }

        [Route("getAllEmployeesOnPage")]
        [HttpGet]
        public IActionResult GetAllEmployees([FromQuery] int numberOfPage)
        {
            List<Employee> result = _employeeService.GetAllEmployeesOnNPage(numberOfPage);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
        
        [Route("getEmployeeById")]
        [HttpGet]
        public IActionResult GetEmployeeById([FromQuery] Guid id)
        {
            if (id != Guid.Empty)
            {
                Employee result = _employeeService.GetEmployeeById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);

        }
        
        [Route("getEmployeeByName")]
        [HttpGet]
        public IActionResult GetEmployeeByName([FromQuery] string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Employee result = _employeeService.GetEmployeeByName(name);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }
        
        [Route("updateLeader")]
        [HttpPut]
        public IActionResult UpdateLeader([FromQuery] Guid employeeId,[FromQuery] Guid leaderId)
        {
            if (employeeId != Guid.Empty && leaderId != Guid.Empty)
            {
                Employee result = _employeeService.UpdateLeader(employeeId, leaderId);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }
        
        [Route("fireEmployee")]
        [HttpPost]
        public HttpStatusCode FireEmployee([FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) return HttpStatusCode.NotFound;
            _employeeService.FireEmployee(employeeId);
            return HttpStatusCode.OK;
        }
    }
}

