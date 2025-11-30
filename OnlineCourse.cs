using System;

namespace UniversityManagementSystem
{
    public class OnlineCourse : Course
    {
        public string Platform { get; }
        public string MeetingLink { get; }

        public OnlineCourse(string id, string name, string platform, string meetingLink, string description = "") : base(id, name, description)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            MeetingLink = meetingLink ?? throw new ArgumentNullException(nameof(meetingLink));
        }

        public override string GetCourseType() => "Online";

        public override string GetCourseInfo()
        {
            return base.GetCourseInfo() + $", Platform: {Platform}, Meeting Link: {MeetingLink}";
        }
    }
}