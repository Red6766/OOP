using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    public sealed class CourseManager
    {
        private static readonly Lazy<CourseManager> _instance = new Lazy<CourseManager>(() => new CourseManager());
        public static CourseManager Instance => _instance.Value;
        private List<Course> _courses;
        private List<Teacher> _teachers;
        private List<Student> _students;
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();
        public IReadOnlyList<Teacher> Teachers => _teachers.AsReadOnly();
        public IReadOnlyList<Student> Students => _students.AsReadOnly();

        private CourseManager()
        {
            _courses = new List<Course>();
            _teachers = new List<Teacher>();
            _students = new List<Student>();
        }

        public void AddCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (_courses.Any(c => c.Id == course.Id)) throw new InvalidOperationException($"Course with ID '{course.Id}' already exists");
            _courses.Add(course);
        }

        public bool RemoveCourse(string courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId)) throw new ArgumentException("Course ID cannot be empty");
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                foreach (var teacher in _teachers) teacher.RemoveFromCourse(course);
                foreach (var student in _students) student.UnenrollFromCourse(course);
                return _courses.Remove(course);
            }
            return false;
        }

        public Course GetCourseById(string courseId)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId);
        }

        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null) throw new ArgumentNullException(nameof(teacher));
            if (_teachers.Any(t => t.Id == teacher.Id)) throw new InvalidOperationException($"Teacher with ID '{teacher.Id}' already exists");
            _teachers.Add(teacher);
        }

        public bool RemoveTeacher(string teacherId)
        {
            var teacher = _teachers.FirstOrDefault(t => t.Id == teacherId);
            if (teacher != null)
            {
                foreach (var course in _courses.Where(c => c.AssignedTeacher?.Id == teacherId)) course.AssignedTeacher = null;
                return _teachers.Remove(teacher);
            }
            return false;
        }

        public Teacher GetTeacherById(string teacherId)
        {
            return _teachers.FirstOrDefault(t => t.Id == teacherId);
        }

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (_students.Any(s => s.Id == student.Id)) throw new InvalidOperationException($"Student with ID '{student.Id}' already exists");
            _students.Add(student);
        }

        public bool RemoveStudent(string studentId)
        {
            var student = _students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                foreach (var course in _courses) course.RemoveStudent(student);
                return _students.Remove(student);
            }
            return false;
        }

        public Student GetStudentById(string studentId)
        {
            return _students.FirstOrDefault(s => s.Id == studentId);
        }

        public void AssignTeacherToCourse(string teacherId, string courseId)
        {
            var teacher = GetTeacherById(teacherId);
            var course = GetCourseById(courseId);
            if (teacher == null) throw new ArgumentException($"Teacher with ID '{teacherId}' not found");
            if (course == null) throw new ArgumentException($"Course with ID '{courseId}' not found");
            teacher.AssignToCourse(course);
        }

        public void EnrollStudentInCourse(string studentId, string courseId)
        {
            var student = GetStudentById(studentId);
            var course = GetCourseById(courseId);
            if (student == null) throw new ArgumentException($"Student with ID '{studentId}' not found");
            if (course == null) throw new ArgumentException($"Course with ID '{courseId}' not found");
            student.EnrollInCourse(course);
        }

        public List<Course> GetCoursesByTeacher(string teacherId)
        {
            var teacher = GetTeacherById(teacherId);
            return teacher?.TeachingCourses ?? new List<Course>();
        }

        public List<Student> GetStudentsInCourse(string courseId)
        {
            var course = GetCourseById(courseId);
            return course?.EnrolledStudents ?? new List<Student>();
        }

        public List<Course> GetCoursesByType(string courseType)
        {
            return _courses.Where(c => c.GetCourseType() == courseType).ToList();
        }

        public string GetStatistics()
        {
            return $"System Statistics:\n" +
                    $"Courses: {_courses.Count}\n" +
                    $"Teachers: {_teachers.Count}\n" +
                    $"Students: {_students.Count}\n" +
                    $"Online Courses: {GetCoursesByType("Online").Count}\n" +
                    $"Offline Courses: {GetCoursesByType("Offline").Count}\n";
        }

        public void ClearAllData()
        {
            _courses.Clear();
            _teachers.Clear();
            _students.Clear();
        }
    }
}
