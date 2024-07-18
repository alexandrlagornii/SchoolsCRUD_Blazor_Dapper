using Dapper;
using Microsoft.Data.SqlClient;
using SchoolsProjectBlazorDapper.Models;
using System.Data;

namespace SchoolsProjectBlazorDapper.Logic
{
    public class SchoolsDbAccessLayer
    {
        // Config
        public IConfiguration Configuration;
        private const string SCHOOLS_DATABASE = "SchoolsDb";

        // Queries
        private const string GET_GRADES = "select * from SH_Grades";
        
        // Constructor
        public SchoolsDbAccessLayer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Function Queries
        public async Task<List<SH_PersonWithTypeSchoolsNames>> GetPersonsWithTypeSchoolsNames()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<SH_PersonWithTypeSchoolsNames> result = await db.QueryAsync<SH_PersonWithTypeSchoolsNames>("GetPersonsWithTypeSchoolsNames", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<SH_Grade>> GetGrades()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<SH_Grade> result = await db.QueryAsync<SH_Grade>(GET_GRADES);
                return result.ToList();
            }
        }

        public async Task<List<SH_GradeWithNames>> GetGradesWithNames()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<SH_GradeWithNames> result = await db.QueryAsync<SH_GradeWithNames>("GetGradesWithNames", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
