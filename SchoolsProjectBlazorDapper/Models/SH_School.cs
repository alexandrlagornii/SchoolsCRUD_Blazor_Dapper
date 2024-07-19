namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_School
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? NumStudents {  get; set; }
        public int? NumTeachers { get; set; }
        public int? CountryId { get; set; }
        public int? CityId {  get; set; }
        public string? Address {  get; set; }
    }
}
