using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10_SQL_ORM
{
    internal class Menu
    {
        public static int ShowMenu(IEnumerable<string> menuOptions, int nrOfRowsPerOption, string title, string optionsADX)
        {

            // Menu with alternatives sent in the list/array
            // you can choose one of the options in the list and get the position back
            // or press A and get max positions +1
            // or press D and get max positions +2
            // or press X and get max positions +3
            // titles for the menu and extra options are also parameters to ShowMenu

            // i needed to hardcode nrOfRowsPerOption to be able to navigate if options have multiple rows, this time.
            // next time i would need to solve it differently, attach the navigation to the printed out options somehow?

            int listLength = menuOptions.Count();

            // if more than 15 rows, use multiple pages
            int maxRowsPerPage = 15;
            int maxOptionsPerPage = (int)Math.Floor((double)maxRowsPerPage / nrOfRowsPerOption);
            int numberOfPages = (int)(Math.Ceiling((double)listLength / maxOptionsPerPage));
            IEnumerable<string> partialMenuOptions;
            int amountOptionsOnThisPage = default;
            int pageNumber = 1;

            while (true)
            {
                Console.Clear();

                Console.WriteLine(title);

                amountOptionsOnThisPage = numberOfPages > pageNumber ? maxOptionsPerPage : ((int)(double)listLength - 1) % maxOptionsPerPage + 1;

                partialMenuOptions = menuOptions
                    .Skip((pageNumber - 1) * maxOptionsPerPage)
                    .Take(amountOptionsOnThisPage);


                foreach (string option in partialMenuOptions)
                {
                    Console.WriteLine($"   {option}");
                }
                Console.WriteLine("");
                if (numberOfPages != 1)
                {
                    Console.WriteLine($"Page {pageNumber} of {numberOfPages}. Press right or left to see more alternatives.\n");
                }
                Console.WriteLine(optionsADX);

                Console.CursorVisible = false;
                int startTop = 1;
                int top = startTop;

                int choice = 0;

                // moving around the arrow and getting the choice
                bool moveAroundAlternatives = true;
                while (moveAroundAlternatives)
                {
                    Console.SetCursorPosition(0, top);
                    Console.WriteLine("=> ");

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            choice = choice > 0 ? choice - 1 : amountOptionsOnThisPage - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            choice = choice < amountOptionsOnThisPage - 1 ? choice + 1 : 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            pageNumber = pageNumber == 1 ? numberOfPages : pageNumber - 1;
                            choice = 0;
                            moveAroundAlternatives = false;
                            break;
                        case ConsoleKey.RightArrow:
                            pageNumber = pageNumber % numberOfPages + 1;
                            choice = 0;
                            moveAroundAlternatives = false;
                            break;
                        case ConsoleKey.Enter:
                            return choice + (pageNumber - 1) * maxOptionsPerPage;
                        case ConsoleKey.A:
                            return listLength;
                        case ConsoleKey.D:
                            return listLength + 1;
                        case ConsoleKey.X:
                            return listLength + 2;
                        default:
                            break;
                    }
                    Console.SetCursorPosition(0, top);
                    Console.WriteLine("   ");
                    top = startTop + choice * nrOfRowsPerOption;

                }
            }
        }
    }
}
