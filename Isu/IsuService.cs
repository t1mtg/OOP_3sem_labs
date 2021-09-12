using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _listOfGroups = new List<Group>();
        private readonly List<Student> _listOfStudents = new List<Student>();

        public Group AddGroup(string name)
        {
            var newGroup = new Group(name);
            _listOfGroups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group @group, string name)
        {
            if (@group.Students.Count >= Group.MaxAmountOfStudents)
            {
                throw new IsuException($"Reached max amount of students in group {@group.Name}");
            }

            var student = new Student(name);
            @group.Students.Add(student);
            _listOfStudents.Add(student);
            student.Group = @group;
            return student;
        }

        public Student GetStudent(int id)
        {
            return _listOfStudents.First(student => student.Id.Equals(id));
        }

        public Student FindStudent(string name)
        {
            return _listOfStudents.First(student => student.Name.Equals(name));
        }

        public List<Student> FindStudents(string groupName)
        {
            return FindGroup(groupName).Students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _listOfStudents.Where(student => int.Parse(student.Group.Name[2].ToString())
                    .Equals(courseNumber.Number))
                .ToList();
        }

        public Group FindGroup(string groupName)
        {
            return _listOfGroups.First(@group => @group.Name.Equals(groupName));
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _listOfGroups.Where(@group => int.Parse(@group.Name[2].ToString()).Equals(courseNumber.Number)).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            Group previousGroup = null;
            foreach (Student st in _listOfStudents.Where(st => st == student))
            {
                previousGroup = st.Group;
                st.Group = newGroup;
            }

            if (previousGroup == null) return;
            previousGroup.Students.Remove(student);
            newGroup.Students.Add(student);
            student.Group = newGroup;
        }
    }
}