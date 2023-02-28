using CSharp_Console_GameOfLife.Resources;
using static CSharp_Console_GOL.Menu;

namespace CSharp_Console_GOL
{
    internal static class Menu
    {
        #region Properties
        private static int FirstMenuLine { get; } = 9;
        #endregion

        #region Methods
        internal static void Show()
        {
            Console.WriteLine(Files.MenuBanner);

            List<Option> options = new()
        {
                new Option("Start", () => Game.Play()),
                new Option("How To Play", () => Util.HowToPlay()),
                new Option("Toggle Music", () => Util.ToggleMusic()),
                new Option("Change Music", () => Util.ChangeMusic()),
                new Option("Change Color", () => Util.ChangeColor()),
                new Option("Reset", () => Util.Reset()),
                new Option("Leave", () => Util.Exit()),
        };

            MainMenu(options);
        }

        internal static void MainMenu(List<Option> list)
        {
            int[] arrowPos = new int[list.Count];

            int line = FirstMenuLine; // The Line To Post Add Closing |
            Console.SetCursorPosition(0, line - 1); // Prepare Closing Position
            Console.WriteLine(new string('-', 36)); // Top Border

            int index = 0;

            foreach (Option option in list) // Output For Each Option Added Into The Options List
            {
                arrowPos[index] = 34 / 2 + (option.OptionName.Length / 2);
                Console.WriteLine("{0}{1," + arrowPos[index++] + "}", '|', option.OptionName); // Somewhat Centering The Name
                Console.SetCursorPosition(35, line); // Prepare Closing Position
                Console.Write('|'); // Closing Menu On Right Side
                Console.SetCursorPosition(0, ++line); // NL
            }
            Console.WriteLine(new string('-', 36)); // Bottom Border

            MenuSelection(arrowPos, list);

            static void MenuSelection(int[] arrowPos, List<Option> list)
            {
                ConsoleKey key; // Variable For The Condition And ReadKey
                int index = 0; // Index For Array And List
                int firstLine = FirstMenuLine; // Copy Value From Constant Field
                do
                {
                    Console.SetCursorPosition(arrowPos[index] - (3 + list[index].OptionName.Length), firstLine); // Setting Position For Menu Arrow
                    Console.Write("-->"); // Printing Menu Arrow

                    key = Console.ReadKey(true).Key; // Reading Input Key

                    /*
                     * Because We Either Enter A Method, Exit Or Move In The Menu We Can Always Execute The Arrow Deletion
                     */

                    Console.SetCursorPosition(arrowPos[index] - (3 + list[index].OptionName.Length), firstLine); // Preparing Arrow Deletion
                    Console.Write("   "); // Delete Arrow (The 🧀 Way)

                    firstLine = FirstMenuLine; // Resetting The Line Position So We Can Math The Index With It

                    switch (key)
                    {
                        case ConsoleKey.DownArrow:
                            index = (++index + arrowPos.Length) % arrowPos.Length; // Index Rollover
                            firstLine += index; // To Align Y / TOP Position Based On The Index To Allow Dynamic Changes
                            break;
                        case ConsoleKey.UpArrow:
                            index = (--index + arrowPos.Length) % arrowPos.Length; // Index Rollover
                            firstLine += index; // To Align Y / TOP Position Based On The Index To Allow Dynamic Changes
                            break;
                        case ConsoleKey.Escape:
                            Util.Exit(); // To Avoid Weird Escape Key Behavior
                            break;
                    }
                } while (key != ConsoleKey.Enter);
                Console.Clear();
                list[index].Selected();
            }
        }

        internal class Option // Custom Option DataType For The Menu List
        {
            internal string OptionName { get; }
            internal Action Selected { get; } // For Methods With No Parameters And Return Types () => CODE

            internal Option(string OptionName, Action Selected)
            {
                this.OptionName = OptionName; // Sign Name
                this.Selected = Selected; // And Function
            }
        }
        #endregion
    }
}
