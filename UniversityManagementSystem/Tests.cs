using System;
using Xunit;

namespace UniversityManagementSystem
{
    public class Tests
    {
        private readonly CourseManager _manager;

        public Tests()
        {
            _manager = CourseManager.Instance;
            _manager.ClearAllData();
        }

        [Fact]
        public void AddCourse_ValidCourse_ShouldAddSuccessfully()
        {
            var course = new OnlineCourse("200001", "Python Programming", "Zoom", "https://zoom.link");

            _manager.AddCourse(course);
            
            Assert.Single(_manager.Courses);
            Assert.Equal("200001", _manager.Courses[0].Id);
        }

        [Fact]
        public void AddCourse_DuplicateId_ShouldThrowException()
        {
            var course1 = new OnlineCourse("200001", "Python", "Zoom", "link1");
            var course2 = new OnlineCourse("200001", "Java", "Teams", "link2");
            
            _manager.AddCourse(course1);
            
            Assert.Throws<InvalidOperationException>(() => _manager.AddCourse(course2));
        }

        [Fact]
        public void AssignTeacherToCourse_ValidData_ShouldAssignSuccessfully()
        {
            var teacher = new Teacher("100001", "Alexander Pushkin", "Mathematics");
            var course = new OnlineCourse("200001", "Python", "Zoom", "link");
            
            _manager.AddTeacher(teacher);
            _manager.AddCourse(course);
            _manager.AssignTeacherToCourse("100001", "200001");
            
            var teacherCourses = _manager.GetCoursesByTeacher("100001");
            Assert.Single(teacherCourses);
            Assert.Equal("200001", teacherCourses[0].Id);
        }

        [Fact]
        public void AssignTeacherToCourse_NonexistentTeacher_ShouldThrowException()
        {
            var course = new OnlineCourse("200001", "Python", "Zoom", "link");
            _manager.AddCourse(course);
            
            Assert.Throws<ArgumentException>(() => _manager.AssignTeacherToCourse("999999", "200001"));
        }

        [Fact]
        public void EnrollStudentInCourse_ValidData_ShouldEnrollSuccessfully()
        {
            var student = new Student("300001", "Anna Karenina");
            var course = new OnlineCourse("200001", "Python", "Zoom", "link");
            
            _manager.AddStudent(student);
            _manager.AddCourse(course);
            _manager.EnrollStudentInCourse("300001", "200001");
            
            var courseStudents = _manager.GetStudentsInCourse("200001");
            Assert.Single(courseStudents);
            Assert.Equal("300001", courseStudents[0].Id);
        }

        [Fact]
        public void GetCoursesByTeacher_ValidTeacher_ShouldReturnCourses()
        {
            var teacher = new Teacher("100001", "Alexander Pushkin", "Mathematics");
            var course1 = new OnlineCourse("200001", "Python", "Zoom", "link1");
            var course2 = new OnlineCourse("200002", "Math", "Teams", "link2");
            
            _manager.AddTeacher(teacher);
            _manager.AddCourse(course1);
            _manager.AddCourse(course2);
            _manager.AssignTeacherToCourse("100001", "200001");
            _manager.AssignTeacherToCourse("100001", "200002");
            
            var courses = _manager.GetCoursesByTeacher("100001");
            
            Assert.Equal(2, courses.Count);
        }

        [Fact]
        public void GetCoursesByType_OnlineCourses_ShouldReturnCorrectCount()
        {
            var onlineCourse = new OnlineCourse("200001", "Python", "Zoom", "link1");
            var offlineCourse = new OfflineCourse("200002", "Math", "Room 101");
            
            _manager.AddCourse(onlineCourse);
            _manager.AddCourse(offlineCourse);
            
            var onlineCourses = _manager.GetCoursesByType("Online");
            var offlineCourses = _manager.GetCoursesByType("Offline");
            
            Assert.Single(onlineCourses);
            Assert.Single(offlineCourses);
        }

        [Fact]
        public void RemoveCourse_ExistingCourse_ShouldRemoveSuccessfully()
        {
            var course = new OnlineCourse("200001", "Python", "Zoom", "link");
            _manager.AddCourse(course);
            
            var result = _manager.RemoveCourse("200001");
            
            Assert.True(result);
            Assert.Empty(_manager.Courses);
        }

        [Fact]
        public void Singleton_ShouldReturnSameInstance()
        {
            var instance1 = CourseManager.Instance;
            var instance2 = CourseManager.Instance;
            
            Assert.Same(instance1, instance2);
        }
    }
} 
