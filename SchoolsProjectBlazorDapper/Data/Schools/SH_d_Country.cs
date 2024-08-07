namespace SchoolsProjectBlazorDapper.Data.Schools
{
    public class SH_d_Country
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public ICollection<SH_School>? Schools { get; set; }
    }
}
