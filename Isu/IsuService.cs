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

        public Student AddStudent(Group group, string name)
        {
            Student student = Group.AddPerson(group, name);
            _listOfStudents.Add(student);
            return student;
        }

        public Student GetStudent(int id)
        {
            Student student = _listOfStudents.FirstOrDefault(student => student.Id.Equals(id));
            if (student == null)
            {
                throw new StudentDoesNotExistException();
            }

            return student;
        }

        public Student FindStudent(string name)
        {
            return _listOfStudents.FirstOrDefault(student => student.Name.Equals(name));
        }

        public IReadOnlyList<Student> FindStudents(string groupName)
        {
            return FindGroup(groupName).StudentsOfGroup.ToList();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _listOfStudents.Where(student => student.Group.CourseNumber.Number.Equals(courseNumber.Number))
                .ToList();
        }

        public Group FindGroup(string groupName)
        {
            return _listOfGroups.First(group => group.Name.Equals(groupName));
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _listOfGroups.Where(group => group.CourseNumber.Number.Equals(courseNumber.Number)).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup) => Group.MovePerson(student, newGroup);
    }
}