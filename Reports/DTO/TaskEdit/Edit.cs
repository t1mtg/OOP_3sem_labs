using System;

namespace DTO.TaskEdit
{
    public abstract class Edit
    {
        protected Edit(Task task, Guid employeeId)
        {
            Task = task;
            EmployeeId = employeeId;
        }

        public Task Task { get; set; }
        public Guid EmployeeId { get; set; }
        public abstract void EditTask();

        public DateTime DateTime { get; set; }
    }
}