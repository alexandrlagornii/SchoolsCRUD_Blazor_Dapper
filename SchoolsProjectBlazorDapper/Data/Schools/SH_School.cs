namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_School
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int NumStudents { get; set; } = 0;
        public int NumTeachers { get; set; } = 0;

        public int CountryId { get; set; } = 0;
        public SH_d_Country? SH_d_Country { get; set; }

        public int CityId { get; set; } = 0;
        public SH_d_City? SH_d_City { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}
