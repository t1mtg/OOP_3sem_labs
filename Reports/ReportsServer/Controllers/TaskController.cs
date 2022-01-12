using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using DTO.Exceptions;
using DTO.TaskEdit;
using Microsoft.AspNetCore.Mvc;

namespace ReportsServer.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }
        
        [Route("addTask")]
        [HttpPost]
        public Task AddNewTask([FromQuery] Guid employeeId, [FromQuery] string name)
        {
            return _taskService.AddNewTask(employeeId, name);
        }
        
        [Route("addComment")]
        [HttpPut]
        public Task AddNewCommentToTask([FromQuery] Guid taskId,[FromQuery] string comment,[FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            if (string.IsNullOrWhiteSpace(comment)) throw new EmptyCommentException();
            return _taskService.AddNewCommentToTask(taskId, comment, employeeId);
        }
        
        [Route("changeStatus")]
        [HttpPut]
        public Task ChangeTaskStatus([FromQuery]Guid taskId, [FromQuery] TaskStatusType taskStatusType,[FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            return _taskService.ChangeTaskStatus(taskId, taskStatusType, employeeId);
        }
        
        [HttpPut]
        [Route("changeTaskEmployee")]
        public Task ChangeTaskEmployee([FromQuery] Guid taskId,[FromQuery] Guid newEmployeeId,[FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            if (taskId == Guid.Empty) throw new IncorrectIdException();
            return _taskService.ChangeTaskEmployee(taskId, newEmployeeId, employeeId);
        }
        
        [Route("allTasks")]
        [HttpGet]
        public List<Task> GetAllTasks()
        {
            return _taskService.GetAllTasks();
        }
        
        [Route("taskById")]
        [HttpGet]
        public Task FindTaskById([FromQuery] Guid taskId)
        {
            if (taskId == Guid.Empty) throw new IncorrectIdException();
            return _taskService.FindTaskById(taskId);
        }
        
        [Route("taskByEmployee")]
        [HttpGet]
        public List<Task> FindTaskByEmployee([FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            return _taskService.FindTaskByEmployee(employeeId);
        }
        
        [Route("taskByEmployeeEdit")]
        [HttpGet]
        public List<Task> FindTaskByEmployeeEdit([FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            return _taskService.FindTaskByEmployeeEdit(employeeId);
        }
        
        [Route("taskByCreationTime")]
        [HttpGet]
        public List<Task> FindTaskByCreationTime([FromQuery] DateTime dateTime)
        {
            return _taskService.FindTaskByCreationTime(dateTime);
        }
        
        [Route("taskByEditTime")]
        [HttpGet]
        public List<Task> FindTaskByEditTime([FromQuery] DateTime dateTime)
        {
            return _taskService.FindTaskByEditTime(dateTime);
        }
        
        [Route("subordinateTasks")]
        [HttpGet]
        public List<Task> FindSubordinateTasks([FromQuery] Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new IncorrectIdException();
            return  _taskService.FindSubordinateTasks(employeeId);
        }
    }
}