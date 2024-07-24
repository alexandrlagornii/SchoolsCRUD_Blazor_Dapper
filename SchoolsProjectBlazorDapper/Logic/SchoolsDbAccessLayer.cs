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


        // Procedures
        private const string PRC_SH_PERSONS_SELECT_ALL = "prc_SH_PersonsSelectAll";
        private const string PRC_SH_PERSONS_SELECT_BY_ID = "prc_SH_PersonsSelectById";
        private const string PRC_SH_PERSONS_SELECT_ALL_BY_TYPE = "prc_SH_PersonsSelectAllByType";
        private const string PRC_SH_PERSONS_INSERT = "prc_SH_PersonsInsert";
        private const string PRC_SH_PERSONS_DELETE_BY_ID = "prc_SH_PersonsDeleteById";

        private const string PRC_SH_SCHOOLS_SELECT_ALL = "prc_SH_SchoolsSelectAll";
        private const string PRC_SH_SCHOOLS_SELECT_BY_ID = "prc_SH_SchoolsSelectById";

        private const string PRC_SH_D_TYPES_SELECT_BY_ID = "prc_SH_d_TypesSelectById";

        private const string PRC_SH_GRADES_INSERT = "prc_SH_GradesInsert";
        private const string PRC_SH_GRADES_SELECT_ALL = "prc_SH_GradesSelectAll";
        private const string PRC_SH_GRADES_SELECT_BY_ID = "prc_SH_GradesSelectById";
        private const string PRC_SH_GRADES_UPDATE = "prc_SH_GradesUpdate";
        private const string PRC_SH_GRADES_DELETE_BY_ID = "prc_SH_GradesDeleteById";

        private const string PRC_SH_D_SUBJECTS_SELECT_ALL = "prc_SH_d_SubjectsSelectAll";
        private const string PRC_SH_D_SUBJECTS_SELECT_BY_ID = "prc_SH_d_SubjectsSelectById";


        // Constructor
        public SchoolsDbAccessLayer(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Function Queries
        public async Task<List<SH_Person>> SH_PersonsSelectAll()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Persons
                var result = await db.QueryAsync<SH_Person>(PRC_SH_PERSONS_SELECT_ALL, commandType: CommandType.StoredProcedure);

                // For each row get the relations SH_School and SH_d_Type
                foreach (SH_Person person in result)
                {
                    // Get SH_School in the SH_Person datatype instance
                    var ps = new DynamicParameters();
                    ps.Add("@Id", person.SchoolId);
                    person.SH_School = await db.QuerySingleOrDefaultAsync<SH_School>(PRC_SH_SCHOOLS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);

                    // Get Sh_d_Type in the SH_Person datatype instance
                    var pt = new DynamicParameters();
                    pt.Add("@Id", person.TypeId);
                    person.SH_d_Type = await db.QuerySingleOrDefaultAsync<SH_d_Type>(PRC_SH_D_TYPES_SELECT_BY_ID, pt, commandType: CommandType.StoredProcedure);
                }
                return result.ToList();
            }
        }

        public async Task<List<SH_Person>> SH_PersonsSelectAllByType(SH_Person selectedPerson)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Persons by type
                var type = new DynamicParameters();
                type.Add("@TypeId", selectedPerson.TypeId);
                var result = await db.QueryAsync<SH_Person>(PRC_SH_PERSONS_SELECT_ALL_BY_TYPE, type, commandType: CommandType.StoredProcedure);

                // For each row get the relations SH_School and SH_d_Type
                foreach (SH_Person person in result)
                {
                    // Get SH_School in the SH_Person datatype instance
                    var ps = new DynamicParameters();
                    ps.Add("@Id", person.SchoolId);
                    person.SH_School = await db.QuerySingleOrDefaultAsync<SH_School>(PRC_SH_SCHOOLS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);

                    // Get Sh_d_Type in the SH_Person datatype instance
                    var pt = new DynamicParameters();
                    pt.Add("@Id", person.TypeId);
                    person.SH_d_Type = await db.QuerySingleOrDefaultAsync<SH_d_Type>(PRC_SH_D_TYPES_SELECT_BY_ID, pt, commandType: CommandType.StoredProcedure);
                }
                return result.ToList();
            }
        }

        public async Task SH_PersonsInsert(SH_Person selectedPerson)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Persons by type
                var p = new DynamicParameters();
                p.Add("@SchoolId", selectedPerson.SchoolId);
                p.Add("@TypeId", selectedPerson.TypeId);
                p.Add("@FirstName", selectedPerson.FirstName);
                p.Add("@LastName", selectedPerson.LastName);
                p.Add("@DateOfBirth", selectedPerson.DateOfBirth);

                await db.QueryAsync<SH_Person>(PRC_SH_PERSONS_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_PersonsDeleteById(SH_Person person)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", person.Id);
                await db.QueryAsync(PRC_SH_PERSONS_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<SH_Grade>> SH_GradesSelectAll()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Grades
                var result = await db.QueryAsync<SH_Grade>(PRC_SH_GRADES_SELECT_ALL, commandType: CommandType.StoredProcedure);

                // For each row get the relations SH_Person (Student), SH_Person (Teacher), SH_d_Subject
                foreach (SH_Grade grade in result)
                {
                    // Get SH_Person for Student
                    var pps = new DynamicParameters();
                    pps.Add("@Id", grade.StudentId);
                    grade.SH_PersonStudent = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, pps, commandType: CommandType.StoredProcedure);

                    // Get SH_Person for Teacher
                    var ppt = new DynamicParameters();
                    ppt.Add("@Id", grade.TeacherId);
                    grade.SH_PersonTeacher = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, ppt, commandType: CommandType.StoredProcedure);

                    // Get SH_d_Subject
                    var ps = new DynamicParameters();
                    ps.Add("@Id", grade.SubjectId);
                    grade.SH_d_Subject = await db.QuerySingleOrDefaultAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);
                }

                return result.ToList();
            }
        }

        public async Task<SH_Grade> SH_GradesSelectById(SH_Grade grade)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get single grade
                var p = new DynamicParameters();
                p.Add("@Id", grade.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_Grade>(PRC_SH_GRADES_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);

                // Get SH_Person for Student
                var pps = new DynamicParameters();
                pps.Add("@Id", result.StudentId);
                result.SH_PersonStudent = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, pps, commandType: CommandType.StoredProcedure);

                // Get SH_Person for Teacher
                var ppt = new DynamicParameters();
                ppt.Add("@Id", result.TeacherId);
                result.SH_PersonTeacher = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, ppt, commandType: CommandType.StoredProcedure);

                // Get SH_d_Subject
                var ps = new DynamicParameters();
                ps.Add("@Id", result.SubjectId);
                result.SH_d_Subject = await db.QuerySingleOrDefaultAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task SH_GradesDeleteById(SH_Grade grade)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", grade.Id);
                await db.QueryAsync(PRC_SH_GRADES_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_GradesUpdate(SH_Grade grade)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", grade.Id);
                p.Add("@Grade", grade.Grade);
                p.Add("@StudentId", grade.StudentId);
                p.Add("@TeacherId", grade.TeacherId);
                p.Add("@SubjectId", grade.SubjectId);
                await db.QueryAsync(PRC_SH_GRADES_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task SH_GradesInsert(SH_Grade grade)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Grade", grade.Grade);
                p.Add("@StudentId", grade.StudentId);
                p.Add("@TeacherId", grade.TeacherId);
                p.Add("@SubjectId", grade.SubjectId);
                await db.QueryAsync(PRC_SH_GRADES_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<List<SH_d_Subject>> SH_d_SubjectsSelectAll()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Persons
                var result = await db.QueryAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
