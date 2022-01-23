using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO;
using DTO.TaskEdit;
using Newtonsoft.Json;

namespace ReportsServer.Repositories
{
    public class TaskRepository
    {
        public TaskRepository(string tasksPath, string editsPath)
        {
            TasksPath = tasksPath;
            EditsPath = editsPath;
        }

        private List<Task> Tasks { get; set; }
        private List<Edit> Edits { get; set; }
        private string TasksPath { get; }
        private string EditsPath { get; }
        
        public void SerializeTasks()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(Tasks, settings);
            using var streamWriter = new StreamWriter(TasksPath);
            streamWriter.WriteLine(json);
        }
        
        public void SerializeEdits()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(Edits, settings);
            using var streamWriter = new StreamWriter(EditsPath);
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
            using var streamReader = new StreamReader(EditsPath);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Edit>>(json, settings);
        }

        public List<Task> GetAllTasks()
        {
            return DeserializeTasks();
        }

        public List<Edit> GetAllEdits()
        {
            return DeserializeEdits();
        }

        public void AddNewTask(Task task)
        {
            Tasks = DeserializeTasks();
            Tasks.Add(task);
            SerializeTasks();
        }

        public void AddNewEdit(Edit edit)
        {
            Edits = DeserializeEdits();
            Edits.Add(edit);
            SerializeEdits();
        }

        public Task FindTaskById(Guid taskId)
        {
            return GetAllTasks().FirstOrDefault(task => task.Id.Equals(taskId));
        }


    }
}