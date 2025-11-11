/*
* Name: Luckshihaa Krishnan 
* Student ID: 186418216
* Section: GAM 531 NSA 
*/

using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTK_Sprite_Animation
{
    public static class Program
    {
        public static void Main() 
        { 
            var nativeWindowSettings = new NativeWindowSettings()
            {
                // Window size of application
                Size = new Vector2i(800, 600),

                // Title of application window
                Title = "Sprite Game",

                // Flags needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var window = new SpriteAnimationGame(GameWindowSettings.Default, nativeWindowSettings))
            {
                // Start the game 
                window.Run();
            }
        }
    }
}
