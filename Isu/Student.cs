namespace Isu
{
    public class Student
    {
        private static int _id = 100000;

        public Student(string name)
        {
            _id++;
            Name = name;
            Id = _id;
        }

        public string Name { get; }
        public int Id { get; }
        public Group Group { get; private set; }

        public void ChangeGroup(Group newGroup)
        {
            Group = newGroup;
        }
    }
}