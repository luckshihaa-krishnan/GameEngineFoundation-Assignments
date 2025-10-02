/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Platform.Windows;


namespace Assignment4
{
    public class Game : IDisposable
    {
        private GameWindow _window;
        private int shaderProgramHandle;
        private int vertexArrayHandle;      //vao
        private int vertexBufferHandle;     //vbo
        private int elementBufferHandle;    //ebo
        private int _texture;
        public int modelLoc, viewLoc, projLoc;

        // Transformation state for the cube
        private float rotationAngles, scaleFactors;
        private bool scalingUp;

        // Default wrapping and filtering
        private TextureWrapMode wrapMode = TextureWrapMode.Repeat;
        private TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear;
        private TextureMagFilter magFilter = TextureMagFilter.Linear;

        // Vertex shader: Positions each vertex
        private readonly string vertexShaderSource =
            @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;     // Vertex position input
                layout(location = 1) in vec2 aTexCoord;

                out vec2 TexCoord;

                uniform mat4 uModel;
                uniform mat4 uView;
                uniform mat4 uProj;
                void main()
                {
                    gl_Position = uProj * uView * uModel * vec4(aPosition, 1.0);    //Converting vec3 to vec4 for output
                    TexCoord = aTexCoord;
                }
            
            ";

        // Fragment shader: outputs a single color or texture
        private readonly string fragmentShaderSource =
            @"
                #version 330 core
                out vec4 FragColor;
                in vec2 TexCoord;

                uniform sampler2D ourTexture;
                  
                void main()
                {
                    FragColor = texture(ourTexture, TexCoord);
                }
            ";

        // Define indices of cube
        uint[] indices =
        {
            0,1,2,
            0,2,3,

            4,5,6,
            4,6,7,

            4,5,1,
            4,1,0,

            7,6,2,
            7,2,3,

            1,5,6,
            1,6,2,

            0,4,7,
            0,7,3
        };

        // Constructor for Game class
        public Game()
        {
            var nativeSettings = new NativeWindowSettings()
            {
                //Set window size to 1280x768
                Size = new Vector2i(1280, 768),
                Title = "Assignment 4 - Textured Cube",
                Flags = ContextFlags.ForwardCompatible
            };

            var gameSettings = new GameWindowSettings()
            {
                UpdateFrequency = 60.0
            };

            _window = new GameWindow(gameSettings, nativeSettings);
            _window.Load += OnLoad;
            _window.RenderFrame += OnRenderFrame;
            _window.UpdateFrame += OnUpdateFrame;
            _window.Resize += OnResize;

        }

        private void OnResize(ResizeEventArgs e)
        {
            // Update the OpenGL viewport to match the new window dimensions
            GL.Viewport(0, 0, e.Width, e.Height);
        }


        // When the game is loading
        private void OnLoad()
        {
            // Setting background color
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);

            // Compile shaders
            int vert = ShaderCompile(ShaderType.VertexShader, vertexShaderSource);
            int frag = ShaderCompile(ShaderType.FragmentShader, fragmentShaderSource);

            shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vert);
            GL.AttachShader(shaderProgramHandle, frag);
            GL.LinkProgram(shaderProgramHandle);
            CheckProgram(shaderProgramHandle);

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);

            //Creating an array with coordinates (to draw cube)
            float[] vertices = new float[]
            {
                -0.5f,0.5f,0.5f,    0.0f,1.0f,
                 0.5f,0.5f,0.5f,    0.0f,0.0f,
                 0.5f,-0.5f,0.5f,   1.0f,0.0f,
                -0.5f,-0.5f,0.5f,   1.0f,1.0f,

                -0.5f,0.5f,-0.5f,   0.0f,1.0f,
                 0.5f,0.5f,-0.5f,   0.0f,0.0f,
                 0.5f,-0.5f,-0.5f,  1.0f,0.0f,
                -0.5f,-0.5f,-0.5f,  1.0f,1.0f,
            };


            // Generate a Vertex Array Object (VAO) to store the VBO configuration
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);

            vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Generate EBO to store indices and reuse vertices
            elementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);

            // Get uniform locations
            modelLoc = GL.GetUniformLocation(shaderProgramHandle, "uModel");
            viewLoc = GL.GetUniformLocation(shaderProgramHandle, "uView");
            projLoc = GL.GetUniformLocation(shaderProgramHandle, "uProj");

            rotationAngles = 0.5f;
            scaleFactors = 1f;
            scalingUp = true;

            // Load Texture
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string texturePath = Path.Combine(baseDir, "Assets", "blueMarble.jpeg");
            _texture = LoadTexture(texturePath);
        }


        // Game is updated
        // Called every frame to update game logic, physics or input handling
        private void OnUpdateFrame(FrameEventArgs args)
        {
            // Rotate continueously 
            rotationAngles += (float)args.Time * 2;

            // Oscillating scale between 0.5 and 1.5
            if (scalingUp)
            {
                scaleFactors += (float)args.Time;
                if (scaleFactors >= 1.5f)
                {
                    scalingUp = false;
                }
            }
            else
            {
                scaleFactors -= (float)args.Time;
                if (scaleFactors <= 0.5f)
                {
                    scalingUp = true;
                }
            }

            if (_window.IsKeyDown(Keys.Escape))
            {
                _window.Close();
            }

            if (_window.IsKeyPressed(Keys.D1))
            {
                SetWrapMode(TextureWrapMode.Repeat);
            }

            if (_window.IsKeyPressed(Keys.D2))
            {
                SetWrapMode(TextureWrapMode.MirroredRepeat);
            }

            if (_window.IsKeyPressed(Keys.D3))
            {
                SetWrapMode(TextureWrapMode.ClampToEdge);
            }

            if (_window.IsKeyPressed(Keys.D4))
            {
                SetWrapMode(TextureWrapMode.ClampToBorder);
            }

            if (_window.IsKeyPressed(Keys.F1))
            {
                SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            }

            if (_window.IsKeyPressed(Keys.F2))
            {
                SetFilter(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear);
            }
        }


        // Called when I need to update any game visuals
        private void OnRenderFrame(FrameEventArgs args)
        {
            //Clear the screen with background color
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // Use our shader program
            GL.UseProgram(shaderProgramHandle);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            // View matrix (camera looking at origin)
            Matrix4 view = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);

            // Projection matrix (perspective)
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                (float)_window.Size.X / _window.Size.Y,
                0.1f,
                100f
            );

            // Send view and projection to shader (for cube)
            GL.UniformMatrix4(viewLoc, false, ref view);
            GL.UniformMatrix4(projLoc, false, ref projection);

            GL.BindVertexArray(vertexArrayHandle);

            // Rotation quaternion for cube
            Quaternion rotation = Quaternion.FromAxisAngle(Vector3.UnitY, rotationAngles);
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(rotation);

            // Scaling
            Matrix4 scaleMatrix = Matrix4.CreateScale(scaleFactors);

            // Translation
            Matrix4 translationMatrix = Matrix4.CreateTranslation(-2f + 1 * 2f, 0f, 0f);

            // Combine transformations: Model = Translation * Rotation * Scale
            Matrix4 model = scaleMatrix * rotationMatrix * translationMatrix;

            // Send matrix model to shader
            GL.UniformMatrix4(modelLoc, false, ref model);

            // Draw out cube
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
            // Display the rendered frame
            _window.SwapBuffers();
        }

        // Helper function to check for shader compilation errors
        private int ShaderCompile(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                throw new Exception(GL.GetShaderInfoLog(shader));
            }
            return shader;
        }

        public void Run() => _window.Run();
        private void CheckProgram(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
        }

        private int LoadTexture(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find file {path}");
            }

            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);

            // Initial wrap and filter
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            using (Bitmap bmp = new Bitmap(path))
            {
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                var data = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);
            }

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return texID;
        }

        private void SetWrapMode(TextureWrapMode mode)
        {
            wrapMode = mode;
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        }

        private void SetFilter(TextureMinFilter min, TextureMagFilter mag)
        {
            minFilter = min;
            magFilter = mag;
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vertexBufferHandle);
            GL.DeleteVertexArray(vertexArrayHandle);
            GL.DeleteTexture(_texture);
            GL.DeleteProgram(shaderProgramHandle);
            _window.Dispose();
        }
    }
}
