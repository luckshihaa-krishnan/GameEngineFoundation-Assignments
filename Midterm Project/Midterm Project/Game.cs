/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using MidtermProject;
using System.Drawing.Text;
namespace MidtermProject
{
    public class Game : GameWindow
    {
        // Creating variables for shader, texture, meshes, camera and light state
        private Shader _shader;
        private Texture _roomTexture;
        private Mesh _roomMesh;
        private Mesh _cubeMesh;
        private Mesh _pyramidMesh;
        private Mesh _triangularPrismMesh;
        private Camera _camera;
        private bool _lightOn = false;

        // Constructor for Game class
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        //Function when the game is loaded
        protected override void OnLoad()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Get shader from files
            _shader = new Shader("Shaders/vertex.glsl", "Shaders/fragment.glsl");

            // Ger texture from file
            _roomTexture = new Texture("Assets/brick_wall.jpg");

            // Create meshes from Mesh.cs
            _roomMesh = Mesh.CreateRoom();
            _cubeMesh = Mesh.CreateCube();
            _pyramidMesh = Mesh.CreatePyramid();
            _triangularPrismMesh = Mesh.CreateTriangularPrism();

            // Initialize camera
            _camera = new Camera(new Vector3(0.0f, 1.0f, 3.0f), Size.X / (float)Size.Y);

            // Initialize mouse position
            _camera.OnMouseMove(0.0f, 0.0f);
            CursorState = CursorState.Grabbed;
        }

        // Function to render frame
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _shader.Use();

            // Set shader
            _shader.SetMatrix4("uView", _camera.GetViewMatrix());
            _shader.SetMatrix4("uProj", _camera.GetProjectionMatrix());
            _shader.SetVector3("viewPos", _camera.Position);

            // Set light 
            _shader.SetVector3("light.lightPos", new Vector3(0.0f, 2.0f, 0.0f));
            _shader.SetVector3("light.lightAmbient", new Vector3(0.1f, 0.1f, 0.1f));
            _shader.SetVector3("light.lightDiffuse", new Vector3(1.0f, 1.0f, 1.0f));
            _shader.SetVector3("light.lightSpecular", new Vector3(1.0f, 1.0f, 1.0f));

            // If light is on, set it to 1, else 0
            if (_lightOn)
            {
                _shader.SetInt("lightOn", 1);
            }
            else
            {
                _shader.SetInt("lightOn", 0);
            }

            // Render room with texture
            _shader.SetInt("tex", 1);
            _shader.SetMatrix4("uModel", Matrix4.Identity);
            _roomMesh.DisplayTexturedShapes(_roomTexture);

            // Render cube with solid color
            _shader.SetInt("tex", 0);
            _shader.SetVector3("objectColor", new Vector3(1.0f, 0.0f, 0.0f));
            Matrix4 cubeModel = Matrix4.CreateTranslation(-1.0f, 0.5f, -1.0f);
            _shader.SetMatrix4("uModel", cubeModel);
            _cubeMesh.DisplayShapes();

            // Render pyramid with solid color
            _shader.SetInt("tex", 0);
            _shader.SetVector3("objectColor", new Vector3(0.0f, 1.0f, 0.0f));
            Matrix4 pyramidModel = Matrix4.CreateTranslation(1.0f, 0.5f, -1.0f);
            _shader.SetMatrix4("uModel", pyramidModel);
            _pyramidMesh.DisplayShapes();

            // Render triangular prism with solid color
            _shader.SetInt("tex", 0);
            _shader.SetVector3("objectColor", new Vector3(0.0f, 0.0f, 1.0f));
            Matrix4 prismModel = Matrix4.CreateTranslation(0.0f, 0.5f, 1.0f);
            _shader.SetMatrix4("uModel", prismModel);
            _triangularPrismMesh.DisplayShapes();

            SwapBuffers();
        }

        // Function to update frame
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;
            float time = (float)e.Time;
            const float _cameraSpeed = 4.5f;
            const float _sensitivity = 0.2f;

            // Temporary position vector when movement keys are pressed
            Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

            // If escape is pressed, close the game
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // Toggle light on and off when E is pressed
            if (input.IsKeyPressed(Keys.E))
            {
                if(_lightOn == false)
                {
                    _lightOn = true;
                }
                else
                {
                    _lightOn = false;
                }
            }

            // If W key is pressed, move forward
            if (input.IsKeyDown(Keys.W))
            {
                pos = new Vector3(0.0f, 0.0f, 1.0f);
                _camera.MoveCamera(pos, _cameraSpeed * time);
            }

            // If S key is pressed, move backward
            if (input.IsKeyDown(Keys.S))
            {
                pos = new Vector3(0.0f, 0.0f, -1.0f);
                _camera.MoveCamera(pos, _cameraSpeed * time);
            }

            // If A key is pressed, move left
            if (input.IsKeyDown(Keys.A))
            {
                pos = new Vector3(-1.0f, 0.0f, 0.0f);
                _camera.MoveCamera(pos, _cameraSpeed * time);
            }

            // If D key is pressed, move right
            if (input.IsKeyDown(Keys.D))
            {
                pos = new Vector3(1.0f, 0.0f, 0.0f);
                _camera.MoveCamera(pos, _cameraSpeed * time);
            }

            var delta = MouseState.Delta * _sensitivity;
            _camera.OnMouseMove(delta.X, delta.Y);
        }

        // Function to handle window resize
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            if (_camera != null)
            {
                _camera.AspectRatio = Size.X / (float)Size.Y;
            }
        }

    }
}
