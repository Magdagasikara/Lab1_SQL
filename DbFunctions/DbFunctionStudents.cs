using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Lab1_SQL.Models;

namespace Lab1_SQL.DbFunctions
{
    internal static class DbFunctionStudents
    {
        static string connectionString = "Data Source=(localdb)\\.;Initial Catalog=School;Integrated Security=True";


        public static void GetStudentsByClass()
        {

            string sqlQuery = $"select Id, ClassName " +
                                $"from Class";

            int classId = 0;
            string className = "";

            Console.Clear();
            Console.WriteLine("------- Det finns följande klasser i skolan: ");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            className = reader.GetString(reader.GetOrdinal("ClassName"));
                            Console.WriteLine($"{className}");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    Console.WriteLine("------- ");
                    Console.WriteLine("Ange ett klassnamn för att se alla elever i den: ");

                    while (true)
                    {
                        string input = Console.ReadLine();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                className = reader.GetString(reader.GetOrdinal("ClassName"));
                                if (input.Trim() == className.Trim())
                                {
                                    classId = reader.GetInt32(reader.GetOrdinal("Id"));
                                    className = input;
                                    break;
                                }
                            }

                        }
                        if (classId != 0) break;
                        Console.WriteLine("Du kan bara ange klasser från listan ovan. Försök igen: ");
                    }
                }
            }

            GetStudents(classId, className);

        }

        public static void GetStudents(int classId = 0, string className = "")
        {
            Console.Clear();

            Console.Write("Vill du se eleverna sorterade stigande (S) eller fallande (F)? ");
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (input.ToUpper() == "S" || input.ToUpper() == "F") break;
                Console.Write("Fel input. Ange S för stigande eller F för fallande: ");
            }
            string sort = "";
            if (input.ToUpper() == "F")
            {
                sort = "desc";
            }

            // om metoden anropas med inparametrar så är det bara klassen som visas, annars allt
            string sqlChooseClass = "";
            if (classId != 0)
            {
                className = " i klassen " + className;
                sqlChooseClass = $"and ClassId={classId} ";

            }

            string sqlQuery = $"select s.FirstName, s.LastName, c.ClassName " +
                                $"from Students s, Class c " +
                                $"where s.ClassId = c.Id {sqlChooseClass}" +
                                $"order by s.FirstName {sort}, s.LastName {sort}";

            Console.WriteLine($"------- Alla elever{className}: ");
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
                            Console.WriteLine($"{firstName} {lastName}");
                        }
                    }
                }
            }
            Console.WriteLine("------- ");
            Console.Write("Tryck Enter för att komma tillbaka till menyn ");
            Console.ReadKey();
        }

        public static void AddStudent()
        {
            string sqlQueryListClasses = "select ClassName from Class";

            string sqlQueryGetClassId = "select Id from Class where ClassName = @ClassName";// OBS förenkling, antar inga dubbletter på namnen

            string sqlQueryInsertStudent = $"insert into Students(FirstName, LastName, ClassId)" +
                                $"values" +
                                $"(@FirstName, @LastName, @ClassId)";



            Console.Clear();
            Console.WriteLine("Här kan du lägga till en ny student.");
            Console.Write("Ange förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Ange efternamn: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("------- Det finns dessa klasser i skolan: ");
            string className;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // visa listan med klasser
                using (SqlCommand command = new SqlCommand(sqlQueryListClasses, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            className = reader.GetString(reader.GetOrdinal("ClassName"));
                            Console.WriteLine($"{className}");
                        }
                    }
                }

                // hämta info om vilken klass ska studenten läggas till
                Console.Write($"------- \nI vilken klass går {firstName} {lastName}? ");
                string input = Console.ReadLine();
                int classId = 0;

                using (SqlCommand command = new SqlCommand(sqlQueryGetClassId, connection))
                {
                    while (true)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@ClassName", input);

                        var test = command.ExecuteScalar();
                        if (test is not null)
                        {
                            classId = (int)test;
                        }
                        if (classId != 0) break;
                        Console.Write("Du kan bara ange klassnamn från listan ovan. Försök igen: ");
                        input = Console.ReadLine();
                    }

                }

                // lägg till studenten
                using (SqlCommand command = new SqlCommand(sqlQueryInsertStudent, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@ClassId", classId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{firstName} {lastName} tillagd i klass {input}!");
                    }
                    else
                    {
                        Console.WriteLine($"Det gick inte att lägga till {firstName} {lastName}.");
                    }
                    Console.WriteLine("------- ");
                    Console.Write("Tryck Enter för att komma tillbaka till menyn");
                    Console.ReadKey();
                }
            }

        }

    }
}
