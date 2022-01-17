using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using DTO.TaskEdit;
using Newtonsoft.Json;
using ReportsServer.Repositories;

namespace DTO
{
    public class TaskService 
    {
        private string TasksPath { get; } = @"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Tasks.json";
        private string EditPath { get; } = @"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Edits.json";
        private readonly EmployeeService _employeeService;
        private readonly TaskRepository _taskRepository;

        public TaskService(EmployeeService employeeService, TaskRepository taskRepository)
        {
            _employeeService = employeeService;
            _taskRepository = taskRepository;
        }

        public Task AddNewTask(Guid employeeId, string name)
        {
            var task = new Task(name, employeeId);
            _taskRepository.AddNewTask(task);
            _taskRepository.SerializeTasks();
            return task;
        }

        public Task AddNewCommentToTask(Guid taskId, string comment, Guid employeeId)
        {
            Task task = FindTaskById(taskId);
            var edit = new EditTaskComment(comment, task, employeeId);
            task.Edit(edit);
            _taskRepository.AddNewEdit(edit);
            _taskRepository.SerializeTasks();
            return task;
        }

        public Task ChangeTaskStatus(Guid taskId, TaskStatusType taskStatus, Guid employeeId)
        {
            Task task = FindTaskById(taskId);
            var edit = new EditTaskStatus(taskStatus, task, employeeId);
            task.Edit(edit);
            _taskRepository.AddNewEdit(edit);
            _taskRepository.SerializeTasks();
            return task;
        }

        public Task ChangeTaskEmployee(Guid taskId, Guid newEmployeeId, Guid employeeId)
        {
            Task task = FindTaskById(taskId);
            var edit = new EditTaskEmployee(newEmployeeId, task, employeeId);
            task.Edit(edit);
            _taskRepository.GetAllEdits().Add(edit);
            _taskRepository.SerializeEdits();
            _taskRepository.SerializeTasks();
            return task;
        }

        public List<Task> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public Task FindTaskById(Guid id)
        {
            return _taskRepository.FindTaskById(id);
        }

        public List<Task> FindTaskByEmployee(Guid employeeId)
        {
            return _taskRepository.GetAllTasks().Where(task => task.EmployeeId.Equals(employeeId)).ToList();
        }
        
        public List<Task> FindTaskByEmployeeEdit(Guid employeeId)
        {
            var edits = _taskRepository.GetAllEdits().Where(edit => edit.EmployeeId == employeeId).ToList();
            var res = edits.Select(edit => edit.Task).ToList();
            return res;
        }

        public List<Task> FindTaskByCreationTime(DateTime dateTime)
        {
            return GetAllTasks().Where(task => task.CreationTime == dateTime).ToList();
        }

        public List<Task> FindTaskByEditTime(DateTime dateTime)
        {
            var edits = _taskRepository.GetAllEdits().Where(edit => edit.DateTime == dateTime).ToList();
            var res = edits.Select(edit => edit.Task).ToList();
            return res;
        }
        
        

        public List<Task> FindSubordinateTasks(Guid employeeId)
        {
            Employee employee = _employeeService.GetEmployeeById(employeeId);
            var res = new List<Task>();
            foreach (Employee sub in employee.Subordinates)
            {
                res.AddRange(GetAllTasks().Where(task => task.EmployeeId.Equals(sub.Id)).ToList());
            }
            return res;
        }
    }
}