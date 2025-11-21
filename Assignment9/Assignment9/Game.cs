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
using System.Drawing.Text;

namespace Assignment9
{
    public class Game : GameWindow
    {

        private Shader _shader;
        private Texture _roomTexture;
        private GameObject _room;
        private Camera _camera;
        private Vector2 _lastMousePosition;
        private bool _firstMove = true;
        private List<GameObject> _objects = new List<GameObject>();


        // Constructor for Game class
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        // Function when the game is loaded
        protected override void OnLoad()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Get shader from files
            _shader = new Shader("Shaders/vertex.glsl", "Shaders/fragment.glsl");

            // Get texture from file
            _roomTexture = new Texture("Assets/brick_wall.jpg");
            _shader.Use();

            _camera = new Camera(new Vector3(0.0f, 1.0f, 3.0f), Size.X / (float)Size.Y);   
            _room = new GameObject(Mesh.CreateRoom(), Vector3.Zero, Vector3.One, Vector3.One);


            // Create objects and store in _objects to check for collisions
            _objects.Add(new GameObject(Mesh.CreateCube(), new Vector3(-2, 0.2f, -2), new Vector3(1.2f, 1.2f, 1.2f), new Vector3(1, 0, 0)));
            _objects.Add(new GameObject(Mesh.CreateTriangularPrism(), new Vector3(2, 0.2f, 1), Vector3.One, new Vector3(0, 0, 1)));
            _objects.Add(new GameObject(Mesh.CreatePyramid(), new Vector3(0, 0.2f, 3), Vector3.One, new Vector3(0, 1, 0)));
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

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;
            float speed = ((float)e.Time) * 2;

            // Current camera position
            Vector3 newPos = _camera.Position;
            Vector3 originalPos = newPos; 
            Vector3 move = Vector3.Zero;

            // Movement input
            if (input.IsKeyDown(Keys.W))
            {
                move += _camera.Front * speed;
            }

            if (input.IsKeyDown(Keys.S))
            {
                move -= _camera.Front * speed;
            }

            if (input.IsKeyDown(Keys.A))
            {
                move -= _camera.Right * speed;
            }

            if (input.IsKeyDown(Keys.D))
            {
                move += _camera.Right * speed;
            }


            // X axis movement - moving camera, checking for collisions to make sure camera is in room and does not pass through objects
            newPos.X += move.X;
            newPos.X = MathHelper.Clamp(newPos.X, -8f, 8f);
            if (_objects.Exists(obj => Collision.CheckCollision(newPos, obj)))
            {
                newPos.X = originalPos.X;
            }

            // Y axis movement - moving camera, checking for collisions to make sure camera is in room and does not pass through objects
            newPos.Y += move.Y;
            newPos.Y = MathHelper.Clamp(newPos.Y, 0.2f, 5f);
            if (_objects.Exists(obj => Collision.CheckCollision(newPos, obj)))
            {
                newPos.Y = originalPos.Y;
            }

            // Z axis movement - moving camera, checking for collisions to make sure camera is in room and does not pass through objects
            newPos.Z += move.Z;
            newPos.Z = MathHelper.Clamp(newPos.Z, -8f, 8f);
            if (_objects.Exists(obj => Collision.CheckCollision(newPos, obj)))
            {
                newPos.Z = originalPos.Z;
            }

            // Update camera position
            _camera.Position = newPos;

            // Mouse movement input
            var mouse = MouseState;

            // If mouse off the first movement, set last mouse position to current position
            if (_firstMove)
            {
                _lastMousePosition = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }

            // Otherwise, calculate the delta and update yaw/pitch
            else
            {
                var deltaX = mouse.X - _lastMousePosition.X;
                var deltaY = mouse.Y - _lastMousePosition.Y;
                _lastMousePosition = new Vector2(mouse.X, mouse.Y);

                _camera._yaw += deltaX * 0.2f;
                _camera._pitch -= deltaY * 0.2f;
                _camera._pitch = MathHelper.Clamp(_camera._pitch, -89f, 89f);
            }

            // Update camera vectors based on new yaw/pitch
            _camera.UpdateVectors();
        }


        // Function to render frame
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _shader.Use();


            _shader.SetMatrix4("uView", _camera.GetViewMatrix());
            _shader.SetMatrix4("uProj", _camera.GetProjectionMatrix());

            // texture for room
            _shader.SetBool("hasTexture", true);
            _roomTexture.UseTexture();

            // Render room
            _room.Render(_shader);

            // no texture for objects
            _shader.SetBool("hasTexture", false);

            // Render each object in the list
            foreach (var i in _objects)
            {
                i.Render(_shader);
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _shader.Dispose();
        }
    }
}
