namespace SchoolsProjectBlazorDapper.Models
{
    public class SchoolWithCityCountryNames
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public int NumStudents { get; set; }
        public int NumTeachers {  get; set; }
        public string Country {  get; set; }
        public string City { get; set; }
        public string Address {  get; set; }
    }
}
