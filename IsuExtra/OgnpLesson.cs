using System;

namespace IsuExtra
{
    public class OgnpLesson : Lesson
    {
        public OgnpLesson(
            string name,
            DayOfWeek day,
            TimeSpan startTime,
            string auditory,
            string teacherName,
            string ognpGroup)
            : base(name, day, startTime, auditory, teacherName)
        {
            OgnpGroup = ognpGroup;
        }

        public string OgnpGroup { get; set; }
    }
}