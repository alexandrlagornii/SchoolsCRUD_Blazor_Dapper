namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_School
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? NumStudents { get; set; }
        public int? NumTeachers { get; set; }

        public int? CountryId { get; set; }
        public SH_d_Country? SH_d_Country { get; set; }

        public int? CityId { get; set; }
        public SH_d_City? SH_d_City { get; set; }

        public string? Address { get; set; }
    }
}
