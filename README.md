# Midterm Project - 3D Scene


## Description
This 3D game scene is built with OpenTK and C# in Visual Studio. This application is a small game scene which consist the user of being and navigating in the textured brick room using their keyboard, controlling light switch and viewing 3D objects.

## Gameplay Instructions
Here are the instructions to navigate and control the user in the application:
- In the start of the application, it will be pitch dark, which means that the light is off. To turn on the light, click on the key 'E' on your keyboard to turn on the light and see the room. You can click key 'E' again to turn off the light
- You can move the mouse key to look around in the room
- You can press key 'W' to move forward
- You can press key 'S' to move backward
- You can press key 'A' to move left
- You can press key 'D' to move right
- Once you're done with the application, you can press key 'ESC' (escape) to exit the game scene.

## Feature List
Here are some features that have been implemented in this application:
- Phong lighting using ambient, diffuse and specular
- Textured room using an image of a brick texture
- Camera system with keyboard and mouse control. This allows users to view the room using their keyboard inputs and mouse to move around
- 3D objects in the room such as a cube, pyramid and triangular prism, using solid colors
- Interaction to toggle the light switch

## How to build/run the project
The packages used in this application that needs to be installed through NuGet packages are OpenTK, StbImageSharp and System.Drawing.Common. Some dependancies used in the application are OpenTK.Graphics.OpenGL4 to create graphics, OpenTK.Mathematics for math tools, System.Drawing.Imaging to manage images such as the texture image, etc. To build and run the project on Visual Studio, there is a green play button that allows you to start the application

## Credits
Textured image for the room is from https://polyhaven.com/textures
