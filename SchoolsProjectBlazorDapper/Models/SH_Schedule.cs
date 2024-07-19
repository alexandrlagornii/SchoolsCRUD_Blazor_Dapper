namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_Schedule
    {
        public int? Id { get; set; }
        public TimeOnly? Time { get; set; }
        public int? TeacherId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
    }
}
