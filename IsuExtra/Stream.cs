using System;
using System.Collections.Generic;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra
{
    public class Stream
    {
        private static uint _number = 0;
        private List<Student> _students;

        public Stream(uint maxAmountOfStudents)
        {
            _number++;
            Id = Guid.NewGuid();
            Number = _number;
            OgnpLessons = new List<OgnpLesson>();
            _students = new List<Student>();
            StudentsOfStream = _students.AsReadOnly();
            MaxAmountOfStudents = maxAmountOfStudents;
        }

        public Guid Id { get; }
        public uint Number { get; }
        public List<OgnpLesson> OgnpLessons { get; }
        public uint MaxAmountOfStudents { get; }
        public IReadOnlyList<Student> StudentsOfStream { get; }

        public Student AddPerson(Student student)
        {
            if (_students.Count >= MaxAmountOfStudents)
            {
                throw new ReachedMaxAmountOfOgnpException();
            }

            _students.Add(student);
            return student;
        }

        public Student RemovePerson(Student student)
        {
            _students.Remove(student);
            return student;
        }
    }
}