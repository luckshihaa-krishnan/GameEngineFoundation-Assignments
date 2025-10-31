/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MidtermProject
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                // Window size of application
                Size = new Vector2i(1280, 720),

                // Title of application window
                Title = "Midterm Project - 3D Game Scene",

                // Flags needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var game = new Game(GameWindowSettings.Default, nativeWindowSettings))
                // Start the game 
                game.Run();
        }
    }
}
