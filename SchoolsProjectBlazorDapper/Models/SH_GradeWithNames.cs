namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_GradeWithNames
    {
        public int Grade { get; set; }
        public int StudentId { get; set; }
        public string StudentName {  get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        
        public string SubjectName { get; set; }
    }
}
