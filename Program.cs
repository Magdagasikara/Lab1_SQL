using Lab1_SQL.DbFunctions;

namespace Lab1_SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DbFunctions.Do1();

            // OBS efter scaffold står det "Grade1" ist för Grade,why? säkert får inte property heta som class
            // scaffold-DbContext "Data Source=(localdb)\.;Initial Catalog=School;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models -ContextDir Data -Context SchoolContext
        
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
                Console.WriteLine("X för att avsluta för alltid");
                Console.Write("\nAnge ditt val: ");
                string input = Console.ReadLine();


                switch (input)
                {
                    case "1":
                        // hämta alla elever
                        DbFunctionStudents.GetStudents();
                        break;
                    case "2":
                        //hämta alla elever i en viss klass
                        DbFunctionStudents.GetStudentsByClass();
                        break;
                    case "3":
                        // lägga till ny personal
                        DbFunctionStaff.AddStaff();
                        break;
                    case "4":
                        // hämta personal
                        DbFunctionStaff.GetStaffByCategory();
                        break;
                    case "5":
                        // hämta alla betyg som sattes senaste månaden
                        DbFunctionGrades.GetLastMonthsGrades();
                        break;
                    case "6":
                        //snittbetyg per kurs
                        DbFunctionGrades.GetGradeStats();
                        break;
                    case "7":
                        //lägga tll nya elever
                        break;
                    case "x":
                    case "X":
                        exit = true;
                        Console.WriteLine("Hejdå!");
                        break;
                    default:
                        Console.WriteLine("wrong input, try again!");
                        break;
                }
            }


        }
    }
}
