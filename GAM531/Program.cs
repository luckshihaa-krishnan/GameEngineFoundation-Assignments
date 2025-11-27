using System;
using GAM531;

namespace GAM531
{
    //Main entry point for c# console
    class Program
    {
        static void Main(String[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}