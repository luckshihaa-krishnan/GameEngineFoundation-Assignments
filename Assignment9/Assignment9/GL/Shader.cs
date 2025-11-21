/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */  


using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Assignment9
{
    public class Shader
    {
        public readonly int Handle;

        public Shader(string vertPath, string fragPath)
        {

            // Load and compile vertex shader
            var shaderSource = File.ReadAllText(vertPath);

            // Creating an empty shader
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Bind GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);

            // Load and compile fragment shader
            var shaderSourceFrag = File.ReadAllText(fragPath);

            // Creating an empty shader
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            // Bind GLSL source code
            GL.ShaderSource(fragmentShader, shaderSourceFrag);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            // Attach shaders and link them togehter
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            // Getting the number of active uniforms
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);

            // If no active uniforms found, throw exception
            if (numberOfUniforms == 0)
            {
                throw new Exception($"Error linking shader program({Handle}). No active uniforms found.");
            }

            // Detach and delete shaders after linking
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        // Function to compile shader 
        private static void CompileShader(int shader)
        {
            // Compile shader
            GL.CompileShader(shader);

            // Checking for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // Used to get information about the error
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        // Function to use the shader program
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // Function to set uniform int on this shader
        public void SetInt(string name, int data)
        {
            GL.Uniform1(GL.GetUniformLocation(Handle, name), data);
        }

        // Function to set uniform float on this shader
        public void SetFloat(string name, float data)
        {
            GL.Uniform1(GL.GetUniformLocation(Handle, name), data);
        }

        // Function to set uniform Matrix4 on this shader
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(Handle, name), false, ref data);
        }

        // Function to set uniform Vector3 on this shader
        public void SetVector3(string name, Vector3 data)
        {
            GL.Uniform3(GL.GetUniformLocation(Handle, name), data);
        }

        // Function to set uniform bool on this shader
        public void SetBool(string name, bool value)
        {
            int num;
            if (value)
            {
                num = 1;    
            }
            else
            {
                num = 0;
            }
                GL.Uniform1(GL.GetUniformLocation(Handle, name), num);
        }

        // Delete shader program
        public void Dispose()
        {
            GL.DeleteProgram(Handle); 
        }


    }
}