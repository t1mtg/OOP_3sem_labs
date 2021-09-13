using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        public const int MaxAmountOfStudents = 25;
        public Group(string name)
        {
            int res = int.Parse(name[2..]);
            if (name.Length != 5 || name[0] != 'M' || name[1] != '3' || res <= 99 || res >= 500)
            {
                throw new IsuException($"Group name {name} is incorrect", new FormatException());
            }

            Name = name;
            CourseNumber = res / 100;
            Students = new List<Student>();
        }

        public List<Student> Students { get; }
        public int CourseNumber { get; }
        public string Name { get; }
    }
}