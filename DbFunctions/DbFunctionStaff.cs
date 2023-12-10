using System.Data.SqlClient;

namespace Lab1_SQL.DbFunctions
{
    internal class DbFunctionStaff
    {
        static string connectionString = "Data Source=(localdb)\\.;Initial Catalog=School;Integrated Security=True";


        public static void AddStaff()
        {
            string sqlQueryListStaffCat = "select Category from StaffCategory";

            string sqlQueryGetCategoryId = "select Id from StaffCategory where Category = @Category";// OBS förenkling, jag gör inga kontroller på att namnet Category inte finns dubbelt för flera Id

            string sqlQueryInsertStaff = $"insert into Staff(FirstName, LastName, CategoryId)" +
                                $"values" +
                                $"(@FirstName, @LastName, @CategoryId)";



            Console.Clear();
            Console.WriteLine("Här kan du lägga till ny personal.");
            Console.Write("Ange förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Ange efternamn: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("------- Det finns följande personalkategorier i skolan: ");
            string category;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // visa listan med personalkategorier
                using (SqlCommand command = new SqlCommand(sqlQueryListStaffCat, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category = reader.GetString(reader.GetOrdinal("Category"));
                            Console.WriteLine($"{category}");
                        }
                    }
                }

                // hämta info om vilken kategori ska personen läggas till
                Console.Write($"------- \nVilken kategori tillhör {firstName} {lastName}? ");
                string input = Console.ReadLine();
                int categoryId = 0;

                using (SqlCommand command = new SqlCommand(sqlQueryGetCategoryId, connection))
                {
                    while (true)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Category", input);

                        var test = command.ExecuteScalar();
                        if (test is not null)
                        {
                            categoryId = (int)test;
                        }
                        if (categoryId != 0) break;
                        Console.Write("Du kan bara ange kategorier från listan ovan. Försök igen: ");
                        input = Console.ReadLine();
                    }
                }

                // lägg till personal
                using (SqlCommand command = new SqlCommand(sqlQueryInsertStaff, connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@CategoryId", categoryId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{firstName} {lastName} tillagd som {input}!");
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

        public static void GetStaffByCategory()
        {

            string sqlQueryListStaffCat = "select Category " +
                                          "from StaffCategory " +
                                          "group by Category " +
                                          "having Count(*) > 0";
            string sqlQueryGetCategoryId = "select Id from StaffCategory " +
                                           "where Category = @Category";//antagande att det bara blir ett träff


            int categoryId = 0;
            string category = "";

            Console.Clear();
            Console.WriteLine("------- Vill du se all personal eller inom en kategori? ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // command 1 - visa listan med personalkategorier
                using (SqlCommand command = new SqlCommand(sqlQueryListStaffCat, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category = reader.GetString(reader.GetOrdinal("Category"));
                            Console.WriteLine($"{category}");
                        }
                    }
                }

                // command 2 - hämta Id för vald category - eller "Alla"
                Console.WriteLine("------- ");
                Console.Write($"Skriv namn på kategori eller \"Alla\" för att se hela personallistan: ");
                string input = Console.ReadLine();

                if (input.ToUpper() != "ALLA") // alla kan gå direkt vidare i koden
                {
                    using (SqlCommand command = new SqlCommand(sqlQueryGetCategoryId, connection))
                    {

                        while (true)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Category", input);

                            var test = command.ExecuteScalar();
                            if (test is not null)
                            {
                                categoryId = (int)test;
                            }
                            if (categoryId != 0) break;
                            Console.Write("Du kan bara ange kategorier från listan ovan. Försök igen: ");
                            input = Console.ReadLine();
                        }

                    }
                    GetStaff(categoryId, input);
                }
                else
                {
                    GetStaff();
                }
            }
        }

        public static void GetStaff(int categoryId = 0, string category = "")
        {
            Console.Clear();

            // om metoden anropas med inparametrar så är det bara klassen som visas, annars allt
            string sqlChooseCategory = "";
            if (categoryId != 0)
            {
                category = " i kategori " + category;
                sqlChooseCategory = $"and CategoryId={categoryId} ";

            }
            string sqlQuery = $"select st.FirstName, st.LastName " +
                                $"from Staff st, StaffCategory sc " +
                                $"where st.CategoryId = sc.Id {sqlChooseCategory}" +
                                $"order by st.FirstName, st.LastName";

            Console.WriteLine($"------- All personal{category}: ");
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
    }
}
