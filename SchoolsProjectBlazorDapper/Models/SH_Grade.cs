namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_Grade
    {
        public int? Id { get; set; }
        public int? Grade { get; set; }
        
        public int? StudentId { get; set; }
        public SH_Person? SH_PersonStudent { get; set; }

        public int? TeacherId { get; set; }
        public SH_Person? SH_PersonTeacher { get; set; }
        
        public int? SubjectId { get; set; }
        public SH_d_Subject? SH_d_Subject { get; set; }
    }
}
