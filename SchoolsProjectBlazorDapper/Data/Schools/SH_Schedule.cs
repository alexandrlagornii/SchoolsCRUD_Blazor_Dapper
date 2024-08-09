namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_Schedule
    {
        public int Id { get; set; } = 0;
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }

        public int TeacherId { get; set; } = 0;
        public SH_Person? SH_PersonTeacher { get; set; }

        public int ClassId { get; set; } = 0;
        public SH_d_Class? SH_d_Class { get; set; }


        public int SubjectId { get; set; } = 0;
        public SH_d_Subject? SH_d_Subject { get; set; }
    }
}
