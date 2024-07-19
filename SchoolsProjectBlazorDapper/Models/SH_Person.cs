namespace SchoolsProjectBlazorDapper.Models
{
    public class SH_Person
    {
        public int? Id { get; set; }
        public int? SchoolId {  get; set; }
        public int? TypeId {  get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
