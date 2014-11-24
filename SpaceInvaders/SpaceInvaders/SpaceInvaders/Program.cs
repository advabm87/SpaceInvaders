using System;

namespace SpaceInvaders
{
#if WINDOWS || XBOX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            using (SpaceInvaders game = new SpaceInvaders())
            {
                game.Run();
            }
        }
    }
#endif
}
