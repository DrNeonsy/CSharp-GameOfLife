namespace CSharp_Console_GOL
{
    internal class GameField
    {
        #region BackingFields
        private int height;
        private int width;
        #endregion

        #region Properties
        internal int Height
        {
            get { return height; }
            init
            {
                if (value <= 44 && value >= 5)
                {
                    height = value;
                }
                else if (value < 5)
                {
                    height = 5;
                }
                else
                {
                    height = 44;
                }
            }
        }
        internal int Width
        {
            get { return width; }
            init
            {
                if (value <= 175 && value >= 5)
                {
                    width = value;
                }
                else if (value < 5)
                {
                    width = 5;
                }
                else
                {
                    width = 175;
                }
            }
        }
        #endregion

        #region Constructor
        internal GameField()
        {

        }
        #endregion

        #region Methods
        internal void RenderField(bool clear = true)
        {
            if (clear)
            {
                Console.Clear();
            }
            HorizontalBorder();

            for (int y = 0; y < Height-2; y++)
            {
                Console.Write('|');
                for (int x = 0; x < Width - 2; x++)
                {
                    if (Game.XPos == x && Game.YPos == y)
                    {
                        Console.BackgroundColor = Util.bg;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    if (Game.cells[y, x].CurrentState)
                    {
                        Console.ForegroundColor = Util.fg;
                        Console.Write('x');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine('|');
            }

            HorizontalBorder(newLine: false);

            void HorizontalBorder(bool newLine = true)
            {
                Console.Write(new string('-', Width));
                if (newLine)
                {
                    Console.WriteLine();
                }
            }
        }
        #endregion
    }
}
