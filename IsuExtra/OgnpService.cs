﻿using System;
using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra
{
    public class OgnpService
    {
        private readonly List<MegaFaculty> _megaFaculties = new List<MegaFaculty>();

        public Ognp AddNewOgnp(MegaFaculty megaFaculty, string name, uint numberOfStreams, uint maxAmountOfStudentsInStream)
        {
            var ognp = new Ognp(name, numberOfStreams, maxAmountOfStudentsInStream);
            megaFaculty.Ognp = ognp;
            return ognp;
        }

        public void AddStudentToOgnp(Student student, Ognp ognp)
        {
            MegaFaculty megaFaculty = _megaFaculties.FirstOrDefault(faculty => faculty.Ognp.Id.Equals(ognp.Id));
            if (megaFaculty == null)
            {
                throw new MegaFacultyDoesntExistException();
            }

            if (megaFaculty.Letter.Equals(student.Group.Name[0]))
            {
                throw new OgnpFromMegaFacultyException();
            }

            if (CountStudentOgnps(student) == 2)
            {
                throw new ReachedMaxAmountOfOgnpException();
            }

            List<FacultyLesson> studentFacultyLessons = GetStudentFacultyLessons(student);
            foreach (Stream ognpStream in ognp.Streams)
            {
                if (ognpStream.StudentsOfStream.Count >= ognpStream.MaxAmountOfStudents)
                {
                    continue;
                }

                if (!IsOgnpStreamLessonsOverlapFacultyLessons(ognpStream.OgnpLessons, studentFacultyLessons))
                {
                    ognpStream.AddPerson(student);
                    return;
                }
            }
        }

        public void DeleteStudentFromOgnp(Student student, Ognp ognp)
        {
            foreach (Stream stream in ognp.Streams)
            {
                if (stream.StudentsOfStream.Any(streamStudent => streamStudent.Id.Equals(student.Id)))
                {
                    stream.RemovePerson(student);
                    return;
                }
            }
        }

        public List<Stream> GetOgnpStreams(Ognp ognp)
        {
            return ognp.Streams;
        }

        public IReadOnlyList<Student> GetStudentsFromStream(Stream stream)
        {
            return stream.StudentsOfStream;
        }

        public List<Student> FindStudentsWithoutOgnp(Group @group)
        {
            return @group.StudentsOfGroup
                .Where(groupStudent => CountStudentOgnps(groupStudent) == 0)
                .ToList();
        }

        public void AddMegaFaculty(MegaFaculty megaFaculty)
        {
            _megaFaculties.Add(megaFaculty);
        }

        private int CountStudentOgnps(Student student)
        {
            var studentsThatHaveOgnp = _megaFaculties
                .Select(faculty => faculty.Ognp)
                .SelectMany(ognp => ognp.Streams)
                .SelectMany(stream => stream.StudentsOfStream)
                .ToList();
            return studentsThatHaveOgnp
                .FindAll(student1 => student.Id.Equals(student1.Id))
                .Count;
        }

        private List<FacultyLesson> GetStudentFacultyLessons(Student student)
        {
            return _megaFaculties
                .FirstOrDefault(faculty => faculty.Letter.Equals(student.Group.Name[0]))
                ?.MegaFacultyLessons;
        }

        private bool IsOgnpStreamLessonsOverlapFacultyLessons(
            List<OgnpLesson> ognpStreamLessons,
            List<FacultyLesson> facultyLessons)
        {
            if (ognpStreamLessons == null || facultyLessons == null)
            {
                return false;
            }

            foreach (OgnpLesson ognpStreamLesson in ognpStreamLessons)
            {
                foreach (FacultyLesson facultyLesson in facultyLessons)
                {
                    if (facultyLesson.Day.Equals(ognpStreamLesson.Day)
                        && IsTwoLessonsOverlap(facultyLesson, ognpStreamLesson))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsTwoLessonsOverlap(Lesson lesson1, Lesson lesson2)
        {
            const int lessonDurationInMinutes = 90;
            return Math.Abs(lesson1.StartTime.Minutes - lesson2.StartTime.Minutes) < lessonDurationInMinutes
                ? true
                : false;
        }
    }
}