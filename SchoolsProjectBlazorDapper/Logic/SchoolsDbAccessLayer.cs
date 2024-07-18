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
        private const string GET_GRADES = "select * from SH_Grades;";
        private const string GET_PERSONS = "select * from SH_Persons;";

        // Procedures
        private const string PROCEDURE_GET_PERSONS_WITH_TYPE_SCHOOL_NAMES = "GetPersonsWithTypeSchoolsNames";
        private const string PROCEDURE_GET_SCHOOLS_WITH_CITY_COUNTRY_NAMES = "GetSchoolsWithCityCountryNames";
        private const string PROCEDURE_GET_GRADES_WITH_NAMES = "GetGradesWithNames";
        private const string PROCEDURE_DELETE_GRADE = "DeleteGrade";

        // Constructor
        public SchoolsDbAccessLayer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Function Queries
        public async Task<List<SH_Person>> GetPersons()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<SH_Person> result = await db.QueryAsync<SH_Person>(GET_PERSONS);
                return result.ToList();
            }
        }
        public async Task<List<PersonWithTypeSchoolsNames>> GetPersonsWithTypeSchoolsNames()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<PersonWithTypeSchoolsNames> result = await db.QueryAsync<PersonWithTypeSchoolsNames>(PROCEDURE_GET_PERSONS_WITH_TYPE_SCHOOL_NAMES, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<SchoolWithCityCountryNames>> GetSchoolsWithCityCountryNames()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<SchoolWithCityCountryNames> result = await db.QueryAsync<SchoolWithCityCountryNames>(PROCEDURE_GET_SCHOOLS_WITH_CITY_COUNTRY_NAMES, commandType: CommandType.StoredProcedure);
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

        public async Task<List<GradeWithNames>> GetGradesWithNames()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                IEnumerable<GradeWithNames> result = await db.QueryAsync<GradeWithNames>(PROCEDURE_GET_GRADES_WITH_NAMES, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task DeleteGrade(int Id)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", Id);
                await db.QueryAsync(PROCEDURE_DELETE_GRADE, p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
