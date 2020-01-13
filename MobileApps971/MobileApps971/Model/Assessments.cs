using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApps971.Model
{
    public class Assessments
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }

        [NotNull]
        public string AssessmentName { get; set; }

        [NotNull]
        public string AssessmentType { get; set; }


        public DateTime AssessmentStart { get; set; }


        public DateTime AssessmentEnd { get; set; }


        public int CourseId { get; set; }


        public int AssessmentNotifications { get; set; }
    }
}
