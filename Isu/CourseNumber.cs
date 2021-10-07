using Isu.Tools;

namespace Isu
{
    public class CourseNumber
    {
        public CourseNumber(int courseNumber)
        {
            if (courseNumber is < 1 or > 4)
            {
                throw new InvalidCourseNumberException();
            }

            Number = courseNumber;
        }

        public int Number { get; }
    }
}