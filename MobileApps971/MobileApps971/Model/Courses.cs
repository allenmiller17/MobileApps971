using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApps971.Model
{
    public class Courses
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }


        public string CourseName { get; set; }


        public int Term { get; set; }


        public DateTime CourseStartDate { get; set; }


        public DateTime CourseEndDate { get; set; }


        public string Status { get; set; }


        public string CourseInstructorName { get; set; }


        public string CourseInstructorPhone { get; set; }


        public string CourseInstructorEmail { get; set; }


        public string CourseNotes { get; set; }


        public int Notifications { get; set; }
    }
}
