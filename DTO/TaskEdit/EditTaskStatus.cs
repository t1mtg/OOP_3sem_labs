using System;

namespace DTO.TaskEdit
{
    public class EditTaskStatus : Edit
    {
        private TaskStatusType TaskStatusType { get; }

        public EditTaskStatus(TaskStatusType taskStatusType, Task task, Guid employeeId) :
            base(task, employeeId)
        {
            TaskStatusType = taskStatusType;
        }

        public override void EditTask()
        {
            Task.TaskStatus = TaskStatusType;
        }

   
    }
}