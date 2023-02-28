using CSharp_Console_GameOfLife.Resources;

namespace CSharp_Console_GOL
{
    internal class EntryPoint
    {
        static void Main(string[] args)
        {
            Util.Maximize(); // Maximizing Console On Startup
            Console.CursorVisible = false;

            #region Intro
            Console.WriteLine(Files.Intro1 + Environment.NewLine);
            Thread.Sleep(555);

            Console.WriteLine(Files.Intro2 + Environment.NewLine);
            Thread.Sleep(555);

            Console.WriteLine(Files.Intro3 + Environment.NewLine);
            Thread.Sleep(999);
            #endregion

            Util.SongSelector(Settings.Default.songNR);

            Util.ColorSelector(Settings.Default.highlightColor, type: true, normal: false);
            Util.ColorSelector(Settings.Default.cellColor, type: false, normal: false);

            while (true)
            {
                Console.Clear();
                Menu.Show(); // Calling All The Menu Functionalities
            }
        }
    }
}