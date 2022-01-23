using System;

namespace DTO.TaskEdit
{
    public class EditTaskComment : Edit
    {
        public EditTaskComment(string comment, Task task, Guid employeeId) :
            base(task, employeeId)
        {
            Comment = comment;
        }
        public string Comment { get; }
        public override void EditTask()
        {
            Task.Comment = Comment;
        }
    }
}