using System;
using System.Collections.Generic;
using DTO.TaskEdit;

namespace DTO
{
    public interface ITaskService
    {
        public Task AddNewTask(Guid employeeId, string name);
        
        public Task AddNewCommentToTask(Guid taskId, string comment, Guid employeeId);
        
        public Task ChangeTaskStatus(Guid taskId, TaskStatusType taskStatus, Guid employeeId);
        
        public Task ChangeTaskEmployee(Guid taskId, Guid newEmployeeId, Guid employeeId);
        
        public List<Task> GetAllTasks();
        
        public Task FindTaskById(Guid id);
        
        public List<Task> FindTaskByEmployee(Guid employeeId);
        
        public List<Task> FindTaskByEmployeeEdit(Guid employeeId);
        
        public List<Task> FindTaskByCreationTime(DateTime dateTime);
        
        public List<Task> FindTaskByEditTime(DateTime dateTime);
        
        public List<Task> FindSubordinateTasks(Guid employeeId);
    }
}