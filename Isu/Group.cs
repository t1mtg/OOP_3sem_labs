using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private const int MaxAmountOfStudents = 25;
        public Group(string name)
        {
            int res = int.Parse(name[2..]);
            if (name.Length != 5 || res is <= 99 or >= 500)
            {
                throw new InvalidGroupNameException();
            }

            Name = name;
            CourseNumber = new CourseNumber(name[2] - '0');
            Students = new List<Student>();
            StudentsOfGroup = Students.AsReadOnly();
        }

        public CourseNumber CourseNumber { get; }
        public string Name { get; }
        public IReadOnlyList<Student> StudentsOfGroup { get; }
        private List<Student> Students { get; }
        public static Student AddPerson(Group group, string name)
        {
            if (group.Students.Count >= MaxAmountOfStudents)
            {
                throw new ReachMaxAmountException();
            }

            var student = new Student(name);
            group.Students.Add(student);
            student.ChangeGroup(group);
            return student;
        }

        public static void MovePerson(Student student, Group newGroup)
        {
            Group previousGroup = student.Group;
            if (previousGroup == null) return;
            student.ChangeGroup(newGroup);
            previousGroup.Students.Remove(student);
            newGroup.Students.Add(student);
        }
    }
}