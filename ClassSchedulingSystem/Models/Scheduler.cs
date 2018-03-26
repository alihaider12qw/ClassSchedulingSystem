//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassSchedulingSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Scheduler
    {
        public int Id { get; set; }
        public int RoomID { get; set; }
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public string DayWeek { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Room Room { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
