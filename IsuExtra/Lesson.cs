using System;

namespace IsuExtra
{
    public abstract class Lesson
    {
        protected Lesson(string name, DayOfWeek day, TimeSpan startTime, string auditory, string teacherName)
        {
            Name = name;
            Day = day;
            StartTime = startTime;
            Auditory = auditory;
            TeacherName = teacherName;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public string Auditory { get; set; }
        public string TeacherName { get; set; }
    }
}