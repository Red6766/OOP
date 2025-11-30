using System;
using System.Collections.Generic;

namespace UniversityManagementSystem
{
    public class Student
    {
        public string Id { get; }
        public string Name { get; }
        public List<Course> EnrolledCourses { get; private set; }

        public Student(string id, string name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            EnrolledCourses = new List<Course>();
        }

        public void EnrollInCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (!EnrolledCourses.Contains(course))
            {
                EnrolledCourses.Add(course);
                course.EnrollStudent(this);
            }
        }

        public void UnenrollFromCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            EnrolledCourses.Remove(course);
            course?.RemoveStudent(this);
        }

        public string GetStudentInfo()
        {
            return $"Student: {Name} (ID: {Id}), Enrolled in {EnrolledCourses.Count} courses";
        }
    }
}
