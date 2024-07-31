namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_Take
    {
        public int? Id { get; set; }

        public int? ClassId { get; set; }
        public SH_d_Class? SH_d_Class
        {
            get; set;
        }
        public int? StudentId { get; set; }
        public SH_Person? SH_Person { get; set; }
    }
}
