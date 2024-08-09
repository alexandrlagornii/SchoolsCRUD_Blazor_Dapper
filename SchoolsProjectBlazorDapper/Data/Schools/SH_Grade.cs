namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_Grade
    {
        public int Id { get; set; } = 0;
        public int Grade { get; set; } = 0;

        public int StudentId { get; set; } = 0;
        public SH_Person? SH_PersonStudent { get; set; }

        public  int TeacherId { get; set; } = 0;
        public SH_Person? SH_PersonTeacher { get; set; }

        public int SubjectId { get; set; } = 0;
        public SH_d_Subject? SH_d_Subject { get; set; }
    }
}
