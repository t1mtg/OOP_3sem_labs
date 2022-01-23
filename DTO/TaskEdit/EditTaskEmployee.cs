using System;

namespace DTO.TaskEdit
{
    public class EditTaskEmployee : Edit
    {
        public EditTaskEmployee(Guid newEmployeeId, Task task, Guid employeeId) :
            base(task, employeeId)
        {
            NewEmployeeId = newEmployeeId;
        }
        public Guid NewEmployeeId { get; }
        public override void EditTask()
        {
            Task.EmployeeId = EmployeeId;
        }
    }
}