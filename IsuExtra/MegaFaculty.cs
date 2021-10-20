using System;
using System.Collections.Generic;

namespace IsuExtra
{
    public class MegaFaculty
    {
        public MegaFaculty(string name, char letter)
        {
            Id = Guid.NewGuid();
            MegaFacultyLessons = new List<FacultyLesson>();
            Name = name;
            Letter = letter;
        }

        public List<FacultyLesson> MegaFacultyLessons { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public char Letter { get; set; }
        public Ognp Ognp { get; set; }
    }
}