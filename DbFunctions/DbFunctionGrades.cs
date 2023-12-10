using ConsoleTables;
using System.Data.SqlClient;

namespace Lab1_SQL.DbFunctions
{
    internal class DbFunctionGrades
    {
        static string connectionString = "Data Source=(localdb)\\.;Initial Catalog=School;Integrated Security=True";

        public static void GetLastMonthGrades()
        {
            string sqlQuery = $"select " +
                                "s.FirstName, " +
                                "s.LastName, " +
                                "c.CourseName, " +
                                "g.Grade, " +
                                "g.GradeDate " +
                            "from Students s " +
                                "left join ClassCourse cc on s.ClassID = cc.ClassId " +
                                "left join Course c on c.Id = cc.CourseId " +
                                "left join Grades g on g.CourseId = c.Id and g.StudentId = s.Id " +
                            "where g.GradeDate between DATEADD(month, -1, convert(date,getdate())) and convert(date,GETDATE()) " +
                            "order by " +
                                "s.FirstName, " +
                                "s.LastName";

            Console.Clear();
            Console.WriteLine("------- Alla betyg som sattes den senaste månaden: ");
            var table = new ConsoleTable("Name", "Kurs", "Grade", "Datum");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                            int grade = reader.GetInt32(reader.GetOrdinal("Grade"));
                            DateTime gradeDate = reader.GetDateTime(reader.GetOrdinal("GradeDate"));
                            table.AddRow($"{firstName} {lastName}", courseName, grade, gradeDate.ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }

            table.Write();
            Console.WriteLine("------- ");
            Console.Write("Tryck Enter för att komma tillbaka till menyn ");
            Console.ReadKey();
        }

        public static void GetGradeStats()
        {
            string sqlQuery = $"select " +
                                "c.CourseName, " +
                                "count(g.Grade) as NumberOfGrades, " +
                                "avg(convert(decimal, g.Grade)) as MeanGrade, " +
                                "min(g.Grade) as MinGrade, " +
                                "max(g.Grade) as MaxGrade " +
                              "from Course c " +
                                 "left join Grades g on g.CourseId = c.Id " +
                              "group by " +
                                 "c.CourseName";

            Console.Clear();
            Console.WriteLine("------- Betygsstatistiken per kurs: ");
            var table = new ConsoleTable("Kurs", "Antal betyg", "Medelbetyg", "Lägsta betyg", "Högsta betyg");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                            int numberOfGrades = reader.GetInt32(reader.GetOrdinal("NumberOfGrades"));
                            decimal meanGrade= reader.GetDecimal(reader.GetOrdinal("MeanGrade"));
                            int minGrade = reader.GetInt32(reader.GetOrdinal("MinGrade"));
                            int maxGrade = reader.GetInt32(reader.GetOrdinal("MaxGrade"));
                            table.AddRow(courseName, numberOfGrades, $"{meanGrade:#.##}", minGrade, maxGrade);
                        }
                    }
                }
            }
            table.Write();
            Console.WriteLine("------- ");
            Console.Write("Tryck Enter för att komma tillbaka till menyn ");
            Console.ReadKey();

        }
    }
}
