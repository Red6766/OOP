using System;

namespace UniversityManagementSystem
{
    public class OfflineCourse : Course
    {
        public string Classroom { get; }

        public OfflineCourse(string id, string name, string classroom, string description = "") : base(id, name, description)
        {
            Classroom = classroom ?? throw new ArgumentNullException(nameof(classroom));
        }

        public override string GetCourseType() => "Offline";

        public override string GetCourseInfo()
        {
            return base.GetCourseInfo() + $", Classroom: {Classroom}";
        }
    }
}
