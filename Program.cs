using Lab1_SQL.DbFunctions;

namespace Lab1_SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                Console.WriteLine("Vad vill du göra?\n");
                Console.WriteLine("1 Hämta alla elever");
                Console.WriteLine("2 Hämta alla elever i en viss klass");
                Console.WriteLine("3 Lägga till ny personal");
                Console.WriteLine("4 Hämta personal");
                Console.WriteLine("5 Hämta alla betyg som satts den senaste månaden");
                Console.WriteLine("6 Snittbetyg per kurs");
                Console.WriteLine("7 Lägga till nya elever");
                Console.WriteLine("\nX för att avsluta för alltid");
                Console.Write("\nAnge ditt val: ");
                string input = Console.ReadLine();


                switch (input)
                {
                    case "1":
                        DbFunctionStudents.GetStudents();
                        break;
                    case "2":
                        DbFunctionStudents.GetStudentsByClass();
                        break;
                    case "3":
                        DbFunctionStaff.AddStaff();
                        break;
                    case "4":
                        DbFunctionStaff.GetStaffByCategory();
                        break;
                    case "5":
                        DbFunctionGrades.GetLastMonthGrades();
                        break;
                    case "6":
                        DbFunctionGrades.GetGradeStats();
                        break;
                    case "7":
                        DbFunctionStudents.AddStudent();
                        break;
                    case "x":
                    case "X":
                        exit = true;
                        break;
                    default:
                        Console.Write("Ogiltigt val, försök igen! ");
                        break;
                }
            }


        }
    }
}
