using System;
using System.IO;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Graphics.OpenGL4;


namespace GAM531
{
    public class Game : GameWindow
    {
        private int _shader;
        private int _vao;
        private int _vbo;

        // Square position (starting point)
        private Vector2 _squarePosition = new Vector2(0.0f, -0.7f);

        // constructor for the game class
        public Game()
              : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            //set window size to 1280x768
            this.CenterWindow(new Vector2i(1280, 768));

            // Center the window on the screen
            this.CenterWindow(this.Size);
        }

        // Called automatically whenever the window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            // Update the OpenGL viewport to match the new window dimensions
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        // Called once when the game starts, ideal for loading resources
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(new Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Viewport(0, 0, Size.X, Size.Y);

            // Load shader source code from files
            string vs = File.ReadAllText("Shaders/shader.vert");
            string fs = File.ReadAllText("Shaders/shader.frag");

            // Compile vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vs);
            GL.CompileShader(vertexShader);
            CheckShaderCompile(vertexShader, "Vertex Shader");

            // Compile fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fs);
            GL.CompileShader(fragmentShader);
            CheckShaderCompile(fragmentShader, "Fragment Shader");

            // Link shaders into a shader program
            _shader = GL.CreateProgram();
            GL.AttachShader(_shader, vertexShader);
            GL.AttachShader(_shader, fragmentShader);
            GL.LinkProgram(_shader);

            // Detach and delete shaders 
            GL.DetachShader(_shader, vertexShader);
            GL.DetachShader(_shader, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.UseProgram(_shader);

            // Define vertices to create square
            float[] _squareVertices =
            {
                // First triangle
                -0.1f,  0.1f, 0.0f,  // Top-left
                 0.1f,  0.1f, 0.0f,  // Top-right
                -0.1f, -0.1f, 0.0f,  // Bottom-left

                // Second triangle
                 0.1f,  0.1f, 0.0f,  // Top-right
                 0.1f, -0.1f, 0.0f,  // Bottom-right
                -0.1f, -0.1f, 0.0f   // Bottom-left
            };

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _squareVertices.Length * sizeof(float), _squareVertices, BufferUsageHint.StaticDraw);

            // Attribute 0
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            // Orthographic projection
            Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1);
            int _projLoc = GL.GetUniformLocation(_shader, "uProj");
            GL.UniformMatrix4(_projLoc, false, ref ortho);
        }

        //called every from to update game logic, phisucs, or input handling
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Read keyboard state
            var keyboard = KeyboardState;

            // Movement speed of square (player)
            float _squareMovementSpeed = 1.0f * (float)args.Time;

            // If Escape is pressed, close the game
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // If W is pressed, square moves up
            if (keyboard.IsKeyDown(Keys.W))
            {
                _squarePosition.Y += _squareMovementSpeed;
            }

            // If S is pressed, square moves down
            if (keyboard.IsKeyDown(Keys.S))
            {
                _squarePosition.Y -= _squareMovementSpeed;
            }

            // If A is pressed, square moves left
            if (keyboard.IsKeyDown(Keys.A))
            {
                _squarePosition.X -= _squareMovementSpeed;
            }

            // If D is pressed, square moves right
            if (keyboard.IsKeyDown(Keys.D))
            {
                _squarePosition.X += _squareMovementSpeed;
            }

            // Clamp square position to stay within window bounds
            _squarePosition.X = MathHelper.Clamp(_squarePosition.X, -1f + 0.1f, 1f - 0.1f);
            _squarePosition.Y = MathHelper.Clamp(_squarePosition.Y, -1f + 0.1f, 1f - 0.1f);
        }


        //called when i need to update any game visuals
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shader);

            // Model Matrix for square position
            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateTranslation(_squarePosition.X, _squarePosition.Y, 0.0f);
            int modelLoc = GL.GetUniformLocation(_shader, "uModel");
            GL.UniformMatrix4(modelLoc, false, ref model);

            //  View Matrix
            Matrix4 view = Matrix4.Identity;
            view *= Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            int viewLoc = GL.GetUniformLocation(_shader, "uView");
            GL.UniformMatrix4(viewLoc, false, ref view);

            //display the rendered frame
            GL.BindVertexArray(_vao);

            // Draw the square (6 vertices)
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            SwapBuffers();
        }

        // Called when the game is closing, ideal for cleaning up resources
        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete VAO, VBO and Shader Program
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteProgram(_shader);
        }

        // Helper function to check for shader compilation errors
        private void CheckShaderCompile(int shaderHandle, string shaderName)
        {
            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shaderHandle);
                Console.WriteLine($"Error compiling {shaderName}: {infoLog}");
            }
        }
    }
}