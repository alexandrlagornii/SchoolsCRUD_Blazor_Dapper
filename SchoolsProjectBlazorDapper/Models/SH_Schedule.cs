namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_Schedule
    {
        public int? Id { get; set; }
        public TimeOnly? Time { get; set; }

        public int? TeacherId { get; set; }
        public SH_Person? SH_PersonTeacher { get; set; }

        public int? ClassId { get; set; }
        public SH_d_Class? SH_d_Class { get; set; }


        public int? SubjectId { get; set; }
        public SH_d_Subject? SH_d_Subject { get; set; }
    }
}
