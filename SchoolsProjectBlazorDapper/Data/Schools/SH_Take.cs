namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_Take
    {
        public int Id { get; set; } = 0;

        public int ClassId { get; set; } = 0;
        public SH_d_Class? SH_d_Class { get; set; }

        public int StudentId { get; set; } = 0;
        public SH_Person? SH_Person { get; set; }
    }
}
