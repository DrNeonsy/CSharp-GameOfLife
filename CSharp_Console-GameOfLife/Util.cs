using CSharp_Console_GameOfLife.Resources;
using System.Runtime.InteropServices;

namespace CSharp_Console_GOL
{
    internal partial class Util
    {
        private static readonly List<Menu.Option> SongMenu = new()
        {
            new Menu.Option("DreamX", () => SongSelector(0)),
            new Menu.Option("SpaceX", () => SongSelector(1)),
            new Menu.Option("VikinX", () => SongSelector(2)),
        };

        private static readonly List<Menu.Option> ColorMenu = new()
        {
            new Menu.Option("DarkMagenta", () => ColorSelector(0, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("Blue", () => ColorSelector(1, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("Cyan", () => ColorSelector(2, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("DarkYellow", () => ColorSelector(3, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("DarkRed", () => ColorSelector(4, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("Yellow", () => ColorSelector(5, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("Green", () => ColorSelector(6, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
            new Menu.Option("White", () => ColorSelector(7, Decision("Do You Wish To Change The (H)ighlight Or (C)ell Color?", ConsoleKey.H, ConsoleKey.C))),
        };

        static readonly System.Media.SoundPlayer player = new();
        private static GameField GameField { get; set; } = new();

        internal static ConsoleColor fg;
        internal static ConsoleColor bg;

        #region Code To Maximize Console
        [LibraryImport("kernel32.dll")]
        private static partial IntPtr GetConsoleWindow();
        private static readonly IntPtr ThisConsole = GetConsoleWindow();
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        #endregion

        /// <summary>
        /// Call To Maximize Console
        /// </summary>
        internal static void Maximize()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }

        /// <summary>
        /// Prompts The User To Press One Of Two Keys
        /// </summary>
        /// <param name="msg">The Message You Wish To Display</param>
        /// <param name="option1">Key 1</param>
        /// <param name="option2">Key 2</param>
        /// <param name="banner">Optional String Banner</param>
        /// <returns>True If Key 1 Has Been Pressed Otherwise False</returns>
        internal static bool Decision(string msg, ConsoleKey option1 = ConsoleKey.Y, ConsoleKey option2 = ConsoleKey.N)
        {
            ConsoleKey ckey;
            Console.Write(msg + $" ( {option1} | {option2} )");

            do
            {
                ckey = Console.ReadKey(true).Key;
            } while (ckey != option1 && ckey != option2);

            Console.Clear();

            return ckey == option1;
        }

        internal static void ToggleMusic()
        {
            if (Settings.Default.musicOn)
            {
                player.Stop();
                Settings.Default.musicOn = false;
                Settings.Default.Save();
            }
            else
            {
                player.PlayLooping();
                Settings.Default.musicOn = true;
                Settings.Default.Save();
            }
        }

        internal static void ChangeMusic()
        {
            Console.Clear();
            Console.WriteLine(Files.Songs);

            Menu.MainMenu(SongMenu);
        }

        internal static void SongSelector(int input)
        {
            if (Settings.Default.musicOn)
            {
                player.Stop();
            }

            switch (input)
            {
                case 0:
                    player.Stream = Files.Daydream;
                    Settings.Default.songNR = 0;
                    break;
                case 1:
                    player.Stream = Files.Space;
                    Settings.Default.songNR = 1;
                    break;
                case 2:
                    player.Stream = Files.Viking;
                    Settings.Default.songNR = 2;
                    break;
            }

            if (Settings.Default.musicOn)
            {
                player.PlayLooping();
            }

            Settings.Default.Save();
        }

        internal static void ChangeColor()
        {
            Console.Clear();
            Console.WriteLine(Files.Color);

            Menu.MainMenu(ColorMenu);
        }

        internal static void ColorSelector(int mode, bool type, bool normal = true)
        {
            ConsoleColor cc = ConsoleColor.Black;
            switch (mode)
            {
                case 0:
                    cc = ConsoleColor.DarkMagenta;
                    break;
                case 1:
                    cc = ConsoleColor.Blue;
                    break;
                case 2:
                    cc = ConsoleColor.Cyan;
                    break;
                case 3:
                    cc = ConsoleColor.DarkYellow;
                    break;
                case 4:
                    cc = ConsoleColor.DarkRed;
                    break;
                case 5:
                    cc = ConsoleColor.Yellow;
                    break;
                case 6:
                    cc = ConsoleColor.Green;
                    break;
                case 7:
                    cc = ConsoleColor.White;
                    break;
            }

            if (type)
            {
                Settings.Default.highlightColor = (ushort)mode;
                bg = cc;

            }
            else
            {
                Settings.Default.cellColor = (ushort)mode;
                fg = cc;
            }

            Settings.Default.Save();

            if (normal) { Previet(); }

            static void Previet()
            {
                Console.Clear();
                Console.BackgroundColor = bg;
                Console.ForegroundColor = fg;
                Console.WriteLine(Files.Preview);
                Thread.Sleep(1800);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal static void HowToPlay()
        {

            Console.WriteLine(Files.Rules);
            Console.WriteLine("""

                   A living Cell dies if it has less than two or more than three neighbors.
                   A dead Cell will come alive if it has exactly three neighbors.

                   Press any Key to go to the next Page.
                   """);

            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(Files.Controls + Environment.NewLine);

            DemoField();

            string[] ctrs = new string[] { "Use the Arrow Keys to Navigate through the Game Field.",
                "Press the Space Bar to change the State of the Cells -\\|/-",
                "Then use the Enter Key to start the Next Generation!!!!!  ",
                "You can use the Escape Key to return to the Main Menu :)  "};

            Console.WriteLine(Environment.NewLine);

            for (int i = 0; i < ctrs.Length; i++)
            {
                var pos = Console.GetCursorPosition();

                Console.Write("\r" + ctrs[i]);
                switch (i)
                {
                    case 0:
                        Console.SetCursorPosition(1, 8);
                        Thread.Sleep(1800);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(' ');
                        Console.SetCursorPosition(6, 10);
                        Console.BackgroundColor = bg;
                        Console.Write(' ');
                        break;
                    case 1:
                        Console.SetCursorPosition(6, 10);
                        Console.ForegroundColor = fg;
                        Console.BackgroundColor = bg;
                        Thread.Sleep(1800);
                        Console.Write('x');
                        break;
                    case 2:
                        Console.SetCursorPosition(6, 10);
                        Console.BackgroundColor = bg;
                        Thread.Sleep(1800);
                        Console.Write(' ');
                        break;
                    case 3:
                        Console.SetCursorPosition(6, 10);
                        Console.BackgroundColor= ConsoleColor.Black;
                        Thread.Sleep(1800);
                        Console.Write(' ');
                        break;
                }
                Console.SetCursorPosition(pos.Left, pos.Top);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }

            static void DemoField()
            {
                GameField = new()
                {
                    Height = 7,
                    Width = 14
                };
                Game.CellsCreation(GameField);
                GameField.RenderField(clear: false);

            }
        }

        internal static void Reset()
        {
            Settings.Default.Reset();
            Settings.Default.Save();

            ColorSelector(Settings.Default.highlightColor, type: true, normal: false);
            ColorSelector(Settings.Default.cellColor, type: false, normal: false);
            SongSelector(Settings.Default.songNR);
        }

        internal static void Exit()
        {
            Console.Clear();
            Settings.Default.Save();
            Environment.Exit(0);
        }
    }
}
