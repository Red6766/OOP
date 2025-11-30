using System;
using System.Collections.Generic;

namespace UniversityManagementSystem
{
    public class Teacher
    {
        public string Id { get; }
        public string Name { get; }
        public string Department { get; }
        public List<Course> TeachingCourses { get; private set; }

        public Teacher(string id, string name, string department)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Department = department;
            TeachingCourses = new List<Course>();
        }

        public void AssignToCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (!TeachingCourses.Contains(course))
            {
                TeachingCourses.Add(course);
                course.AssignedTeacher = this;
            }
        }

        public void RemoveFromCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            TeachingCourses.Remove(course);
            if (course.AssignedTeacher == this)
                course.AssignedTeacher = null;
        }
    }
}
