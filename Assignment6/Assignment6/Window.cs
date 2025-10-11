/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */

using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace Assignment6
{
    public class Window : GameWindow
    {

        private int _vbo;
        private int _vao;
        private Shader _shader;

        private int modelLoc;
        private int viewLoc;
        private int projLoc;

        private int lightPosition;
        private int viewPosition;
        private int lightColorPosition;
        private int objectColorPosition;

        private int _normalMatrixLocation;
        private Vector3 lightStartingPos = new Vector3(1.2f, 1.0f, 2.0f);
        private readonly Vector3 lightColor = new Vector3(1.0f, 1.0f, 1.0f);
        private readonly Vector3 objectColor = new Vector3(0.8f, 0.2f, 0.2f);
        private Vector3 newPosition; 


        // EXERCISE #1 - defining camera
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private double _time;


        // Cube vertices (positions + texture coordinates)
        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(new Color4(0.4f, 0.2f, 0.5f, 1f));
            GL.Enable(EnableCap.DepthTest);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = _shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            modelLoc = GL.GetUniformLocation(_shader.Handle, "model");
            viewLoc = GL.GetUniformLocation(_shader.Handle, "view");
            projLoc = GL.GetUniformLocation(_shader.Handle, "projection");

            lightPosition = GL.GetUniformLocation(_shader.Handle, "lightPos");
            viewPosition = GL.GetUniformLocation(_shader.Handle, "viewPos");
            lightColorPosition = GL.GetUniformLocation(_shader.Handle, "lightColor");
            objectColorPosition = GL.GetUniformLocation(_shader.Handle, "objectColor");
            _normalMatrixLocation = GL.GetUniformLocation(_shader.Handle, "normalMatrix");

            _camera = new Camera(new Vector3(0, 0, 3), Size.X / (float)Size.Y);
            newPosition = lightStartingPos - _camera.Position;
            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vao);
            _shader.Use();

            Vector3 currentLightPos = _camera.Position + newPosition;

            GL.Uniform3(lightPosition, currentLightPos); 
            GL.Uniform3(viewPosition, _camera.Position); 
            GL.Uniform3(lightColorPosition, lightColor);
            GL.Uniform3(objectColorPosition, objectColor);


            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();

            Matrix4 normalMatrix = new Matrix4(Matrix3.Transpose(Matrix3.Invert(new Matrix3(model))));

            GL.UniformMatrix4(modelLoc, false, ref model);
            GL.UniformMatrix4(viewLoc, false, ref view);
            GL.UniformMatrix4(projLoc, false, ref projection);
            GL.UniformMatrix4(_normalMatrixLocation, false, ref normalMatrix);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 6); 

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }
            var input = KeyboardState;
            float time = (float)e.Time;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;

            // EXERCISE #2 - keyboard movement based on user's input
            if (input.IsKeyDown(Keys.W)) 
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S)) { 
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }

            if (input.IsKeyDown(Keys.A)) {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }

            if (input.IsKeyDown(Keys.D)) { 
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }

            if (input.IsKeyDown(Keys.Space)) { 
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;    // Up
            }

            if (input.IsKeyDown(Keys.LeftShift)) {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time;   // Down
            }

        }

        // EXERCISE #3 - controlling the move movement/position
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            const float sensitivity = 0.2f;

            if (_firstMove)
            {
                _lastPos = new Vector2(e.X, e.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = e.X - _lastPos.X;
                var deltaY = e.Y - _lastPos.Y;
                _lastPos = new Vector2(e.X, e.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }

        // EXERCISE #4 - controlling zoom functionality using mouse scroll wheel
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            if (_camera != null)
            {
                _camera.AspectRatio = Size.X / (float)Size.Y;
            }
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }
    }
}

