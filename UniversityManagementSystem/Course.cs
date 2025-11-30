using System;
using System.Collections.Generic;

namespace UniversityManagementSystem
{
    public abstract class Course
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Teacher AssignedTeacher { get; set; }
        public List<Student> EnrolledStudents { get; private set; }
        
        protected Course(string id, string name, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            EnrolledStudents = new List<Student>();
        }
        
        public abstract string GetCourseType();
        
        public virtual void EnrollStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (!EnrolledStudents.Contains(student))
            {
                EnrolledStudents.Add(student);
                student.EnrollInCourse(this);
            }
        }
        
        public virtual void RemoveStudent(Student student)
        {
            EnrolledStudents?.Remove(student);
        }
        
        public virtual string GetCourseInfo()
        {
            return $"Course: {Name} (ID: {Id}), Type: {GetCourseType()}, " + $"Students: {EnrolledStudents.Count}, Teacher: {AssignedTeacher?.Name ?? "Not assigned"}";
        }
    }
}
