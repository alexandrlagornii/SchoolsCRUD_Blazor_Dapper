using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SchoolsProjectBlazorDapper.Data.Schools
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
        private const string PRC_SH_PERSONS_UPDATE = "prc_SH_PersonsUpdate";
        private const string PRC_SH_PERSON_JOIN_SCHOOL_JOIN_TYPE = "prc_SH_PersonJoinSchoolJoinType";

        private const string PRC_SH_SCHOOLS_SELECT_ALL = "prc_SH_SchoolsSelectAll";
        private const string PRC_SH_SCHOOLS_SELECT_BY_ID = "prc_SH_SchoolsSelectById";
        private const string PRC_SH_SCHOOLS_DELETE_BY_ID = "prc_SH_SchoolsDeleteById";
        private const string PRC_SH_SCHOOLS_INSERT = "prc_SH_SchoolsInsert";
        private const string PRC_SH_SCHOOLS_UPDATE = "prc_SH_SchoolsUpdate";
        private const string PRC_SH_SCHOOLS_JOIN_COUNTRY_JOIN_CITY = "prc_SH_SchoolsJoinCountriesJoinCities";

        private const string PRC_SH_D_TYPES_SELECT_ALL = "prc_SH_d_TypesSelectAll";
        private const string PRC_SH_D_TYPES_SELECT_BY_ID = "prc_SH_d_TypesSelectById";

        private const string PRC_SH_GRADES_INSERT = "prc_SH_GradesInsert";
        private const string PRC_SH_GRADES_SELECT_ALL = "prc_SH_GradesSelectAll";
        private const string PRC_SH_GRADES_SELECT_BY_ID = "prc_SH_GradesSelectById";
        private const string PRC_SH_GRADES_UPDATE = "prc_SH_GradesUpdate";
        private const string PRC_SH_GRADES_DELETE_BY_ID = "prc_SH_GradesDeleteById";

        private const string PRC_SH_TAKES_SELECT_ALL = "prc_SH_TakesSelectAll";
        private const string PRC_SH_TAKES_SELECT_BY_ID = "prc_SH_TakesSelectById";
        private const string PRC_SH_TAKES_DELETE_BY_ID = "prc_SH_TakesDeleteById";
        private const string PRC_SH_TAKES_INSERT = "prc_SH_TakesInsert";
        private const string PRC_SH_TAKES_UPDATE = "prc_SH_TakesUpdate";

        private const string PRC_SH_D_SUBJECTS_SELECT_ALL = "prc_SH_d_SubjectsSelectAll";
        private const string PRC_SH_D_SUBJECTS_SELECT_BY_ID = "prc_SH_d_SubjectsSelectById";
        private const string PRC_SH_D_SUBJECTS_INSERT = "prc_SH_d_SubjectsInsert";
        private const string PRC_SH_D_SUBJECTS_DELETE_BY_ID = "prc_SH_d_SubjectsDeleteById";
        private const string PRC_SH_D_SUBJECTS_UPDATE = "prc_SH_d_SubjectsUpdate";

        private const string PRC_SH_D_COUNTRY_SELECT_BY_ID = "prc_SH_d_CountrySelectById";
        private const string PRC_SH_D_COUNTRIES_SELECT_ALL = "prc_SH_d_CountriesSelectAll";

        private const string PRC_SH_D_CITY_SELECT_BY_ID = "prc_SH_d_CitySelectById";
        private const string PRC_SH_D_CITIES_SELECT_ALL = "prc_SH_d_CitiesSelectAll";

        private const string PRC_SH_D_CLASSES_SELECT_ALL = "prc_SH_d_ClassesSelectAll";
        private const string PRC_SH_D_CLASSES_SELECT_BY_ID = "prc_SH_d_ClassesSelectById";
        private const string PRC_SH_D_CLASSES_INSERT = "prc_SH_d_ClassesInsert";
        private const string PRC_SH_D_CLASSES_DELETE_BY_ID = "prc_SH_d_ClassesDeleteById";
        private const string PRC_SH_D_CLASSES_UPDATE = "prc_SH_d_ClassesUpdate";

        private const string PRC_SH_SCHEDULES_SELECT_ALL = "prc_SH_SchedulesSelectAll";
        private const string PRC_SH_SCHEDULES_INSERT = "prc_SH_SchedulesInsert";
        private const string PRC_SH_SCHEDULES_DELETE_BY_ID = "prc_SH_SchedulesDeleteById";
        private const string PRC_SH_SCHEDULES_SELECT_BY_ID = "prc_SH_SchedulesSelectById";
        private const string PRC_SH_SCHEDULES_UPDATE = "prc_SH_SchedulesUpdate";

        // Constructor
        public SchoolsDbAccessLayer(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Person Methods
        public async Task<IEnumerable<SH_Person>> SH_PersonsSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var result = await db.QueryAsync<SH_Person, SH_School, SH_d_Type, SH_Person>(
                    PRC_SH_PERSON_JOIN_SCHOOL_JOIN_TYPE,
                    (person, school, type) =>
                    {
                        person.SH_School = school;
                        person.SH_d_Type = type;
                        return person;
                    },
                    commandType: CommandType.StoredProcedure
                    );
                return result;
            }
        }

        public async Task<IEnumerable<SH_Person>> SH_PersonsSelectAllByType(SH_Person selectedPerson)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var result = (await SH_PersonsSelectAll()).Where(p => p.TypeId == selectedPerson.TypeId);
                return result;
            }

        }

        public async Task<SH_Person> SH_PersonSelectById(SH_Person selectedPerson)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var result = (await SH_PersonsSelectAll()).First(p => p.Id == selectedPerson.Id);
                return result;
            }
        }

        public async Task SH_PersonsInsert(SH_Person selectedPerson)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
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
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", person.Id);
                await db.QueryAsync(PRC_SH_PERSONS_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_PersonsUpdate(SH_Person person)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", person.Id);
                p.Add("@SchoolId", person.SchoolId);
                p.Add("@TypeId", person.TypeId);
                p.Add("@FirstName", person.FirstName);
                p.Add("@LastName", person.LastName);
                p.Add("@DateOfBirth", person.DateOfBirth);
                await db.QueryAsync(PRC_SH_PERSONS_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        // Schools Methods
        public async Task<IEnumerable<SH_School>> SH_SchoolsSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var result = await db.QueryAsync<SH_School, SH_d_Country, SH_d_City, SH_School>(
                    PRC_SH_SCHOOLS_JOIN_COUNTRY_JOIN_CITY,
                    (school, country, city) =>
                    {
                        school.SH_d_Country = country;
                        school.SH_d_City = city;
                        return school;
                    },
                    commandType: CommandType.StoredProcedure
                    );
                return result;
            }
        }

        public async Task<SH_School> SH_SchoolsSelectById(SH_School school)
        {
            var result = (await SH_SchoolsSelectAll()).First(s => s.Id == school.Id);
            return result;
        }

        public async Task SH_SchoolsInsert(SH_School school)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Name", school.Name);
                p.Add("@CountryId", school.CountryId);
                p.Add("@CityId", school.CityId);
                p.Add("@Address", school.Address);
                await db.QueryAsync(PRC_SH_SCHOOLS_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task SH_SchoolsUpdate(SH_School school)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", school.Id);
                p.Add("@Name", school.Name);
                p.Add("@CountryId", school.CountryId);
                p.Add("@CityId", school.CityId);
                p.Add("@Address", school.Address);
                await db.QueryAsync(PRC_SH_SCHOOLS_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_SchoolsDeleteById(SH_School school)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", school.Id);
                await db.QueryAsync(PRC_SH_SCHOOLS_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        // Grades Methods
        public async Task<List<SH_Grade>> SH_GradesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
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
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
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
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();
                var p = new DynamicParameters();
                p.Add("@Id", grade.Id);
                await db.QueryAsync(PRC_SH_GRADES_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_GradesUpdate(SH_Grade grade)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
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
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
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

        public async Task<List<SH_Take>> SH_TakeSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Grades
                var result = await db.QueryAsync<SH_Take>(PRC_SH_TAKES_SELECT_ALL, commandType: CommandType.StoredProcedure);

                // For each row get the relations SH_Person (Student), SH_Person (Teacher), SH_d_Subject
                foreach (SH_Take take in result)
                {
                    // Get SH_Class
                    var pc = new DynamicParameters();
                    pc.Add("@Id", take.ClassId);
                    take.SH_d_Class = await db.QuerySingleOrDefaultAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_BY_ID, pc, commandType: CommandType.StoredProcedure);

                    // Get SH_Person (Student)
                    var ps = new DynamicParameters();
                    ps.Add("@Id", take.StudentId);
                    take.SH_Person = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);
                }

                return result.ToList();
            }
        }

        public async Task<SH_Take> SH_TakeSelectById(SH_Take take)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get selected take
                var p = new DynamicParameters();
                p.Add("@Id", take.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_Take>(PRC_SH_TAKES_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);

                // Get classes
                var pc = new DynamicParameters();
                pc.Add("@Id", result.ClassId);
                result.SH_d_Class = await db.QuerySingleOrDefaultAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_BY_ID, pc, commandType: CommandType.StoredProcedure);

                // Get students
                var ps = new DynamicParameters();
                ps.Add("@Id", result.StudentId);
                result.SH_Person = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task SH_TakesInsert(SH_Take take)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("ClassId", take.ClassId);
                p.Add("StudentId", take.StudentId);
                await db.QueryAsync(PRC_SH_TAKES_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_TakeDeleteById(SH_Take take)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("@Id", take.Id);
                await db.QueryAsync(PRC_SH_TAKES_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_TakesUpdate(SH_Take take)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("@Id", take.Id);
                p.Add("@ClassId", take.ClassId);
                p.Add("@StudentId", take.StudentId);
                await db.QueryAsync(PRC_SH_TAKES_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<SH_Schedule>> SH_SchedulesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get schedules
                var result = await db.QueryAsync<SH_Schedule>(PRC_SH_SCHEDULES_SELECT_ALL, commandType: CommandType.StoredProcedure);

                foreach (var schedule in result)
                {
                    // Get teacher
                    var pt = new DynamicParameters();
                    pt.Add("@Id", schedule.TeacherId);
                    schedule.SH_PersonTeacher = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, pt, commandType: CommandType.StoredProcedure);

                    // Get class
                    var pc = new DynamicParameters();
                    pc.Add("@Id", schedule.ClassId);
                    schedule.SH_d_Class = await db.QuerySingleOrDefaultAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_BY_ID, pc, commandType: CommandType.StoredProcedure);

                    // Get Subject
                    var ps = new DynamicParameters();
                    ps.Add("@Id", schedule.SubjectId);
                    schedule.SH_d_Subject = await db.QuerySingleOrDefaultAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);
                }

                return result.ToList();
            }
        }

        public async Task SH_SchedulesInsert(SH_Schedule schedule)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("@ClassId", schedule.ClassId);
                p.Add("@SubjectId", schedule.SubjectId);
                p.Add("@TeacherId", schedule.TeacherId);
                p.Add("@TimeStart", schedule.TimeStart);
                p.Add("@TimeEnd", schedule.TimeEnd);
                await db.QueryAsync(PRC_SH_SCHEDULES_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_SchedulesDeleteById(SH_Schedule schedule)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("@Id", schedule.Id);
                await db.QueryAsync(PRC_SH_SCHEDULES_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<SH_Schedule> SH_SchedulesSelectById(SH_Schedule schedule)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get schedules
                var p = new DynamicParameters();
                p.Add("@Id", schedule.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_Schedule>(PRC_SH_SCHEDULES_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);

                // Get teacher
                var pt = new DynamicParameters();
                pt.Add("@Id", result.TeacherId);
                result.SH_PersonTeacher = await db.QuerySingleOrDefaultAsync<SH_Person>(PRC_SH_PERSONS_SELECT_BY_ID, pt, commandType: CommandType.StoredProcedure);

                // Get class
                var pc = new DynamicParameters();
                pc.Add("@Id", result.ClassId);
                result.SH_d_Class = await db.QuerySingleOrDefaultAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_BY_ID, pc, commandType: CommandType.StoredProcedure);

                // Get Subject
                var ps = new DynamicParameters();
                ps.Add("@Id", result.SubjectId);
                result.SH_d_Subject = await db.QuerySingleOrDefaultAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_BY_ID, ps, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task SH_SchedulesUpdate(SH_Schedule schedule)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                var p = new DynamicParameters();
                p.Add("@Id", schedule.Id);
                p.Add("@TimeStart", schedule.TimeStart);
                p.Add("@TimeEnd", schedule.TimeEnd);
                p.Add("@TeacherId", schedule.TeacherId);
                p.Add("@ClassId", schedule.ClassId);
                p.Add("@SubjectId", schedule.SubjectId);
                await db.QueryAsync(PRC_SH_SCHEDULES_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<SH_d_Subject>> SH_d_SubjectsSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_Persons
                var result = await db.QueryAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<SH_d_Subject> SH_d_SubjectsSelectById(SH_d_Subject subject)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_d_Subject
                var p = new DynamicParameters();
                p.Add("@Id", subject.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_d_Subject>(PRC_SH_D_SUBJECTS_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task SH_d_SubjectsInsert(SH_d_Subject subject)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Insert
                var p = new DynamicParameters();
                p.Add("@Name", subject.Name);
                await db.QueryAsync(PRC_SH_D_SUBJECTS_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_d_SubjectsDeleteById(SH_d_Subject subject)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Delete
                var p = new DynamicParameters();
                p.Add("@Id", subject.Id);
                await db.QueryAsync(PRC_SH_D_SUBJECTS_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_d_SubjectsUpdate(SH_d_Subject subject)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Update
                var p = new DynamicParameters();
                p.Add("@Id", subject.Id);
                p.Add("@Name", subject.Name);
                await db.QueryAsync(PRC_SH_D_SUBJECTS_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<SH_d_Type>> SH_d_TypesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_d_Types
                var result = await db.QueryAsync<SH_d_Type>(PRC_SH_D_TYPES_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<SH_d_Country>> SH_d_CountriesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // SH_d_Countries
                var result = await db.QueryAsync<SH_d_Country>(PRC_SH_D_COUNTRIES_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<SH_d_Country> SH_d_CountrySelectById(SH_d_Country country)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get single country
                var p = new DynamicParameters();
                p.Add("@Id", country.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_d_Country>(PRC_SH_D_COUNTRY_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<SH_d_City>> SH_d_CitiesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get SH_d_Cities
                var result = await db.QueryAsync<SH_d_City>(PRC_SH_D_CITIES_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<SH_d_City> SH_d_CitySelectById(SH_d_City city)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get single country
                var p = new DynamicParameters();
                p.Add("@Id", city.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_d_City>(PRC_SH_D_CITY_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<SH_d_Class>> SH_d_ClassesSelectAll()
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get all classes
                var result = await db.QueryAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_ALL, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<SH_d_Class> SH_d_ClassesSelectById(SH_d_Class _class)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Get selected class
                var p = new DynamicParameters();
                p.Add("@Id", _class.Id);
                var result = await db.QuerySingleOrDefaultAsync<SH_d_Class>(PRC_SH_D_CLASSES_SELECT_BY_ID, p, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task SH_d_ClassesInsert(SH_d_Class _class)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Insert class
                var p = new DynamicParameters();
                p.Add("@Name", _class.Name);
                await db.QueryAsync(PRC_SH_D_CLASSES_INSERT, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_d_ClassesUpdate(SH_d_Class _class)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Update class
                var p = new DynamicParameters();
                p.Add("@Id", _class.Id);
                p.Add("@Name", _class.Name);
                await db.QueryAsync(PRC_SH_D_CLASSES_UPDATE, p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SH_d_ClassesDeleteById(SH_d_Class _class)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(SCHOOLS_DATABASE)))
            {
                db.Open();

                // Delete by id
                var p = new DynamicParameters();
                p.Add("@Id", _class.Id);
                await db.QueryAsync(PRC_SH_D_CLASSES_DELETE_BY_ID, p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
