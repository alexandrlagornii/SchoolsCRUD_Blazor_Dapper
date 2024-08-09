namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_Person
    {
        public int Id { get; set; } = 0;

        public int SchoolId { get; set; } = 0;
        public SH_School? SH_School { get; set; }

        public int TypeId { get; set; } = 0;
        public SH_d_Type? SH_d_Type { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public DateTime? DateOfBirth { get; set; }
    }
}
