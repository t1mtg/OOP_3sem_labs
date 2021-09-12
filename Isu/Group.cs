using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        public Group(string name)
        {
            int res = int.Parse(name[2..]);
            if (name.Length == 5 && name[0] == 'M' && name[1] == '3' && res > 99 && res < 500)
            {
                Name = name;
                Students = new List<Student>();
            }
            else
            {
                throw new IsuException($"Group name {name} is incorrect", new FormatException());
            }
        }

        public static int MaxAmountOfStudents { get; } = 25;
        public List<Student> Students { get; }
        public string Name { get; }
    }
}