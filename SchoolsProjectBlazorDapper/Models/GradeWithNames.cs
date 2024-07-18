namespace SchoolsProjectBlazorDapper.Models
{
    public class GradeWithNames
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int StudentId { get; set; }
        public string StudentName {  get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        
        public string SubjectName { get; set; }
    }
}
