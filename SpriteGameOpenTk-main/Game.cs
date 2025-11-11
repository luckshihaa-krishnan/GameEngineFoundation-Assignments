/*
* Name: Luckshihaa Krishnan 
* Student ID: 186418216
* Section: GAM 531 NSA 
*/


using OpenTK.Graphics.OpenGL4;                       // OpenGL API
using OpenTK.Mathematics;                            // Matrix4, Vector types
using OpenTK.Windowing.Common;                       // Frame events (OnLoad/OnUpdate/OnRender)
using OpenTK.Windowing.Desktop;                      // GameWindow/NativeWindowSettings
using OpenTK.Windowing.GraphicsLibraryFramework;     // Keyboard state
using SixLabors.ImageSharp.PixelFormats;             // Rgba32 pixel type
using System;
using System.IO;
using ImageSharp = SixLabors.ImageSharp.Image;       // Alias for brevity


namespace OpenTK_Sprite_Animation
{
    public class SpriteAnimationGame : GameWindow
    {
        private Character _character;                    // Handles animation state + UV selection
        private int _shaderProgram;                      // Linked GLSL program
        private int _vao, _vbo;                          // Geometry
        private int _walkTexture, _runTexture, _shotTexture, _attackTexture;    // Sprite Sheets 
        private int _projLoc, _modelLoc;

        private Vector2 _position = new(400, 300);
        private  float _walkSpeed = 200f;
        private float _runSpeed = 350f;

        public SpriteAnimationGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(1f, 1f, 1f, 1f);             // White background
            GL.Enable(EnableCap.Blend);                // Enable alpha blending
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _shaderProgram = CreateShaderProgram();
            _walkTexture = LoadTexture("Walk.png");
            _runTexture = LoadTexture("Run.png");
            _shotTexture = LoadTexture("Shot_2.png");
            _attackTexture = LoadTexture("Attack.png");

            // Quad vertices: [pos.x, pos.y, uv.x, uv.y], centered model space
            float w = 100f, h = 128f;

            float[] vertices =
            {
                -w, -h, 0f, 0f,
                 w, -h, 1f, 0f,
                 w,  h, 1f, 1f,
                -w,  h, 0f, 1f
            };

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Attribute 0: vec2 position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            // Attribute 1: vec2 texcoord
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            GL.UseProgram(_shaderProgram);

            int texLoc = GL.GetUniformLocation(_shaderProgram, "uTexture");
            GL.Uniform1(texLoc, 0);

            _projLoc = GL.GetUniformLocation(_shaderProgram, "projection");
            _modelLoc = GL.GetUniformLocation(_shaderProgram, "model");

            Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, -1, 1);
            GL.UniformMatrix4(_projLoc, false, ref ortho);

            Matrix4 model = Matrix4.CreateTranslation(400, 300, 0);
            GL.UniformMatrix4(_modelLoc, false, ref model);

            _character = new Character(_shaderProgram, _walkTexture, _runTexture, _shotTexture, _attackTexture);
            _character.Update(0f, Direction.Right, Action.Idle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = KeyboardState;
            Direction dir = Direction.None;
            Action action = Action.Idle;

            bool _isMoving = false;
            float speed = _walkSpeed;

            if (keyboard.IsKeyDown(Keys.Right))
            {
                dir = Direction.Right;
                _isMoving = true;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                dir = Direction.Left;
                _isMoving = true;
            }

            if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
            {
                speed = _runSpeed;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                action = Action.Shot;
                dir = Direction.None;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                action = Action.Attack;
                dir = Direction.None;
            }
            else if (_isMoving)
            {
                if (speed == _runSpeed)
                {
                    action = Action.Run;
                }
                else
                {
                    action = Action.Walk;
                }

                // Update position based on direction and speed
                _position.X += (dir == Direction.Right ? 1 : -1) * speed * (float)e.Time;

                // Clamp position to window bounds
                _position.X = MathHelper.Clamp(_position.X, 100 - 100, Size.X - 100 + 60);
            }

            // Update character only if action changed or not idle
            if (action != Action.Idle || _character._currentAction != Action.Idle)
            {
                _character.Update((float)e.Time, dir, action);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(_shaderProgram);

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateTranslation(_position.X, _position.Y, 0);

            if (_character.isRefected)
            {
                model *= Matrix4.CreateScale(-1, 1, 1);
                model *= Matrix4.CreateTranslation(_position.X * 2, 0, 0);
            }

            GL.UniformMatrix4(_modelLoc, false, ref model);

            _character.Render();

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            // Deleting shader program, textures, buffers
            GL.DeleteProgram(_shaderProgram);
            GL.DeleteTexture(_walkTexture);
            GL.DeleteTexture(_runTexture);
            GL.DeleteTexture(_shotTexture);
            GL.DeleteTexture(_attackTexture);
            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(_vao);
            base.OnUnload();
        }

        private int CreateShaderProgram()
        {
            string vs = File.ReadAllText("Shaders/vertex.glsl");
            string fs = File.ReadAllText("Shaders/fragment.glsl");

            int v = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(v, vs);
            GL.CompileShader(v);
            CheckShaderCompile(v, "VERTEX");

            int f = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(f, fs);
            GL.CompileShader(f);
            CheckShaderCompile(f, "FRAGMENT");

            int p = GL.CreateProgram();
            GL.AttachShader(p, v);
            GL.AttachShader(p, f);
            GL.LinkProgram(p);
            CheckProgramLink(p);

            GL.DetachShader(p, v);
            GL.DetachShader(p, f);
            GL.DeleteShader(v);
            GL.DeleteShader(f);

            return p;
        }

        private static void CheckShaderCompile(int shader, string stage)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int ok);
            if (ok == 0)
                throw new Exception($"{stage} SHADER COMPILE ERROR:\n{GL.GetShaderInfoLog(shader)}");
        }

        private static void CheckProgramLink(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int ok);
            if (ok == 0)
                throw new Exception($"PROGRAM LINK ERROR:\n{GL.GetProgramInfoLog(program)}");
        }

        private int LoadTexture(string path)
        {
            if (!File.Exists(path)) 
            { 
                throw new FileNotFoundException($"Texture not found: {path}"); 
            }

            using var img = ImageSharp.Load<Rgba32>(path); // decode to RGBA8

            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);

            // Copy raw pixels to managed buffer then upload
            var pixels = new byte[4 * img.Width * img.Height];
            img.CopyPixelDataTo(pixels);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          img.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            // Nearest: prevents bleeding between adjacent frames on the atlas
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // Clamp: avoid wrap artifacts at frame borders
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            return tex;
        }
    }

    public enum Direction { None, Right, Left }
    public enum Action { Idle, Walk, Run, Shot, Attack }

    public class Character
    {

        private readonly int _shader;  // Program containing uOffset/uSize
        private float _timer;          // Accumulated time for frame stepping
        private int _frame;            // Current frame column (0..FrameCount-1)
        private Direction _currentDir; // Last non-none direction

        public Action _currentAction { get; private set; } = Action.Idle;

        public bool isRefected => _currentDir == Direction.Left;

        // Texture 
        private readonly int _walkTex, _runTex, _shotTex, _attackTex;

        // Timing and frame counds for each action
        private const float _walkFrameTime = 0.08f;
        private const int _walkFrameCount = 7;

        private const float _runFrameTime = 0.06f;
        private const int _runFrameCount = 8;

        private const float _shotFrameTime = 0.10f;
        private const int _shotFrameCount = 4;

        private const float _attackFrameTime = 0.10f;
        private const int _attackFrameCount = 3;


        // Sprite sheet layout (pixel units)
        private const float _walkFrameW = 48f;
        private const float _walkSheetW = 336f;

        private const float _runFrameW = 48f;
        private const float _runSheetW = 384f;

        private const float _shotFrameW = 128f;
        private const float _shotSheetW = 512f;

        private const float _attackFrameW = 128f;
        private const float _attackSheetW = 384f;

        public Character(int shader, int walkTex, int runTex, int shotTex, int attackTex)
        {
            _shader = shader;
            _walkTex = walkTex;
            _runTex = runTex;
            _shotTex = shotTex;
            _attackTex = attackTex;
            _currentDir = Direction.None;
        }

        public void Update(float delta, Direction dir, Action action)
        {
            if (dir != Direction.None)
            {
                _currentDir = dir;
            }

            if (action != _currentAction)
            {
                _timer = 0f;
                _frame = 0;
                _currentAction = action;
            }

            if (_currentAction == Action.Idle)
            {
                SetFrame(0, _walkTex, _walkFrameW, _walkSheetW);
                return;
            }

            int Texture;
            float FrameTime;
            int FrameCount;
            float FrameW;
            float SheetW;

            if (_currentAction == Action.Walk)
            {
                Texture = _walkTex;
                FrameTime = _walkFrameTime;
                FrameCount = _walkFrameCount;
                FrameW = _walkFrameW;
                SheetW = _walkSheetW;
            }
            else if(_currentAction == Action.Run)
            {
                Texture = _runTex;
                FrameTime = _runFrameTime;
                FrameCount = _runFrameCount;
                FrameW = _runFrameW;
                SheetW = _runSheetW;
            }
            else if(_currentAction == Action.Shot)
            {
                Texture = _shotTex;
                FrameTime = _shotFrameTime;
                FrameCount = _shotFrameCount;
                FrameW = _shotFrameW;
                SheetW = _shotSheetW;
            }
            else if(_currentAction == Action.Attack)
            {
                Texture = _attackTex;
                FrameTime = _attackFrameTime;
                FrameCount = _attackFrameCount;
                FrameW = _attackFrameW;
                SheetW = _attackSheetW;
            }
            else
            {
                Texture = _walkTex;
                FrameTime = _walkFrameTime;
                FrameCount = _walkFrameCount;
                FrameW = _walkFrameW;
                SheetW = _walkSheetW;
            }

            _timer += delta;
            if (_timer >= FrameTime)
            {
                _timer -= FrameTime;
                _frame = (_frame + 1) % FrameCount;

                if (_frame >= FrameCount)
                {
                    if (_currentAction == Action.Shot || _currentAction == Action.Attack)
                    {
                        _currentAction = Action.Idle;
                        _frame = 0;
                    }
                    else
                        _frame %= FrameCount;
                }
            }
            SetFrame(_frame, Texture, FrameW, SheetW);
        }

        public void Render()
        {
            int texture;

            if(_currentAction == Action.Run)
            {
                texture = _runTex;
            }
            else if(_currentAction == Action.Shot)
            {
                texture = _shotTex;
            }
            else if(_currentAction == Action.Attack)
            {
                texture = _attackTex;
            }
            else
            {
                texture = _walkTex;
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
        }

        private void SetFrame(int col, int texture, float frameW, float sheetW)
        {
            float x = (col * frameW) / sheetW;
            float y = 0f;
            float w = frameW / sheetW;
            float h = 1.0f;

            GL.UseProgram(_shader);
            int off = GL.GetUniformLocation(_shader, "uOffset");
            int sz = GL.GetUniformLocation(_shader, "uSize");
            GL.Uniform2(off, x, y);
            GL.Uniform2(sz, w, h);
        }
    }
}
