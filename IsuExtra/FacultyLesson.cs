using System;
using Isu;

namespace IsuExtra
{
    public class FacultyLesson : Lesson
    {
        public FacultyLesson(
            string name,
            DayOfWeek day,
            TimeSpan startTime,
            string auditory,
            string teacherName,
            Group @group)
            : base(name, day, startTime, auditory, teacherName)
        {
            Group = @group;
        }

        public Group Group { get; set; }
    }
}