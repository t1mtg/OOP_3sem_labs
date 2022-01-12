using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using DTO.TaskEdit;
using Newtonsoft.Json;

namespace DTO
{
    public class TaskService 
    {
        private string TasksPath { get; } = @"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Tasks.json";
        private string EditPath { get; } = @"C:\Users\BaHo\Documents\GitHub\t1mtg\Reports\Jsons\Edits.json";
        public List<Task> Tasks { get; set; }
        public List<Edit> Edits { get; set; }

        private EmployeeService _employeeService;

        public TaskService(EmployeeService employeeService)
        {
            Tasks = new List<Task>();
            Edits = new List<Edit>();
            _employeeService = employeeService;
        }
        
        private void SerializeTasks()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(Tasks, settings);
            using var streamWriter = new StreamWriter(TasksPath);
            streamWriter.WriteLine(json);
        }
        
        private void SerializeEdits()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(Edits, settings);
            using var streamWriter = new StreamWriter(EditPath);
            streamWriter.WriteLine(json);
        }
        
        private List<Task> DeserializeTasks()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            using var streamReader = new StreamReader(TasksPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Task>>(json, settings);
        }

        private List<Edit> DeserializeEdits()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            using var streamReader = new StreamReader(EditPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Edit>>(json, settings);
        }

        public Task AddNewTask(Guid employeeId, string name)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            var task = new Task(name, employeeId);
            Tasks.Add(task);
            SerializeTasks();
            SerializeEdits();
            return task;
        }

        public Task AddNewCommentToTask(Guid taskId, string comment, Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            Task task = FindTaskById(taskId);
            var edit = new EditTaskComment(comment, task, employeeId);
            task.Edit(edit);
            Edits.Add(edit);
            SerializeTasks();
            SerializeEdits();
            return task;
        }

        public Task ChangeTaskStatus(Guid taskId, TaskStatusType taskStatus, Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            Task task = FindTaskById(taskId);
            var edit = new EditTaskStatus(taskStatus, task, employeeId);
            task.Edit(edit);
            Edits.Add(edit);
            SerializeTasks();
            SerializeEdits();
            return task;
        }

        public Task ChangeTaskEmployee(Guid taskId, Guid newEmployeeId, Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            Task task = FindTaskById(taskId);
            var edit = new EditTaskEmployee(newEmployeeId, task, employeeId);
            task.Edit(edit);
            Edits.Add(edit);
            SerializeTasks();
            SerializeEdits();
            return task;
        }

        public List<Task> GetAllTasks()
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            return Tasks;
        }

        public Task FindTaskById(Guid id)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            return Tasks.FirstOrDefault(task => task.Id.Equals(id));
        }

        public List<Task> FindTaskByEmployee(Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            return Tasks.Where(task => task.EmployeeId.Equals(employeeId)).ToList();
        }
        
        public List<Task> FindTaskByEmployeeEdit(Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            var edits = Edits.Where(edit => edit.EmployeeId == employeeId).ToList();
            var res = edits.Select(edit => edit.Task).ToList();
            return res;
        }

        public List<Task> FindTaskByCreationTime(DateTime dateTime)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            return Tasks.Where(task => task.CreationTime == dateTime).ToList();
        }

        public List<Task> FindTaskByEditTime(DateTime dateTime)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            var edits = Edits.Where(edit => (edit.DateTime == dateTime)).ToList();
            var res = edits.Select(edit => edit.Task).ToList();
            return res;
        }
        
        

        public List<Task> FindSubordinateTasks(Guid employeeId)
        {
            Tasks = DeserializeTasks();
            Edits = DeserializeEdits();
            List<Employee> employees = _employeeService.GetAllEmployees();
            Employee employee = employees.FirstOrDefault(employee => employee.Id.Equals(employeeId));
            var res = new List<Task>();
            foreach (Employee sub in employee.Subordinates)
            {
                res.AddRange(Tasks.Where(task => task.EmployeeId.Equals(sub.Id)).ToList());
            }
            SerializeTasks();
            SerializeEdits();
            return res;
        }
    }
}