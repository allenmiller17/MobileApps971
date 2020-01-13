using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApps971.Model
{
    public class Terms
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        public string TermName { get; set; }


        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }
    }
}
