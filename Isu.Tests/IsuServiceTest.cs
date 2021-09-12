using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group newGroup = _isuService.AddGroup("M3204");
            _isuService.AddStudent(newGroup, "Timofey Gurman");
            Assert.IsNotEmpty(newGroup.Students);
            Assert.IsNotNull(newGroup.Students[0].Group);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Group newGroup = _isuService.AddGroup("M3204");
            for (int i = 0; i < Group.MaxAmountOfStudents; i++)
            {
                _isuService.AddStudent(newGroup, "student");
                
            }
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddStudent(newGroup, "student");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("11111");
            });
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M5343");
            });
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M5439");
            });
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M222");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group newGroup = _isuService.AddGroup("M3204");
            Group newGroup2 = _isuService.AddGroup("M3205");

            _isuService.AddStudent(newGroup, "Timofey Gurman");
            _isuService.ChangeStudentGroup(_isuService.FindStudent("Timofey Gurman"), newGroup2);
            Assert.IsEmpty(newGroup.Students);
            Assert.IsNotNull(newGroup2.Students[0].Group);
        }
    }
}