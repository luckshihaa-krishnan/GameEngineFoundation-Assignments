/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using System;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;


namespace Assignment3
{
    public class Game : GameWindow
    {
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;
        private int elementBufferHandle;
        public int modelLoc, viewLoc, projLoc;

        // Transformation state for the cube
        private float rotationAngles, scaleFactors;
        private bool scalingUp;

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
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            //Set window size to 1280x768
            this.Size = new Vector2i(1280, 768);

            // Center the window on the screen
            this.CenterWindow(this.Size);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // Update the OpenGL viewport to match the new window dimensions
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }


        // When the game is loading
        protected override void OnLoad()
        {
            base.OnLoad();

            // Setting background color
            GL.ClearColor(new Color4(0.4f, 0.2f, 0.5f, 1f));
            GL.Enable(EnableCap.DepthTest);

            //Creating an array with coordinates (to draw cube)
            float[] vertices = new float[]
            {
                -0.5f,0.5f,0.5f,
                 0.5f,0.5f,0.5f,
                 0.5f,-0.5f,0.5f,
                -0.5f,-0.5f,0.5f,

                -0.5f,0.5f,-0.5f,
                 0.5f,0.5f,-0.5f,
                 0.5f,-0.5f,-0.5f,
                -0.5f,-0.5f,-0.5f,
            };

            Vector3 a = new Vector3(-0.2f, 0.5f, 0);    // top left of rectangle
            Vector3 b = new Vector3(0.2f, 0.5f, 0);     // top right of rectangle
            Vector3 c = new Vector3(1f, 1f, 0);

            Vector3 addA = new Vector3(a + c);          // addition
            Vector3 addB = new Vector3(b + c);          // addition

            Vector3 subtractA = new Vector3(a - c);     // subtraction
            Vector3 subtractB = new Vector3(b - c);     // subtraction

            Console.WriteLine($"Vector a = {a}");
            Console.WriteLine($"Vector b = {b}");
            Console.WriteLine($"Vector c = {c}");
            Console.WriteLine($"addA = {addA}, addB = {addB}");                         //output for addition
            Console.WriteLine($"subtractA = {subtractA}, subtractB = {subtractB}");     //output for subtraction

            float dot = Vector3.Dot(a, b);          // dot product 
            Vector3 cross = Vector3.Cross(a, b);    // cross product
            Vector3 normalized = a.Normalized();

            Console.WriteLine($"Dot = {dot}, Cross = {cross}, Normalized = {normalized}");

            // Generate a Vertex Array Object (VAO) to store the VBO configuration
            vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayHandle);

            vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Generate EBO to store indices and reuse vertices
            elementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);


            // Vertex shader: Positions each vertex
            string vertexShaderCode =
                @"
                    #version 330 core
                    layout(location = 0) in vec3 aPosition;     // Vertex position input

                    uniform mat4 uModel;
                    uniform mat4 uView;
                    uniform mat4 uProj;
                    void main()
                    {
                        gl_Position = uProj * uView * uModel * vec4(aPosition, 1.0);    //Converting vec3 to vec4 for output
                    }
            
                ";

            // Fragment shader: outputs a single color
            string fragmentShaderCode =
                @"
                    #version 330 core
                    out vec4 FragColor;
                    
                    void main()
                    {
                        FragColor = vec4(0.0f, 1.0f, 0.0f, 0.0f);  //(color of cube)
                    }
                    
                ";


            // Creating Compiler Shaders
            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);
            CheckShaderCompile(vertexShaderHandle, "Vertex Shader");

            int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderCode);
            GL.CompileShader(fragmentShaderHandle);
            CheckShaderCompile(fragmentShaderHandle, "Fragment Shader");

            // Create shader program and link shaders
            shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.LinkProgram(shaderProgramHandle);

            // Cleanup shaders after linking (no longer needed individually)
            GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            // Get uniform locations
            modelLoc = GL.GetUniformLocation(shaderProgramHandle, "uModel");
            viewLoc = GL.GetUniformLocation(shaderProgramHandle, "uView");
            projLoc = GL.GetUniformLocation(shaderProgramHandle, "uProj");

            rotationAngles = 0.5f;
            scaleFactors = 1f;
            scalingUp = true;

        }


        // When the game is unloading
        protected override void OnUnload()
        {
            // Unbind and delete buffers and shader program
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBufferHandle);

            GL.DeleteBuffer(elementBufferHandle);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vertexArrayHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(shaderProgramHandle);

            base.OnUnload();
        }


        // Game is updated
        // Called every frame to update game logic, physics or input handling
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

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
        }


        // Called when I need to update any game visuals
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            //Clear the screen with background color
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // Use our shader program
            GL.UseProgram(shaderProgramHandle);

            // View matrix (camera looking at origin)
            Matrix4 view = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);

            // Projection matrix (perspective)
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                (float)Size.X / Size.Y,
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
            SwapBuffers();
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