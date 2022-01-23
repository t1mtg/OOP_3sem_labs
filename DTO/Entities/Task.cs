using System;
using System.Collections.Generic;
using DTO.TaskEdit;

namespace DTO
{
    public enum TaskStatusType
    {
        Open,
        Active,
        Resolved,
    }

    public class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid EmployeeId { get; set; }
        public TaskStatusType TaskStatus { get; set; }
        public string Comment { get; set; }

        public DateTime CreationTime { get; set; }
        
        public Task()
        {
        }
        public Task(string name, Guid employeeId)
        {
            Name = name;
            EmployeeId = employeeId;
            Id = Guid.NewGuid();
            TaskStatus = TaskStatusType.Open;
            CreationTime = DateTime.Now;
        }


        public void Edit(Edit edit)
        {
            edit.EditTask();
        }
        
    }
}