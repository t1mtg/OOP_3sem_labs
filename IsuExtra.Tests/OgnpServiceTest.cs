using System;
using Isu;
using Isu.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class OgnpServiceTest
    {
        private OgnpService _ognpService;

        [SetUp]
        public void Setup()
        {
            _ognpService = new OgnpService();
        }

        [Test]
        public void AddNewOgnpOgnpCreated()
        {
            var megaFaculty = new MegaFaculty("ITIP", char.Parse("M"));
            Ognp ognp = _ognpService
                .AddNewOgnp(megaFaculty, "Data analysis", 5, 25);
            Assert.IsNotNull(ognp);
            Assert.AreEqual(5, ognp.Streams.Count);
        }

        [Test]
        public void AddStudentToOgnp_ThrowsException()
        {
            var group1 = new Group("M3204");
            Student student = Group.AddPerson(group1, "Timofey Gurman");
            var megaFaculty = new MegaFaculty("ITIP", char.Parse("M"));
            _ognpService.AddMegaFaculty(megaFaculty);
            Ognp ognp = _ognpService
                .AddNewOgnp(megaFaculty, "Data analysis", 3, 25);
            Assert.Catch<OgnpFromMegaFacultyException>(() => _ognpService.AddStudentToOgnp(student, ognp));
        }

        [Test]
        public void DeleteStudentsFromOgnp_StudentDeleted()
        {
            var group1 = new Group("M3204");
            Student student = Group.AddPerson(group1, "Timofey Gurman");
            var megaFaculty = new MegaFaculty("ITIP", char.Parse("K"));
            _ognpService.AddMegaFaculty(megaFaculty);
            megaFaculty.MegaFacultyLessons.Add(new FacultyLesson("Alghoritms", DayOfWeek.Friday,
                TimeSpan.FromMinutes(500), "466", "Nina", student.Group));
            Ognp ognp = _ognpService
                .AddNewOgnp(megaFaculty, "Data analysis", 2, 45);
            ognp.Streams[0].OgnpLessons.Add(new OgnpLesson("ML", DayOfWeek.Monday, TimeSpan.FromMinutes(500),
                "203", "Kate", "Data analysis/1"));
            _ognpService.AddStudentToOgnp(student, ognp);
            Assert.IsNotEmpty(ognp.Streams[0].StudentsOfStream);
            _ognpService.DeleteStudentFromOgnp(student, ognp);
            Assert.IsEmpty(ognp.Streams[0].StudentsOfStream);
        }

        [Test]
        public void GetOgnpStreams_ReturnStreams()
        {
            var group1 = new Group("M3204");
            Student student = Group.AddPerson(group1, "Timofey Gurman");
            var megaFaculty = new MegaFaculty("ITIP", char.Parse("Z"));
            _ognpService.AddMegaFaculty(megaFaculty);
            Ognp ognp = _ognpService
                .AddNewOgnp(megaFaculty, "Data analysis", 2, 50);
            megaFaculty.MegaFacultyLessons.Add(new FacultyLesson("Alghoritms", DayOfWeek.Friday,
                TimeSpan.FromMinutes(500), "466", "Nina", student.Group));
            ognp.Streams[0].OgnpLessons.Add(new OgnpLesson("Alghoritms", DayOfWeek.Monday, TimeSpan.FromMinutes(500),
                "466", "Nina", "Data analysis/3.3"));
            Assert.AreEqual(ognp.Streams, _ognpService.GetOgnpStreams(ognp));
        }

        [Test]
        public void GetStudentsFromStream_ReturnStudents()
        {
            var group1 = new Group("M3204");
            Student student = Group.AddPerson(group1, "Timofey Gurman");
            var megaFaculty = new MegaFaculty("ITIP", char.Parse("Z"));
            _ognpService.AddMegaFaculty(megaFaculty);
            Ognp ognp = _ognpService
                .AddNewOgnp(megaFaculty, "Data analysis", 2, 50);
            _ognpService.AddStudentToOgnp(student, ognp);
            Assert.AreEqual(ognp.Streams[0].StudentsOfStream, _ognpService.GetStudentsFromStream(ognp.Streams[0]));
        }
    }
}