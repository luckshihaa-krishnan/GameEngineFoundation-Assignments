/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using Assignment9;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace Assignment9
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
                Title = "Assignment 9 - Collisions",

                // Flags needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var game = new Game(GameWindowSettings.Default, nativeWindowSettings))
                // Start the game 
                game.Run();
        }
    }
}